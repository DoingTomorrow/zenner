// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MeasurementDataHeader
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class MeasurementDataHeader
  {
    private static Logger logger = LogManager.GetLogger(nameof (MeasurementDataHeader));
    public const int SIZE = 12;

    public uint ID { get; set; }

    public MeasurementDataType Type { get; set; }

    public byte Flags { get; set; }

    public DateTime TimepointOfFirstValue { get; set; }

    public int SizeOfValue { get; set; }

    public int CountOfExpectedValues { get; set; }

    public int CountOfExpectedBytes
    {
      get
      {
        int countOfExpectedBytes = this.CountOfExpectedValues * this.SizeOfValue;
        if (this.Type == MeasurementDataType.MonthAndHalfMonth)
          countOfExpectedBytes *= 2;
        return countOfExpectedBytes;
      }
    }

    public static MeasurementDataHeader Parse(byte[] payload, int offset)
    {
      if (payload == null)
        throw new ArgumentNullException("Payload can not be null!");
      if (payload.Length < 12 + offset)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse the MeasurementDataHeader! Payload can not be less as {0} bytes. Payload: {1}", (object) (12 + offset), (object) Util.ByteArrayToHexString(payload)));
        MeasurementDataHeader.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      uint uint32 = BitConverter.ToUInt32(payload, offset);
      byte num1 = payload[4 + offset];
      if (!Enum.IsDefined(typeof (MeasurementDataType), (object) num1))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse the MeasurementDataHeader! Unknown MeasurementDataType received! Value: 0x{0}. Payload: {1}", (object) num1.ToString("X2"), (object) Util.ByteArrayToHexString(payload)));
        MeasurementDataHeader.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      MeasurementDataType measurementDataType = (MeasurementDataType) Enum.ToObject(typeof (MeasurementDataType), num1);
      byte num2 = payload[5 + offset];
      int num3 = ((int) num2 & 1) == 1 ? 4 : 2;
      byte num4 = payload[6 + offset];
      byte num5 = payload[7 + offset];
      byte num6 = payload[8 + offset];
      byte num7 = payload[9 + offset];
      int num8 = 2000 + (int) num4;
      int num9 = 0;
      if (num7 != byte.MaxValue)
        num9 = (int) num7 * 15;
      if (num6 == (byte) 0)
        num6 = (byte) 1;
      if (num5 == (byte) 0)
        num5 = (byte) 1;
      DateTime dateTime;
      if (!Util.TryParseToDateTime(string.Format("{0:00}-{1:00}-{2:00}T00:{3:00}:00", (object) num8, (object) num5, (object) num6, (object) 0), out dateTime))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse the MeasurementDataHeader! Invalid timepoint received! Year: {0}, Month: {1}, Day: {2}, Quarter: {3}, Offset: {4}, Payload: {5}", (object) num4, (object) num5, (object) num6, (object) num7, (object) offset, (object) Util.ByteArrayToHexString(payload)));
        MeasurementDataHeader.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      dateTime = dateTime.AddMinutes((double) num9);
      ushort uint16 = BitConverter.ToUInt16(payload, 10 + offset);
      return new MeasurementDataHeader()
      {
        ID = uint32,
        Type = measurementDataType,
        Flags = num2,
        SizeOfValue = num3,
        TimepointOfFirstValue = dateTime,
        CountOfExpectedValues = (int) uint16
      };
    }
  }
}
