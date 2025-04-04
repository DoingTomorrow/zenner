// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.ThemeManager
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Windows;

#nullable disable
namespace MahApps.Metro
{
  public static class ThemeManager
  {
    private static IList<Accent> _accents;
    private static IList<AppTheme> _appThemes;

    public static IEnumerable<Accent> Accents
    {
      get
      {
        if (ThemeManager._accents != null)
          return (IEnumerable<Accent>) ThemeManager._accents;
        string[] strArray = new string[23]
        {
          "Red",
          "Green",
          "Blue",
          "Purple",
          "Orange",
          "Lime",
          "Emerald",
          "Teal",
          "Cyan",
          "Cobalt",
          "Indigo",
          "Violet",
          "Pink",
          "Magenta",
          "Crimson",
          "Amber",
          "Yellow",
          "Brown",
          "Olive",
          "Steel",
          "Mauve",
          "Taupe",
          "Sienna"
        };
        ThemeManager._accents = (IList<Accent>) new List<Accent>(strArray.Length);
        try
        {
          foreach (string name in strArray)
          {
            Uri resourceAddress = new Uri(string.Format("pack://application:,,,/MahApps.Metro;component/Styles/Accents/{0}.xaml", (object) name));
            ThemeManager._accents.Add(new Accent(name, resourceAddress));
          }
        }
        catch (Exception ex)
        {
          throw new MahAppsException("This exception happens because you are maybe running that code out of the scope of a WPF application. Most likely because you are testing your configuration inside a unit test.", ex);
        }
        return (IEnumerable<Accent>) ThemeManager._accents;
      }
    }

    public static IEnumerable<AppTheme> AppThemes
    {
      get
      {
        if (ThemeManager._appThemes != null)
          return (IEnumerable<AppTheme>) ThemeManager._appThemes;
        string[] strArray = new string[2]
        {
          "BaseLight",
          "BaseDark"
        };
        ThemeManager._appThemes = (IList<AppTheme>) new List<AppTheme>(strArray.Length);
        try
        {
          foreach (string name in strArray)
          {
            Uri resourceAddress = new Uri(string.Format("pack://application:,,,/MahApps.Metro;component/Styles/Accents/{0}.xaml", (object) name));
            ThemeManager._appThemes.Add(new AppTheme(name, resourceAddress));
          }
        }
        catch (Exception ex)
        {
          throw new MahAppsException("This exception happens because you are maybe running that code out of the scope of a WPF application. Most likely because you are testing your configuration inside a unit test.", ex);
        }
        return (IEnumerable<AppTheme>) ThemeManager._appThemes;
      }
    }

    public static bool AddAccent(string name, Uri resourceAddress)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (resourceAddress == (Uri) null)
        throw new ArgumentNullException(nameof (resourceAddress));
      if (ThemeManager.GetAccent(name) != null)
        return false;
      ThemeManager._accents.Add(new Accent(name, resourceAddress));
      return true;
    }

