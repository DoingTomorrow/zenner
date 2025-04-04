// Decompiled with JetBrains decompiler
// Type: NLog.LogFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using JetBrains.Annotations;
using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Internal.Fakeables;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog
{
  public class LogFactory : IDisposable
  {
    private const int ReconfigAfterFileChangedTimeout = 1000;
    internal Timer _reloadTimer;
    private readonly MultiFileWatcher _watcher;
    private static readonly TimeSpan DefaultFlushTimeout = TimeSpan.FromSeconds(15.0);
    private static IAppDomain currentAppDomain;
    internal readonly object _syncRoot = new object();
    private LoggingConfiguration _config;
    private LogLevel _globalThreshold = LogLevel.MinLevel;
    private bool _configLoaded;
    private int _logsEnabled;
    private readonly LogFactory.LoggerCache _loggerCache = new LogFactory.LoggerCache();
    private List<string> _candidateConfigFilePaths;
    private bool _isDisposing;

    public event EventHandler<LoggingConfigurationChangedEventArgs> ConfigurationChanged;

    public event EventHandler<LoggingConfigurationReloadedEventArgs> ConfigurationReloaded;

    private static event EventHandler<EventArgs> LoggerShutdown;

    static LogFactory() => LogFactory.RegisterEvents(LogFactory.CurrentAppDomain);

    public LogFactory()
    {
      this._watcher = new MultiFileWatcher();
      this._watcher.FileChanged += new FileSystemEventHandler(this.ConfigFileChanged);
      LogFactory.LoggerShutdown += new EventHandler<EventArgs>(this.OnStopLogging);
    }

    public LogFactory(LoggingConfiguration config)
      : this()
    {
      this.Configuration = config;
    }

    public static IAppDomain CurrentAppDomain
    {
      get
      {
        return LogFactory.currentAppDomain ?? (LogFactory.currentAppDomain = (IAppDomain) new AppDomainWrapper(AppDomain.CurrentDomain));
      }
      set
      {
        LogFactory.UnregisterEvents(LogFactory.currentAppDomain);
        LogFactory.UnregisterEvents(value);
        LogFactory.RegisterEvents(value);
        LogFactory.currentAppDomain = value;
      }
    }

    public bool ThrowExceptions { get; set; }

    public bool? ThrowConfigExceptions { get; set; }

    public bool KeepVariablesOnReload { get; set; }

    public LoggingConfiguration Configuration
    {
      get
      {
        if (this._configLoaded)
          return this._config;
        lock (this._syncRoot)
        {
          if (this._configLoaded || this._isDisposing)
            return this._config;
          if (this._config == null)
            this.TryLoadFromAppConfig();
          if (this._config == null)
            this.TryLoadFromFilePaths();
          if (this._config != null)
          {
            try
            {
              this._config.Dump();
              this.ReconfigExistingLoggers();
              this.TryWachtingConfigFile();
              LogFactory.LogConfigurationInitialized();
            }
            finally
            {
              this._configLoaded = true;
            }
          }
          return this._config;
        }
      }
      set
      {
        try
        {
          this._watcher.StopWatching();
        }
        catch (Exception ex)
        {
          InternalLogger.Error(ex, "Cannot stop file watching.");
          if (ex.MustBeRethrown())
            throw;
        }
        lock (this._syncRoot)
        {
          LoggingConfiguration config = this._config;
          if (config != null)
          {
            InternalLogger.Info("Closing old configuration.");
            this.Flush();
            config.Close();
          }
          this._config = value;
          if (this._config == null)
          {
            this._configLoaded = false;
          }
          else
          {
            try
            {
              this._config.Dump();
              this.ReconfigExistingLoggers();
              this.TryWachtingConfigFile();
            }
            finally
            {
              this._configLoaded = true;
            }
          }
          this.OnConfigurationChanged(new LoggingConfigurationChangedEventArgs(value, config));
        }
      }
    }

    private void TryWachtingConfigFile()
    {
      try
      {
        this._watcher.Watch(this._config.FileNamesToWatch);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrown())
        {
          throw;
        }
        else
        {
          object[] objArray = new object[1]
          {
            (object) string.Join(",", this._config.FileNamesToWatch.ToArray<string>())
          };
          InternalLogger.Warn(ex, "Cannot start file watching: {0}", objArray);
        }
      }
    }

    private void TryLoadFromFilePaths()
    {
      foreach (string candidateConfigFilePath in this.GetCandidateConfigFilePaths())
      {
        if (File.Exists(candidateConfigFilePath))
        {
          this._config = this.TryLoadLoggingConfiguration(candidateConfigFilePath);
          break;
        }
      }
    }

    private void TryLoadFromAppConfig()
    {
      try
      {
        this._config = XmlLoggingConfiguration.AppConfig;
      }
      catch (Exception ex)
      {
        if (!ex.MustBeRethrown())
          return;
        throw;
      }
    }

    public LogLevel GlobalThreshold
    {
      get => this._globalThreshold;
      set
      {
        lock (this._syncRoot)
        {
          this._globalThreshold = value;
          this.ReconfigExistingLoggers();
        }
      }
    }

    [CanBeNull]
    public CultureInfo DefaultCultureInfo => this.Configuration?.DefaultCultureInfo;

    internal static void LogConfigurationInitialized()
    {
      InternalLogger.Info("Configuration initialized.");
      try
      {
        InternalLogger.LogAssemblyVersion(typeof (ILogger).GetAssembly());
      }
      catch (SecurityException ex)
      {
        InternalLogger.Debug((Exception) ex, "Not running in full trust");
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public Logger CreateNullLogger() => (Logger) new NullLogger(this);

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Logger GetCurrentClassLogger()
    {
      return this.GetLogger(StackTraceUsageUtils.GetClassFullName(new StackFrame(1, false)));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public T GetCurrentClassLogger<T>() where T : Logger
    {
      return (T) this.GetLogger(StackTraceUsageUtils.GetClassFullName(new StackFrame(1, false)), typeof (T));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Logger GetCurrentClassLogger(Type loggerType)
    {
      return this.GetLogger(StackTraceUsageUtils.GetClassFullName(new StackFrame(1, false)), loggerType);
    }

    public Logger GetLogger(string name)
    {
      return this.GetLogger(new LogFactory.LoggerCacheKey(name, typeof (Logger)));
    }

    public T GetLogger<T>(string name) where T : Logger
    {
      return (T) this.GetLogger(new LogFactory.LoggerCacheKey(name, typeof (T)));
    }

    public Logger GetLogger(string name, Type loggerType)
    {
      return this.GetLogger(new LogFactory.LoggerCacheKey(name, loggerType));
    }

    public void ReconfigExistingLoggers()
    {
      List<Logger> loggers;
      lock (this._syncRoot)
      {
        this._config?.InitializeAll();
        loggers = this._loggerCache.GetLoggers();
      }
      foreach (Logger logger in loggers)
        logger.SetConfiguration(this.GetConfigurationForLogger(logger.Name, this._config));
    }

    public void Flush() => this.Flush(LogFactory.DefaultFlushTimeout);

    public void Flush(TimeSpan timeout)
    {
      try
      {
        AsyncHelpers.RunSynchronously((AsynchronousAction) (cb => this.Flush(cb, timeout)));
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "Error with flush.");
        if (!ex.MustBeRethrown())
          return;
        throw;
      }
    }

    public void Flush(int timeoutMilliseconds)
    {
      this.Flush(TimeSpan.FromMilliseconds((double) timeoutMilliseconds));
    }

    public void Flush(AsyncContinuation asyncContinuation)
    {
      this.Flush(asyncContinuation, TimeSpan.MaxValue);
    }

    public void Flush(AsyncContinuation asyncContinuation, int timeoutMilliseconds)
    {
      this.Flush(asyncContinuation, TimeSpan.FromMilliseconds((double) timeoutMilliseconds));
    }

    public void Flush(AsyncContinuation asyncContinuation, TimeSpan timeout)
    {
      try
      {
        InternalLogger.Trace<TimeSpan>("LogFactory.Flush({0})", timeout);
        LoggingConfiguration loggingConfiguration = (LoggingConfiguration) null;
        lock (this._syncRoot)
          loggingConfiguration = this._config;
        if (loggingConfiguration != null)
          loggingConfiguration.FlushAllTargets(AsyncHelpers.WithTimeout(asyncContinuation, timeout));
        else
          asyncContinuation((Exception) null);
      }
      catch (Exception ex)
      {
        if (this.ThrowExceptions)
          throw;
        else
          InternalLogger.Error(ex, "Error with flush.");
      }
    }

    [Obsolete("Use SuspendLogging() instead. Marked obsolete on NLog 4.0")]
    public IDisposable DisableLogging() => this.SuspendLogging();

    [Obsolete("Use ResumeLogging() instead. Marked obsolete on NLog 4.0")]
    public void EnableLogging() => this.ResumeLogging();

    public IDisposable SuspendLogging()
    {
      lock (this._syncRoot)
      {
        --this._logsEnabled;
        if (this._logsEnabled == -1)
          this.ReconfigExistingLoggers();
      }
      return (IDisposable) new LogFactory.LogEnabler(this);
    }

    public void ResumeLogging()
    {
      lock (this._syncRoot)
      {
        ++this._logsEnabled;
        if (this._logsEnabled != 0)
          return;
        this.ReconfigExistingLoggers();
      }
    }

    public bool IsLoggingEnabled() => this._logsEnabled >= 0;

    protected virtual void OnConfigurationChanged(LoggingConfigurationChangedEventArgs e)
    {
      EventHandler<LoggingConfigurationChangedEventArgs> configurationChanged = this.ConfigurationChanged;
      if (configurationChanged == null)
        return;
      configurationChanged((object) this, e);
    }

    protected virtual void OnConfigurationReloaded(LoggingConfigurationReloadedEventArgs e)
    {
      EventHandler<LoggingConfigurationReloadedEventArgs> configurationReloaded = this.ConfigurationReloaded;
      if (configurationReloaded == null)
        return;
      configurationReloaded((object) this, e);
    }

    internal void ReloadConfigOnTimer(object state)
    {
      if (this._reloadTimer == null && this._isDisposing)
        return;
      LoggingConfiguration loggingConfiguration1 = (LoggingConfiguration) state;
      InternalLogger.Info("Reloading configuration...");
      lock (this._syncRoot)
      {
        try
        {
          if (this._isDisposing)
            return;
          Timer reloadTimer = this._reloadTimer;
          if (reloadTimer != null)
          {
            this._reloadTimer = (Timer) null;
            reloadTimer.WaitForDispose(TimeSpan.Zero);
          }
          this._watcher.StopWatching();
          LoggingConfiguration loggingConfiguration2 = this._config == loggingConfiguration1 ? loggingConfiguration1.Reload() : throw new NLogConfigurationException("Config changed in between. Not reloading.");
          if (loggingConfiguration2 is XmlLoggingConfiguration loggingConfiguration3)
          {
            bool? initializeSucceeded = loggingConfiguration3.InitializeSucceeded;
            bool flag = true;
            if ((initializeSucceeded.GetValueOrDefault() == flag ? (!initializeSucceeded.HasValue ? 1 : 0) : 1) != 0)
              throw new NLogConfigurationException("Configuration.Reload() failed. Invalid XML?");
          }
          if (loggingConfiguration2 == null)
            throw new NLogConfigurationException("Configuration.Reload() returned null. Not reloading.");
          if (this.KeepVariablesOnReload && this._config != null)
            loggingConfiguration2.CopyVariables(this._config.Variables);
          this.Configuration = loggingConfiguration2;
          this.OnConfigurationReloaded(new LoggingConfigurationReloadedEventArgs(true));
        }
        catch (Exception ex)
        {
          InternalLogger.Warn(ex, "NLog configuration while reloading");
          if (ex.MustBeRethrownImmediately())
          {
            throw;
          }
          else
          {
            this._watcher.Watch(loggingConfiguration1.FileNamesToWatch);
            this.OnConfigurationReloaded(new LoggingConfigurationReloadedEventArgs(false, ex));
          }
        }
      }
    }

    private void GetTargetsByLevelForLogger(
      string name,
      List<LoggingRule> loggingRules,
      TargetWithFilterChain[] targetsByLevel,
      TargetWithFilterChain[] lastTargetsByLevel,
      bool[] suppressedLevels)
    {
      foreach (LoggingRule loggingRule in loggingRules)
      {
        if (loggingRule.NameMatches(name))
        {
          for (int ordinal = 0; ordinal <= LogLevel.MaxLevel.Ordinal; ++ordinal)
          {
            if (ordinal >= this.GlobalThreshold.Ordinal && !suppressedLevels[ordinal] && loggingRule.IsLoggingEnabledForLevel(LogLevel.FromOrdinal(ordinal)))
            {
              if (loggingRule.Final)
                suppressedLevels[ordinal] = true;
              foreach (Target target in loggingRule.GetTargetsThreadSafe())
              {
                TargetWithFilterChain targetWithFilterChain = new TargetWithFilterChain(target, loggingRule.Filters);
                if (lastTargetsByLevel[ordinal] != null)
                  lastTargetsByLevel[ordinal].NextInChain = targetWithFilterChain;
                else
                  targetsByLevel[ordinal] = targetWithFilterChain;
                lastTargetsByLevel[ordinal] = targetWithFilterChain;
              }
            }
          }
          if (loggingRule.ChildRules.Count != 0)
            this.GetTargetsByLevelForLogger(name, loggingRule.GetChildRulesThreadSafe(), targetsByLevel, lastTargetsByLevel, suppressedLevels);
        }
      }
      for (int index = 0; index <= LogLevel.MaxLevel.Ordinal; ++index)
      {
        TargetWithFilterChain targetWithFilterChain = targetsByLevel[index];
        if (targetWithFilterChain != null)
        {
          int num = (int) targetWithFilterChain.PrecalculateStackTraceUsage();
        }
      }
    }

    internal LoggerConfiguration GetConfigurationForLogger(
      string name,
      LoggingConfiguration configuration)
    {
      TargetWithFilterChain[] targetsByLevel = new TargetWithFilterChain[LogLevel.MaxLevel.Ordinal + 1];
      TargetWithFilterChain[] lastTargetsByLevel = new TargetWithFilterChain[LogLevel.MaxLevel.Ordinal + 1];
      bool[] suppressedLevels = new bool[LogLevel.MaxLevel.Ordinal + 1];
      if (configuration != null && this.IsLoggingEnabled())
      {
        List<LoggingRule> loggingRulesThreadSafe = configuration.GetLoggingRulesThreadSafe();
        this.GetTargetsByLevelForLogger(name, loggingRulesThreadSafe, targetsByLevel, lastTargetsByLevel, suppressedLevels);
      }
      if (InternalLogger.IsDebugEnabled)
      {
        InternalLogger.Debug<string>("Targets for {0} by level:", name);
        for (int ordinal = 0; ordinal <= LogLevel.MaxLevel.Ordinal; ++ordinal)
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0} =>", new object[1]
          {
            (object) LogLevel.FromOrdinal(ordinal)
          });
          for (TargetWithFilterChain nextInChain = targetsByLevel[ordinal]; nextInChain != null; nextInChain = nextInChain.NextInChain)
          {
            stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, " {0}", new object[1]
            {
              (object) nextInChain.Target.Name
            });
            if (nextInChain.FilterChain.Count > 0)
              stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, " ({0} filters)", new object[1]
              {
                (object) nextInChain.FilterChain.Count
              });
          }
          InternalLogger.Debug(stringBuilder.ToString());
        }
      }
      return new LoggerConfiguration(targetsByLevel, configuration != null && configuration.ExceptionLoggingOldStyle);
    }

    private void Close(TimeSpan flushTimeout)
    {
      if (this._isDisposing)
        return;
      this._isDisposing = true;
      LogFactory.LoggerShutdown -= new EventHandler<EventArgs>(this.OnStopLogging);
      this.ConfigurationReloaded = (EventHandler<LoggingConfigurationReloadedEventArgs>) null;
      if (this._watcher != null)
      {
        this._watcher.FileChanged -= new FileSystemEventHandler(this.ConfigFileChanged);
        this._watcher.StopWatching();
      }
      if (Monitor.TryEnter(this._syncRoot, 500))
      {
        try
        {
          Timer reloadTimer = this._reloadTimer;
          if (reloadTimer != null)
          {
            this._reloadTimer = (Timer) null;
            reloadTimer.WaitForDispose(TimeSpan.Zero);
          }
          this._watcher?.Dispose();
          LoggingConfiguration config = this._config;
          if (this._configLoaded)
          {
            if (config != null)
              this.CloseOldConfig(flushTimeout, config);
          }
        }
        finally
        {
          Monitor.Exit(this._syncRoot);
        }
      }
      this.ConfigurationChanged = (EventHandler<LoggingConfigurationChangedEventArgs>) null;
    }

    private void CloseOldConfig(TimeSpan flushTimeout, LoggingConfiguration oldConfig)
    {
      try
      {
        bool flag = true;
        if (flushTimeout != TimeSpan.Zero && !PlatformDetector.IsMono && !PlatformDetector.IsUnix)
        {
          ManualResetEvent flushCompleted = new ManualResetEvent(false);
          oldConfig.FlushAllTargets((AsyncContinuation) (ex => flushCompleted.Set()));
          flag = flushCompleted.WaitOne(flushTimeout);
        }
        this._config = (LoggingConfiguration) null;
        this.ReconfigExistingLoggers();
        if (!flag)
        {
          InternalLogger.Warn("Target flush timeout. One or more targets did not complete flush operation, skipping target close.");
        }
        else
        {
          oldConfig.Close();
          this.OnConfigurationChanged(new LoggingConfigurationChangedEventArgs((LoggingConfiguration) null, oldConfig));
        }
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "Error with close.");
      }
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.Close(TimeSpan.Zero);
    }

    internal void Shutdown()
    {
      InternalLogger.Info("Logger closing down...");
      if (!this._isDisposing && this._configLoaded)
      {
        lock (this._syncRoot)
        {
          if (this._isDisposing || !this._configLoaded)
            return;
          this.Configuration = (LoggingConfiguration) null;
          this._configLoaded = true;
          this.ReconfigExistingLoggers();
        }
      }
      InternalLogger.Info("Logger has been closed down.");
    }

    public IEnumerable<string> GetCandidateConfigFilePaths()
    {
      return this._candidateConfigFilePaths != null ? (IEnumerable<string>) this._candidateConfigFilePaths.AsReadOnly() : LogFactory.GetDefaultCandidateConfigFilePaths();
    }

    public void SetCandidateConfigFilePaths(IEnumerable<string> filePaths)
    {
      this._candidateConfigFilePaths = new List<string>();
      if (filePaths == null)
        return;
      this._candidateConfigFilePaths.AddRange(filePaths);
    }

    public void ResetCandidateConfigFilePath()
    {
      this._candidateConfigFilePaths = (List<string>) null;
    }

    private static IEnumerable<string> GetDefaultCandidateConfigFilePaths()
    {
      if (LogFactory.CurrentAppDomain?.BaseDirectory != null)
      {
        yield return Path.Combine(LogFactory.CurrentAppDomain.BaseDirectory, "NLog.config");
        yield return Path.Combine(LogFactory.CurrentAppDomain.BaseDirectory, "nlog.config");
      }
      else
      {
        yield return "NLog.config";
        yield return "nlog.config";
      }
      string configurationFile = LogFactory.CurrentAppDomain?.ConfigurationFile;
      if (configurationFile != null)
      {
        yield return Path.ChangeExtension(configurationFile, ".nlog");
        if (configurationFile.Contains(".vshost."))
          yield return Path.ChangeExtension(configurationFile.Replace(".vshost.", "."), ".nlog");
        IEnumerable<string> privateBinPath = LogFactory.CurrentAppDomain.PrivateBinPath;
        if (privateBinPath != null)
        {
          foreach (string path in privateBinPath)
          {
            if (path != null)
            {
              yield return Path.Combine(path, "NLog.config");
              yield return Path.Combine(path, "nlog.config");
            }
          }
        }
      }
      Assembly assembly = typeof (LogFactory).Assembly;
      if (!assembly.GlobalAssemblyCache && !string.IsNullOrEmpty(assembly.Location))
        yield return assembly.Location + ".nlog";
    }

    private Logger GetLogger(LogFactory.LoggerCacheKey cacheKey)
    {
      lock (this._syncRoot)
      {
        Logger logger1 = this._loggerCache.Retrieve(cacheKey);
        if (logger1 != null)
          return logger1;
        if (cacheKey.ConcreteType != (Type) null && cacheKey.ConcreteType != typeof (Logger))
        {
          string fullName = cacheKey.ConcreteType.FullName;
          try
          {
            if (cacheKey.ConcreteType.IsStaticClass())
            {
              string message = string.Format("GetLogger / GetCurrentClassLogger is '{0}' as loggerType can be a static class and should inherit from Logger", (object) fullName);
              InternalLogger.Error(message);
              if (this.ThrowExceptions)
                throw new NLogRuntimeException(message);
              logger2 = LogFactory.CreateDefaultLogger(ref cacheKey);
            }
            else if (!(FactoryHelper.CreateInstance(cacheKey.ConcreteType) is Logger logger2))
            {
              string message = string.Format("GetLogger / GetCurrentClassLogger got '{0}' as loggerType which doesn't inherit from Logger", (object) fullName);
              InternalLogger.Error(message);
              if (this.ThrowExceptions)
                throw new NLogRuntimeException(message);
              logger2 = LogFactory.CreateDefaultLogger(ref cacheKey);
            }
          }
          catch (Exception ex)
          {
            InternalLogger.Error(ex, "GetLogger / GetCurrentClassLogger. Cannot create instance of type '{0}'. It should have an default contructor. ", (object) fullName);
            if (ex.MustBeRethrown())
              throw;
            else
              logger2 = LogFactory.CreateDefaultLogger(ref cacheKey);
          }
        }
        else
          logger2 = new Logger();
        if (cacheKey.ConcreteType != (Type) null)
          logger2.Initialize(cacheKey.Name, this.GetConfigurationForLogger(cacheKey.Name, this.Configuration), this);
        this._loggerCache.InsertOrUpdate(cacheKey, logger2);
        return logger2;
      }
    }

    private static Logger CreateDefaultLogger(ref LogFactory.LoggerCacheKey cacheKey)
    {
      cacheKey = new LogFactory.LoggerCacheKey(cacheKey.Name, typeof (Logger));
      return new Logger();
    }

    public LogFactory LoadConfiguration(string configFile)
    {
      if (FilePathLayout.DetectFilePathKind(configFile) == FilePathKind.Relative)
        configFile = Path.Combine(LogFactory.CurrentAppDomain.BaseDirectory, configFile);
      this.Configuration = this.TryLoadLoggingConfiguration(configFile);
      return this;
    }

    private LoggingConfiguration TryLoadLoggingConfiguration(string configFile)
    {
      InternalLogger.Debug<string>("Loading config from {0}", configFile);
      XmlLoggingConfiguration loggingConfiguration = new XmlLoggingConfiguration(configFile, this);
      bool? initializeSucceeded = loggingConfiguration.InitializeSucceeded;
      bool flag = true;
      if ((initializeSucceeded.GetValueOrDefault() == flag ? (!initializeSucceeded.HasValue ? 1 : 0) : 1) == 0)
        return (LoggingConfiguration) loggingConfiguration;
      InternalLogger.Warn<string>("Failed loading config from {0}. Invalid XML?", configFile);
      return (LoggingConfiguration) loggingConfiguration;
    }

    private void ConfigFileChanged(object sender, EventArgs args)
    {
      InternalLogger.Info<int>("Configuration file change detected! Reloading in {0}ms...", 1000);
      lock (this._syncRoot)
      {
        if (this._isDisposing)
          return;
        if (this._reloadTimer == null)
        {
          LoggingConfiguration configuration = this.Configuration;
          if (configuration == null)
            return;
          this._reloadTimer = new Timer(new TimerCallback(this.ReloadConfigOnTimer), (object) configuration, 1000, -1);
        }
        else
          this._reloadTimer.Change(1000, -1);
      }
    }

    private static void RegisterEvents(IAppDomain appDomain)
    {
      if (appDomain == null)
        return;
      try
      {
        appDomain.ProcessExit += new EventHandler<EventArgs>(LogFactory.OnLoggerShutdown);
        appDomain.DomainUnload += new EventHandler<EventArgs>(LogFactory.OnLoggerShutdown);
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "Error setting up termination events.");
        if (!ex.MustBeRethrown())
          return;
        throw;
      }
    }

    private static void UnregisterEvents(IAppDomain appDomain)
    {
      if (appDomain == null)
        return;
      appDomain.DomainUnload -= new EventHandler<EventArgs>(LogFactory.OnLoggerShutdown);
      appDomain.ProcessExit -= new EventHandler<EventArgs>(LogFactory.OnLoggerShutdown);
    }

    private static void OnLoggerShutdown(object sender, EventArgs args)
    {
      try
      {
        EventHandler<EventArgs> loggerShutdown = LogFactory.LoggerShutdown;
        if (loggerShutdown == null)
          return;
        loggerShutdown(sender, args);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
        else
          InternalLogger.Error(ex, "LogFactory failed to shut down properly.");
      }
      finally
      {
        LogFactory.LoggerShutdown = (EventHandler<EventArgs>) null;
        if (LogFactory.currentAppDomain != null)
          LogFactory.CurrentAppDomain = (IAppDomain) null;
      }
    }

    private void OnStopLogging(object sender, EventArgs args)
    {
      try
      {
        InternalLogger.Info("Shutting down logging...");
        this.Close(TimeSpan.FromMilliseconds(1500.0));
        InternalLogger.Info("Logger has been shut down.");
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
        else
          InternalLogger.Error(ex, "Logger failed to shut down properly.");
      }
    }

    internal struct LoggerCacheKey(string name, Type concreteType) : 
      IEquatable<LogFactory.LoggerCacheKey>
    {
      public readonly string Name = name;
      public readonly Type ConcreteType = concreteType;

      public override int GetHashCode()
      {
        return this.ConcreteType.GetHashCode() ^ this.Name.GetHashCode();
      }

      public override bool Equals(object obj)
      {
        return obj is LogFactory.LoggerCacheKey other && this.Equals(other);
      }

      public bool Equals(LogFactory.LoggerCacheKey other)
      {
        return this.ConcreteType == other.ConcreteType && string.Equals(other.Name, this.Name, StringComparison.Ordinal);
      }
    }

    private class LoggerCache
    {
      private readonly Dictionary<LogFactory.LoggerCacheKey, WeakReference> _loggerCache = new Dictionary<LogFactory.LoggerCacheKey, WeakReference>();

      public void InsertOrUpdate(LogFactory.LoggerCacheKey cacheKey, Logger logger)
      {
        this._loggerCache[cacheKey] = new WeakReference((object) logger);
      }

      public Logger Retrieve(LogFactory.LoggerCacheKey cacheKey)
      {
        WeakReference weakReference;
        return this._loggerCache.TryGetValue(cacheKey, out weakReference) ? weakReference.Target as Logger : (Logger) null;
      }

      public List<Logger> GetLoggers()
      {
        List<Logger> loggers = new List<Logger>(this._loggerCache.Count);
        foreach (WeakReference weakReference in this._loggerCache.Values)
        {
          if (weakReference.Target is Logger target)
            loggers.Add(target);
        }
        return loggers;
      }
    }

    private class LogEnabler : IDisposable
    {
      private readonly LogFactory _factory;

      public LogEnabler(LogFactory factory) => this._factory = factory;

      void IDisposable.Dispose() => this._factory.ResumeLogging();
    }
  }
}
