// Decompiled with JetBrains decompiler
// Type: StartupLib.UserInterfaceServices
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace StartupLib
{
  public static class UserInterfaceServices
  {
    public static int AddDefaultMenu(
      MenuItem componentsMenuItem,
      RoutedEventHandler componentsClick)
    {
      if (PlugInLoader.IsWindowEnabled("GMM"))
      {
        MenuItem newItem1 = new MenuItem();
        newItem1.Header = (object) "StartWindow";
        newItem1.Click += new RoutedEventHandler(componentsClick.Invoke);
        componentsMenuItem.Items.Add((object) newItem1);
        MenuItem newItem2 = new MenuItem();
        newItem2.Header = (object) "GlobalMeterManager";
        newItem2.Click += new RoutedEventHandler(componentsClick.Invoke);
        componentsMenuItem.Items.Add((object) newItem2);
        MenuItem newItem3 = new MenuItem();
        newItem3.Header = (object) "Back";
        newItem3.Click += new RoutedEventHandler(componentsClick.Invoke);
        componentsMenuItem.Items.Add((object) newItem3);
        MenuItem newItem4 = new MenuItem();
        newItem4.Header = (object) "Quit";
        newItem4.Click += new RoutedEventHandler(componentsClick.Invoke);
        componentsMenuItem.Items.Add((object) newItem4);
      }
      return componentsMenuItem.Items.Count;
    }

    public static void AddMenuItem(
      string componentName,
      MenuItem componentsMenuItem,
      RoutedEventHandler componentsClick)
    {
      if (!UserManager.CheckPermission("Plugin\\" + componentName))
        return;
      MenuItem newItem = new MenuItem();
      newItem.Header = (object) componentName;
      newItem.Click += new RoutedEventHandler(componentsClick.Invoke);
      componentsMenuItem.Items.Add((object) newItem);
    }
  }
}
