// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Deployment.WebPagesDeployment
// Assembly: System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 9BAA0F73-1735-489C-B5F8-24E366CE85FF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Deployment.dll

using Microsoft.Internal.Web.Utils;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.WebPages.Deployment.Resources;

#nullable disable
namespace System.Web.WebPages.Deployment
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class WebPagesDeployment
  {
    private const string AppSettingsVersionKey = "webpages:Version";
    private const string AppSettingsEnabledKey = "webpages:Enabled";
    private const string ForceRecompilationFile = "WebPagesRecompilation.deleteme";
    private const string WebPagesRegistryKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\ASP.NET Web Pages\\v{0}.{1}";
    internal static readonly string CacheKeyPrefix = "__System.Web.WebPages.Deployment__";
    private static readonly string[] _webPagesExtensions = new string[2]
    {
      ".cshtml",
      ".vbhtml"
    };
    private static readonly object _installPathNotFound = new object();
    private static readonly IFileSystem _fileSystem = (IFileSystem) new PhysicalFileSystem();

    public static Version GetVersionWithoutEnabledCheck(string path)
    {
      return WebPagesDeployment.GetVersionWithoutEnabledCheckInternal(path, AssemblyUtils.WebPagesV1Version);
    }

    public static Version GetExplicitWebPagesVersion(string path)
    {
      Version defaultVersion = (Version) null;
      return WebPagesDeployment.GetVersionWithoutEnabledCheckInternal(path, defaultVersion);
    }

    [Obsolete("This method is obsolete and is meant for legacy code. Use GetVersionWithoutEnabled instead.")]
    public static Version GetVersion(string path)
    {
      return WebPagesDeployment.GetObsoleteVersionInternal(path, WebPagesDeployment.GetAppSettings(path), (IFileSystem) new PhysicalFileSystem());
    }

    internal static Version GetObsoleteVersionInternal(
      string path,
      NameValueCollection configuration,
      IFileSystem fileSystem)
    {
      Version binVersion = !string.IsNullOrEmpty(path) ? AssemblyUtils.GetVersionFromBin(WebPagesDeployment.GetBinDirectory(path), WebPagesDeployment._fileSystem) : throw ExceptionHelper.CreateArgumentNullOrEmptyException(nameof (path));
      Version defaultVersion = (Version) null;
      Version versionInternal = WebPagesDeployment.GetVersionInternal(configuration, binVersion, defaultVersion);
      if (versionInternal != (Version) null)
        return versionInternal;
      return WebPagesDeployment.AppRootContainsWebPagesFile(fileSystem, path) ? AssemblyUtils.WebPagesV1Version : (Version) null;
    }

    public static Version GetMaxVersion() => AssemblyUtils.GetMaxWebPagesVersion();

    public static bool IsEnabled(string path)
    {
      return !string.IsNullOrEmpty(path) ? WebPagesDeployment.IsEnabled(WebPagesDeployment._fileSystem, path, WebPagesDeployment.GetAppSettings(path)) : throw ExceptionHelper.CreateArgumentNullOrEmptyException(nameof (path));
    }

    public static bool IsExplicitlyDisabled(string path)
    {
      return !string.IsNullOrEmpty(path) ? WebPagesDeployment.IsExplicitlyDisabled(WebPagesDeployment.GetAppSettings(path)) : throw ExceptionHelper.CreateArgumentNullOrEmptyException(nameof (path));
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static IDictionary<string, Version> GetIncompatibleDependencies(string appPath)
    {
      string configPath = !string.IsNullOrEmpty(appPath) ? Path.Combine(appPath, "web.config") : throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (appPath));
      return AssemblyUtils.GetAssembliesMatchingOtherVersions(AppDomainHelper.GetBinAssemblyReferences(appPath, configPath));
    }

    internal static bool IsExplicitlyDisabled(NameValueCollection appSettings)
    {
      bool? enabled = WebPagesDeployment.GetEnabled(appSettings);
      return enabled.HasValue && !enabled.Value;
    }

    internal static bool IsEnabled(
      IFileSystem fileSystem,
      string path,
      NameValueCollection appSettings)
    {
      bool? enabled = WebPagesDeployment.GetEnabled(appSettings);
      return !enabled.HasValue ? WebPagesDeployment.AppRootContainsWebPagesFile(fileSystem, path) : enabled.Value;
    }

    private static bool? GetEnabled(NameValueCollection appSettings)
    {
      string str = appSettings.Get("webpages:Enabled");
      return string.IsNullOrEmpty(str) ? new bool?() : new bool?(bool.Parse(str));
    }

    internal static Version GetVersionInternal(
      NameValueCollection appSettings,
      Version binVersion,
      Version defaultVersion)
    {
      Version versionFromConfig = WebPagesDeployment.GetVersionFromConfig(appSettings);
      if ((object) versionFromConfig != null)
        return versionFromConfig;
      Version version = binVersion;
      return (object) version != null ? version : defaultVersion;
    }

    private static Version GetVersionWithoutEnabledCheckInternal(
      string path,
      Version defaultVersion)
    {
      Version binVersion = !string.IsNullOrEmpty(path) ? AssemblyUtils.GetVersionFromBin(WebPagesDeployment.GetBinDirectory(path), WebPagesDeployment._fileSystem) : throw ExceptionHelper.CreateArgumentNullOrEmptyException(nameof (path));
      return WebPagesDeployment.GetVersionInternal(WebPagesDeployment.GetAppSettings(path), binVersion, defaultVersion);
    }

    public static string GetAssemblyPath(Version version)
    {
      if (version == (Version) null)
        throw new ArgumentNullException(nameof (version));
      string keyName = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\ASP.NET Web Pages\\v{0}.{1}", new object[2]
      {
        (object) version.Major,
        (object) version.Minor
      });
      object path1 = Registry.GetValue(keyName, "InstallPath", WebPagesDeployment._installPathNotFound);
      if (path1 == null)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ConfigurationResources.WebPagesRegistryKeyDoesNotExist, new object[1]
        {
          (object) keyName
        }));
      return path1 != WebPagesDeployment._installPathNotFound ? Path.Combine((string) path1, "Assemblies") : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ConfigurationResources.InstallPathNotFound, new object[1]
      {
        (object) keyName
      }));
    }

    public static IEnumerable<AssemblyName> GetWebPagesAssemblies()
    {
      return AssemblyUtils.GetAssembliesForVersion(AssemblyUtils.ThisAssemblyName.Version);
    }

    private static NameValueCollection GetAppSettings(string path)
    {
      if (path.StartsWith("~/", StringComparison.Ordinal))
        return (NameValueCollection) WebConfigurationManager.GetSection("appSettings", path);
      AppSettingsSection appSettings1 = WebConfigurationManager.OpenMappedWebConfiguration(new WebConfigurationFileMap()
      {
        VirtualDirectories = {
          {
            "/",
            new VirtualDirectoryMapping(path, true)
          }
        }
      }, "/").AppSettings;
      NameValueCollection appSettings2 = new NameValueCollection();
      foreach (KeyValueConfigurationElement setting in (ConfigurationElementCollection) appSettings1.Settings)
        appSettings2.Add(setting.Key, setting.Value);
      return appSettings2;
    }

    internal static Version GetVersionFromConfig(NameValueCollection appSettings)
    {
      string version = appSettings.Get("webpages:Version");
      if (string.IsNullOrEmpty(version))
        return (Version) null;
      Version versionFromConfig = new Version(version);
      if (versionFromConfig.Build == -1 || versionFromConfig.Revision == -1)
        versionFromConfig = new Version(versionFromConfig.Major, versionFromConfig.Minor, versionFromConfig.Build == -1 ? 0 : versionFromConfig.Build, versionFromConfig.Revision == -1 ? 0 : versionFromConfig.Revision);
      return versionFromConfig;
    }

    internal static bool AppRootContainsWebPagesFile(IFileSystem fileSystem, string path)
    {
      return fileSystem.EnumerateFiles(path).Any<string>(new Func<string, bool>(WebPagesDeployment.IsWebPagesFile));
    }

    private static bool IsWebPagesFile(string file)
    {
      string extension = Path.GetExtension(file);
      return ((IEnumerable<string>) WebPagesDeployment._webPagesExtensions).Contains<string>(extension, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    private static string GetBinDirectory(string path)
    {
      return HostingEnvironment.IsHosted ? HttpRuntime.BinDirectory : Path.Combine(path, "bin");
    }

    internal static Version GetPreviousRuntimeVersion(IBuildManager buildManagerFileSystem)
    {
      string cachedFileName = WebPagesDeployment.GetCachedFileName();
      try
      {
        Stream stream = buildManagerFileSystem.ReadCachedFile(cachedFileName);
        if (stream == null)
          return (Version) null;
        using (StreamReader streamReader = new StreamReader(stream))
        {
          Version result;
          if (Version.TryParse(streamReader.ReadLine(), out result))
            return result;
        }
      }
      catch
      {
      }
      return (Version) null;
    }

    internal static void PersistRuntimeVersion(IBuildManager buildManager, Version version)
    {
      string cachedFileName = WebPagesDeployment.GetCachedFileName();
      try
      {
        using (StreamWriter streamWriter = new StreamWriter(buildManager.CreateCachedFile(cachedFileName)))
          streamWriter.WriteLine(version.ToString());
      }
      catch
      {
      }
    }

    internal static void ForceRecompile(IFileSystem fileSystem, string binDirectory)
    {
      string path = Path.Combine(binDirectory, "WebPagesRecompilation.deleteme");
      try
      {
        using (StreamWriter streamWriter = new StreamWriter(fileSystem.OpenFile(path)))
          streamWriter.WriteLine();
      }
      catch
      {
      }
    }

    private static string GetCachedFileName() => typeof (WebPagesDeployment).Namespace;

    private static string RemoveTrailingSlash(string path)
    {
      if (!string.IsNullOrEmpty(path))
        path = path.TrimEnd(Path.DirectorySeparatorChar);
      return path;
    }
  }
}
