// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommPortSingleThreader
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal class WidcommPortSingleThreader : IDisposable
  {
    private Queue<WidcommPortSingleThreader.StCommand> m_actions = new Queue<WidcommPortSingleThreader.StCommand>();
    private Thread m_thread;
    private volatile bool _ended;

    public WidcommPortSingleThreader()
    {
      Thread thread = new Thread(new ThreadStart(this.SingleThreader_Runner));
      thread.Name = "32feetWidcommST";
      thread.SetApartmentState(ApartmentState.STA);
      thread.IsBackground = true;
      this.m_thread = thread;
      this.m_thread.Start();
    }

    public static bool IsWidcommSingleThread(WidcommPortSingleThreader st)
    {
      return Thread.CurrentThread == st.m_thread;
    }

    internal T AddCommand<T>(T command) where T : WidcommPortSingleThreader.StCommand
    {
      if (WidcommBtInterface.IsWidcommSingleThread(this))
        throw new NotSupportedException("Internal error -- Widcomm main thread calling itself!?!");
      command.SetRunner(this);
      lock (this.m_actions)
      {
        this.m_actions.Enqueue((WidcommPortSingleThreader.StCommand) command);
        Monitor.Pulse((object) this.m_actions);
      }
      return command;
    }

    private void SingleThreader_Runner()
    {
      try
      {
        while (true)
        {
          WidcommPortSingleThreader.StCommand stCommand;
          lock (this.m_actions)
          {
            while (this.m_actions.Count == 0)
              Monitor.Wait((object) this.m_actions);
            stCommand = this.m_actions.Dequeue();
          }
          if (!(stCommand is WidcommPortSingleThreader.ExitCommand))
          {
            stCommand.Action();
            stCommand = (WidcommPortSingleThreader.StCommand) null;
          }
          else
            break;
        }
      }
      finally
      {
        this._ended = true;
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public void Dispose(bool disposing)
    {
      MiscUtils.Trace_WriteLine("WidcommPortSingleThreader.Dispose(b)");
      this._ended = true;
      this.AddCommand<WidcommPortSingleThreader.ExitCommand>(new WidcommPortSingleThreader.ExitCommand());
    }

    public bool IsEnded => this._ended;

    internal abstract class StCommand
    {
      private ManualResetEvent _complete = new ManualResetEvent(false);
      private Exception _error;
      private WidcommPortSingleThreader _runner;

      internal void SetRunner(WidcommPortSingleThreader runner) => this._runner = runner;

      internal void Action()
      {
        try
        {
          this.ActionCore();
        }
        catch (Exception ex)
        {
          this._error = ex;
        }
        finally
        {
          try
          {
            this._complete.Set();
          }
          catch (ObjectDisposedException ex)
          {
            MiscUtils.Trace_WriteLine("!StCommand.Action ObjectDisposedException (" + this.GetType().Name + ")");
          }
        }
      }

      protected abstract void ActionCore();

      public Exception Error => this._error;

      internal void WaitCompletion() => this.WaitCompletion(true);

      internal void WaitCompletion(bool shouldWait)
      {
        bool isEnded = this._runner.IsEnded;
        if (shouldWait && !isEnded)
        {
          this._complete.WaitOne();
          this._complete.Close();
        }
        Exception error = this.Error;
        if (error != null)
          throw new TargetInvocationException(error.Message == null ? "Widcomm WaitCompletion error." : error.Message, error);
      }
    }

    internal sealed class ExitCommand : WidcommPortSingleThreader.StCommand
    {
      protected override void ActionCore()
      {
      }
    }

    internal abstract class PortCommand : WidcommPortSingleThreader.StCommand
    {
      private IRfcommPort _port;

      protected IRfcommPort Port => this._port;

      public PortCommand(IRfcommPort port) => this._port = port;
    }

    internal sealed class PortWriteCommand : WidcommPortSingleThreader.PortCommand
    {
      private byte[] _data;
      private ushort _lenToWrite;
      private ushort _lenWritten;
      private PORT_RETURN_CODE _result;

      public PortWriteCommand(byte[] data, ushort lenToWrite, IRfcommPort port)
        : base(port)
      {
        this._data = data;
        this._lenToWrite = lenToWrite;
      }

      protected override void ActionCore()
      {
        this._result = this.Port.Write(this._data, this._lenToWrite, out this._lenWritten);
      }

      private new void WaitCompletion()
      {
        throw new NotSupportedException("Use WaitCompletion(out ushort lenWritten).");
      }

      internal PORT_RETURN_CODE WaitCompletion(out ushort lenWritten)
      {
        base.WaitCompletion();
        lenWritten = this._lenWritten;
        return this._result;
      }
    }

    internal sealed class PortCreateCommand(IRfcommPort port) : WidcommPortSingleThreader.PortCommand(port)
    {
      protected override void ActionCore() => this.Port.Create();
    }

    internal sealed class OpenServerCommand : WidcommPortSingleThreader.PortCommand
    {
      private int _scn;
      private PORT_RETURN_CODE _result;

      public OpenServerCommand(int scn, IRfcommPort port)
        : base(port)
      {
        this._scn = scn;
      }

      public PORT_RETURN_CODE WaitCompletion()
      {
        base.WaitCompletion();
        return this._result;
      }

      protected override void ActionCore() => this._result = this.Port.OpenServer(this._scn);
    }

    internal sealed class OpenClientCommand : WidcommPortSingleThreader.PortCommand
    {
      private int _scn;
      private byte[] _address;
      private PORT_RETURN_CODE _result;

      public OpenClientCommand(int scn, byte[] address, IRfcommPort port)
        : base(port)
      {
        this._address = address;
        this._scn = scn;
      }

      public PORT_RETURN_CODE WaitCompletion()
      {
        base.WaitCompletion();
        return this._result;
      }

      protected override void ActionCore()
      {
        this._result = this.Port.OpenClient(this._scn, this._address);
      }
    }

    internal sealed class PortCloseCommand(IRfcommPort port) : WidcommPortSingleThreader.PortCommand(port)
    {
      private PORT_RETURN_CODE _result = PORT_RETURN_CODE.NotSet;

      protected override void ActionCore() => this._result = this.Port.Close();

      public PORT_RETURN_CODE WaitCompletion(bool shouldWait)
      {
        base.WaitCompletion(shouldWait);
        return this._result;
      }
    }

    internal sealed class MiscNoReturnCommand : WidcommPortSingleThreader.StCommand
    {
      private ThreadStart _dlgt;

      public MiscNoReturnCommand(ThreadStart dlgt) => this._dlgt = dlgt;

      protected override void ActionCore() => this._dlgt();
    }

    internal sealed class MiscReturnCommand<TResult> : WidcommPortSingleThreader.StCommand
    {
      private Func<TResult> _dlgt;
      private TResult _result;

      public MiscReturnCommand(Func<TResult> dlgt) => this._dlgt = dlgt;

      protected override void ActionCore() => this._result = this._dlgt();

      public TResult WaitCompletion()
      {
        base.WaitCompletion();
        return this._result;
      }
    }
  }
}
