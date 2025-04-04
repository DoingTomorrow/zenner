// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.SQLite.TableAttribute
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.SQLite
{
  [AttributeUsage(AttributeTargets.Class)]
  public class TableAttribute : Attribute
  {
    public string Name { get; set; }

    public TableAttribute(string name) => this.Name = name;
  }
}
