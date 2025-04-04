// Decompiled with JetBrains decompiler
// Type: WpfKb.Controls.OnScreenKeypad
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System.Collections.ObjectModel;
using WindowsInput;
using WpfKb.LogicalKeys;

#nullable disable
namespace WpfKb.Controls
{
  public class OnScreenKeypad : UniformOnScreenKeyboard
  {
    public OnScreenKeypad()
    {
      ObservableCollection<OnScreenKey> observableCollection = new ObservableCollection<OnScreenKey>();
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 0,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.VK_7, "7")
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 1,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.VK_8, "8")
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 2,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.VK_9, "9")
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 0,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.VK_4, "4")
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 1,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.VK_5, "5")
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 2,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.VK_6, "6")
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 0,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.VK_1, "1")
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 1,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.VK_2, "2")
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 2,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.VK_3, "3")
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 0,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.DELETE, "Clear")
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 1,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.VK_0, "0")
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 2,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.BACK, "Del")
      });
      this.Keys = observableCollection;
    }
  }
}
