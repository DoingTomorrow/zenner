// Decompiled with JetBrains decompiler
// Type: NLog.Config.ConfigurationItemFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Filters;
using NLog.Internal;
using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog.Targets;
using NLog.Time;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;

#nullable disable
namespace NLog.Config
{
  public class ConfigurationItemFactory
  {
    private static ConfigurationItemFactory defaultInstance;
    private readonly IList<object> _allFactories;
    private readonly Factory<Target, TargetAttribute> _targets;
    private readonly Factory<NLog.Filters.Filter, FilterAttribute> _filters;
    private readonly LayoutRendererFactory _layoutRenderers;
    private readonly Factory<Layout, LayoutAttribute> _layouts;
    private readonly MethodFactory<ConditionMethodsAttribute, ConditionMethodAttribute> _conditionMethods;
    private readonly Factory<LayoutRenderer, AmbientPropertyAttribute> _ambientProperties;
    private readonly Factory<TimeSource, TimeSourceAttribute> _timeSources;
    private IJsonConverter _jsonSerializer = (IJsonConverter) DefaultJsonSerializer.Instance;

    public static event EventHandler<AssemblyLoadingEventArgs> AssemblyLoading;

    public ConfigurationItemFactory(params Assembly[] assemblies)
    {
      this.CreateInstance = new ConfigurationItemCreator(FactoryHelper.CreateInstance);
      this._targets = new Factory<Target, TargetAttribute>(this);
      this._filters = new Factory<NLog.Filters.Filter, FilterAttribute>(this);
      this._layoutRenderers = new LayoutRendererFactory(this);
      this._layouts = new Factory<Layout, LayoutAttribute>(this);
      this._conditionMethods = new MethodFactory<ConditionMethodsAttribute, ConditionMethodAttribute>();
      this._ambientProperties = new Factory<LayoutRenderer, AmbientPropertyAttribute>(this);
      this._timeSources = new Factory<TimeSource, TimeSourceAttribute>(this);
      this._allFactories = (IList<object>) new List<object>()
      {
        (object) this._targets,
        (object) this._filters,
        (object) this._layoutRenderers,
        (object) this._layouts,
        (object) this._conditionMethods,
        (object) this._ambientProperties,
        (object) this._timeSources
      };
      foreach (Assembly assembly in assemblies)
        this.RegisterItemsFromAssembly(assembly);
    }

    public static ConfigurationItemFactory Default
    {
      get
      {
        return ConfigurationItemFactory.defaultInstance ?? (ConfigurationItemFactory.defaultInstance = ConfigurationItemFactory.BuildDefaultFactory());
      }
      set => ConfigurationItemFactory.defaultInstance = value;
    }

    public ConfigurationItemCreator CreateInstance { get; set; }

    public INamedItemFactory<Target, Type> Targets
    {
      get => (INamedItemFactory<Target, Type>) this._targets;
    }

    public INamedItemFactory<NLog.Filters.Filter, Type> Filters
    {
      get => (INamedItemFactory<NLog.Filters.Filter, Type>) this._filters;
    }

    internal LayoutRendererFactory GetLayoutRenderers() => this._layoutRenderers;

    public INamedItemFactory<LayoutRenderer, Type> LayoutRenderers
    {
      get => (INamedItemFactory<LayoutRenderer, Type>) this._layoutRenderers;
    }

    public INamedItemFactory<Layout, Type> Layouts
    {
      get => (INamedItemFactory<Layout, Type>) this._layouts;
    }

    public INamedItemFactory<LayoutRenderer, Type> AmbientProperties
    {
      get => (INamedItemFactory<LayoutRenderer, Type>) this._ambientProperties;
    }

    [Obsolete("Use JsonConverter property instead. Marked obsolete on NLog 4.5")]
    public IJsonSerializer JsonSerializer
    {
      get => this._jsonSerializer as IJsonSerializer;
      set
      {
        this._jsonSerializer = value != null ? (IJsonConverter) new JsonConverterLegacy(value) : (IJsonConverter) DefaultJsonSerializer.Instance;
      }
    }

    public IJsonConverter JsonConverter
    {
      get => this._jsonSerializer;
      set => this._jsonSerializer = value ?? (IJsonConverter) DefaultJsonSerializer.Instance;
    }

    public IValueFormatter ValueFormatter
    {
      get => NLog.MessageTemplates.ValueFormatter.Instance;
      set => NLog.MessageTemplates.ValueFormatter.Instance = value;
    }

