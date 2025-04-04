// Decompiled with JetBrains decompiler
// Type: SQLite.MaxLengthAttribute
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;

#nullable disable
namespace SQLite
{
  [AttributeUsage(AttributeTargets.Property)]
  public class MaxLengthAttribute : Attribute
  {
    public int Value { get; private set; }

    public MaxLengthAttribute(int length) => this.Value = length;
  }
}
