// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.NfcFrame
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using NLog;
using System;
using System.Security.Cryptography;
using System.Text;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib.NFC
{
  public class NfcFrame
  {
    private static Logger Base_NfcFrameLogger = LogManager.GetLogger(nameof (NfcFrameLogger));
    internal ChannelLogger NfcFrameLogger;
    public NfcCommands NfcCommand;
    public string ProgressParameter = (string) null;
    public object Tag;
    private ushort? CrcInitValue;
    private static byte[] InitVector = new byte[6]
    {
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32
    };

    public byte[] NfcRequestFrame { get; private set; }

    public byte[] NfcResponseFrame { get; set; }

    public byte[] EncryptedNfcFrame { get; private set; }

    public NfcFrame(
      NfcCommands nfcCommand,
      string readingChannelIdentification,
      ushort? crcInitValue = null)
    {
      this.CrcInitValue = crcInitValue;
      this.NfcFrameLogger = new ChannelLogger(NfcFrame.Base_NfcFrameLogger, readingChannelIdentification);
      this.NfcCommand = nfcCommand;
      this.NfcRequestFrame = new byte[6];
      this.NfcRequestFrame[0] = (byte) (this.NfcRequestFrame.Length - 3);
      this.NfcRequestFrame[1] = (byte) nfcCommand;
      this.NfcRequestFrame[2] = (byte) 90;
      this.NfcRequestFrame[3] = (byte) 165;
      this.AddCrc();
    }

    public NfcFrame(
      NfcCommands nfcCommand,
      byte[] frameData,
      string readingChannelIdentification,
      ushort? crcInitValue = null)
    {
      this.CrcInitValue = crcInitValue;
      this.NfcFrameLogger = new ChannelLogger(NfcFrame.Base_NfcFrameLogger, readingChannelIdentification);
      this.NfcCommand = nfcCommand;
      if (frameData.Length < 60)
      {
        this.NfcRequestFrame = new byte[frameData.Length + 4];
        this.NfcRequestFrame[0] = (byte) (frameData.Length + 1);
        this.NfcRequestFrame[1] = (byte) nfcCommand;
        Buffer.BlockCopy((Array) frameData, 0, (Array) this.NfcRequestFrame, 2, frameData.Length);
      }
      else
      {
        this.NfcRequestFrame = new byte[frameData.Length + 6];
        ushort num = (ushort) (frameData.Length + 1);
        this.NfcRequestFrame[0] = byte.MaxValue;
        this.NfcRequestFrame[1] = (byte) num;
        this.NfcRequestFrame[2] = (byte) ((uint) num >> 8);
        this.NfcRequestFrame[3] = (byte) nfcCommand;
        Buffer.BlockCopy((Array) frameData, 0, (Array) this.NfcRequestFrame, 4, frameData.Length);
      }
      this.AddCrc();
    }

    public NfcFrame(byte[] poorFrame, string readingChannelIdentification)
    {
      this.NfcFrameLogger = new ChannelLogger(NfcFrame.Base_NfcFrameLogger, readingChannelIdentification);
      this.NfcRequestFrame = poorFrame;
    }

    public NfcFrame(byte[] encryptedFrame, byte[] aesKey, string readingChannelIdentification)
    {
      this.NfcFrameLogger = new ChannelLogger(NfcFrame.Base_NfcFrameLogger, readingChannelIdentification);
      this.EncryptedNfcFrame = encryptedFrame;
      this.DecryptFrame(aesKey);
    }

    private void DecryptFrame(byte[] aesKey)
    {
      using (ICryptoTransform decryptor = this.GetEncryptionObject(aesKey).CreateDecryptor())
        this.NfcRequestFrame = decryptor.TransformFinalBlock(this.EncryptedNfcFrame, 0, this.EncryptedNfcFrame.Length);
    }

    private void EncryptFrame(byte[] aesKey)
    {
      this.GetEncryptionObject(aesKey).CreateEncryptor().TransformFinalBlock(this.NfcRequestFrame, 0, this.NfcRequestFrame.Length);
    }

    private RijndaelManaged GetEncryptionObject(byte[] aesKey)
    {
      RijndaelManaged encryptionObject = new RijndaelManaged();
      encryptionObject.Mode = CipherMode.CBC;
      encryptionObject.IV = NfcFrame.InitVector;
      encryptionObject.Key = aesKey;
      encryptionObject.Padding = PaddingMode.Zeros;
      return encryptionObject;
    }

    private void AddCrc()
    {
      ushort crc = NfcFrame.createCRC(this.NfcRequestFrame, this.CrcInitValue);
      this.NfcRequestFrame[this.NfcRequestFrame.Length - 2] = (byte) crc;
      this.NfcRequestFrame[this.NfcRequestFrame.Length - 1] = (byte) ((uint) crc >> 8);
    }

    public void IsResponseCrcOk()
    {
      ushort crc = NfcFrame.createCRC(this.NfcResponseFrame, this.CrcInitValue);
      if ((int) this.NfcResponseFrame[this.NfcResponseFrame.Length - 2] != (int) (byte) crc || (int) this.NfcResponseFrame[this.NfcResponseFrame.Length - 1] != (int) (byte) ((uint) crc >> 8))
      {
        this.NfcFrameLogger.Error("NFC_CRC error: " + Util.ByteArrayToHexStringFormated(this.NfcResponseFrame));
        throw new NfcFrameException("NFC_CRC Error: " + Util.ByteArrayToHexStringFormated(this.NfcResponseFrame));
      }
      this.NfcFrameLogger.Trace("CRC OK!");
    }

    public static ushort createCRC(byte[] frameBuffer, ushort? crcInitValue = null)
    {
      return crcInitValue.HasValue ? HandlerLib.CRC.CRC_CCITT(frameBuffer, (ushort) 0, (ushort) (frameBuffer.Length - 2), crcInitValue.Value) : HandlerLib.CRC.CRC_CCITT(frameBuffer, (ushort) 0, (ushort) (frameBuffer.Length - 2));
    }

    public void IsResponseErrorMsg()
    {
      byte num = this.NfcResponseFrame[0] == byte.MaxValue ? this.NfcResponseFrame[3] : this.NfcResponseFrame[1];
      if (this.NfcResponseFrame[0] == byte.MaxValue || num != (byte) 254)
        return;
      this.NfcFrameLogger.Error("NFC_Error Frame: " + Util.ByteArrayToHexStringFormated(this.NfcResponseFrame));
      this.ThrowErrorException(this.NfcResponseFrame);
    }

    private void ThrowErrorException(byte[] frameBuffer)
    {
      bool flag = true;
      StringBuilder stringBuilder = new StringBuilder();
      if (frameBuffer[2] == byte.MaxValue)
      {
        stringBuilder.Append("SubUnit_Device error: \r\n");
        stringBuilder.Append(frameBuffer[3].ToString("X02") + " -> Device Command: " + frameBuffer[3].ToString("x") + "\r\n");
        if (frameBuffer[4] < (byte) 128)
          stringBuilder.Append(frameBuffer[4].ToString("X02") + " -> " + ((NFC_DEVICE_ERROR) frameBuffer[4]).ToString() + "r\n");
        else
          stringBuilder.Append(frameBuffer[4].ToString("X02") + " -> " + ((CR95_RESULTCODE) frameBuffer[4]).ToString() + "\r\n");
      }
      else if (((int) frameBuffer[2] & 128) == 128)
      {
        frameBuffer[2] = (byte) ((uint) frameBuffer[2] & 4294967167U);
        stringBuilder.Append("Nfc_Device error: \r\n");
        stringBuilder.Append(frameBuffer[2].ToString("X02") + " -> " + ((NfcCommands) frameBuffer[2]).ToString() + "\r\n");
        stringBuilder.Append(frameBuffer[3].ToString("X02") + " -> Nfc_Version: " + frameBuffer[3].ToString() + "\r\n");
        stringBuilder.Append(frameBuffer[4].ToString("X02") + " -> " + ((NFC_DEVICE_ERROR) frameBuffer[4]).ToString() + "\r\n");
        NFC_DEVICE_ERROR nfcDeviceError = (NFC_DEVICE_ERROR) frameBuffer[4];
        flag = false;
        switch (nfcDeviceError)
        {
          case NFC_DEVICE_ERROR.NFC_ERR_CMD:
            stringBuilder.Insert(0, "The command is not supported by this firmware" + Environment.NewLine + Environment.NewLine);
            break;
          case NFC_DEVICE_ERROR.NFC_ERR_WRTPRM:
            stringBuilder.Insert(0, "Write access not allowed" + Environment.NewLine + Environment.NewLine);
            break;
          case NFC_DEVICE_ERROR.NFC_ERR_ISPROTECTED:
            stringBuilder.Insert(0, "Write access to protected area" + Environment.NewLine + Environment.NewLine);
            break;
          default:
            flag = true;
            break;
        }
      }
      else
      {
        stringBuilder.Append("Communication error: \r\n");
        stringBuilder.Append(frameBuffer[2].ToString("X02") + " -> " + ((NfcCommands) frameBuffer[2]).ToString() + "\r\n");
        stringBuilder.Append(frameBuffer[3].ToString("X02") + " -> " + ((NFC_TRANSMISSION_STATE) frameBuffer[3]).ToString() + "\r\n");
        stringBuilder.Append(frameBuffer[4].ToString("X02") + " -> " + ((CR95_RESULTCODE) frameBuffer[4]).ToString() + "\r\n");
        if (frameBuffer[4] == (byte) 144)
        {
          if (frameBuffer[5] != (byte) 4)
          {
            byte num = frameBuffer[frameBuffer.Length - 5];
            stringBuilder.Append(num.ToString("X02") + " -> ");
            if (((int) num & 128) == 128)
              stringBuilder.Append(CR95_RESULTCODE_90.RF_ERROR_COLLISION.ToString() + "\r\n");
            if (((int) num & 32) == 32)
              stringBuilder.Append(CR95_RESULTCODE_90.RF_ERROR_CRC.ToString() + "\r\n");
            if (((int) num & 16) == 16)
              stringBuilder.Append(CR95_RESULTCODE_90.RF_ERROR_PARITY.ToString() + "\r\n");
          }
          else
          {
            stringBuilder.Append(frameBuffer[6].ToString("X02"));
            if (frameBuffer[6] == (byte) 10)
              stringBuilder.Append(" -> ACK \r\n");
            else
              stringBuilder.Append(" -> NACK \r\n");
          }
        }
      }
      stringBuilder.Append(Util.ByteArrayToHexString(frameBuffer) + "\r\n");
      this.NfcFrameLogger.Error(stringBuilder.ToString());
      if (flag)
        throw new NfcFrameException(stringBuilder.ToString());
      throw new DeviceMessageException(stringBuilder.ToString());
    }

    public static byte[] GetFrameData(byte[] frame)
    {
      ushort frameLength = NfcFrame.GetFrameLength(frame);
      byte[] dst = new byte[(int) frameLength];
      if (frame[0] == byte.MaxValue)
        Buffer.BlockCopy((Array) frame, 3, (Array) dst, 0, (int) frameLength);
      else
        Buffer.BlockCopy((Array) frame, 1, (Array) dst, 0, (int) frameLength);
      return dst;
    }

    public static ushort GetFrameLength(byte[] frame)
    {
      return frame[0] != byte.MaxValue ? (ushort) frame[0] : (ushort) ((uint) frame[1] + ((uint) frame[2] << 8));
    }
  }
}
