// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionEventType
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public enum SQLiteConnectionEventType
  {
    Invalid = -1, // 0xFFFFFFFF
    Unknown = 0,
    Opening = 1,
    ConnectionString = 2,
    Opened = 3,
    ChangeDatabase = 4,
    NewTransaction = 5,
    EnlistTransaction = 6,
    NewCommand = 7,
    NewDataReader = 8,
    NewCriticalHandle = 9,
    Closing = 10, // 0x0000000A
    Closed = 11, // 0x0000000B
    DisposingCommand = 12, // 0x0000000C
    DisposingDataReader = 13, // 0x0000000D
    ClosingDataReader = 14, // 0x0000000E
    OpenedFromPool = 15, // 0x0000000F
    ClosedToPool = 16, // 0x00000010
  }
}
