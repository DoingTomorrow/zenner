// Decompiled with JetBrains decompiler
// Type: NLog.Config.XmlLoggingConfiguration
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Internal;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;
using NLog.Time;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

#nullable disable
namespace NLog.Config
{
  public class XmlLoggingConfiguration : LoggingConfiguration
  {
    private readonly Dictionary<string, bool> _fileMustAutoReloadLookup = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private string _originalFileName;
    private LogFactory _logFactory;

    private ConfigurationItemFactory ConfigurationItemFactory => ConfigurationItemFactory.Default;

    public XmlLoggingConfiguration(string fileName)
      : this(fileName, LogManager.LogFactory)
    {
    }

    public XmlLoggingConfiguration(string fileName, LogFactory logFactory)
      : this(fileName, false, logFactory)
    {
    }

    public XmlLoggingConfiguration(string fileName, bool ignoreErrors)
      : this(fileName, ignoreErrors, LogManager.LogFactory)
    {
    }

    public XmlLoggingConfiguration(string fileName, bool ignoreErrors, LogFactory logFactory)
    {
      this._logFactory = logFactory;
      using (XmlReader fileReader = XmlLoggingConfiguration.CreateFileReader(fileName))
        this.Initialize(fileReader, fileName, ignoreErrors);
    }

    private static XmlReader CreateFileReader(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
        return (XmlReader) null;
      fileName = fileName.Trim();
      return XmlReader.Create(fileName);
    }

    public XmlLoggingConfiguration(XmlReader reader, string fileName)
      : this(reader, fileName, LogManager.LogFactory)
    {
    }

    public XmlLoggingConfiguration(XmlReader reader, string fileName, LogFactory logFactory)
      : this(reader, fileName, false, logFactory)
    {
    }

    public XmlLoggingConfiguration(XmlReader reader, string fileName, bool ignoreErrors)
      : this(reader, fileName, ignoreErrors, LogManager.LogFactory)
    {
    }

    public XmlLoggingConfiguration(
      XmlReader reader,
      string fileName,
      bool ignoreErrors,
      LogFactory logFactory)
    {
      this._logFactory = logFactory;
      this.Initialize(reader, fileName, ignoreErrors);
    }

    internal XmlLoggingConfiguration(XmlElement element, string fileName)
    {
      this._logFactory = LogManager.LogFactory;
      using (StringReader input = new StringReader(element.OuterXml))
        this.Initialize(XmlReader.Create((TextReader) input), fileName, false);
    }

    internal XmlLoggingConfiguration(XmlElement element, string fileName, bool ignoreErrors)
    {
      this._logFactory = LogManager.LogFactory;
      using (StringReader input = new StringReader(element.OuterXml))
        this.Initialize(XmlReader.Create((TextReader) input), fileName, ignoreErrors);
    }

    public static LoggingConfiguration AppConfig
    {
      get => System.Configuration.ConfigurationManager.GetSection("nlog") as LoggingConfiguration;
    }

    public bool? InitializeSucceeded { get; private set; }

    public bool AutoReload
    {
      get
      {
        return this._fileMustAutoReloadLookup.Values.All<bool>((Func<bool, bool>) (mustAutoReload => mustAutoReload));
      }
      set
      {
        foreach (string key in this._fileMustAutoReloadLookup.Keys.ToList<string>())
          this._fileMustAutoReloadLookup[key] = value;
      }
    }

    public override IEnumerable<string> FileNamesToWatch
    {
      get
      {
        return this._fileMustAutoReloadLookup.Where<KeyValuePair<string, bool>>((Func<KeyValuePair<string, bool>, bool>) (entry => entry.Value)).Select<KeyValuePair<string, bool>, string>((Func<KeyValuePair<string, bool>, string>) (entry => entry.Key));
      }
    }

    public override LoggingConfiguration Reload()
    {
      return (LoggingConfiguration) new XmlLoggingConfiguration(this._originalFileName);
    }

    public static IEnumerable<string> GetCandidateConfigFilePaths()
    {
      return LogManager.LogFactory.GetCandidateConfigFilePaths();
    }

    public static void SetCandidateConfigFilePaths(IEnumerable<string> filePaths)
    {
      LogManager.LogFactory.SetCandidateConfigFilePaths(filePaths);
    }

    public static void ResetCandidateConfigFilePath()
    {
      LogManager.LogFactory.ResetCandidateConfigFilePath();
    }

