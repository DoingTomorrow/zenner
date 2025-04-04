// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.CustomControls.KeyboardMetroWindow
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using MSS.Client.UI.Tablet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using WpfKb.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.CustomControls
{
  public class KeyboardMetroWindow : ResizableMetroWindow
  {
    public virtual void RegisterKeyboardEvents(TouchScreenKeyboardUserControl kb)
    {
      IEnumerable<TextBox> visualChildren = this.FindVisualChildren<TextBox>();
      visualChildren.ForEach<TextBox>((Action<TextBox>) (_ =>
      {
        if (_.InputScope == null)
          _.InputScope = new InputScope()
          {
            Names = {
              (object) new InputScopeName()
              {
                NameValue = InputScopeNameValue.EmailSmtpAddress
              }
            }
          };
        CommonHandlers<OpenKeyboardEventParams>.RegisterKeyboardEvents((Control) _, kb);
      }));
      CommonHandlers<OpenKeyboardEventParams>.SetFirstFocusableItem((Control) visualChildren.FirstOrDefault<TextBox>());
    }

    public void KeyboardButton_TouchDown(object sender, TouchEventArgs e)
    {
      CommonHandlers<OpenKeyboardEventParams>.FocusLastItem();
    }
  }
}
