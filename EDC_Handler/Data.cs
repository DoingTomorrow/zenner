// Decompiled with JetBrains decompiler
// Type: EDC_Handler.Data
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public sealed class Data
  {
    public string FullSerialNumber { get; set; }

    public byte[] AesKey { get; set; }

    public string AesKeyAsHexString
    {
      get => this.AesKey == null ? string.Empty : Util.ByteArrayToHexString(this.AesKey);
    }

    public string SapMaterialNumber { get; set; }

    public int MeterID { get; set; }
  }
}
