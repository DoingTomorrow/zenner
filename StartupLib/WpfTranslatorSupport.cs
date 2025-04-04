// Decompiled with JetBrains decompiler
// Type: StartupLib.WpfTranslatorSupport
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using GmmDbLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace StartupLib
{
  public static class WpfTranslatorSupport
  {
    public static void TranslateWindow(Tg translationGroup, Window theWindow)
    {
      if (theWindow.Content == null)
        return;
      WpfTranslatorSupport.TranslateContent(translationGroup, theWindow.ToString() + ".", theWindow.Content);
    }

    public static void TranslateWindow(
      Tg translationGroup,
      Window theWindow,
      List<object> ignorContent)
    {
      if (theWindow.Content == null)
        return;
      WpfTranslatorSupport.TranslateContent(translationGroup, theWindow.ToString() + ".", theWindow.Content, ignorContent);
    }

    public static void TranslateUserControl(Tg translationGroup, UserControl theControl)
    {
      if (theControl.Content == null)
        return;
      WpfTranslatorSupport.TranslateContent(translationGroup, theControl.ToString(), theControl.Content);
    }

    public static void TranslateWindow(
      Tg translationGroup,
      UserControl theControl,
      List<object> ignorContent)
    {
      if (theControl.Content == null)
        return;
      WpfTranslatorSupport.TranslateContent(translationGroup, theControl.ToString(), theControl.Content, ignorContent);
    }

    public static void TranslateContent(
      Tg translationGroup,
      string translaterBaseKey,
      object content)
    {
      switch (content)
      {
        case Panel _:
          Panel panel = (Panel) content;
          if (panel.Children == null || panel.Children.Count <= 0)
            break;
          IEnumerator enumerator1 = panel.Children.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              object current = enumerator1.Current;
              WpfTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey, current);
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case Label _:
          WpfTranslatorSupport.TranslateLabel(translationGroup, translaterBaseKey, (Label) content);
          break;
        case Button _:
          WpfTranslatorSupport.TranslateButton(translationGroup, translaterBaseKey, (Button) content);
          break;
        case CheckBox _:
          CheckBox checkBox = (CheckBox) content;
          checkBox.Content = (object) Ot.Gtt(translationGroup, translaterBaseKey + checkBox.Name, checkBox.Content.ToString());
          break;
        case Menu _:
          WpfTranslatorSupport.TranslateMenu(translationGroup, translaterBaseKey, (Menu) content);
          break;
        case MenuItem _:
          WpfTranslatorSupport.TranslateMenuItem(translationGroup, translaterBaseKey, (MenuItem) content);
          break;
        case TextBlock _:
          WpfTranslatorSupport.TranslateTextBlock(translationGroup, translaterBaseKey, (TextBlock) content);
          break;
        case ScrollViewer _:
          ScrollViewer scrollViewer = (ScrollViewer) content;
          WpfTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey, scrollViewer.Content);
          break;
        case GroupBox _:
          WpfTranslatorSupport.TranslateGroupBox(translationGroup, translaterBaseKey, (GroupBox) content);
          break;
        case TabControl _:
          TabControl tabControl = (TabControl) content;
          if (tabControl.Items == null || tabControl.Items.Count <= 0)
            break;
          IEnumerator enumerator2 = ((IEnumerable) tabControl.Items).GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              object current = enumerator2.Current;
              WpfTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey, current);
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
        case TabItem _:
          WpfTranslatorSupport.TranslateTabItem(translationGroup, translaterBaseKey, (TabItem) content);
          break;
        case ContentControl _:
          WpfTranslatorSupport.TranslateContentControl(translationGroup, translaterBaseKey, (ContentControl) content);
          break;
        case HeaderedContentControl _:
          WpfTranslatorSupport.TranslateHeaderedContentControl(translationGroup, translaterBaseKey, (HeaderedContentControl) content);
          break;
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
      WpfTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey, content);
    }

    public static void TranslateLabel(
      Tg translationGroup,
      string translaterBaseKey,
      Label theLabel)
    {
      if (theLabel.Content == null)
        return;
      string str = !string.IsNullOrEmpty(theLabel.Name) ? theLabel.Name : "Label";
      if (theLabel.Content is string)
        theLabel.Content = (object) Ot.Gtt(translationGroup, translaterBaseKey + str, theLabel.Content.ToString());
      else
        WpfTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey + str, theLabel.Content);
    }

    public static void TranslateButton(
      Tg translationGroup,
      string translaterBaseKey,
      Button theButton)
    {
      if (theButton.Content == null)
        return;
      if (theButton.Content is string)
        theButton.Content = (object) Ot.Gtt(translationGroup, translaterBaseKey + theButton.Name, theButton.Content.ToString());
      else
        WpfTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey + "." + theButton.Name, theButton.Content);
    }

    public static void TranslateTextBlock(
      Tg translationGroup,
      string translaterBaseKey,
      TextBlock theTextBlock)
    {
      theTextBlock.Text = Ot.Gtt(translationGroup, translaterBaseKey + theTextBlock.Name, theTextBlock.Text);
    }

    public static void TranslateMenu(Tg translationGroup, string translaterBaseKey, Menu menu)
    {
      if (menu.Items == null)
        return;
      foreach (object obj in (IEnumerable) menu.Items)
      {
        if (obj is MenuItem)
          WpfTranslatorSupport.TranslateMenuItem(translationGroup, translaterBaseKey, (MenuItem) obj);
      }
    }

    public static void TranslateMenuItem(
      Tg translationGroup,
      string translaterBaseKey,
      MenuItem menuItem)
    {
      if (!string.IsNullOrEmpty(menuItem.Name) && menuItem.Header != null)
        menuItem.Header = (object) Ot.Gtt(translationGroup, translaterBaseKey + menuItem.Name, menuItem.Header.ToString());
      if (menuItem.Items == null)
        return;
      foreach (object obj in (IEnumerable) menuItem.Items)
      {
        if (obj is MenuItem)
          WpfTranslatorSupport.TranslateMenuItem(translationGroup, translaterBaseKey, (MenuItem) obj);
      }
    }

    public static void TranslateGroupBox(
      Tg translationGroup,
      string translaterBaseKey,
      GroupBox theGroupBox)
    {
      if (theGroupBox.Header != null)
        theGroupBox.Header = (object) Ot.Gtt(translationGroup, translaterBaseKey + theGroupBox.Name, theGroupBox.Header.ToString());
      WpfTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey, theGroupBox.Content);
    }

    public static void TranslateTabItem(
      Tg translationGroup,
      string translaterBaseKey,
      TabItem theTabItem)
    {
      theTabItem.Header = (object) Ot.Gtt(translationGroup, translaterBaseKey + theTabItem.Name, theTabItem.Header.ToString());
      WpfTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey, theTabItem.Content);
    }

    public static void TranslateContentControl(
      Tg translationGroup,
      string translaterBaseKey,
      ContentControl theControl)
    {
      if (theControl.Content == null)
        return;
      string str;
      if (string.IsNullOrEmpty(theControl.Name))
        str = "cc";
      else
        str = theControl.Name;
    }

    public static void TranslateHeaderedContentControl(
      Tg translationGroup,
      string translaterBaseKey,
      HeaderedContentControl theHeaderedContentControl)
    {
      WpfTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey, theHeaderedContentControl.Header);
      WpfTranslatorSupport.TranslateContent(translationGroup, translaterBaseKey, theHeaderedContentControl.Content);
    }
  }
}
