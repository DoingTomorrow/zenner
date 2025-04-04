// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.IWindowPlacementSettings
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Native;

#nullable disable
namespace MahApps.Metro.Controls
{
  public interface IWindowPlacementSettings
  {
    WINDOWPLACEMENT? Placement { get; set; }

    void Reload();

    bool UpgradeSettings { get; set; }

    void Upgrade();

    void Save();
  }
}
