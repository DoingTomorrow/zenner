// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndexConstraintOp
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public enum SQLiteIndexConstraintOp : byte
  {
    EqualTo = 2,
    GreaterThan = 4,
    LessThanOrEqualTo = 8,
    LessThan = 16, // 0x10
    GreaterThanOrEqualTo = 32, // 0x20
    Match = 64, // 0x40
    Like = 65, // 0x41
    Glob = 66, // 0x42
    Regexp = 67, // 0x43
  }
}
