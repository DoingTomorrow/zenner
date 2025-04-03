// Decompiled with JetBrains decompiler
// Type: AsyncCom.AsyncFunctionsBase
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using NLog;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace AsyncCom
{
  public abstract class AsyncFunctionsBase
  {
    private static Logger logger = LogManager.GetLogger(nameof (AsyncFunctionsBase));
    protected AsyncFunctions MyAsyncFunctions;

    public virtual event System.EventHandler ConnectionLost;

    public virtual event System.EventHandler BatterieLow;

    protected AsyncFunctionsBase(AsyncFunctions MyParent) => this.MyAsyncFunctions = MyParent;

    public abstract bool Open();

    public abstract bool Close();

    public abstract void ClearCom();

    public abstract void ClearComErrors();

    public abstract bool ClearBreak();

    public abstract bool SetBreak();

    public abstract void TestComState();

    public abstract bool SetHandshakeState(HandshakeStates HandshakeState);

    public virtual bool TransmitString(string DataString) => false;

    public virtual bool ReceiveString(out string DataString)
    {
      DataString = (string) null;
      return false;
    }

    public abstract bool TransmitBlock(string DataString);

    public abstract bool TransmitBlock(ref ByteField DataBlock);

    public abstract bool SendBlock(ref ByteField DataBlock);

    public abstract void PureTransmit(byte[] byteList);

    public abstract bool ReceiveBlock(ref ByteField DataBlock, int MinByteNb, bool first);

    public abstract bool ReceiveBlock(ref ByteField DataBlock);

    public abstract bool TryReceiveBlock(out byte[] buffer);

    public abstract bool TryReceiveBlock(out byte[] buffer, int numberOfBytesToReceive);

    public abstract bool ReceiveLine(out string ReceivedData);

    public abstract bool ReceiveCRLF_Line(out string ReceivedData);

    public abstract bool ReceiveBlockToChar(ref ByteField DataBlock, byte EndChar);

    public abstract bool ReceiveLine(
      out string ReceivedData,
      char[] EndCharacters,
      bool GetEmpty_CRLF_Line);

    public abstract bool TransmitControlCommand(string strSendData);

    public abstract bool ReceiveControlBlock(
      out string ReceivedData,
      string startTag,
      string endTag);

    public abstract bool CallTransceiverDeviceFunction(
      TransceiverDeviceFunction function,
      object param1,
      object param2);

    public abstract object GetChannel();

    public abstract long InputBufferLength { get; }

    public void ResetLastTransmitEndTime()
    {
      this.MyAsyncFunctions.LastTransmitEndTime = SystemValues.DateTimeNow;
    }

    protected void GetReceiveBlockTiming(
      int NumberOfBytes,
      bool first,
      out DateTime EndTime,
      out int ActualTimeout)
    {
      EndTime = SystemValues.DateTimeNow;
      ActualTimeout = 0;
      if (first)
      {
        if (first && this.MyAsyncFunctions.IgnoreReceiveErrorsOnTransmitTime)
        {
          while (SystemValues.DateTimeNow < this.MyAsyncFunctions.LastTransmitEndTime)
          {
            if (!Util.Wait(50L, nameof (GetReceiveBlockTiming), (ICancelable) this.MyAsyncFunctions, AsyncFunctionsBase.logger))
              return;
          }
          this.MyAsyncFunctions.MyComType.ClearComErrors();
        }
        ActualTimeout = this.MyAsyncFunctions.RecTime_OffsetPerBlock + this.MyAsyncFunctions.RecTime_GlobalOffset + this.MyAsyncFunctions.AnswerOffsetTime + this.MyAsyncFunctions.RecTime_BeforFirstByte + (int) ((double) NumberOfBytes * (this.MyAsyncFunctions.ByteTime + this.MyAsyncFunctions.RecTime_OffsetPerByte) + 1.0);
        this.MyAsyncFunctions.EarliestTransmitTime = SystemValues.DateTimeNow.AddMilliseconds((double) (this.MyAsyncFunctions.RecTime_OffsetPerBlock + this.MyAsyncFunctions.RecTime_GlobalOffset + this.MyAsyncFunctions.AnswerOffsetTime + this.MyAsyncFunctions.RecTime_BeforFirstByte + (int) (280.0 * (this.MyAsyncFunctions.ByteTime + this.MyAsyncFunctions.RecTime_OffsetPerByte) + 1.0) + this.MyAsyncFunctions.WaitBeforeRepeatTime));
        this.MyAsyncFunctions.FirstCalculatedEarliestTransmitTime = this.MyAsyncFunctions.EarliestTransmitTime;
        EndTime = this.MyAsyncFunctions.LastTransmitEndTime.AddMilliseconds((double) ActualTimeout);
      }
      else
      {
        ActualTimeout = this.MyAsyncFunctions.RecTime_OffsetPerBlock + this.MyAsyncFunctions.RecTime_GlobalOffset + (int) ((double) NumberOfBytes * (this.MyAsyncFunctions.ByteTime + this.MyAsyncFunctions.RecTime_OffsetPerByte) + 1.0);
        EndTime = SystemValues.DateTimeNow.AddMilliseconds((double) ActualTimeout);
        this.MyAsyncFunctions.EarliestTransmitTime = SystemValues.DateTimeNow.AddMilliseconds((double) (ActualTimeout + this.MyAsyncFunctions.WaitBeforeRepeatTime));
      }
      if (!(SystemValues.DateTimeNow > EndTime))
        return;
      AsyncFunctionsBase.logger.Fatal("Timing was calculated at {0}. NumberOfBytes={1}, First={2}, EndTime={3}, ActualTimeout={4}", new object[5]
      {
        (object) SystemValues.DateTimeNow.ToString("HH:mm:ss.fff"),
        (object) NumberOfBytes,
        (object) first,
        (object) EndTime.ToString("HH:mm:ss.fff"),
        (object) ActualTimeout
      });
      AsyncFunctionsBase.logger.Fatal("Calculated timeout is in the past!");
    }

    public virtual bool GetCurrentInputBuffer(out byte[] buffer)
    {
      throw new NotImplementedException();
    }
  }
}
