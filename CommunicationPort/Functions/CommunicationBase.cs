// Decompiled with JetBrains decompiler
// Type: CommunicationPort.Functions.CommunicationBase
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using System;
using System.IO.Ports;
using System.Threading;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace CommunicationPort.Functions
{
  public abstract class CommunicationBase
  {
    internal ICommunicationFunctions MyFunctions;
    internal bool ConfigurationChanged;
    internal DateTime earliestNextTransmitTime;
    internal DateTime nextWakeupTime;
    private int MaxAdditionalStateTime_ms = 0;

    internal ConfigList configList => this.MyFunctions.GetReadoutConfiguration();

    internal int PollingErrorTime_ms { get; set; }

    public CommunicationBase(ICommunicationFunctions comPortFunctions)
    {
      this.MyFunctions = comPortFunctions;
      this.nextWakeupTime = DateTime.MinValue;
    }

    public Parity Parity
    {
      get
      {
        switch (this.configList.Parity)
        {
          case "even":
            return Parity.Even;
          case "odd":
            return Parity.Odd;
          default:
            return Parity.None;
        }
      }
      set
      {
        switch (value)
        {
          case Parity.Odd:
            this.configList.Parity = "odd";
            this.ConfigurationChanged = true;
            break;
          case Parity.Even:
            this.configList.Parity = "even";
            this.ConfigurationChanged = true;
            break;
          default:
            this.configList.Parity = "no";
            this.ConfigurationChanged = true;
            break;
        }
      }
    }

    public double ByteTime
    {
      get
      {
        if (this.configList.Baudrate == 0)
          return 0.0;
        return this.Parity == Parity.None ? 1.0 / (double) this.configList.Baudrate * 10.0 * 1000.0 : 1.0 / (double) this.configList.Baudrate * 11.0 * 1000.0;
      }
    }

    protected int MaxAdditionalStateBytes
    {
      get => (int) ((double) this.MaxAdditionalStateTime_ms / (55.0 / 576.0));
      set
      {
        this.MaxAdditionalStateTime_ms = (int) ((double) value * 1000.0 / 115200.0 * 11.0 + 0.99);
      }
    }

    public WakeupSystem Wakeup
    {
      get => this.configList.Get<WakeupSystem>(nameof (Wakeup));
      set => this.configList.Wakeup = value.ToString();
    }

    public ZENNER.CommonLibrary.MinoConnectBaseStates MinoConnectBaseState
    {
      get => this.configList.Get<ZENNER.CommonLibrary.MinoConnectBaseStates>(nameof (MinoConnectBaseState));
      set => this.configList.MinoConnectBaseState = value.ToString();
    }

    public IrDaSelection IrDaSelection
    {
      get => this.configList.Get<IrDaSelection>(nameof (IrDaSelection));
      set => this.configList.IrDaSelection = value.ToString();
    }

    public CombiHeadSelection CombiHeadSelection
    {
      get => this.configList.Get<CombiHeadSelection>(nameof (CombiHeadSelection));
      set => this.configList.CombiHeadSelection = value.ToString();
    }

    public abstract bool IsOpen { get; }

    public abstract int BytesToRead { get; }

    public abstract void Open();

    public abstract void Close();

    public abstract int Read(byte[] buffer, int offset, int count);

    public abstract void Write(byte[] data, int offset, int count);

    public abstract void Write(byte[] buffer);

    public abstract void WriteWithoutDiscardInputBuffer(byte[] buffer);

    public abstract byte[] ReadHeader(int count);

    public abstract byte[] ReadEnd(int count);

    public abstract bool DiscardInBuffer();

    public abstract void DiscardCurrentInBuffer();

    protected int GetReadHeaderTimeout(int headerByteCount)
    {
      int num = headerByteCount + this.MaxAdditionalStateBytes;
      return (int) ((double) (this.configList.RecTime_BeforFirstByte + this.configList.RecTime_OffsetPerBlock) + (double) headerByteCount * this.ByteTime + (double) ((headerByteCount - 1) * this.configList.RecTime_OffsetPerByte) + (double) this.configList.RecTime_GlobalOffset + (double) this.MaxAdditionalStateTime_ms + (double) CommunicationPortFunctions.SystemCommunicationTimeoutOffset);
    }

    protected int GetReadEndTimeout(int additionalByteCount)
    {
      return (int) ((double) this.configList.RecTime_OffsetPerBlock + (double) additionalByteCount * this.ByteTime + (double) ((additionalByteCount - 1) * this.configList.RecTime_OffsetPerByte) + (double) this.configList.RecTime_GlobalOffset + (double) this.MaxAdditionalStateTime_ms + (double) CommunicationPortFunctions.SystemCommunicationTimeoutOffset);
    }

    protected int GetByteTime(int count) => (int) ((double) count * this.ByteTime + 0.99);

    protected void SetNextTimeAfterOpen()
    {
      this.earliestNextTransmitTime = DateTime.Now.AddMilliseconds((double) this.configList.TransTime_AfterOpen);
    }

    protected void SetNextTimeAfterRead()
    {
      this.earliestNextTransmitTime = DateTime.Now.AddMilliseconds((double) this.configList.RecTransTime);
    }

    protected void WaitToNextTransmitTime()
    {
      int totalMilliseconds = (int) this.earliestNextTransmitTime.Subtract(DateTime.Now).TotalMilliseconds;
      if (totalMilliseconds > 0)
        Thread.Sleep(totalMilliseconds);
      if (this.Wakeup == 0 || !(DateTime.Now > this.nextWakeupTime))
        return;
      if (this.Wakeup == WakeupSystem.BaudrateCarrier)
        this.WriteBaudrateCarrier(this.configList.TransTime_BreakTime);
      else if (this.Wakeup == WakeupSystem.Break)
      {
        this.SetBreak();
        Thread.Sleep(this.configList.TransTime_BreakTime);
        this.ClearBreak();
      }
      this.nextWakeupTime = DateTime.Now.AddMilliseconds((double) this.configList.BreakIntervalTime);
      if (this.configList.TransTime_AfterBreak > 0)
        Thread.Sleep(this.configList.TransTime_AfterBreak);
    }

    public DateTime CalculateEndTimepoint(int count)
    {
      return SystemValues.DateTimeNow.AddMilliseconds(this.Parity == 0 ? (double) count * 1000.0 / ((double) this.configList.Baudrate / 10.0 + 1.0) : (double) count * 1000.0 / ((double) this.configList.Baudrate / 11.0 + 1.0));
    }

    public void WaitOf(DateTime timepoint)
    {
      double totalMilliseconds = timepoint.Subtract(SystemValues.DateTimeNow).TotalMilliseconds;
      if (totalMilliseconds < 0.0 || totalMilliseconds > (double) int.MaxValue)
        return;
      Thread.Sleep(Convert.ToInt32(totalMilliseconds));
    }

    public void ForceWakeup()
    {
      if (this.Wakeup == 0)
        return;
      this.nextWakeupTime = DateTime.Now.AddMilliseconds(-10.0);
    }

    protected virtual void WriteBaudrateCarrier(int carrierTime_ms)
    {
    }

    public virtual void SetBreak()
    {
    }

    public virtual void ClearBreak()
    {
    }

    public virtual void Dispose()
    {
    }
  }
}
