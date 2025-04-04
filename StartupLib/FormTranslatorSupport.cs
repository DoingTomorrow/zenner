// Decompiled with JetBrains decompiler
// Type: StartupLib.FormTranslatorSupport
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using GmmDbLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace StartupLib
{
  public static class FormTranslatorSupport
  {
    public static void TranslateWindow(Tg translationGroup, Form theWindow)
    {
      if (theWindow.Controls == null)
        return;
      Control.ControlCollection controls = theWindow.Controls;
      FormTranslatorSupport.TranslateContent(translationGroup, theWindow.Name.ToString() + ".", (object) controls);
    }

    public static void TranslateWindow(
      Tg translationGroup,
      Form theWindow,
      List<object> ignorContent)
    {
      if (theWindow.Controls == null)
        return;
      FormTranslatorSupport.TranslateContent(translationGroup, theWindow.ToString() + ".", (object) theWindow.Controls, ignorContent);
    }

    public static void TranslateLabel(
      Tg translationGroup,
      string translaterBaseKey,
      Label theLabel)
    {
      string str = !string.IsNullOrEmpty(theLabel.Name) ? theLabel.Name : "Label";
      if (!(theLabel.Text != string.Empty))
        return;
      theLabel.Text = Ot.Gtt(translationGroup, translaterBaseKey + str, theLabel.Text.ToString());
    }

    public static void TranslateButton(
      Tg translationGroup,
      string translaterBaseKey,
      Button theButton)
    {
      if (!(theButton.Text != string.Empty))
        return;
      theButton.Text = Ot.Gtt(translationGroup, translaterBaseKey + theButton.Name, theButton.Text.ToString());
    }

    public static void TranslateMenuStrip(
      Tg translationGroup,
      string translaterBaseKey,
      MenuStrip menu)
    {
      foreach (ToolStripMenuItem menu1 in (ArrangedElementCollection) menu.Items)
      {
        if (menu1 != null)
          FormTranslatorSupport.TranslateToolStripMenuItem(translationGroup, translaterBaseKey, menu1);
      }
    }

    public static void TranslateToolStripMenuItem(
      Tg translationGroup,
      string translaterBaseKey,
      ToolStripMenuItem menu)
    {
      if (menu.Text != string.Empty)
        menu.Text = Ot.Gtt(translationGroup, translaterBaseKey + menu.Name, menu.Text.ToString());
      for (int index = 0; index < menu.DropDownItems.Count; ++index)
      {
        if (menu.DropDownItems[index] is ToolStripMenuItem)
          FormTranslatorSupport.TranslateToolStripMenuItem(translationGroup, translaterBaseKey, (ToolStripMenuItem) menu.DropDownItems[index]);
      }
    }

    public static void TranslateGroupBox(
      Tg translationGroup,
      string translaterBaseKey,
      GroupBox theGroupBox)
    {
      if (theGroupBox.Text != string.Empty)
        theGroupBox.Text = Ot.Gtt(translationGroup, translaterBaseKey + theGroupBox.Name, theGroupBox.Text.ToString());
      FormTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey, (object) theGroupBox.Controls);
    }

    public static void TranslateContent(
      Tg translationGroup,
      string translaterBaseKey,
      object controls)
    {
      if (controls == null || !(controls is Control.ControlCollection))
        return;
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        switch (control)
        {
          case Panel _:
            Panel panel = (Panel) control;
            if (panel.Controls != null && panel.Controls.Count > 0)
            {
              IEnumerator enumerator = panel.Controls.GetEnumerator();
              try
              {
                while (enumerator.MoveNext())
                {
                  object current = enumerator.Current;
                  FormTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey, current);
                }
                break;
              }
              finally
              {
                if (enumerator is IDisposable disposable)
                  disposable.Dispose();
              }
            }
            else
              break;
          case Label _:
            FormTranslatorSupport.TranslateLabel(translationGroup, translaterBaseKey, (Label) control);
            break;
          case Button _:
            FormTranslatorSupport.TranslateButton(translationGroup, translaterBaseKey, (Button) control);
            break;
          case CheckBox _:
            CheckBox checkBox = (CheckBox) control;
            checkBox.Text = Ot.Gtt(translationGroup, translaterBaseKey + checkBox.Name, checkBox.Text.ToString());
            break;
          case MenuStrip _:
            FormTranslatorSupport.TranslateMenuStrip(translationGroup, translaterBaseKey, (MenuStrip) control);
            break;
          case GroupBox _:
            FormTranslatorSupport.TranslateGroupBox(translationGroup, translaterBaseKey, (GroupBox) control);
            break;
        }
      }
    }

    public static void TranslateContent(
      Tg translationGroup,
      string translaterBaseKey,
      object content,
      List<object> ignorContent)
    {
      if (content == null || ignorContent.Contains(content))
        return;
      FormTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey, content);
    }
  }
}
