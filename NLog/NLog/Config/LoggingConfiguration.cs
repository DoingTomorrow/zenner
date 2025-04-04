// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingConfiguration
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using JetBrains.Annotations;
using NLog.Common;
using NLog.Internal;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;

#nullable disable
namespace NLog.Config
{
  public class LoggingConfiguration
  {
    private readonly IDictionary<string, Target> _targets = (IDictionary<string, Target>) new Dictionary<string, Target>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private List<object> _configItems = new List<object>();
    private readonly Dictionary<string, SimpleLayout> _variables = new Dictionary<string, SimpleLayout>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private static readonly IEqualityComparer<Target> TargetNameComparer = (IEqualityComparer<Target>) new LoggingConfiguration.TargetNameEqualityComparer();

    public LoggingConfiguration()
    {
      this.LoggingRules = (IList<LoggingRule>) new List<LoggingRule>();
    }

    [Obsolete("This option will be removed in NLog 5. Marked obsolete on NLog 4.1")]
    public bool ExceptionLoggingOldStyle { get; set; }

    public IDictionary<string, SimpleLayout> Variables
    {
      get => (IDictionary<string, SimpleLayout>) this._variables;
    }

    public ReadOnlyCollection<Target> ConfiguredNamedTargets
    {
      get => this.GetAllTargetsThreadSafe().AsReadOnly();
    }

    public virtual IEnumerable<string> FileNamesToWatch
    {
      get => (IEnumerable<string>) ArrayHelper.Empty<string>();
    }

    public IList<LoggingRule> LoggingRules { get; private set; }

    internal List<LoggingRule> GetLoggingRulesThreadSafe()
    {
      lock (this.LoggingRules)
        return this.LoggingRules.ToList<LoggingRule>();
    }

    private void AddLoggingRulesThreadSafe(LoggingRule rule)
    {
      lock (this.LoggingRules)
        this.LoggingRules.Add(rule);
    }

    private bool TryGetTargetThreadSafe(string name, out Target target)
    {
      lock (this._targets)
        return this._targets.TryGetValue(name, out target);
    }

    private List<Target> GetAllTargetsThreadSafe()
    {
      lock (this._targets)
        return this._targets.Values.ToList<Target>();
    }

    private Target RemoveTargetThreadSafe(string name)
    {
      Target target;
      lock (this._targets)
      {
        if (this._targets.TryGetValue(name, out target))
          this._targets.Remove(name);
      }
      if (target != null)
        InternalLogger.Debug<string, string>("Unregistered target {0}: {1}", name, target.GetType().FullName);
      return target;
    }

    private void AddTargetThreadSafe(string name, Target target, bool forceOverwrite)
    {
      if (string.IsNullOrEmpty(name) && !forceOverwrite)
        return;
      lock (this._targets)
      {
        if (!forceOverwrite && this._targets.ContainsKey(name))
          return;
        this._targets[name] = target;
      }
      if (!string.IsNullOrEmpty(target.Name) && target.Name != name)
        InternalLogger.Info<string, string, string>("Registered target {0}: {1} (Target created with different name: {2})", name, target.GetType().FullName, target.Name);
      else
        InternalLogger.Debug<string, string>("Registered target {0}: {1}", name, target.GetType().FullName);
    }

    [CanBeNull]
    public CultureInfo DefaultCultureInfo { get; set; }

    public ReadOnlyCollection<Target> AllTargets
    {
      get
      {
        return this._configItems.OfType<Target>().Concat<Target>((IEnumerable<Target>) this.GetAllTargetsThreadSafe()).Distinct<Target>(LoggingConfiguration.TargetNameComparer).ToList<Target>().AsReadOnly();
      }
    }

    public void AddTarget([NotNull] Target target)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      this.AddTargetThreadSafe(target.Name, target, true);
    }

    public void AddTarget(string name, Target target)
    {
      if (name == null)
        throw new ArgumentException("Target name cannot be null", nameof (name));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      this.AddTargetThreadSafe(name, target, true);
    }

    public Target FindTargetByName(string name)
    {
      Target target;
      return !this.TryGetTargetThreadSafe(name, out target) ? (Target) null : target;
    }

    public TTarget FindTargetByName<TTarget>(string name) where TTarget : Target
    {
      return this.FindTargetByName(name) as TTarget;
    }

    public void AddRule(
      NLog.LogLevel minLevel,
      NLog.LogLevel maxLevel,
      string targetName,
      string loggerNamePattern = "*")
    {
      this.AddRule(minLevel, maxLevel, this.FindTargetByName(targetName) ?? throw new NLogRuntimeException("Target '{0}' not found", new object[1]
      {
        (object) targetName
      }), loggerNamePattern, false);
    }

