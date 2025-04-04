// Decompiled with JetBrains decompiler
// Type: MinomatHandler.RadioChannel
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public class RadioChannel
  {
    private static Logger logger = LogManager.GetLogger(nameof (RadioChannel));

    public short? ID { get; set; }

    public string Description { get; set; }

    public RadioChannelError Error { get; set; }

    public RadioChannel() => this.Error = RadioChannelError.None;

    public RadioChannel(short id)
      : this()
    {
      this.ID = new short?(id);
    }

    public static RadioChannel Parse(byte[] payload)
    {
      if (payload == null)
        throw new ArgumentNullException("SCGi payload can not be null!");
      if (payload.Length < 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Wrong length of SCGi payload! Expected: >=4, Buffer: {0}", (object) Util.ByteArrayToHexString(payload)));
        RadioChannel.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      short int16 = BitConverter.ToInt16(new byte[2]
      {
        payload[2],
        payload[3]
      }, 0);
      RadioChannel radioChannel = new RadioChannel();
      if (int16 >= (short) 0)
      {
        radioChannel.ID = new short?(int16);
        if (payload.Length > 4)
          radioChannel.Description = Encoding.ASCII.GetString(payload, 4, payload.Length - 4).TrimEnd(new char[1]);
      }
      else
        radioChannel.Error = (RadioChannelError) Enum.ToObject(typeof (RadioChannelError), int16);
      return radioChannel;
    }

    public override string ToString()
    {
      return this.ID.HasValue && this.Error == RadioChannelError.None ? string.Format("ID: {0}, Desc: {1}", (object) this.ID, (object) this.Description) : string.Format("Error: {0}", (object) this.Error);
    }
  }
}