    public bool? ParseMessageTemplates
    {
      get
      {
        if (LogEventInfo.DefaultMessageFormatter == LogEventInfo.StringFormatMessageFormatter)
          return new bool?(false);
        return LogEventInfo.DefaultMessageFormatter == LogMessageTemplateFormatter.Default.MessageFormatter ? new bool?(true) : new bool?();
      }
      set => LogEventInfo.SetDefaultMessageFormatter(value);
    }

    public INamedItemFactory<TimeSource, Type> TimeSources
    {
      get => (INamedItemFactory<TimeSource, Type>) this._timeSources;
    }

    public INamedItemFactory<MethodInfo, MethodInfo> ConditionMethods
    {
      get => (INamedItemFactory<MethodInfo, MethodInfo>) this._conditionMethods;
    }

    public void RegisterItemsFromAssembly(Assembly assembly)
    {
      this.RegisterItemsFromAssembly(assembly, string.Empty);
    }

    public void RegisterItemsFromAssembly(Assembly assembly, string itemNamePrefix)
    {
      if (ConfigurationItemFactory.AssemblyLoading != null)
      {
        AssemblyLoadingEventArgs e = new AssemblyLoadingEventArgs(assembly);
        ConfigurationItemFactory.AssemblyLoading((object) this, e);
        if (e.Cancel)
        {
          InternalLogger.Info<string>("Loading assembly '{0}' is canceled", assembly.FullName);
          return;
        }
      }
      InternalLogger.Debug<string>("ScanAssembly('{0}')", assembly.FullName);
      Type[] types = assembly.SafeGetTypes();
      this.PreloadAssembly(types);
      foreach (IFactory allFactory in (IEnumerable<object>) this._allFactories)
        allFactory.ScanTypes(types, itemNamePrefix);
    }

    public void PreloadAssembly(Type[] typesToScan)
    {
      foreach (Type type in ((IEnumerable<Type>) typesToScan).Where<Type>((Func<Type, bool>) (t => t.Name.Equals("NLogPackageLoader", StringComparison.OrdinalIgnoreCase))))
        ConfigurationItemFactory.CallPreload(type);
    }

    private static void CallPreload(Type type)
    {
      if (!(type != (Type) null))
        return;
      InternalLogger.Debug<string>("Found for preload'{0}'", type.FullName);
      MethodInfo method = type.GetMethod("Preload");
      if (method != (MethodInfo) null)
      {
        if (method.IsStatic)
        {
          InternalLogger.Debug("NLogPackageLoader contains Preload method");
          try
          {
            method.Invoke((object) null, (object[]) null);
            InternalLogger.Debug<string>("Preload succesfully invoked for '{0}'", type.FullName);
          }
          catch (Exception ex)
          {
            object[] objArray = new object[1]
            {
              (object) type.FullName
            };
            InternalLogger.Warn(ex, "Invoking Preload for '{0}' failed", objArray);
          }
        }
        else
          InternalLogger.Debug("NLogPackageLoader contains a preload method, but isn't static");
      }
      else
        InternalLogger.Debug<string>("{0} doesn't contain Preload method", type.FullName);
    }

    public void Clear()
    {
      foreach (IFactory allFactory in (IEnumerable<object>) this._allFactories)
        allFactory.Clear();
    }

    public void RegisterType(Type type, string itemNamePrefix)
    {
      foreach (IFactory allFactory in (IEnumerable<object>) this._allFactories)
        allFactory.RegisterType(type, itemNamePrefix);
    }

