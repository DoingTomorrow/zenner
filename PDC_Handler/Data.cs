// Decompiled with JetBrains decompiler
// Type: PDC_Handler.Data
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
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
