// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MessUnitMetadata
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  [Serializable]
  public sealed class MessUnitMetadata
  {
    private static Logger logger = LogManager.GetLogger(nameof (MessUnitMetadata));

    public uint ID { get; set; }

    public DateTime Timepoint { get; set; }

    public DateTime TimepointSlave { get; set; }

    public MessUnitState State { get; set; }

    public DateTime? TimepointError { get; set; }

    public static MessUnitMetadata Parse(byte[] payload)
    {
      if (payload == null)
        throw new ArgumentNullException("Payload can not be null!");
      if (payload.Length != 18)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse the MessUnitMetadata! Payload can not be less as 18 bytes. Payload: {0}", (object) Util.ByteArrayToHexString(payload)));
        MessUnitMetadata.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      MessUnitMetadata messUnitMetadata = new MessUnitMetadata();
      messUnitMetadata.ID = BitConverter.ToUInt32(payload, 2);
      int hour = (int) payload[7];
      int minute = (int) payload[6];
      byte num1 = payload[9];
      byte num2 = payload[8];
      int day1 = (int) num2 & 31;
      int month1 = (int) num1 & 15;
      int num3 = ((int) num2 & 224) >> 5 | ((int) num1 & 240) >> 1;
      int year1 = num3 < 80 ? num3 + 2000 : num3 + 1900;
      try
      {
        if (!Util.IsValidTimePoint(year1, month1, day1, hour, minute))
        {
          MessUnitMetadata.logger.Error("Device has invalid date and time! Buffer: {0}", Util.ByteArrayToHexString(payload));
          messUnitMetadata.Timepoint = DateTime.MinValue;
        }
        else
          messUnitMetadata.Timepoint = new DateTime(year1, month1, day1, hour, minute, 0);
      }
      catch (Exception ex)
      {
        MessUnitMetadata.logger.Error<string, string>("Device has invalid date and time! Error: {0}, Buffer: {1}", ex.Message, Util.ByteArrayToHexString(payload));
        messUnitMetadata.Timepoint = DateTime.MinValue;
      }
      byte[] numArray = new byte[4]
      {
        payload[13],
        payload[12],
        payload[11],
        payload[10]
      };
      int num4 = numArray[0] != byte.MaxValue || numArray[1] != byte.MaxValue || numArray[2] != byte.MaxValue ? 0 : (numArray[3] == byte.MaxValue ? 1 : 0);
      messUnitMetadata.TimepointSlave = num4 == 0 ? new DateTime(2001, 1, 1).AddSeconds((double) BitConverter.ToUInt32(numArray, 0)) : new DateTime(2001, 1, 1);
      messUnitMetadata.State = MessUnitState.Parse(payload[14]);
      if (BitConverter.ToUInt16(payload, 16) == ushort.MaxValue)
      {
        messUnitMetadata.TimepointError = new DateTime?();
      }
      else
      {
        byte num5 = payload[17];
        byte num6 = payload[16];
        int day2 = (int) num6 & 31;
        int month2 = (int) num5 & 15;
        int num7 = ((int) num6 & 224) >> 5 | ((int) num5 & 240) >> 1;
        int year2 = year1 < 80 ? num7 + 2000 : num7 + 1900;
        try
        {
          messUnitMetadata.TimepointError = new DateTime?(new DateTime(year2, month2, day2, 0, 0, 0));
        }
        catch (Exception ex)
        {
          MessUnitMetadata.logger.Error<string, string>("Device has invalid error date and time! Error: {0}, Buffer: {1}", ex.Message, Util.ByteArrayToHexString(payload));
          messUnitMetadata.TimepointError = new DateTime?(DateTime.MinValue);
        }
      }
      return messUnitMetadata;
    }
  }
}
