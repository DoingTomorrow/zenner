// Decompiled with JetBrains decompiler
// Type: WpfKb.Controls.OnScreenKeyboard
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput;
using WpfKb.LogicalKeys;

#nullable disable
namespace WpfKb.Controls
{
  public class OnScreenKeyboard : Grid
  {
    public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register(nameof (AreAnimationsEnabled), typeof (bool), typeof (OnScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(OnScreenKeyboard.OnAreAnimationsEnabledPropertyChanged)));
    private ObservableCollection<OnScreenKeyboardSection> _sections;
    private List<ModifierKeyBase> _modifierKeys;
    private List<ILogicalKey> _allLogicalKeys;
    private List<OnScreenKey> _allOnScreenKeys;

    public bool AreAnimationsEnabled
    {
      get => (bool) this.GetValue(OnScreenKeyboard.AreAnimationsEnabledProperty);
      set => this.SetValue(OnScreenKeyboard.AreAnimationsEnabledProperty, (object) value);
    }

    private static void OnAreAnimationsEnabledPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((OnScreenKeyboard) d)._allOnScreenKeys.ToList<OnScreenKey>().ForEach((Action<OnScreenKey>) (x => x.AreAnimationsEnabled = (bool) e.NewValue));
    }

    public override void BeginInit()
    {
      this.SetValue(FocusManager.IsFocusScopeProperty, (object) true);
      this._modifierKeys = new List<ModifierKeyBase>();
      this._allLogicalKeys = new List<ILogicalKey>();
      this._allOnScreenKeys = new List<OnScreenKey>();
      this._sections = new ObservableCollection<OnScreenKeyboardSection>();
      OnScreenKeyboardSection element = new OnScreenKeyboardSection();
      ObservableCollection<OnScreenKey> observableCollection = new ObservableCollection<OnScreenKey>();
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 0,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.OEM_3, (IList<string>) new List<string>()
        {
          "`",
          "~"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 1,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.VK_1, (IList<string>) new List<string>()
        {
          "1",
          "!"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 2,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.VK_2, (IList<string>) new List<string>()
        {
          "2",
          "@"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 3,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.VK_3, (IList<string>) new List<string>()
        {
          "3",
          "#"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 4,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.VK_4, (IList<string>) new List<string>()
        {
          "4",
          "$"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 5,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.VK_5, (IList<string>) new List<string>()
        {
          "5",
          "%"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 6,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.VK_6, (IList<string>) new List<string>()
        {
          "6",
          "^"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 7,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.VK_7, (IList<string>) new List<string>()
        {
          "7",
          "&"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 8,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.VK_8, (IList<string>) new List<string>()
        {
          "8",
          "*"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 9,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.VK_9, (IList<string>) new List<string>()
        {
          "9",
          "("
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 10,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.VK_0, (IList<string>) new List<string>()
        {
          "0",
          ")"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 11,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.OEM_MINUS, (IList<string>) new List<string>()
        {
          "-",
          "_"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 12,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.OEM_PLUS, (IList<string>) new List<string>()
        {
          "=",
          "+"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 0,
        GridColumn = 13,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.BACK, "Bksp"),
        GridWidth = new GridLength(2.0, GridUnitType.Star)
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 0,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.TAB, "Tab"),
        GridWidth = new GridLength(1.5, GridUnitType.Star)
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 1,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_Q, (IList<string>) new List<string>()
        {
          "q",
          "Q"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 2,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_W, (IList<string>) new List<string>()
        {
          "w",
          "W"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 3,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_E, (IList<string>) new List<string>()
        {
          "e",
          "E"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 4,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_R, (IList<string>) new List<string>()
        {
          "r",
          "R"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 5,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_T, (IList<string>) new List<string>()
        {
          "t",
          "T"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 6,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_Y, (IList<string>) new List<string>()
        {
          "y",
          "Y"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 7,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_U, (IList<string>) new List<string>()
        {
          "u",
          "U"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 8,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_I, (IList<string>) new List<string>()
        {
          "i",
          "I"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 9,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_O, (IList<string>) new List<string>()
        {
          "o",
          "O"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 10,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_P, (IList<string>) new List<string>()
        {
          "p",
          "P"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 11,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.OEM_4, (IList<string>) new List<string>()
        {
          "[",
          "{"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 12,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.OEM_6, (IList<string>) new List<string>()
        {
          "]",
          "}"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 1,
        GridColumn = 13,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.OEM_5, (IList<string>) new List<string>()
        {
          "\\",
          "|"
        }),
        GridWidth = new GridLength(1.3, GridUnitType.Star)
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 0,
        Key = (ILogicalKey) new TogglingModifierKey("Caps", VirtualKeyCode.CAPITAL),
        GridWidth = new GridLength(1.7, GridUnitType.Star)
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 1,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_A, (IList<string>) new List<string>()
        {
          "a",
          "A"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 2,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_S, (IList<string>) new List<string>()
        {
          "s",
          "S"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 3,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_D, (IList<string>) new List<string>()
        {
          "d",
          "D"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 4,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_F, (IList<string>) new List<string>()
        {
          "f",
          "F"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 5,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_G, (IList<string>) new List<string>()
        {
          "g",
          "G"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 6,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_H, (IList<string>) new List<string>()
        {
          "h",
          "H"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 7,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_J, (IList<string>) new List<string>()
        {
          "j",
          "J"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 8,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_K, (IList<string>) new List<string>()
        {
          "k",
          "K"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 9,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_L, (IList<string>) new List<string>()
        {
          "l",
          "L"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 10,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.OEM_1, (IList<string>) new List<string>()
        {
          ";",
          ":"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 11,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.OEM_7, (IList<string>) new List<string>()
        {
          "\"",
          "\""
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 2,
        GridColumn = 12,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.RETURN, "Enter"),
        GridWidth = new GridLength(1.8, GridUnitType.Star)
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 0,
        Key = (ILogicalKey) new InstantaneousModifierKey("Shift", VirtualKeyCode.SHIFT),
        GridWidth = new GridLength(2.4, GridUnitType.Star)
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 1,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_Z, (IList<string>) new List<string>()
        {
          "z",
          "Z"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 2,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_X, (IList<string>) new List<string>()
        {
          "x",
          "X"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 3,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_C, (IList<string>) new List<string>()
        {
          "c",
          "C"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 4,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_V, (IList<string>) new List<string>()
        {
          "v",
          "V"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 5,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_B, (IList<string>) new List<string>()
        {
          "b",
          "B"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 6,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_N, (IList<string>) new List<string>()
        {
          "n",
          "N"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 7,
        Key = (ILogicalKey) new CaseSensitiveKey(VirtualKeyCode.VK_M, (IList<string>) new List<string>()
        {
          "m",
          "M"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 8,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.OEM_COMMA, (IList<string>) new List<string>()
        {
          ",",
          "<"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 9,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.OEM_PERIOD, (IList<string>) new List<string>()
        {
          ".",
          ">"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 10,
        Key = (ILogicalKey) new ShiftSensitiveKey(VirtualKeyCode.OEM_2, (IList<string>) new List<string>()
        {
          "/",
          "?"
        })
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 3,
        GridColumn = 11,
        Key = (ILogicalKey) new InstantaneousModifierKey("Shift", VirtualKeyCode.SHIFT),
        GridWidth = new GridLength(2.4, GridUnitType.Star)
      });
      observableCollection.Add(new OnScreenKey()
      {
        GridRow = 4,
        GridColumn = 0,
        Key = (ILogicalKey) new VirtualKey(VirtualKeyCode.SPACE, " "),
        GridWidth = new GridLength(5.0, GridUnitType.Star)
      });
      ObservableCollection<OnScreenKey> source = observableCollection;
      element.Keys = source;
      element.SetValue(Grid.ColumnProperty, (object) 0);
      this._sections.Add(element);
      this.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3.0, GridUnitType.Star)
      });
      this.Children.Add((UIElement) element);
      this._allLogicalKeys.AddRange(source.Select<OnScreenKey, ILogicalKey>((Func<OnScreenKey, ILogicalKey>) (x => x.Key)));
      this._allOnScreenKeys.AddRange((IEnumerable<OnScreenKey>) element.Keys);
      this._modifierKeys.AddRange(this._allLogicalKeys.OfType<ModifierKeyBase>());
      this._allOnScreenKeys.ForEach((Action<OnScreenKey>) (x => x.OnScreenKeyPress += new OnScreenKeyEventHandler(this.OnScreenKeyPress)));
      this.SynchroniseModifierKeyState();
      base.BeginInit();
    }

    private void OnScreenKeyPress(DependencyObject sender, OnScreenKeyEventArgs e)
    {
      if (e.OnScreenKey.Key is ModifierKeyBase)
      {
        ModifierKeyBase key = (ModifierKeyBase) e.OnScreenKey.Key;
        if (key.KeyCode == VirtualKeyCode.SHIFT)
          this.HandleShiftKeyPressed(key);
        else if (key.KeyCode == VirtualKeyCode.CAPITAL)
          this.HandleCapsLockKeyPressed(key);
        else if (key.KeyCode == VirtualKeyCode.NUMLOCK)
          this.HandleNumLockKeyPressed(key);
      }
      else
        this.ResetInstantaneousModifierKeys();
      this._modifierKeys.OfType<InstantaneousModifierKey>().ToList<InstantaneousModifierKey>().ForEach((Action<InstantaneousModifierKey>) (x => x.SynchroniseKeyState()));
    }

    private void SynchroniseModifierKeyState()
    {
      this._modifierKeys.ToList<ModifierKeyBase>().ForEach((Action<ModifierKeyBase>) (x => x.SynchroniseKeyState()));
    }

    private void ResetInstantaneousModifierKeys()
    {
      this._modifierKeys.OfType<InstantaneousModifierKey>().ToList<InstantaneousModifierKey>().ForEach((Action<InstantaneousModifierKey>) (x =>
      {
        if (!x.IsInEffect)
          return;
        x.Press();
      }));
    }

    private void HandleShiftKeyPressed(ModifierKeyBase shiftKey)
    {
      this._allLogicalKeys.OfType<CaseSensitiveKey>().ToList<CaseSensitiveKey>().ForEach((Action<CaseSensitiveKey>) (x => x.SelectedIndex = InputSimulator.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL) ^ shiftKey.IsInEffect ? 1 : 0));
      this._allLogicalKeys.OfType<ShiftSensitiveKey>().ToList<ShiftSensitiveKey>().ForEach((Action<ShiftSensitiveKey>) (x => x.SelectedIndex = shiftKey.IsInEffect ? 1 : 0));
    }

    private void HandleCapsLockKeyPressed(ModifierKeyBase capsLockKey)
    {
      this._allLogicalKeys.OfType<CaseSensitiveKey>().ToList<CaseSensitiveKey>().ForEach((Action<CaseSensitiveKey>) (x => x.SelectedIndex = capsLockKey.IsInEffect ^ InputSimulator.IsKeyDownAsync(VirtualKeyCode.SHIFT) ? 1 : 0));
    }

    private void HandleNumLockKeyPressed(ModifierKeyBase numLockKey)
    {
      this._allLogicalKeys.OfType<NumLockSensitiveKey>().ToList<NumLockSensitiveKey>().ForEach((Action<NumLockSensitiveKey>) (x => x.SelectedIndex = numLockKey.IsInEffect ? 1 : 0));
    }
  }
}
