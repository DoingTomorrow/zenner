// Decompiled with JetBrains decompiler
// Type: SQLitePCL.CurrentPlatform
// Assembly: SQLitePCL.Ext, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 28DC4D07-0E35-45C1-8EF3-CED69BFBD581
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.Ext.dll

#nullable disable
namespace SQLitePCL
{
  internal sealed class CurrentPlatform : IPlatform
  {
    IPlatformMarshal IPlatform.PlatformMarshal => PlatformMarshal.Instance;

    IPlatformStorage IPlatform.PlatformStorage => PlatformStorage.Instance;

    ISQLite3Provider IPlatform.SQLite3Provider => SQLite3Provider.Instance;
  }
}
