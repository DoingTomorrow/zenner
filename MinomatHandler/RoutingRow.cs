// Decompiled with JetBrains decompiler
// Type: MinomatHandler.RoutingRow
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class RoutingRow
  {
    private static Logger logger = LogManager.GetLogger(nameof (RoutingRow));

    public ushort NodeId { get; set; }

    public ushort ParentNodeId { get; set; }

    public byte HopCount { get; set; }

    public byte RSSI { get; set; }

    public int RSSI_dBm => Util.RssiToRssi_dBm(this.RSSI);

    public static RoutingRow Parse(byte[] payload)
    {
      if (payload == null)
        throw new ArgumentNullException("Payload can not be null!");
      if (payload.Length != 6)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Wrong length of RoutingRow by routing table! Expected: 6 bytes, Buffer: {0}", (object) Util.ByteArrayToHexString(payload)));
        RoutingRow.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return new RoutingRow()
      {
        NodeId = BitConverter.ToUInt16(new byte[2]
        {
          payload[0],
          payload[1]
        }, 0),
        ParentNodeId = BitConverter.ToUInt16(new byte[2]
        {
          payload[2],
          payload[3]
        }, 0),
        HopCount = payload[4],
        RSSI = payload[5]
      };
    }
  }
}