    private static ConfigurationItemFactory BuildDefaultFactory()
    {
      Assembly assembly1 = typeof (ILogger).GetAssembly();
      ConfigurationItemFactory configurationItemFactory = new ConfigurationItemFactory(new Assembly[1]
      {
        assembly1
      });
      configurationItemFactory.RegisterExtendedItems();
      try
      {
        string str = ConfigurationItemFactory.GetAssemblyFileLocation(assembly1);
        string[] nlogExtensionFiles = ConfigurationItemFactory.GetNLogExtensionFiles(str);
        if (nlogExtensionFiles.Length == 0)
        {
          string assemblyFileLocation = ConfigurationItemFactory.GetAssemblyFileLocation(Assembly.GetEntryAssembly());
          if (!string.IsNullOrEmpty(assemblyFileLocation) && !string.Equals(assemblyFileLocation, str, StringComparison.OrdinalIgnoreCase))
          {
            str = assemblyFileLocation;
            nlogExtensionFiles = ConfigurationItemFactory.GetNLogExtensionFiles(assemblyFileLocation);
          }
          else
          {
            string baseDirectory = LogFactory.CurrentAppDomain.BaseDirectory;
            if (!string.IsNullOrEmpty(baseDirectory) && !string.Equals(baseDirectory, str, StringComparison.OrdinalIgnoreCase))
            {
              str = baseDirectory;
              nlogExtensionFiles = ConfigurationItemFactory.GetNLogExtensionFiles(baseDirectory);
            }
          }
        }
        InternalLogger.Debug<string>("Start auto loading, location: {0}", str);
        HashSet<string> stringSet = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
        stringSet.Add(assembly1.FullName);
        foreach (string assemblyFileName in nlogExtensionFiles)
        {
          InternalLogger.Info<string>("Auto loading assembly file: {0}", assemblyFileName);
          bool flag = false;
          try
          {
            Assembly assembly2 = AssemblyHelpers.LoadFromPath(assemblyFileName);
            InternalLogger.LogAssemblyVersion(assembly2);
            configurationItemFactory.RegisterItemsFromAssembly(assembly2);
            stringSet.Add(assembly2.FullName);
            flag = true;
          }
          catch (Exception ex)
          {
            if (ex.MustBeRethrownImmediately())
            {
              throw;
            }
            else
            {
              object[] objArray = new object[1]
              {
                (object) assemblyFileName
              };
              InternalLogger.Warn(ex, "Auto loading assembly file: {0} failed! Skipping this file.", objArray);
            }
          }
          if (flag)
            InternalLogger.Info<string>("Auto loading assembly file: {0} succeeded!", assemblyFileName);
        }
        foreach (Assembly assembly3 in LogFactory.CurrentAppDomain.GetAssemblies())
        {
          if (assembly3.FullName.StartsWith("NLog.", StringComparison.OrdinalIgnoreCase) && !stringSet.Contains(assembly3.FullName))
            configurationItemFactory.RegisterItemsFromAssembly(assembly3);
          if (assembly3.FullName.StartsWith("NLog.Extensions.Logging,", StringComparison.OrdinalIgnoreCase) || assembly3.FullName.StartsWith("NLog.Web,", StringComparison.OrdinalIgnoreCase) || assembly3.FullName.StartsWith("NLog.Web.AspNetCore,", StringComparison.OrdinalIgnoreCase) || assembly3.FullName.StartsWith("Microsoft.Extensions.Logging,", StringComparison.OrdinalIgnoreCase) || assembly3.FullName.StartsWith("Microsoft.Extensions.Logging.Abstractions,", StringComparison.OrdinalIgnoreCase) || assembly3.FullName.StartsWith("Microsoft.Extensions.Logging.Filter,", StringComparison.OrdinalIgnoreCase) || assembly3.FullName.StartsWith("Microsoft.Logging,", StringComparison.OrdinalIgnoreCase))
            LogManager.AddHiddenAssembly(assembly3);
        }
      }
      catch (SecurityException ex)
      {
        InternalLogger.Warn((Exception) ex, "Seems that we do not have permission");
        if (ex.MustBeRethrown())
          throw;
      }
      catch (UnauthorizedAccessException ex)
      {
        InternalLogger.Warn((Exception) ex, "Seems that we do not have permission");
        if (ex.MustBeRethrown())
          throw;
      }
      InternalLogger.Debug("Auto loading done");
      return configurationItemFactory;
    }

    private static string GetAssemblyFileLocation(Assembly assembly)
    {
      string str = string.Empty;
      try
      {
        if (assembly == (Assembly) null)
          return string.Empty;
        str = assembly.FullName;
        Uri result;
        if (!Uri.TryCreate(assembly.CodeBase, UriKind.RelativeOrAbsolute, out result))
        {
          InternalLogger.Warn<string, string>("Skipping auto loading location because code base is unknown: '{0}' ({1})", assembly.CodeBase, str);
          return string.Empty;
        }
        string directoryName = Path.GetDirectoryName(result.LocalPath);
        if (string.IsNullOrEmpty(directoryName))
        {
          InternalLogger.Warn<string, string>("Skipping auto loading location because it is not a valid directory: '{0}' ({1})", result.LocalPath, str);
          return string.Empty;
        }
        if (Directory.Exists(directoryName))
          return string.Empty;
        InternalLogger.Warn<string, string>("Skipping auto loading location because directory doesn't exists: '{0}' ({1})", directoryName, str);
        return string.Empty;
      }
      catch (PlatformNotSupportedException ex)
      {
        InternalLogger.Warn((Exception) ex, "Skipping auto loading location because assembly lookup is not supported: {0}", (object) str);
        if (!ex.MustBeRethrown())
          return string.Empty;
        throw;
      }
      catch (SecurityException ex)
      {
        InternalLogger.Warn((Exception) ex, "Skipping auto loading location because assembly lookup is not allowed: {0}", (object) str);
        if (!ex.MustBeRethrown())
          return string.Empty;
        throw;
      }
      catch (UnauthorizedAccessException ex)
      {
        InternalLogger.Warn((Exception) ex, "Skipping auto loading location because assembly lookup is not allowed: {0}", (object) str);
        if (!ex.MustBeRethrown())
          return string.Empty;
        throw;
      }
    }

