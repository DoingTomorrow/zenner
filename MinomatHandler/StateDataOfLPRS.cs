// Decompiled with JetBrains decompiler
// Type: MinomatHandler.StateDataOfLPRS
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class StateDataOfLPRS
  {
    private static Logger logger = LogManager.GetLogger(nameof (StateDataOfLPRS));

    public ushort SlotTme { get; set; }

    public byte FrameLength { get; set; }

    public StateOfLPSR Mode { get; set; }

    public byte AccessCounter { get; set; }

    public byte TableLength { get; set; }

    public ushort Reserved { get; set; }

    public static StateDataOfLPRS Parse(byte[] payload)
    {
      if (payload == null)
        throw new ArgumentNullException("Payload can not be null!");
      if (payload.Length != 10)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Wrong length of StateDataOfLPRS payload! Expected: 10 bytes, Buffer: {0}", (object) Util.ByteArrayToHexString(payload)));
        StateDataOfLPRS.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return new StateDataOfLPRS()
      {
        SlotTme = BitConverter.ToUInt16(new byte[2]
        {
          payload[2],
          payload[3]
        }, 0),
        FrameLength = payload[4],
        Mode = (StateOfLPSR) Enum.ToObject(typeof (StateOfLPSR), payload[5]),
        AccessCounter = payload[6],
        TableLength = payload[7],
        Reserved = BitConverter.ToUInt16(new byte[2]
        {
          payload[8],
          payload[9]
        }, 0)
      };
    }

    public override string ToString()
    {
      return string.Format("SlotTme: {0}, \nFrameLength: {1}, \nMode: {2}, \nAccessCounter: {3}, \nTableLength: {4}, \nReserved: {5}", (object) this.SlotTme, (object) this.FrameLength, (object) this.Mode, (object) this.AccessCounter, (object) this.TableLength, (object) this.Reserved);
    }
  }
}