    private static bool IsTargetElement(string name)
    {
      return name.Equals("target", StringComparison.OrdinalIgnoreCase) || name.Equals("wrapper", StringComparison.OrdinalIgnoreCase) || name.Equals("wrapper-target", StringComparison.OrdinalIgnoreCase) || name.Equals("compound-target", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsTargetRefElement(string name)
    {
      return name.Equals("target-ref", StringComparison.OrdinalIgnoreCase) || name.Equals("wrapper-target-ref", StringComparison.OrdinalIgnoreCase) || name.Equals("compound-target-ref", StringComparison.OrdinalIgnoreCase);
    }

    private static string CleanSpaces(string s)
    {
      s = s.Replace(" ", string.Empty);
      return s;
    }

    private static string StripOptionalNamespacePrefix(string attributeValue)
    {
      if (attributeValue == null)
        return (string) null;
      int num = attributeValue.IndexOf(':');
      return num < 0 ? attributeValue : attributeValue.Substring(num + 1);
    }

    private static Target WrapWithAsyncTargetWrapper(Target target)
    {
      AsyncTargetWrapper asyncTargetWrapper = new AsyncTargetWrapper();
      asyncTargetWrapper.WrappedTarget = target;
      asyncTargetWrapper.Name = target.Name;
      target.Name += "_wrapped";
      InternalLogger.Debug<string, string>("Wrapping target '{0}' with AsyncTargetWrapper and renaming to '{1}", asyncTargetWrapper.Name, target.Name);
      target = (Target) asyncTargetWrapper;
      return target;
    }

    private void Initialize(XmlReader reader, string fileName, bool ignoreErrors)
    {
      try
      {
        this.InitializeSucceeded = new bool?();
        this._originalFileName = fileName;
        int content = (int) reader.MoveToContent();
        NLogXmlElement nlogXmlElement = new NLogXmlElement(reader);
        if (fileName != null)
        {
          this.ParseTopLevel(nlogXmlElement, fileName, false);
          InternalLogger.Info<string>("Configured from an XML element in {0}...", fileName);
        }
        else
          this.ParseTopLevel(nlogXmlElement, (string) null, false);
        this.InitializeSucceeded = new bool?(true);
        this.CheckParsingErrors(nlogXmlElement);
        this.CheckUnusedTargets();
      }
      catch (Exception ex)
      {
        this.InitializeSucceeded = new bool?(false);
        if (ex.MustBeRethrownImmediately())
        {
          throw;
        }
        else
        {
          object[] objArray = new object[1]
          {
            (object) fileName
          };
          NLogConfigurationException configurationException = new NLogConfigurationException(ex, "Exception when parsing {0}. ", objArray);
          InternalLogger.Error((Exception) configurationException, "Parsing configuration from {0} failed.", (object) fileName);
          if (!ignoreErrors && configurationException.MustBeRethrown())
            throw configurationException;
        }
      }
    }

    private void CheckParsingErrors(NLogXmlElement rootContentElement)
    {
      string[] array = rootContentElement.GetParsingErrors().ToArray<string>();
      if (!((IEnumerable<string>) array).Any<string>())
        return;
      if (((int) LogManager.ThrowConfigExceptions ?? (LogManager.ThrowExceptions ? 1 : 0)) != 0)
        throw new NLogConfigurationException(string.Join(Environment.NewLine, array));
      foreach (string message in array)
        InternalLogger.Log(NLog.LogLevel.Warn, message);
    }

    private void CheckUnusedTargets()
    {
      if (!this.InitializeSucceeded.HasValue)
        InternalLogger.Warn("Unused target checking is canceled -> initialize not started yet.");
      else if (!this.InitializeSucceeded.Value)
      {
        InternalLogger.Warn("Unused target checking is canceled -> initialize not succeeded.");
      }
      else
      {
        ReadOnlyCollection<Target> configuredNamedTargets = this.ConfiguredNamedTargets;
        InternalLogger.Debug<int, int>("Unused target checking is started... Rule Count: {0}, Target Count: {1}", this.LoggingRules.Count, configuredNamedTargets.Count);
        HashSet<string> targetNamesAtRules = new HashSet<string>(this.GetLoggingRulesThreadSafe().SelectMany<LoggingRule, Target>((Func<LoggingRule, IEnumerable<Target>>) (r => (IEnumerable<Target>) r.Targets)).Select<Target, string>((Func<Target, string>) (t => t.Name)));
        HashSet<string> wrappedTargetNames = new HashSet<string>(configuredNamedTargets.OfType<WrapperTargetBase>().Select<WrapperTargetBase, string>((Func<WrapperTargetBase, string>) (wt => wt.WrappedTarget.Name)));
        int unusedCount = 0;
        configuredNamedTargets.ToList<Target>().ForEach((Action<Target>) (target =>
        {
          if (targetNamesAtRules.Contains(target.Name) || wrappedTargetNames.Contains(target.Name))
            return;
          InternalLogger.Warn<string>("Unused target detected. Add a rule for this target to the configuration. TargetName: {0}", target.Name);
          ++unusedCount;
        }));
        InternalLogger.Debug<int, int, int>("Unused target checking is completed. Total Rule Count: {0}, Total Target Count: {1}, Unused Target Count: {2}", this.LoggingRules.Count, configuredNamedTargets.Count, unusedCount);
      }
    }

    private void ConfigureFromFile(string fileName, bool autoReloadDefault)
    {
      if (this._fileMustAutoReloadLookup.ContainsKey(XmlLoggingConfiguration.GetFileLookupKey(fileName)))
        return;
      this.ParseTopLevel(new NLogXmlElement(fileName), fileName, autoReloadDefault);
    }

    private void ParseTopLevel(NLogXmlElement content, string filePath, bool autoReloadDefault)
    {
      content.AssertName("nlog", "configuration");
      switch (content.LocalName.ToUpperInvariant())
      {
        case "CONFIGURATION":
          this.ParseConfigurationElement(content, filePath, autoReloadDefault);
          break;
        case "NLOG":
          this.ParseNLogElement(content, filePath, autoReloadDefault);
          break;
      }
    }

    private void ParseConfigurationElement(
      NLogXmlElement configurationElement,
      string filePath,
      bool autoReloadDefault)
    {
      InternalLogger.Trace(nameof (ParseConfigurationElement));
      configurationElement.AssertName("configuration");
      foreach (NLogXmlElement nlogElement in configurationElement.Elements("nlog").ToList<NLogXmlElement>())
        this.ParseNLogElement(nlogElement, filePath, autoReloadDefault);
    }

    private void ParseNLogElement(
      NLogXmlElement nlogElement,
      string filePath,
      bool autoReloadDefault)
    {
      InternalLogger.Trace(nameof (ParseNLogElement));
      nlogElement.AssertName("nlog");
      if (nlogElement.GetOptionalBooleanAttribute("useInvariantCulture", false))
        this.DefaultCultureInfo = CultureInfo.InvariantCulture;
      InternalLogger.LogLevel = NLog.LogLevel.FromString(nlogElement.GetOptionalAttribute("internalLogLevel", InternalLogger.LogLevel.Name));
      this.ExceptionLoggingOldStyle = nlogElement.GetOptionalBooleanAttribute("exceptionLoggingOldStyle", false);
      bool booleanAttribute = nlogElement.GetOptionalBooleanAttribute("autoReload", autoReloadDefault);
      if (filePath != null)
        this._fileMustAutoReloadLookup[XmlLoggingConfiguration.GetFileLookupKey(filePath)] = booleanAttribute;
      this._logFactory.ThrowExceptions = nlogElement.GetOptionalBooleanAttribute("throwExceptions", this._logFactory.ThrowExceptions);
      this._logFactory.ThrowConfigExceptions = nlogElement.GetOptionalBooleanAttribute("throwConfigExceptions", this._logFactory.ThrowConfigExceptions);
      this._logFactory.KeepVariablesOnReload = nlogElement.GetOptionalBooleanAttribute("keepVariablesOnReload", this._logFactory.KeepVariablesOnReload);
      InternalLogger.LogToConsole = nlogElement.GetOptionalBooleanAttribute("internalLogToConsole", InternalLogger.LogToConsole);
      InternalLogger.LogToConsoleError = nlogElement.GetOptionalBooleanAttribute("internalLogToConsoleError", InternalLogger.LogToConsoleError);
      InternalLogger.LogFile = nlogElement.GetOptionalAttribute("internalLogFile", InternalLogger.LogFile);
      this.ConfigurationItemFactory.ParseMessageTemplates = nlogElement.GetOptionalBooleanAttribute("parseMessageTemplates", new bool?());
      InternalLogger.LogToTrace = nlogElement.GetOptionalBooleanAttribute("internalLogToTrace", InternalLogger.LogToTrace);
      InternalLogger.IncludeTimestamp = nlogElement.GetOptionalBooleanAttribute("internalLogIncludeTimestamp", InternalLogger.IncludeTimestamp);
      this._logFactory.GlobalThreshold = NLog.LogLevel.FromString(nlogElement.GetOptionalAttribute("globalThreshold", this._logFactory.GlobalThreshold.Name));
      List<NLogXmlElement> list = nlogElement.Children.ToList<NLogXmlElement>();
      foreach (NLogXmlElement extensionsElement in list.Where<NLogXmlElement>((Func<NLogXmlElement, bool>) (child => child.LocalName.Equals("EXTENSIONS", StringComparison.OrdinalIgnoreCase))).ToList<NLogXmlElement>())
        this.ParseExtensionsElement(extensionsElement, Path.GetDirectoryName(filePath));
      List<NLogXmlElement> nlogXmlElementList = new List<NLogXmlElement>();
      foreach (NLogXmlElement nlogXmlElement in list)
      {
        switch (nlogXmlElement.LocalName.ToUpperInvariant())
        {
          case "APPENDERS":
          case "TARGETS":
            this.ParseTargetsElement(nlogXmlElement);
            continue;
          case "EXTENSIONS":
            continue;
          case "INCLUDE":
            this.ParseIncludeElement(nlogXmlElement, Path.GetDirectoryName(filePath), booleanAttribute);
            continue;
          case "RULES":
            nlogXmlElementList.Add(nlogXmlElement);
            continue;
          case "TIME":
            this.ParseTimeElement(nlogXmlElement);
            continue;
          case "VARIABLE":
            this.ParseVariableElement(nlogXmlElement);
            continue;
          default:
            InternalLogger.Warn<string>("Skipping unknown node: {0}", nlogXmlElement.LocalName);
            continue;
        }
      }
      foreach (NLogXmlElement rulesElement in nlogXmlElementList)
        this.ParseRulesElement(rulesElement, this.LoggingRules);
    }

    private void ParseRulesElement(NLogXmlElement rulesElement, IList<LoggingRule> rulesCollection)
    {
      InternalLogger.Trace(nameof (ParseRulesElement));
      rulesElement.AssertName("rules");
      foreach (NLogXmlElement loggerElement in rulesElement.Elements("logger").ToList<NLogXmlElement>())
        this.ParseLoggerElement(loggerElement, rulesCollection);
    }

    private void ParseLoggerElement(
      NLogXmlElement loggerElement,
      IList<LoggingRule> rulesCollection)
    {
      loggerElement.AssertName("logger");
      string optionalAttribute = loggerElement.GetOptionalAttribute("name", "*");
      if (!loggerElement.GetOptionalBooleanAttribute("enabled", true))
      {
        InternalLogger.Debug("The logger named '{0}' are disabled");
      }
      else
      {
        LoggingRule rule = new LoggingRule();
        string str1 = loggerElement.GetOptionalAttribute("appendTo", (string) null) ?? loggerElement.GetOptionalAttribute("writeTo", (string) null);
        rule.LoggerNamePattern = optionalAttribute;
        if (str1 != null)
        {
          string str2 = str1;
          char[] chArray = new char[1]{ ',' };
          foreach (string str3 in str2.Split(chArray))
          {
            string name = str3.Trim();
            if (!string.IsNullOrEmpty(name))
              rule.Targets.Add(this.FindTargetByName(name) ?? throw new NLogConfigurationException("Target " + name + " not found."));
          }
        }
        rule.Final = loggerElement.GetOptionalBooleanAttribute("final", false);
        XmlLoggingConfiguration.ParseLevels(loggerElement, rule);
        foreach (NLogXmlElement nlogXmlElement in loggerElement.Children.ToList<NLogXmlElement>())
        {
          switch (nlogXmlElement.LocalName.ToUpperInvariant())
          {
            case "FILTERS":
              this.ParseFilters(rule, nlogXmlElement);
              continue;
            case "LOGGER":
              this.ParseLoggerElement(nlogXmlElement, rule.ChildRules);
              continue;
            default:
              continue;
          }
        }
        lock (rulesCollection)
          rulesCollection.Add(rule);
      }
    }

    private static void ParseLevels(NLogXmlElement loggerElement, LoggingRule rule)
    {
      string str1;
      if (loggerElement.AttributeValues.TryGetValue("level", out str1))
      {
        NLog.LogLevel level = NLog.LogLevel.FromString(str1);
        rule.EnableLoggingForLevel(level);
      }
      else if (loggerElement.AttributeValues.TryGetValue("levels", out str1))
      {
        string str2 = XmlLoggingConfiguration.CleanSpaces(str1);
        char[] chArray = new char[1]{ ',' };
        foreach (string levelName in str2.Split(chArray))
        {
          if (!string.IsNullOrEmpty(levelName))
          {
            NLog.LogLevel level = NLog.LogLevel.FromString(levelName);
            rule.EnableLoggingForLevel(level);
          }
        }
      }
      else
      {
        int num = 0;
        int ordinal1 = NLog.LogLevel.MaxLevel.Ordinal;
        string levelName1;
        if (loggerElement.AttributeValues.TryGetValue("minLevel", out levelName1))
          num = NLog.LogLevel.FromString(levelName1).Ordinal;
        string levelName2;
        if (loggerElement.AttributeValues.TryGetValue("maxLevel", out levelName2))
          ordinal1 = NLog.LogLevel.FromString(levelName2).Ordinal;
        for (int ordinal2 = num; ordinal2 <= ordinal1; ++ordinal2)
          rule.EnableLoggingForLevel(NLog.LogLevel.FromOrdinal(ordinal2));
      }
    }

    private void ParseFilters(LoggingRule rule, NLogXmlElement filtersElement)
    {
      filtersElement.AssertName("filters");
      foreach (NLogXmlElement element in filtersElement.Children.ToList<NLogXmlElement>())
      {
        NLog.Filters.Filter instance = this.ConfigurationItemFactory.Filters.CreateInstance(element.LocalName);
        this.ConfigureObjectFromAttributes((object) instance, element, false);
        rule.Filters.Add(instance);
      }
    }

    private void ParseVariableElement(NLogXmlElement variableElement)
    {
      variableElement.AssertName("variable");
      this.Variables[variableElement.GetRequiredAttribute("name")] = (SimpleLayout) this.ExpandSimpleVariables(variableElement.GetRequiredAttribute("value"));
    }

    private void ParseTargetsElement(NLogXmlElement targetsElement)
    {
      targetsElement.AssertName("targets", "appenders");
      bool booleanAttribute = targetsElement.GetOptionalBooleanAttribute("async", false);
      NLogXmlElement defaultParameters = (NLogXmlElement) null;
      Dictionary<string, NLogXmlElement> typeNameToDefaultTargetParameters = new Dictionary<string, NLogXmlElement>();
      foreach (NLogXmlElement targetElement in targetsElement.Children.ToList<NLogXmlElement>())
      {
        string localName = targetElement.LocalName;
        string str = XmlLoggingConfiguration.StripOptionalNamespacePrefix(targetElement.GetOptionalAttribute("type", (string) null));
        switch (localName.ToUpperInvariant())
        {
          case "APPENDER":
          case "COMPOUND-TARGET":
          case "TARGET":
          case "WRAPPER":
          case "WRAPPER-TARGET":
            if (str == null)
              throw new NLogConfigurationException("Missing 'type' attribute on <" + localName + "/>.");
            Target target = this.ConfigurationItemFactory.Targets.CreateInstance(str);
            this.ParseTargetElement(target, targetElement, typeNameToDefaultTargetParameters);
            if (booleanAttribute)
              target = XmlLoggingConfiguration.WrapWithAsyncTargetWrapper(target);
            if (defaultParameters != null)
              target = this.WrapWithDefaultWrapper(target, defaultParameters);
            InternalLogger.Info<Target>("Adding target {0}", target);
            this.AddTarget(target.Name, target);
            continue;
          case "DEFAULT-TARGET-PARAMETERS":
            if (str == null)
              throw new NLogConfigurationException("Missing 'type' attribute on <" + localName + "/>.");
            typeNameToDefaultTargetParameters[str] = targetElement;
            continue;
          case "DEFAULT-WRAPPER":
            defaultParameters = targetElement;
            continue;
          default:
            continue;
        }
      }
    }

    private void ParseTargetElement(
      Target target,
      NLogXmlElement targetElement,
      Dictionary<string, NLogXmlElement> typeNameToDefaultTargetParameters = null)
    {
      string key = XmlLoggingConfiguration.StripOptionalNamespacePrefix(targetElement.GetRequiredAttribute("type"));
      NLogXmlElement targetElement1;
      if (typeNameToDefaultTargetParameters != null && typeNameToDefaultTargetParameters.TryGetValue(key, out targetElement1))
        this.ParseTargetElement(target, targetElement1);
      CompoundTargetBase compound = target as CompoundTargetBase;
      WrapperTargetBase wrapper = target as WrapperTargetBase;
      this.ConfigureObjectFromAttributes((object) target, targetElement, true);
      foreach (NLogXmlElement nlogXmlElement in targetElement.Children.ToList<NLogXmlElement>())
      {
        string localName = nlogXmlElement.LocalName;
        if ((compound == null || !this.ParseCompoundTarget(typeNameToDefaultTargetParameters, localName, nlogXmlElement, compound)) && (wrapper == null || !this.ParseTargetWrapper(typeNameToDefaultTargetParameters, localName, nlogXmlElement, wrapper)))
          this.SetPropertyFromElement((object) target, nlogXmlElement);
      }
    }

    private bool ParseTargetWrapper(
      Dictionary<string, NLogXmlElement> typeNameToDefaultTargetParameters,
      string name,
      NLogXmlElement childElement,
      WrapperTargetBase wrapper)
    {
      if (XmlLoggingConfiguration.IsTargetRefElement(name))
      {
        string requiredAttribute = childElement.GetRequiredAttribute(nameof (name));
        wrapper.WrappedTarget = this.FindTargetByName(requiredAttribute) ?? throw new NLogConfigurationException("Referenced target '" + requiredAttribute + "' not found.");
        return true;
      }
      if (!XmlLoggingConfiguration.IsTargetElement(name))
        return false;
      Target instance = this.ConfigurationItemFactory.Targets.CreateInstance(XmlLoggingConfiguration.StripOptionalNamespacePrefix(childElement.GetRequiredAttribute("type")));
      if (instance != null)
      {
        this.ParseTargetElement(instance, childElement, typeNameToDefaultTargetParameters);
        if (instance.Name != null)
          this.AddTarget(instance.Name, instance);
        wrapper.WrappedTarget = wrapper.WrappedTarget == null ? instance : throw new NLogConfigurationException("Wrapped target already defined.");
      }
      return true;
    }

    private bool ParseCompoundTarget(
      Dictionary<string, NLogXmlElement> typeNameToDefaultTargetParameters,
      string name,
      NLogXmlElement childElement,
      CompoundTargetBase compound)
    {
      if (XmlLoggingConfiguration.IsTargetRefElement(name))
      {
        string requiredAttribute = childElement.GetRequiredAttribute(nameof (name));
        compound.Targets.Add(this.FindTargetByName(requiredAttribute) ?? throw new NLogConfigurationException("Referenced target '" + requiredAttribute + "' not found."));
        return true;
      }
      if (!XmlLoggingConfiguration.IsTargetElement(name))
        return false;
      Target instance = this.ConfigurationItemFactory.Targets.CreateInstance(XmlLoggingConfiguration.StripOptionalNamespacePrefix(childElement.GetRequiredAttribute("type")));
      if (instance != null)
      {
        this.ParseTargetElement(instance, childElement, typeNameToDefaultTargetParameters);
        if (instance.Name != null)
          this.AddTarget(instance.Name, instance);
        compound.Targets.Add(instance);
      }
      return true;
    }

    private void ParseExtensionsElement(NLogXmlElement extensionsElement, string baseDirectory)
    {
      extensionsElement.AssertName("extensions");
      foreach (NLogXmlElement nlogXmlElement in extensionsElement.Elements("add").ToList<NLogXmlElement>())
      {
        string optionalAttribute1 = nlogXmlElement.GetOptionalAttribute("prefix", (string) null);
        if (optionalAttribute1 != null)
          optionalAttribute1 += ".";
        string typeName = XmlLoggingConfiguration.StripOptionalNamespacePrefix(nlogXmlElement.GetOptionalAttribute("type", (string) null));
        if (typeName != null)
        {
          try
          {
            this.ConfigurationItemFactory.RegisterType(Type.GetType(typeName, true), optionalAttribute1);
          }
          catch (Exception ex)
          {
            if (ex.MustBeRethrownImmediately())
            {
              throw;
            }
            else
            {
              InternalLogger.Error(ex, "Error loading extensions.");
              NLogConfigurationException exception = new NLogConfigurationException("Error loading extensions: " + typeName, ex);
              if (exception.MustBeRethrown())
                throw exception;
            }
          }
        }
        string optionalAttribute2 = nlogXmlElement.GetOptionalAttribute("assemblyFile", (string) null);
        if (optionalAttribute2 != null)
        {
          this.ParseExtensionWithAssemblyFle(baseDirectory, optionalAttribute2, optionalAttribute1);
        }
        else
        {
          string optionalAttribute3 = nlogXmlElement.GetOptionalAttribute("assembly", (string) null);
          if (optionalAttribute3 != null)
            this.ParseExtensionWithAssembly(optionalAttribute3, optionalAttribute1);
        }
      }
    }

    private void ParseExtensionWithAssembly(string assemblyName, string prefix)
    {
      try
      {
        this.ConfigurationItemFactory.RegisterItemsFromAssembly(AssemblyHelpers.LoadFromName(assemblyName), prefix);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
        {
          throw;
        }
        else
        {
          InternalLogger.Error(ex, "Error loading extensions.");
          NLogConfigurationException exception = new NLogConfigurationException("Error loading extensions: " + assemblyName, ex);
          if (exception.MustBeRethrown())
            throw exception;
        }
      }
    }

    private void ParseExtensionWithAssemblyFle(
      string baseDirectory,
      string assemblyFile,
      string prefix)
    {
      try
      {
        this.ConfigurationItemFactory.RegisterItemsFromAssembly(AssemblyHelpers.LoadFromPath(assemblyFile, baseDirectory), prefix);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
        {
          throw;
        }
        else
        {
          InternalLogger.Error(ex, "Error loading extensions.");
          NLogConfigurationException exception = new NLogConfigurationException("Error loading extensions: " + assemblyFile, ex);
          if (exception.MustBeRethrown())
            throw exception;
        }
      }
    }

    private void ParseIncludeElement(
      NLogXmlElement includeElement,
      string baseDirectory,
      bool autoReloadDefault)
    {
      includeElement.AssertName("include");
      string str1 = includeElement.GetRequiredAttribute("file");
      bool booleanAttribute = includeElement.GetOptionalBooleanAttribute("ignoreErrors", false);
      try
      {
        str1 = this.ExpandSimpleVariables(str1);
        str1 = SimpleLayout.Evaluate(str1);
        string str2 = str1;
        if (baseDirectory != null)
          str2 = Path.Combine(baseDirectory, str1);
        if (File.Exists(str2))
        {
          InternalLogger.Debug<string>("Including file '{0}'", str2);
          this.ConfigureFromFile(str2, autoReloadDefault);
        }
        else if (str1.Contains("*"))
        {
          this.ConfigureFromFilesByMask(baseDirectory, str1, autoReloadDefault);
        }
        else
        {
          if (!booleanAttribute)
            throw new FileNotFoundException("Included file not found: " + str2);
          InternalLogger.Debug<string>("Skipping included file '{0}' as it can't be found", str2);
        }
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "Error when including '{0}'.", (object) str1);
        if (booleanAttribute)
          return;
        if (!ex.MustBeRethrown())
          throw new NLogConfigurationException("Error when including: " + str1, ex);
        throw;
      }
    }

    private void ConfigureFromFilesByMask(
      string baseDirectory,
      string fileMask,
      bool autoReloadDefault)
    {
      string path = baseDirectory;
      if (Path.IsPathRooted(fileMask))
      {
        path = Path.GetDirectoryName(fileMask);
        if (path == null)
        {
          InternalLogger.Warn<string>("directory is empty for include of '{0}'", fileMask);
          return;
        }
        string fileName = Path.GetFileName(fileMask);
        if (fileName == null)
        {
          InternalLogger.Warn<string>("filename is empty for include of '{0}'", fileMask);
          return;
        }
        fileMask = fileName;
      }
      foreach (string file in Directory.GetFiles(path, fileMask))
        this.ConfigureFromFile(file, autoReloadDefault);
    }

    private void ParseTimeElement(NLogXmlElement timeElement)
    {
      timeElement.AssertName("time");
      TimeSource instance = this.ConfigurationItemFactory.TimeSources.CreateInstance(timeElement.GetRequiredAttribute("type"));
      this.ConfigureObjectFromAttributes((object) instance, timeElement, true);
      InternalLogger.Info<TimeSource>("Selecting time source {0}", instance);
      TimeSource.Current = instance;
    }

    private static string GetFileLookupKey(string fileName) => Path.GetFullPath(fileName);

    private void SetPropertyFromElement(object o, NLogXmlElement element)
    {
      if (this.AddArrayItemFromElement(o, element) || this.SetLayoutFromElement(o, element) || this.SetItemFromElement(o, element))
        return;
      string str = this.ExpandSimpleVariables(element.Value);
      try
      {
        PropertyHelper.SetPropertyFromString(o, element.LocalName, str, this.ConfigurationItemFactory);
      }
      catch (NLogConfigurationException ex)
      {
        InternalLogger.Warn<string, string>("Error when setting '{0}' from '<{1}>'", element.LocalName, str);
        throw;
      }
    }

    private bool AddArrayItemFromElement(object o, NLogXmlElement element)
    {
      string localName = element.LocalName;
      PropertyInfo result;
      if (!PropertyHelper.TryGetPropertyInfo(o, localName, out result))
        return false;
      Type arrayItemType = PropertyHelper.GetArrayItemType(result);
      if (!(arrayItemType != (Type) null))
        return false;
      IList list = (IList) result.GetValue(o, (object[]) null);
      object targetObject = (object) this.TryCreateLayoutInstance(element, arrayItemType) ?? FactoryHelper.CreateInstance(arrayItemType);
      this.ConfigureObjectFromAttributes(targetObject, element, true);
      this.ConfigureObjectFromElement(targetObject, element);
      object obj = targetObject;
      list.Add(obj);
      return true;
    }

    private void ConfigureObjectFromAttributes(
      object targetObject,
      NLogXmlElement element,
      bool ignoreType)
    {
      foreach (KeyValuePair<string, string> keyValuePair in element.AttributeValues.ToList<KeyValuePair<string, string>>())
      {
        string key = keyValuePair.Key;
        string input = keyValuePair.Value;
        if (!ignoreType || !key.Equals("type", StringComparison.OrdinalIgnoreCase))
        {
          try
          {
            PropertyHelper.SetPropertyFromString(targetObject, key, this.ExpandSimpleVariables(input), this.ConfigurationItemFactory);
          }
          catch (NLogConfigurationException ex)
          {
            InternalLogger.Warn<string, string>("Error when setting '{0}' on attibute '{1}'", input, key);
            throw;
          }
        }
      }
    }

    private bool SetLayoutFromElement(object o, NLogXmlElement layoutElement)
    {
      string localName = layoutElement.LocalName;
      PropertyInfo result;
      if (PropertyHelper.TryGetPropertyInfo(o, localName, out result))
      {
        Layout layoutInstance = this.TryCreateLayoutInstance(layoutElement, result.PropertyType);
        if (layoutInstance != null)
        {
          this.ConfigureObjectFromAttributes((object) layoutInstance, layoutElement, true);
          this.ConfigureObjectFromElement((object) layoutInstance, layoutElement);
          result.SetValue(o, (object) layoutInstance, (object[]) null);
          return true;
        }
      }
      return false;
    }

    private bool SetItemFromElement(object o, NLogXmlElement element)
    {
      if (element.Value != null)
        return false;
      string localName = element.LocalName;
      PropertyInfo result;
      if (!PropertyHelper.TryGetPropertyInfo(o, localName, out result))
        return false;
      object targetObject = result.GetValue(o, (object[]) null);
      this.ConfigureObjectFromAttributes(targetObject, element, true);
      this.ConfigureObjectFromElement(targetObject, element);
      return true;
    }

    private void ConfigureObjectFromElement(object targetObject, NLogXmlElement element)
    {
      foreach (NLogXmlElement element1 in element.Children.ToList<NLogXmlElement>())
        this.SetPropertyFromElement(targetObject, element1);
    }

    private Target WrapWithDefaultWrapper(Target t, NLogXmlElement defaultParameters)
    {
      Target instance = this.ConfigurationItemFactory.Targets.CreateInstance(XmlLoggingConfiguration.StripOptionalNamespacePrefix(defaultParameters.GetRequiredAttribute("type")));
      if (!(instance is WrapperTargetBase wrapperTargetBase2))
        throw new NLogConfigurationException("Target type specified on <default-wrapper /> is not a wrapper.");
      this.ParseTargetElement(instance, defaultParameters);
      while (wrapperTargetBase2.WrappedTarget != null)
      {
        if (!(wrapperTargetBase2.WrappedTarget is WrapperTargetBase wrapperTargetBase2))
          throw new NLogConfigurationException("Child target type specified on <default-wrapper /> is not a wrapper.");
      }
      wrapperTargetBase2.WrappedTarget = t;
      instance.Name = t.Name;
      t.Name += "_wrapped";
      InternalLogger.Debug<string, string, string>("Wrapping target '{0}' with '{1}' and renaming to '{2}", instance.Name, instance.GetType().Name, t.Name);
      return instance;
    }

    private Layout TryCreateLayoutInstance(NLogXmlElement element, Type type)
    {
      if (!typeof (Layout).IsAssignableFrom(type))
        return (Layout) null;
      string input = XmlLoggingConfiguration.StripOptionalNamespacePrefix(element.GetOptionalAttribute(nameof (type), (string) null));
      return input == null ? (Layout) null : this.ConfigurationItemFactory.Layouts.CreateInstance(this.ExpandSimpleVariables(input));
    }

    private string ExpandSimpleVariables(string input)
    {
      string str = input;
      foreach (KeyValuePair<string, SimpleLayout> keyValuePair in this.Variables.ToList<KeyValuePair<string, SimpleLayout>>())
      {
        SimpleLayout simpleLayout = keyValuePair.Value;
        if (simpleLayout != null)
          str = str.Replace("${" + keyValuePair.Key + "}", simpleLayout.OriginalText);
      }
      return str;
    }
  }
}
