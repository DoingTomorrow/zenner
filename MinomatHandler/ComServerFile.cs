// Decompiled with JetBrains decompiler
// Type: MinomatHandler.ComServerFile
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class ComServerFile
  {
    private static Logger logger = LogManager.GetLogger(nameof (ComServerFile));

    public bool IsASCII { get; private set; }

    public byte[] File { get; private set; }

    public static ComServerFile Parse(SCGiFrame response, bool isASCII)
    {
      List<byte> byteList = new List<byte>();
      foreach (SCGiPacket scGiPacket in (List<SCGiPacket>) response)
      {
        int index = 0;
        if (scGiPacket.IsFirstPacketOfFrame)
          index = 2;
        byte count = scGiPacket.Payload[index];
        if ((int) count > scGiPacket.Payload.Length - index + 1)
        {
          ComServerFile.logger.Error<byte, string>("Wrong length byte received! Value: {0}, Buffer: {1}", count, Util.ByteArrayToHexString(scGiPacket.Payload));
          return (ComServerFile) null;
        }
        byte[] numArray = new byte[(int) count];
        Buffer.BlockCopy((Array) scGiPacket.Payload, index + 1, (Array) numArray, 0, (int) count);
        byteList.AddRange((IEnumerable<byte>) numArray);
      }
      return new ComServerFile()
      {
        IsASCII = isASCII,
        File = byteList.ToArray()
      };
    }

    public override string ToString()
    {
      return this.IsASCII ? Encoding.ASCII.GetString(this.File, 0, this.File.Length).TrimEnd(new char[1]) : Util.ByteArrayToHexString(this.File);
    }
  }
}
