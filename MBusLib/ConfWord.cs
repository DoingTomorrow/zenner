// Decompiled with JetBrains decompiler
// Type: MBusLib.ConfWord
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MBusLib
{
  [Serializable]
  public class ConfWord
  {
    public ushort ConfigurationField { get; set; }

    public byte? ConfigurationFieldExtension1 { get; set; }

    public byte? ConfigurationFieldExtension2 { get; set; }

    public int EncryptionMode => ((int) this.ConfigurationField & 3840) >> 8;

    public byte CountOfBlocks
    {
      get => (byte) (((int) this.ConfigurationField & 240) >> 4);
      set
      {
        this.ConfigurationField &= (ushort) 65295;
        this.ConfigurationField |= (ushort) (((int) value & 15) << 4);
      }
    }

    public int Size => (int) this.CountOfBlocks * 16;

    public bool? Synchronous
    {
      get => new bool?(Convert.ToBoolean(((int) this.ConfigurationField & 8192) >> 13));
    }

    public override string ToString()
    {
      return string.Format("({0:X2}h) Encryption mode: {1}", (object) this.ConfigurationField, (object) this.EncryptionMode);
    }

    internal static ConfWord Decode(byte[] buffer, ref int offset)
    {
      ConfWord confWord = new ConfWord();
      confWord.ConfigurationField = (ushort) buffer[offset++];
      confWord.ConfigurationField |= (ushort) ((uint) buffer[offset++] << 8);
      if (confWord.EncryptionMode == 7 || confWord.EncryptionMode == 10 || confWord.EncryptionMode == 13)
        confWord.ConfigurationFieldExtension1 = new byte?(buffer[offset++]);
      if (confWord.EncryptionMode == 10)
        confWord.ConfigurationFieldExtension2 = new byte?(buffer[offset++]);
      return confWord;
    }

    public byte[] GetBytes()
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.ConfigurationField));
      if (this.ConfigurationFieldExtension1.HasValue)
        byteList.Add(this.ConfigurationFieldExtension1.Value);
      if (this.ConfigurationFieldExtension2.HasValue)
        byteList.Add(this.ConfigurationFieldExtension2.Value);
      return byteList.ToArray();
    }
  }
}
