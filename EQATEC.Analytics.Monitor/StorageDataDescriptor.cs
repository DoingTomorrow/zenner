// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.StorageDataDescriptor
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  public struct StorageDataDescriptor
  {
    internal StorageDataDescriptor(string productId, StorageDataType type)
      : this()
    {
      this.ProductId = productId;
      this.DataType = type;
    }

    public string ProductId { get; private set; }

    public StorageDataType DataType { get; private set; }

    public override string ToString()
    {
      return string.Format("DataType={0} - {1}", (object) this.DataType.ToString(), (object) this.ProductId);
    }
  }
}
