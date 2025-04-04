// Decompiled with JetBrains decompiler
// Type: MSS.Localisation.CultureResources
// Assembly: MSS.Localisation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 466F16EF-FA49-4972-B382-5A7CC9B90731
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Localisation.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace MSS.Localisation
{
  public class CultureResources
  {
    private static bool iHasFoundInstalledCultures = false;
    private static List<CultureInfo> iSupportedCultures = new List<CultureInfo>();
    private static ObjectDataProvider iProvider;
    private static Logger LOG = LogManager.GetCurrentClassLogger();
    private static List<ILocalisationChanged> iLocalisationChangedListeners = new List<ILocalisationChanged>();

    public CultureResources() => this.DetermineAvailableCultures();

    public static void Attach(ILocalisationChanged i)
    {
      CultureResources.iLocalisationChangedListeners.Add(i);
    }

    public static void Detach(ILocalisationChanged i)
    {
      CultureResources.iLocalisationChangedListeners.Remove(i);
    }

    private static void NotifyListeners()
    {
      foreach (ILocalisationChanged localisationChangedListener in CultureResources.iLocalisationChangedListeners)
        localisationChangedListener.LocalisationChanged();
    }

    public void DetermineAvailableCultures()
    {
      CultureResources.LOG.Info("DetermineAvailableCultures - Enter");
      if (!CultureResources.iHasFoundInstalledCultures)
      {
        CultureInfo cultureInfo1 = new CultureInfo("");
        foreach (string directory in Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory))
        {
          try
          {
            CultureInfo cultureInfo2 = CultureInfo.GetCultureInfo(new DirectoryInfo(directory).Name);
            CultureResources.iSupportedCultures.Add(cultureInfo2);
            CultureResources.LOG.Info("DetermineAvailableCultures - " + string.Format("Found Culture: {0} [{1}]", (object) cultureInfo2.DisplayName, (object) cultureInfo2.Name));
          }
          catch (ArgumentException ex)
          {
          }
        }
        CultureResources.iHasFoundInstalledCultures = true;
      }
      CultureResources.LOG.Info("DetermineAvailableCultures - Leave");
    }

    public static List<CultureInfo> SupportedCultures => CultureResources.iSupportedCultures;

    public Resources GetResourceInstance()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Resources resourceInstance = new Resources();
      return resourceInstance;
    }

    public static ObjectDataProvider ResourceProvider
    {
      get
      {
        if (CultureResources.iProvider == null)
          CultureResources.iProvider = (ObjectDataProvider) Application.Current.FindResource((object) "Resources");
        return CultureResources.iProvider;
      }
    }

    public static void ChangeCulture(CultureInfo culture)
    {
      if (CultureResources.iSupportedCultures.Contains(culture))
      {
        Resources.Culture = culture;
        CultureResources.ResourceProvider.Refresh();
      }
      else
      {
        Resources.Culture = new CultureInfo("en");
        CultureResources.ResourceProvider.Refresh();
      }
      CultureResources.NotifyListeners();
    }

    public static void SetDefaultCulture()
    {
      Resources.Culture = new CultureInfo("en");
      CultureResources.ResourceProvider.Refresh();
    }

    public static string GetValue(string key)
    {
      return Resources.ResourceManager.GetString(key, Resources.Culture);
    }
  }
}
