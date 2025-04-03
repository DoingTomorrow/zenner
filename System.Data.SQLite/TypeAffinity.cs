// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.TypeAffinity
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public enum TypeAffinity
  {
    Uninitialized = 0,
    Int64 = 1,
    Double = 2,
    Text = 3,
    Blob = 4,
    Null = 5,
    DateTime = 10, // 0x0000000A
    None = 11, // 0x0000000B
  }
}
