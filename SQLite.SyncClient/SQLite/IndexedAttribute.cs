// Decompiled with JetBrains decompiler
// Type: SQLite.IndexedAttribute
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;

#nullable disable
namespace SQLite
{
  [AttributeUsage(AttributeTargets.Property)]
  public class IndexedAttribute : Attribute
  {
    public string Name { get; set; }

    public int Order { get; set; }

    public virtual bool Unique { get; set; }

    public IndexedAttribute()
    {
    }

    public IndexedAttribute(string name, int order)
    {
      this.Name = name;
      this.Order = order;
    }
  }
}
