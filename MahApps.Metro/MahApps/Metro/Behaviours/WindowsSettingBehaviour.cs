// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Behaviours.WindowsSettingBehaviour
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Interactivity;

#nullable disable
namespace MahApps.Metro.Behaviours
{
  public class WindowsSettingBehaviour : Behavior<MetroWindow>
  {
    protected override void OnAttached()
    {
      if (this.AssociatedObject == null || !this.AssociatedObject.SaveWindowPosition)
        return;
      WindowSettings.SetSave((DependencyObject) this.AssociatedObject, this.AssociatedObject.GetWindowPlacementSettings());
    }
  }
}
