// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Deployment.Resources.ConfigurationResources
// Assembly: System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 9BAA0F73-1735-489C-B5F8-24E366CE85FF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Deployment.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Web.WebPages.Deployment.Resources
{
  [DebuggerNonUserCode]
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal class ConfigurationResources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal ConfigurationResources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) ConfigurationResources.resourceMan, (object) null))
          ConfigurationResources.resourceMan = new ResourceManager("System.Web.WebPages.Deployment.Resources.ConfigurationResources", typeof (ConfigurationResources).Assembly);
        return ConfigurationResources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => ConfigurationResources.resourceCulture;
      set => ConfigurationResources.resourceCulture = value;
    }

    internal static string InstallPathNotFound
    {
      get
      {
        return ConfigurationResources.ResourceManager.GetString(nameof (InstallPathNotFound), ConfigurationResources.resourceCulture);
      }
    }

    internal static string WebPagesImplicitVersionFailure
    {
      get
      {
        return ConfigurationResources.ResourceManager.GetString(nameof (WebPagesImplicitVersionFailure), ConfigurationResources.resourceCulture);
      }
    }

    internal static string WebPagesRegistryKeyDoesNotExist
    {
      get
      {
        return ConfigurationResources.ResourceManager.GetString(nameof (WebPagesRegistryKeyDoesNotExist), ConfigurationResources.resourceCulture);
      }
    }

    internal static string WebPagesVersionChanges
    {
      get
      {
        return ConfigurationResources.ResourceManager.GetString(nameof (WebPagesVersionChanges), ConfigurationResources.resourceCulture);
      }
    }

    internal static string WebPagesVersionConflict
    {
      get
      {
        return ConfigurationResources.ResourceManager.GetString(nameof (WebPagesVersionConflict), ConfigurationResources.resourceCulture);
      }
    }

    internal static string WebPagesVersionNotFound
    {
      get
      {
        return ConfigurationResources.ResourceManager.GetString(nameof (WebPagesVersionNotFound), ConfigurationResources.resourceCulture);
      }
    }
  }
}
