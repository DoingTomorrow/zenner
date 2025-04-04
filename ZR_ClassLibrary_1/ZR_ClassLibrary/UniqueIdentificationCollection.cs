// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.UniqueIdentificationCollection
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Collections.Generic;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class UniqueIdentificationCollection : Dictionary<string, UniqueIdentification>
  {
    public event UniqueIdentificationCollection.EventHandler UniqueIdentificationAdded;

    public void Add(UniqueIdentification newType)
    {
      if (newType == null || string.IsNullOrEmpty(newType.Key) || this.ContainsKey(newType.Key) || string.IsNullOrEmpty(newType.ParameterListStringWithSeparator))
        return;
      this.Add(newType.Key, newType);
      if (this.UniqueIdentificationAdded == null)
        return;
      this.UniqueIdentificationAdded(newType);
    }

    public delegate void EventHandler(UniqueIdentification sender);
  }
}
