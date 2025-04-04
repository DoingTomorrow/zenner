// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SECOLUMNATTRIB
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  [Flags]
  internal enum SECOLUMNATTRIB
  {
    NAME = 1,
    IDCOL = 2,
    IDRANGE = 4,
    WRITEABLE = 16, // 0x00000010
    NULLABLE = 32, // 0x00000020
    TYPE = 64, // 0x00000040
    IDENTITY = 128, // 0x00000080
    IDNEXT = 256, // 0x00000100
    SYSCOL = 512, // 0x00000200
    IDRANGE1 = 1024, // 0x00000400
    IDRANGE2 = 2048, // 0x00000800
  }
}