    public static bool AddAppTheme(string name, Uri resourceAddress)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (resourceAddress == (Uri) null)
        throw new ArgumentNullException(nameof (resourceAddress));
      if (ThemeManager.GetAppTheme(name) != null)
        return false;
      ThemeManager._appThemes.Add(new AppTheme(name, resourceAddress));
      return true;
    }

    public static AppTheme GetAppTheme(ResourceDictionary resources)
    {
      if (resources == null)
        throw new ArgumentNullException(nameof (resources));
      return ThemeManager.AppThemes.FirstOrDefault<AppTheme>((Func<AppTheme, bool>) (x => ThemeManager.AreResourceDictionarySourcesEqual(x.Resources.Source, resources.Source)));
    }

    public static AppTheme GetAppTheme(string appThemeName)
    {
      if (appThemeName == null)
        throw new ArgumentNullException(nameof (appThemeName));
      return ThemeManager.AppThemes.FirstOrDefault<AppTheme>((Func<AppTheme, bool>) (x => x.Name.Equals(appThemeName, StringComparison.InvariantCultureIgnoreCase)));
    }

    public static AppTheme GetInverseAppTheme(AppTheme appTheme)
    {
      if (appTheme == null)
        throw new ArgumentNullException(nameof (appTheme));
      if (appTheme.Name.EndsWith("dark", StringComparison.InvariantCultureIgnoreCase))
        return ThemeManager.GetAppTheme(appTheme.Name.ToLower().Replace("dark", string.Empty) + "light");
      return appTheme.Name.EndsWith("light", StringComparison.InvariantCultureIgnoreCase) ? ThemeManager.GetAppTheme(appTheme.Name.ToLower().Replace("light", string.Empty) + "dark") : (AppTheme) null;
    }

    public static Accent GetAccent(string accentName)
    {
      if (accentName == null)
        throw new ArgumentNullException(nameof (accentName));
      return ThemeManager.Accents.FirstOrDefault<Accent>((Func<Accent, bool>) (x => x.Name.Equals(accentName, StringComparison.InvariantCultureIgnoreCase)));
    }

    public static Accent GetAccent(ResourceDictionary resources)
    {
      if (resources == null)
        throw new ArgumentNullException(nameof (resources));
      Accent accent = ThemeManager.Accents.FirstOrDefault<Accent>((Func<Accent, bool>) (x => ThemeManager.AreResourceDictionarySourcesEqual(x.Resources.Source, resources.Source)));
      if (accent != null)
        return accent;
      if (!(resources.Source == (Uri) null) || !ThemeManager.IsAccentDictionary(resources))
        return (Accent) null;
      return new Accent()
      {
        Name = "Runtime accent",
        Resources = resources
      };
    }

    public static bool IsAccentDictionary(ResourceDictionary resources)
    {
      if (resources == null)
        throw new ArgumentNullException(nameof (resources));
      string[] collection = new string[10]
      {
        "HighlightColor",
        "AccentColor",
        "AccentColor2",
        "AccentColor3",
        "AccentColor4",
        "HighlightBrush",
        "AccentColorBrush",
        "AccentColorBrush2",
        "AccentColorBrush3",
        "AccentColorBrush4"
      };
      foreach (string str in new List<string>((IEnumerable<string>) collection))
      {
        string styleKey = str;
        if (!resources.Keys.Cast<object>().Select<object, string>((Func<object, string>) (resourceKey => resourceKey as string)).Any<string>((Func<string, bool>) (keyAsString => string.Equals(keyAsString, styleKey))))
          return false;
      }
      return true;
    }

    public static object GetResourceFromAppStyle(Window window, string key)
    {
      Tuple<AppTheme, Accent> tuple = window != null ? ThemeManager.DetectAppStyle(window) : ThemeManager.DetectAppStyle(Application.Current);
      if (tuple == null && window != null)
        tuple = ThemeManager.DetectAppStyle(Application.Current);
      if (tuple == null)
        return (object) null;
      object resource = tuple.Item1.Resources[(object) key];
      return tuple.Item2.Resources[(object) key] ?? resource;
    }

    [SecurityCritical]
    public static void ChangeAppTheme(Application app, string themeName)
    {
      if (app == null)
        throw new ArgumentNullException(nameof (app));
      if (themeName == null)
        throw new ArgumentNullException(nameof (themeName));
      Tuple<AppTheme, Accent> oldThemeInfo = ThemeManager.DetectAppStyle(app);
      AppTheme appTheme;
      if ((appTheme = ThemeManager.GetAppTheme(themeName)) == null)
        return;
      ThemeManager.ChangeAppStyle(app.Resources, oldThemeInfo, oldThemeInfo.Item2, appTheme);
    }

    [SecurityCritical]
    public static void ChangeAppTheme(Window window, string themeName)
    {
      if (window == null)
        throw new ArgumentNullException(nameof (window));
      if (themeName == null)
        throw new ArgumentNullException(nameof (themeName));
      Tuple<AppTheme, Accent> oldThemeInfo = ThemeManager.DetectAppStyle(window);
      AppTheme appTheme;
      if ((appTheme = ThemeManager.GetAppTheme(themeName)) == null)
        return;
      ThemeManager.ChangeAppStyle(window.Resources, oldThemeInfo, oldThemeInfo.Item2, appTheme);
    }

    [SecurityCritical]
    public static void ChangeAppStyle(Application app, Accent newAccent, AppTheme newTheme)
    {
      Tuple<AppTheme, Accent> oldThemeInfo = app != null ? ThemeManager.DetectAppStyle(app) : throw new ArgumentNullException(nameof (app));
      ThemeManager.ChangeAppStyle(app.Resources, oldThemeInfo, newAccent, newTheme);
    }

    [SecurityCritical]
    public static void ChangeAppStyle(Window window, Accent newAccent, AppTheme newTheme)
    {
      Tuple<AppTheme, Accent> oldThemeInfo = window != null ? ThemeManager.DetectAppStyle(window) : throw new ArgumentNullException(nameof (window));
      ThemeManager.ChangeAppStyle(window.Resources, oldThemeInfo, newAccent, newTheme);
    }

    [SecurityCritical]
    private static void ChangeAppStyle(
      ResourceDictionary resources,
      Tuple<AppTheme, Accent> oldThemeInfo,
      Accent newAccent,
      AppTheme newTheme)
    {
      bool flag = false;
      if (oldThemeInfo != null)
      {
        Accent accent = oldThemeInfo.Item2;
        if (accent != null && accent.Name != newAccent.Name)
        {
          string key = accent.Resources.Source.ToString().ToLower();
          ResourceDictionary resourceDictionary = resources.MergedDictionaries.Where<ResourceDictionary>((Func<ResourceDictionary, bool>) (x => x.Source != (Uri) null)).FirstOrDefault<ResourceDictionary>((Func<ResourceDictionary, bool>) (d => d.Source.ToString().ToLower() == key));
          if (resourceDictionary != null)
          {
            resources.MergedDictionaries.Add(newAccent.Resources);
            resources.MergedDictionaries.Remove(resourceDictionary);
            flag = true;
          }
        }
        AppTheme appTheme = oldThemeInfo.Item1;
        if (appTheme != null && appTheme != newTheme)
        {
          string key = appTheme.Resources.Source.ToString().ToLower();
          ResourceDictionary resourceDictionary = resources.MergedDictionaries.Where<ResourceDictionary>((Func<ResourceDictionary, bool>) (x => x.Source != (Uri) null)).FirstOrDefault<ResourceDictionary>((Func<ResourceDictionary, bool>) (d => d.Source.ToString().ToLower() == key));
          if (resourceDictionary != null)
          {
            resources.MergedDictionaries.Add(newTheme.Resources);
            resources.MergedDictionaries.Remove(resourceDictionary);
            flag = true;
          }
        }
      }
      else
      {
        ThemeManager.ChangeAppStyle(resources, newAccent, newTheme);
        flag = true;
      }
      if (!flag)
        return;
      ThemeManager.OnThemeChanged(newAccent, newTheme);
    }

    [SecurityCritical]
    public static void ChangeAppStyle(
      ResourceDictionary resources,
      Accent newAccent,
      AppTheme newTheme)
    {
      if (resources == null)
        throw new ArgumentNullException(nameof (resources));
      if (newAccent == null)
        throw new ArgumentNullException(nameof (newAccent));
      if (newTheme == null)
        throw new ArgumentNullException(nameof (newTheme));
      ThemeManager.ApplyResourceDictionary(newAccent.Resources, resources);
      ThemeManager.ApplyResourceDictionary(newTheme.Resources, resources);
    }

    [SecurityCritical]
    private static void ApplyResourceDictionary(ResourceDictionary newRd, ResourceDictionary oldRd)
    {
      oldRd.BeginInit();
      foreach (DictionaryEntry dictionaryEntry in newRd)
      {
        if (oldRd.Contains(dictionaryEntry.Key))
          oldRd.Remove(dictionaryEntry.Key);
        oldRd.Add(dictionaryEntry.Key, dictionaryEntry.Value);
      }
      oldRd.EndInit();
    }

    internal static void CopyResource(ResourceDictionary fromRD, ResourceDictionary toRD)
    {
      if (fromRD == null)
        throw new ArgumentNullException(nameof (fromRD));
      if (toRD == null)
        throw new ArgumentNullException(nameof (toRD));
      ThemeManager.ApplyResourceDictionary(fromRD, toRD);
      foreach (ResourceDictionary mergedDictionary in fromRD.MergedDictionaries)
        ThemeManager.CopyResource(mergedDictionary, toRD);
    }

    public static Tuple<AppTheme, Accent> DetectAppStyle()
    {
      try
      {
        return ThemeManager.DetectAppStyle(Application.Current.MainWindow);
      }
      catch (Exception ex)
      {
        return ThemeManager.DetectAppStyle(Application.Current);
      }
    }

    public static Tuple<AppTheme, Accent> DetectAppStyle(Window window)
    {
      if (window == null)
        throw new ArgumentNullException(nameof (window));
      return ThemeManager.DetectAppStyle(window.Resources) ?? ThemeManager.DetectAppStyle(Application.Current.Resources);
    }

    public static Tuple<AppTheme, Accent> DetectAppStyle(Application app)
    {
      return app != null ? ThemeManager.DetectAppStyle(app.Resources) : throw new ArgumentNullException(nameof (app));
    }

    private static Tuple<AppTheme, Accent> DetectAppStyle(ResourceDictionary resources)
    {
      if (resources == null)
        throw new ArgumentNullException(nameof (resources));
      AppTheme detectedTheme = (AppTheme) null;
      Tuple<AppTheme, Accent> detectedAccentTheme = (Tuple<AppTheme, Accent>) null;
      return ThemeManager.DetectThemeFromResources(ref detectedTheme, resources) && ThemeManager.GetThemeFromResources(detectedTheme, resources, ref detectedAccentTheme) ? new Tuple<AppTheme, Accent>(detectedAccentTheme.Item1, detectedAccentTheme.Item2) : (Tuple<AppTheme, Accent>) null;
    }

    internal static bool DetectThemeFromAppResources(out AppTheme detectedTheme)
    {
      detectedTheme = (AppTheme) null;
      return ThemeManager.DetectThemeFromResources(ref detectedTheme, Application.Current.Resources);
    }

    private static bool DetectThemeFromResources(
      ref AppTheme detectedTheme,
      ResourceDictionary dict)
    {
      IEnumerator<ResourceDictionary> enumerator = dict.MergedDictionaries.GetEnumerator();
      while (enumerator.MoveNext())
      {
        ResourceDictionary current = enumerator.Current;
        AppTheme appTheme;
        if ((appTheme = ThemeManager.GetAppTheme(current)) != null)
        {
          detectedTheme = appTheme;
          enumerator.Dispose();
          return true;
        }
        if (ThemeManager.DetectThemeFromResources(ref detectedTheme, current))
          return true;
      }
      enumerator.Dispose();
      return false;
    }

    internal static bool GetThemeFromResources(
      AppTheme presetTheme,
      ResourceDictionary dict,
      ref Tuple<AppTheme, Accent> detectedAccentTheme)
    {
      AppTheme appTheme = presetTheme;
      Accent accent;
      if ((accent = ThemeManager.GetAccent(dict)) != null)
      {
        detectedAccentTheme = Tuple.Create<AppTheme, Accent>(appTheme, accent);
        return true;
      }
      foreach (ResourceDictionary mergedDictionary in dict.MergedDictionaries)
      {
        if (ThemeManager.GetThemeFromResources(presetTheme, mergedDictionary, ref detectedAccentTheme))
          return true;
      }
      return false;
    }

    public static event EventHandler<OnThemeChangedEventArgs> IsThemeChanged;

    [SecurityCritical]
    private static void OnThemeChanged(Accent newAccent, AppTheme newTheme)
    {
      SafeRaise.Raise<OnThemeChangedEventArgs>(ThemeManager.IsThemeChanged, (object) Application.Current, new OnThemeChangedEventArgs()
      {
        AppTheme = newTheme,
        Accent = newAccent
      });
    }

    private static bool AreResourceDictionarySourcesEqual(Uri first, Uri second)
    {
      return Uri.Compare(first, second, UriComponents.Host | UriComponents.Path, UriFormat.SafeUnescaped, StringComparison.InvariantCultureIgnoreCase) == 0;
    }
  }
}
