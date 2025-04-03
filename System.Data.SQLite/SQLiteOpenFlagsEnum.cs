// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteOpenFlagsEnum
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  [Flags]
  internal enum SQLiteOpenFlagsEnum
  {
    None = 0,
    ReadOnly = 1,
    ReadWrite = 2,
    Create = 4,
    Uri = 64, // 0x00000040
    SharedCache = 16777216, // 0x01000000
    Default = Create | ReadWrite, // 0x00000006
  }
}
