// Decompiled with JetBrains decompiler
// Type: MinomatHandler.Slave
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class Slave
  {
    private static Logger logger = LogManager.GetLogger(nameof (Slave));

    public ushort SlaveNodeID { get; set; }

    public uint MinolID { get; set; }

    public static List<Slave> Parse(byte[] payload)
    {
      if (payload == null)
        throw new ArgumentNullException("Payload can not be null!");
      if (payload.Length % 6 != 0)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Wrong length to parse slave list! Expected: mod 6, Buffer: {0}, Size: {1}", (object) Util.ByteArrayToHexString(payload), (object) payload.Length));
        Slave.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      List<Slave> slaveList = new List<Slave>();
      for (int startIndex = 0; startIndex < payload.Length; startIndex += 6)
        slaveList.Add(new Slave()
        {
          SlaveNodeID = BitConverter.ToUInt16(payload, startIndex),
          MinolID = BitConverter.ToUInt32(payload, startIndex + 2)
        });
      return slaveList;
    }
  }
}
