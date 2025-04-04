// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Storage.VoidStorage
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

#nullable disable
namespace EQATEC.Analytics.Monitor.Storage
{
  internal class VoidStorage : IStorage
  {
    public void Save(StorageDataDescriptor descriptor, byte[] data)
    {
    }

    public byte[] Load(StorageDataDescriptor descriptor) => (byte[]) null;

    public byte[] ReadAbandonedSessionData(string productId) => (byte[]) null;
  }
}
