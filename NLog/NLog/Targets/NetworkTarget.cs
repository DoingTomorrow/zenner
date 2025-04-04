// Decompiled with JetBrains decompiler
// Type: NLog.Targets.NetworkTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Internal;
using NLog.Internal.NetworkSenders;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.Targets
{
  [Target("Network")]
  public class NetworkTarget : TargetWithLayout
  {
    private readonly Dictionary<string, LinkedListNode<NetworkSender>> _currentSenderCache = new Dictionary<string, LinkedListNode<NetworkSender>>();
    private readonly LinkedList<NetworkSender> _openNetworkSenders = new LinkedList<NetworkSender>();
    private readonly ReusableBufferCreator _reusableEncodingBuffer = new ReusableBufferCreator(16384);

    public NetworkTarget()
    {
      this.SenderFactory = NetworkSenderFactory.Default;
      this.Encoding = Encoding.UTF8;
      this.OnOverflow = NetworkTargetOverflowAction.Split;
      this.KeepConnection = true;
      this.MaxMessageSize = 65000;
      this.ConnectionCacheSize = 5;
      this.LineEnding = LineEndingMode.CRLF;
      this.OptimizeBufferReuse = this.GetType() == typeof (NetworkTarget);
    }

    public NetworkTarget(string name)
      : this()
    {
      this.Name = name;
    }

    public Layout Address { get; set; }

    [DefaultValue(true)]
    public bool KeepConnection { get; set; }

    [DefaultValue(false)]
    public bool NewLine { get; set; }

    [DefaultValue("CRLF")]
    public LineEndingMode LineEnding { get; set; }

    [DefaultValue(65000)]
    public int MaxMessageSize { get; set; }

    [DefaultValue(5)]
    public int ConnectionCacheSize { get; set; }

    public int MaxConnections { get; set; }

    public NetworkTargetConnectionsOverflowAction OnConnectionOverflow { get; set; }

    [DefaultValue(0)]
    public int MaxQueueSize { get; set; }

    [DefaultValue(NetworkTargetOverflowAction.Split)]
    public NetworkTargetOverflowAction OnOverflow { get; set; }

    [DefaultValue("utf-8")]
    public Encoding Encoding { get; set; }

    internal INetworkSenderFactory SenderFactory { get; set; }

    protected override void FlushAsync(AsyncContinuation asyncContinuation)
    {
      int remainingCount = 0;
      AsyncContinuation continuation = (AsyncContinuation) (ex =>
      {
        if (Interlocked.Decrement(ref remainingCount) != 0)
          return;
        asyncContinuation((Exception) null);
      });
      lock (this._openNetworkSenders)
      {
        remainingCount = this._openNetworkSenders.Count;
        if (remainingCount == 0)
        {
          asyncContinuation((Exception) null);
        }
        else
        {
          foreach (NetworkSender openNetworkSender in this._openNetworkSenders)
            openNetworkSender.FlushAsync(continuation);
        }
      }
    }

    protected override void CloseTarget()
    {
      base.CloseTarget();
      lock (this._openNetworkSenders)
      {
        foreach (NetworkSender openNetworkSender in this._openNetworkSenders)
          openNetworkSender.Close((AsyncContinuation) (ex => { }));
        this._openNetworkSenders.Clear();
      }
    }

    protected override void Write(AsyncLogEventInfo logEvent)
    {
      string str = this.RenderLogEvent(this.Address, logEvent.LogEvent);
      InternalLogger.Trace<string, string>("NetworkTarget(Name={0}): Sending to address: '{1}'", this.Name, str);
      byte[] bytesToWrite = this.GetBytesToWrite(logEvent.LogEvent);
      if (this.KeepConnection)
      {
        LinkedListNode<NetworkSender> senderNode = this.GetCachedNetworkSender(str);
        this.ChunkedSend(senderNode.Value, bytesToWrite, (AsyncContinuation) (ex =>
        {
          if (ex != null)
          {
            InternalLogger.Error(ex, "NetworkTarget(Name={0}): Error when sending.", (object) this.Name);
            this.ReleaseCachedConnection(senderNode);
          }
          logEvent.Continuation(ex);
        }));
      }
      else
      {
        LinkedListNode<NetworkSender> linkedListNode;
        NetworkSender sender;
        lock (this._openNetworkSenders)
        {
          if (this._openNetworkSenders.Count >= this.MaxConnections && this.MaxConnections > 0)
          {
            switch (this.OnConnectionOverflow)
            {
              case NetworkTargetConnectionsOverflowAction.AllowNewConnnection:
                InternalLogger.Debug<string>("NetworkTarget(Name={0}): Too may connections, but this is allowed", this.Name);
                break;
              case NetworkTargetConnectionsOverflowAction.DiscardMessage:
                InternalLogger.Warn<string>("NetworkTarget(Name={0}): Discarding message otherwise to many connections.", this.Name);
                logEvent.Continuation((Exception) null);
                return;
              case NetworkTargetConnectionsOverflowAction.Block:
                while (this._openNetworkSenders.Count >= this.MaxConnections)
                {
                  InternalLogger.Debug<string>("NetworkTarget(Name={0}): Blocking networktarget otherwhise too many connections.", this.Name);
                  Monitor.Wait((object) this._openNetworkSenders);
                  InternalLogger.Trace<string>("NetworkTarget(Name={0}): Entered critical section.", this.Name);
                }
                InternalLogger.Trace<string>("NetworkTarget(Name={0}): Limit ok.", this.Name);
                break;
            }
          }
          sender = this.SenderFactory.Create(str, this.MaxQueueSize);
          sender.Initialize();
          linkedListNode = this._openNetworkSenders.AddLast(sender);
        }
        this.ChunkedSend(sender, bytesToWrite, (AsyncContinuation) (ex =>
        {
          lock (this._openNetworkSenders)
          {
            NetworkTarget.TryRemove<NetworkSender>(this._openNetworkSenders, linkedListNode);
            if (this.OnConnectionOverflow == NetworkTargetConnectionsOverflowAction.Block)
              Monitor.PulseAll((object) this._openNetworkSenders);
          }
          if (ex != null)
            InternalLogger.Error(ex, "NetworkTarget(Name={0}): Error when sending.", (object) this.Name);
          sender.Close((AsyncContinuation) (ex2 => { }));
          logEvent.Continuation(ex);
        }));
      }
    }

    private static bool TryRemove<T>(LinkedList<T> list, LinkedListNode<T> node)
    {
      if (node == null || list != node.List)
        return false;
      list.Remove(node);
      return true;
    }

    protected virtual byte[] GetBytesToWrite(LogEventInfo logEvent)
    {
      if (this.OptimizeBufferReuse)
      {
        object obj;
        if (!this.NewLine && logEvent.TryGetCachedLayoutValue(this.Layout, out obj))
        {
          InternalLogger.Trace<string, object>("NetworkTarget(Name={0}): Sending {1}", this.Name, obj);
          return this.Encoding.GetBytes(obj.ToString());
        }
        using (ReusableObjectCreator<StringBuilder>.LockOject lockOject1 = this.ReusableLayoutBuilder.Allocate())
        {
          this.Layout.RenderAppendBuilder(logEvent, lockOject1.Result);
          if (this.NewLine)
            lockOject1.Result.Append(this.LineEnding.NewLineCharacters);
          InternalLogger.Trace<string, int>("NetworkTarget(Name={0}): Sending {1} chars", this.Name, lockOject1.Result.Length);
          using (ReusableObjectCreator<char[]>.LockOject lockOject2 = this._reusableEncodingBuffer.Allocate())
          {
            if (lockOject1.Result.Length > lockOject2.Result.Length)
              return this.Encoding.GetBytes(lockOject1.Result.ToString());
            lockOject1.Result.CopyTo(0, lockOject2.Result, 0, lockOject1.Result.Length);
            return this.Encoding.GetBytes(lockOject2.Result, 0, lockOject1.Result.Length);
          }
        }
      }
      else
      {
        string s = this.Layout.Render(logEvent);
        InternalLogger.Trace<string, string>("NetworkTarget(Name={0}): Sending: {1}", this.Name, s);
        if (this.NewLine)
          s += this.LineEnding.NewLineCharacters;
        return this.Encoding.GetBytes(s);
      }
    }

    private LinkedListNode<NetworkSender> GetCachedNetworkSender(string address)
    {
      lock (this._currentSenderCache)
      {
        LinkedListNode<NetworkSender> cachedNetworkSender;
        if (this._currentSenderCache.TryGetValue(address, out cachedNetworkSender))
        {
          cachedNetworkSender.Value.CheckSocket();
          return cachedNetworkSender;
        }
        if (this._currentSenderCache.Count >= this.ConnectionCacheSize)
        {
          int num = int.MaxValue;
          LinkedListNode<NetworkSender> senderNode = (LinkedListNode<NetworkSender>) null;
          foreach (KeyValuePair<string, LinkedListNode<NetworkSender>> keyValuePair in this._currentSenderCache)
          {
            NetworkSender networkSender = keyValuePair.Value.Value;
            if (networkSender.LastSendTime < num)
            {
              num = networkSender.LastSendTime;
              senderNode = keyValuePair.Value;
            }
          }
          if (senderNode != null)
            this.ReleaseCachedConnection(senderNode);
        }
        NetworkSender networkSender1 = this.SenderFactory.Create(address, this.MaxQueueSize);
        networkSender1.Initialize();
        lock (this._openNetworkSenders)
          cachedNetworkSender = this._openNetworkSenders.AddLast(networkSender1);
        this._currentSenderCache.Add(address, cachedNetworkSender);
        return cachedNetworkSender;
      }
    }

    private void ReleaseCachedConnection(LinkedListNode<NetworkSender> senderNode)
    {
      lock (this._currentSenderCache)
      {
        NetworkSender networkSender = senderNode.Value;
        lock (this._openNetworkSenders)
        {
          if (NetworkTarget.TryRemove<NetworkSender>(this._openNetworkSenders, senderNode))
            networkSender.Close((AsyncContinuation) (ex => { }));
        }
        LinkedListNode<NetworkSender> linkedListNode;
        if (!this._currentSenderCache.TryGetValue(networkSender.Address, out linkedListNode) || senderNode != linkedListNode)
          return;
        this._currentSenderCache.Remove(networkSender.Address);
      }
    }

    private void ChunkedSend(NetworkSender sender, byte[] buffer, AsyncContinuation continuation)
    {
      int tosend = buffer.Length;
      if (tosend <= this.MaxMessageSize)
      {
        InternalLogger.Trace<int, int>("Sending chunk, position: {0}, length: {1}", 0, tosend);
        if (tosend <= 0)
          continuation((Exception) null);
        else
          sender.Send(buffer, 0, tosend, continuation);
      }
      else
      {
        int pos = 0;
        AsyncContinuation sendNextChunk = (AsyncContinuation) null;
        sendNextChunk = (AsyncContinuation) (ex =>
        {
          if (ex != null)
          {
            continuation(ex);
          }
          else
          {
            InternalLogger.Trace<string, int, int>("NetworkTarget(Name={0}): Sending chunk, position: {1}, length: {2}", this.Name, pos, tosend);
            if (tosend <= 0)
            {
              continuation((Exception) null);
            }
            else
            {
              int length = tosend;
              if (length > this.MaxMessageSize)
              {
                if (this.OnOverflow == NetworkTargetOverflowAction.Discard)
                {
                  InternalLogger.Trace<string>("NetworkTarget(Name={0}): Discard because chunksize > this.MaxMessageSize", this.Name);
                  continuation((Exception) null);
                  return;
                }
                if (this.OnOverflow == NetworkTargetOverflowAction.Error)
                {
                  continuation((Exception) new OverflowException(string.Format("Attempted to send a message larger than MaxMessageSize ({0}). Actual size was: {1}. Adjust OnOverflow and MaxMessageSize parameters accordingly.", (object) this.MaxMessageSize, (object) buffer.Length)));
                  return;
                }
                length = this.MaxMessageSize;
              }
              int offset = pos;
              tosend -= length;
              pos += length;
              sender.Send(buffer, offset, length, sendNextChunk);
            }
          }
        });
        sendNextChunk((Exception) null);
      }
    }
  }
}