    private static string[] GetNLogExtensionFiles(string assemblyLocation)
    {
      try
      {
        if (string.IsNullOrEmpty(assemblyLocation))
          return ArrayHelper.Empty<string>();
        InternalLogger.Debug<string>("Search for auto loading files, location: {0}", assemblyLocation);
        return ((IEnumerable<string>) Directory.GetFiles(assemblyLocation, "NLog*.dll")).Select<string, string>(new Func<string, string>(Path.GetFileName)).Where<string>((Func<string, bool>) (x => !x.Equals("NLog.dll", StringComparison.OrdinalIgnoreCase))).Where<string>((Func<string, bool>) (x => !x.Equals("NLog.UnitTests.dll", StringComparison.OrdinalIgnoreCase))).Where<string>((Func<string, bool>) (x => !x.Equals("NLog.Extended.dll", StringComparison.OrdinalIgnoreCase))).Select<string, string>((Func<string, string>) (x => Path.Combine(assemblyLocation, x))).ToArray<string>();
      }
      catch (DirectoryNotFoundException ex)
      {
        InternalLogger.Warn((Exception) ex, "Skipping auto loading location because assembly directory does not exist: {0}", (object) assemblyLocation);
        if (!ex.MustBeRethrown())
          return ArrayHelper.Empty<string>();
        throw;
      }
      catch (SecurityException ex)
      {
        InternalLogger.Warn((Exception) ex, "Skipping auto loading location because access not allowed to assembly directory: {0}", (object) assemblyLocation);
        if (!ex.MustBeRethrown())
          return ArrayHelper.Empty<string>();
        throw;
      }
      catch (UnauthorizedAccessException ex)
      {
        InternalLogger.Warn((Exception) ex, "Skipping auto loading location because access not allowed to assembly directory: {0}", (object) assemblyLocation);
        if (!ex.MustBeRethrown())
          return ArrayHelper.Empty<string>();
        throw;
      }
    }

    private void RegisterExtendedItems()
    {
      string assemblyQualifiedName = typeof (ILogger).AssemblyQualifiedName;
      string str1 = "NLog,";
      string str2 = "NLog.Extended,";
      int num = assemblyQualifiedName.IndexOf(str1, StringComparison.OrdinalIgnoreCase);
      if (num < 0)
        return;
      string str3 = ", " + str2 + assemblyQualifiedName.Substring(num + str1.Length);
      string str4 = typeof (DebugTarget).Namespace;
      this._targets.RegisterNamedType("AspNetTrace", str4 + ".AspNetTraceTarget" + str3);
      this._targets.RegisterNamedType("MSMQ", str4 + ".MessageQueueTarget" + str3);
      this._targets.RegisterNamedType("AspNetBufferingWrapper", str4 + ".Wrappers.AspNetBufferingTargetWrapper" + str3);
      string str5 = typeof (MessageLayoutRenderer).Namespace;
      this._layoutRenderers.RegisterNamedType("appsetting", str5 + ".AppSettingLayoutRenderer" + str3);
      this._layoutRenderers.RegisterNamedType("aspnet-application", str5 + ".AspNetApplicationValueLayoutRenderer" + str3);
      this._layoutRenderers.RegisterNamedType("aspnet-request", str5 + ".AspNetRequestValueLayoutRenderer" + str3);
      this._layoutRenderers.RegisterNamedType("aspnet-sessionid", str5 + ".AspNetSessionIDLayoutRenderer" + str3);
      this._layoutRenderers.RegisterNamedType("aspnet-session", str5 + ".AspNetSessionValueLayoutRenderer" + str3);
      this._layoutRenderers.RegisterNamedType("aspnet-user-authtype", str5 + ".AspNetUserAuthTypeLayoutRenderer" + str3);
      this._layoutRenderers.RegisterNamedType("aspnet-user-identity", str5 + ".AspNetUserIdentityLayoutRenderer" + str3);
    }
  }
}