    public void AddRule(
      NLog.LogLevel minLevel,
      NLog.LogLevel maxLevel,
      Target target,
      string loggerNamePattern = "*")
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      this.AddRule(minLevel, maxLevel, target, loggerNamePattern, false);
    }

    public void AddRule(
      NLog.LogLevel minLevel,
      NLog.LogLevel maxLevel,
      Target target,
      string loggerNamePattern,
      bool final)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      this.AddLoggingRulesThreadSafe(new LoggingRule(loggerNamePattern, minLevel, maxLevel, target)
      {
        Final = final
      });
      this.AddTargetThreadSafe(target.Name, target, false);
    }

    public void AddRuleForOneLevel(NLog.LogLevel level, string targetName, string loggerNamePattern = "*")
    {
      this.AddRuleForOneLevel(level, this.FindTargetByName(targetName) ?? throw new NLogConfigurationException("Target '{0}' not found", new object[1]
      {
        (object) targetName
      }), loggerNamePattern, false);
    }

    public void AddRuleForOneLevel(NLog.LogLevel level, Target target, string loggerNamePattern = "*")
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      this.AddRuleForOneLevel(level, target, loggerNamePattern, false);
    }

    public void AddRuleForOneLevel(
      NLog.LogLevel level,
      Target target,
      string loggerNamePattern,
      bool final)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      LoggingRule rule = new LoggingRule(loggerNamePattern, target)
      {
        Final = final
      };
      rule.EnableLoggingForLevel(level);
      this.AddLoggingRulesThreadSafe(rule);
      this.AddTargetThreadSafe(target.Name, target, false);
    }

    public void AddRuleForAllLevels(string targetName, string loggerNamePattern = "*")
    {
      this.AddRuleForAllLevels(this.FindTargetByName(targetName) ?? throw new NLogRuntimeException("Target '{0}' not found", new object[1]
      {
        (object) targetName
      }), loggerNamePattern, false);
    }

    public void AddRuleForAllLevels(Target target, string loggerNamePattern = "*")
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      this.AddRuleForAllLevels(target, loggerNamePattern, false);
    }

    public void AddRuleForAllLevels(Target target, string loggerNamePattern, bool final)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      LoggingRule rule = new LoggingRule(loggerNamePattern, target)
      {
        Final = final
      };
      rule.EnableLoggingForLevels(NLog.LogLevel.MinLevel, NLog.LogLevel.MaxLevel);
      this.AddLoggingRulesThreadSafe(rule);
      this.AddTargetThreadSafe(target.Name, target, false);
    }

    public virtual LoggingConfiguration Reload() => this;

    public void RemoveTarget(string name)
    {
      HashSet<Target> targetSet = new HashSet<Target>();
      Target target1 = this.RemoveTargetThreadSafe(name);
      if (target1 != null)
        targetSet.Add(target1);
      if (!string.IsNullOrEmpty(name) || target1 != null)
      {
        foreach (LoggingRule loggingRule in this.GetLoggingRulesThreadSafe())
        {
          foreach (Target target2 in loggingRule.GetTargetsThreadSafe())
          {
            if (target1 == target2 || !string.IsNullOrEmpty(name) && target2.Name == name)
            {
              targetSet.Add(target2);
              loggingRule.RemoveTargetThreadSafe(target2);
            }
          }
        }
      }
      if (targetSet.Count <= 0)
        return;
      this.ValidateConfig();
      LogManager.ReconfigExistingLoggers();
      ManualResetEvent flushCompleted = new ManualResetEvent(false);
      foreach (Target target3 in targetSet)
      {
        flushCompleted.Reset();
        target3.Flush((AsyncContinuation) (ex => flushCompleted.Set()));
        flushCompleted.WaitOne(TimeSpan.FromSeconds(15.0));
        target3.Close();
      }
    }

    public void Install(InstallationContext installationContext)
    {
      if (installationContext == null)
        throw new ArgumentNullException(nameof (installationContext));
      this.InitializeAll();
      foreach (IInstallable installableItem in this.GetInstallableItems())
      {
        installationContext.Info("Installing '{0}'", (object) installableItem);
        try
        {
          installableItem.Install(installationContext);
          installationContext.Info("Finished installing '{0}'.", (object) installableItem);
        }
        catch (Exception ex)
        {
          InternalLogger.Error(ex, "Install of '{0}' failed.", (object) installableItem);
          if (ex.MustBeRethrownImmediately() || installationContext.ThrowExceptions)
            throw;
          else
            installationContext.Error("Install of '{0}' failed: {1}.", (object) installableItem, (object) ex);
        }
      }
    }

    public void Uninstall(InstallationContext installationContext)
    {
      if (installationContext == null)
        throw new ArgumentNullException(nameof (installationContext));
      this.InitializeAll();
      foreach (IInstallable installableItem in this.GetInstallableItems())
      {
        installationContext.Info("Uninstalling '{0}'", (object) installableItem);
        try
        {
          installableItem.Uninstall(installationContext);
          installationContext.Info("Finished uninstalling '{0}'.", (object) installableItem);
        }
        catch (Exception ex)
        {
          InternalLogger.Error(ex, "Uninstall of '{0}' failed.", (object) installableItem);
          if (ex.MustBeRethrownImmediately())
            throw;
          else
            installationContext.Error("Uninstall of '{0}' failed: {1}.", (object) installableItem, (object) ex);
        }
      }
    }

    internal void Close()
    {
      InternalLogger.Debug("Closing logging configuration...");
      foreach (ISupportsInitialize supportsInitializ in this.GetSupportsInitializes())
      {
        InternalLogger.Trace<ISupportsInitialize>("Closing {0}", supportsInitializ);
        try
        {
          supportsInitializ.Close();
        }
        catch (Exception ex)
        {
          InternalLogger.Warn(ex, "Exception while closing.");
          if (ex.MustBeRethrown())
            throw;
        }
      }
      InternalLogger.Debug("Finished closing logging configuration.");
    }

    internal void Dump()
    {
      if (!InternalLogger.IsDebugEnabled)
        return;
      InternalLogger.Debug("--- NLog configuration dump ---");
      InternalLogger.Debug("Targets:");
      foreach (Target target in this.GetAllTargetsThreadSafe())
        InternalLogger.Debug<Target>("{0}", target);
      InternalLogger.Debug("Rules:");
      foreach (LoggingRule loggingRule in this.GetLoggingRulesThreadSafe())
        InternalLogger.Debug<LoggingRule>("{0}", loggingRule);
      InternalLogger.Debug("--- End of NLog configuration dump ---");
    }

    internal void FlushAllTargets(AsyncContinuation asyncContinuation)
    {
      InternalLogger.Trace("Flushing all targets...");
      List<Target> values = new List<Target>();
      foreach (LoggingRule loggingRule in this.GetLoggingRulesThreadSafe())
      {
        foreach (Target target in loggingRule.GetTargetsThreadSafe())
        {
          if (!values.Contains(target))
            values.Add(target);
        }
      }
      AsyncHelpers.ForEachItemInParallel<Target>((IEnumerable<Target>) values, asyncContinuation, (AsynchronousAction<Target>) ((target, cont) => target.Flush(cont)));
    }

    internal void ValidateConfig()
    {
      List<object> objectList = new List<object>();
      foreach (LoggingRule loggingRule in this.GetLoggingRulesThreadSafe())
        objectList.Add((object) loggingRule);
      foreach (Target target in this.GetAllTargetsThreadSafe())
        objectList.Add((object) target);
      this._configItems = ObjectGraphScanner.FindReachableObjects<object>(true, objectList.ToArray());
      InternalLogger.Info<int>("Found {0} configuration items", this._configItems.Count);
      foreach (object configItem in this._configItems)
        PropertyHelper.CheckRequiredParameters(configItem);
    }

    internal void InitializeAll()
    {
      this.ValidateConfig();
      foreach (ISupportsInitialize supportsInitializ in this.GetSupportsInitializes(true))
      {
        InternalLogger.Trace<ISupportsInitialize>("Initializing {0}", supportsInitializ);
        try
        {
          supportsInitializ.Initialize(this);
        }
        catch (Exception ex)
        {
          if (ex.MustBeRethrown())
            throw;
          else if (LogManager.ThrowExceptions)
            throw new NLogConfigurationException(string.Format("Error during initialization of {0}", (object) supportsInitializ), ex);
        }
      }
    }

    internal void EnsureInitialized() => this.InitializeAll();

    private List<IInstallable> GetInstallableItems()
    {
      return this._configItems.OfType<IInstallable>().ToList<IInstallable>();
    }

    private List<ISupportsInitialize> GetSupportsInitializes(bool reverse = false)
    {
      IEnumerable<ISupportsInitialize> source = this._configItems.OfType<ISupportsInitialize>();
      if (reverse)
        source = source.Reverse<ISupportsInitialize>();
      return source.ToList<ISupportsInitialize>();
    }

    internal void CopyVariables(IDictionary<string, SimpleLayout> masterVariables)
    {
      foreach (KeyValuePair<string, SimpleLayout> masterVariable in (IEnumerable<KeyValuePair<string, SimpleLayout>>) masterVariables)
        this.Variables[masterVariable.Key] = masterVariable.Value;
    }

    private class TargetNameEqualityComparer : IEqualityComparer<Target>
    {
      public bool Equals(Target x, Target y) => string.Equals(x.Name, y.Name);

      public int GetHashCode(Target obj) => obj.Name == null ? 0 : obj.Name.GetHashCode();
    }
  }
}
