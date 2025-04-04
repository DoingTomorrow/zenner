// Decompiled with JetBrains decompiler
// Type: StartupLib.TestbenchStateItem
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System;

#nullable disable
namespace StartupLib
{
  public class TestbenchStateItem
  {
    public InstallationData.InstallationChangeLogRow DbRow;

    public DateTime ChangeTime { get; set; }

    public string BasicState { get; set; }
  }
}
