// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Deployment.PreApplicationStartCode
// Assembly: System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 9BAA0F73-1735-489C-B5F8-24E366CE85FF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Deployment.dll

using Microsoft.Internal.Web.Utils;
using Microsoft.Web.Infrastructure;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Caching;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.WebPages.Deployment.Resources;

#nullable disable
namespace System.Web.WebPages.Deployment
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class PreApplicationStartCode
  {
    private const string ToolingIndicatorKey = "WebPages.VersionChange";
    private static readonly IFileSystem _physicalFileSystem = (IFileSystem) new PhysicalFileSystem();
    private static bool _startWasCalled;

    public static void Start()
    {
      if (PreApplicationStartCode._startWasCalled)
        return;
      PreApplicationStartCode._startWasCalled = true;
      PreApplicationStartCode.StartCore();
    }

    internal static bool StartCore()
    {
      BuildManagerWrapper buildManagerWrapper = new BuildManagerWrapper();
      NameValueCollection appSettings = WebConfigurationManager.AppSettings;
      Action<Version> loadWebPages = new Action<Version>(PreApplicationStartCode.LoadWebPages);
      Action registerForChangeNotification = new Action(PreApplicationStartCode.RegisterForChangeNotifications);
      IEnumerable<AssemblyName> loadedAssemblies = AssemblyUtils.GetLoadedAssemblies();
      return PreApplicationStartCode.StartCore(PreApplicationStartCode._physicalFileSystem, HttpRuntime.AppDomainAppPath, HttpRuntime.BinDirectory, appSettings, loadedAssemblies, (IBuildManager) buildManagerWrapper, loadWebPages, registerForChangeNotification);
    }

    internal static bool StartCore(
      IFileSystem fileSystem,
      string appDomainAppPath,
      string binDirectory,
      NameValueCollection appSettings,
      IEnumerable<AssemblyName> loadedAssemblies,
      IBuildManager buildManager,
      Action<Version> loadWebPages,
      Action registerForChangeNotification,
      Func<string, AssemblyName> getAssemblyNameThunk = null)
    {
      if (WebPagesDeployment.IsExplicitlyDisabled(appSettings))
        return false;
      Version maxWebPagesVersion = AssemblyUtils.GetMaxWebPagesVersion(loadedAssemblies);
      if (AssemblyUtils.ThisAssemblyName.Version != maxWebPagesVersion)
        return false;
      bool flag = WebPagesDeployment.IsEnabled(fileSystem, appDomainAppPath, appSettings);
      Version versionFromBin = AssemblyUtils.GetVersionFromBin(binDirectory, fileSystem, getAssemblyNameThunk);
      Version versionFromConfig = WebPagesDeployment.GetVersionFromConfig(appSettings);
      Version version1 = versionFromConfig;
      if ((object) version1 == null)
        version1 = versionFromBin ?? AssemblyUtils.WebPagesV1Version;
      Version version2 = version1;
      if (versionFromBin != (Version) null && versionFromBin != version2)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ConfigurationResources.WebPagesVersionConflict, new object[2]
        {
          (object) version2,
          (object) versionFromBin
        }));
      if (versionFromBin != (Version) null)
        return false;
      if (!flag)
      {
        registerForChangeNotification();
        return false;
      }
      if (!AssemblyUtils.IsVersionAvailable(loadedAssemblies, version2))
      {
        if (version2 == AssemblyUtils.WebPagesV1Version && versionFromConfig == (Version) null && versionFromBin == (Version) null)
          throw new InvalidOperationException(ConfigurationResources.WebPagesImplicitVersionFailure);
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ConfigurationResources.WebPagesVersionNotFound, new object[2]
        {
          (object) version2,
          (object) AssemblyUtils.ThisAssemblyName.Version
        }));
      }
      PreApplicationStartCode.InvalidateCompilationResultsIfVersionChanged(buildManager, fileSystem, binDirectory, version2);
      loadWebPages(version2);
      return true;
    }

    private static void InvalidateCompilationResultsIfVersionChanged(
      IBuildManager buildManager,
      IFileSystem fileSystem,
      string binDirectory,
      Version currentVersion)
    {
      Version previousRuntimeVersion = WebPagesDeployment.GetPreviousRuntimeVersion(buildManager);
      WebPagesDeployment.PersistRuntimeVersion(buildManager, currentVersion);
      if (!(previousRuntimeVersion == (Version) null) && previousRuntimeVersion != currentVersion)
      {
        WebPagesDeployment.ForceRecompile(fileSystem, binDirectory);
        HttpCompileException compileException = new HttpCompileException(ConfigurationResources.WebPagesVersionChanges);
        compileException.Data[(object) "WebPages.VersionChange"] = (object) true;
        throw compileException;
      }
    }

    internal static ICollection<MethodInfo> GetPreStartInitMethodsFromAssemblyCollection(
      IEnumerable<Assembly> assemblies)
    {
      List<MethodInfo> assemblyCollection = new List<MethodInfo>();
      foreach (Assembly assembly1 in assemblies)
      {
        PreApplicationStartMethodAttribute[] startMethodAttributeArray = (PreApplicationStartMethodAttribute[]) null;
        try
        {
          Assembly assembly2 = assembly1;
          bool flag = true;
          Type attributeType = typeof (PreApplicationStartMethodAttribute);
          int num = flag ? 1 : 0;
          startMethodAttributeArray = (PreApplicationStartMethodAttribute[]) assembly2.GetCustomAttributes(attributeType, num != 0);
        }
        catch
        {
        }
        if (startMethodAttributeArray != null && startMethodAttributeArray.Length != 0)
        {
          PreApplicationStartMethodAttribute startMethodAttribute = startMethodAttributeArray[0];
          MethodInfo methodInfo = (MethodInfo) null;
          if (startMethodAttribute.Type != (Type) null && !string.IsNullOrEmpty(startMethodAttribute.MethodName) && startMethodAttribute.Type.Assembly == assembly1)
            methodInfo = PreApplicationStartCode.FindPreStartInitMethod(startMethodAttribute.Type, startMethodAttribute.MethodName);
          if (methodInfo != (MethodInfo) null)
            assemblyCollection.Add(methodInfo);
        }
      }
      return (ICollection<MethodInfo>) assemblyCollection;
    }

    internal static MethodInfo FindPreStartInitMethod(Type type, string methodName)
    {
      MethodInfo preStartInitMethod = (MethodInfo) null;
      if (type.IsPublic)
      {
        Type type1 = type;
        Binder binder1 = (Binder) null;
        Type[] emptyTypes = Type.EmptyTypes;
        ParameterModifier[] parameterModifierArray = (ParameterModifier[]) null;
        string name = methodName;
        Binder binder2 = binder1;
        Type[] types = emptyTypes;
        ParameterModifier[] modifiers = parameterModifierArray;
        preStartInitMethod = type1.GetMethod(name, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public, binder2, types, modifiers);
      }
      return preStartInitMethod;
    }

    private static void RegisterForChangeNotifications()
    {
      string appDomainAppPath = HttpRuntime.AppDomainAppPath;
      CacheDependency dependencies = new CacheDependency(appDomainAppPath, DateTime.UtcNow);
      HttpRuntime.Cache.Insert(WebPagesDeployment.CacheKeyPrefix + appDomainAppPath, (object) appDomainAppPath, dependencies, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, new CacheItemRemovedCallback(PreApplicationStartCode.OnChanged));
    }

    private static void OnChanged(string key, object value, CacheItemRemovedReason reason)
    {
      if (reason != CacheItemRemovedReason.DependencyChanged)
        return;
      if (WebPagesDeployment.AppRootContainsWebPagesFile(PreApplicationStartCode._physicalFileSystem, HttpRuntime.AppDomainAppPath))
        InfrastructureHelper.UnloadAppDomain();
      else
        PreApplicationStartCode.RegisterForChangeNotifications();
    }

    private static void LoadWebPages(Version version)
    {
      IEnumerable<Assembly> assemblies = AssemblyUtils.GetAssembliesForVersion(version).Select<AssemblyName, Assembly>(new Func<AssemblyName, Assembly>(PreApplicationStartCode.LoadAssembly));
      foreach (Assembly assembly in assemblies)
        BuildManager.AddReferencedAssembly(assembly);
      foreach (MethodBase methodsFromAssembly in (IEnumerable<MethodInfo>) PreApplicationStartCode.GetPreStartInitMethodsFromAssemblyCollection(assemblies))
        methodsFromAssembly.Invoke((object) null, (object[]) null);
    }

    private static Assembly LoadAssembly(AssemblyName name) => Assembly.Load(name);
  }
}
