// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Enhanced.SequenceStyleGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Id.Enhanced
{
  public class SequenceStyleGenerator : 
    IPersistentIdentifierGenerator,
    IIdentifierGenerator,
    IConfigurable
  {
    public const string DefaultSequenceName = "hibernate_sequence";
    public const int DefaultInitialValue = 1;
    public const int DefaultIncrementSize = 1;
    public const string SequenceParam = "sequence_name";
    public const string InitialParam = "initial_value";
    public const string IncrementParam = "increment_size";
    public const string OptimizerParam = "optimizer";
    public const string ForceTableParam = "force_table_use";
    public const string ValueColumnParam = "value_column";
    public const string DefaultValueColumnName = "next_val";
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (SequenceStyleGenerator));

    public IDatabaseStructure DatabaseStructure { get; private set; }

    public IOptimizer Optimizer { get; private set; }

    public IType IdentifierType { get; private set; }

    public virtual void Configure(IType type, IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      this.IdentifierType = type;
      bool forceTableUse = PropertiesHelper.GetBoolean("force_table_use", parms, false);
      string sequenceName = this.DetermineSequenceName(parms, dialect);
      int initialValue = this.DetermineInitialValue(parms);
      int incrementSize = this.DetermineIncrementSize(parms);
      string optimizationStrategy = this.DetermineOptimizationStrategy(parms, incrementSize);
      int adjustedIncrementSize = this.DetermineAdjustedIncrementSize(optimizationStrategy, incrementSize);
      this.Optimizer = OptimizerFactory.BuildOptimizer(optimizationStrategy, this.IdentifierType.ReturnedClass, adjustedIncrementSize, (long) PropertiesHelper.GetInt32("initial_value", parms, -1));
      if (!forceTableUse && this.RequiresPooledSequence(initialValue, adjustedIncrementSize, this.Optimizer) && !dialect.SupportsPooledSequences)
      {
        forceTableUse = true;
        SequenceStyleGenerator.Log.Info((object) "Forcing table use for sequence-style generator due to optimizer selection where db does not support pooled sequences.");
      }
      this.DatabaseStructure = this.BuildDatabaseStructure(type, parms, dialect, forceTableUse, sequenceName, initialValue, adjustedIncrementSize);
      this.DatabaseStructure.Prepare(this.Optimizer);
    }

    protected string DetermineSequenceName(IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      string table = PropertiesHelper.GetString("sequence_name", parms, "hibernate_sequence");
      if (table.IndexOf('.') < 0)
      {
        string schema;
        parms.TryGetValue(PersistentIdGeneratorParmsNames.Schema, out schema);
        string catalog;
        parms.TryGetValue(PersistentIdGeneratorParmsNames.Catalog, out catalog);
        table = dialect.Qualify(catalog, schema, table);
      }
      return table;
    }

    protected string DetermineValueColumnName(IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      return PropertiesHelper.GetString("value_column", parms, "next_val");
    }

    protected int DetermineInitialValue(IDictionary<string, string> parms)
    {
      return PropertiesHelper.GetInt32("initial_value", parms, 1);
    }

    protected int DetermineIncrementSize(IDictionary<string, string> parms)
    {
      return PropertiesHelper.GetInt32("increment_size", parms, 1);
    }

    protected string DetermineOptimizationStrategy(
      IDictionary<string, string> parms,
      int incrementSize)
    {
      string str = PropertiesHelper.GetBoolean("id.optimizer.pooled.prefer_lo", parms, false) ? "pooled-lo" : "pooled";
      string defaultValue = incrementSize <= 1 ? "none" : str;
      return PropertiesHelper.GetString("optimizer", parms, defaultValue);
    }

    protected int DetermineAdjustedIncrementSize(string optimizationStrategy, int incrementSize)
    {
      if ("none".Equals(optimizationStrategy) && incrementSize > 1)
      {
        SequenceStyleGenerator.Log.Warn((object) ("config specified explicit optimizer of [none], but [increment_size=" + (object) incrementSize + "; honoring optimizer setting"));
        incrementSize = 1;
      }
      return incrementSize;
    }

    protected IDatabaseStructure BuildDatabaseStructure(
      IType type,
      IDictionary<string, string> parms,
      NHibernate.Dialect.Dialect dialect,
      bool forceTableUse,
      string sequenceName,
      int initialValue,
      int incrementSize)
    {
      if (dialect.SupportsSequences && !forceTableUse)
        return (IDatabaseStructure) new SequenceStructure(dialect, sequenceName, initialValue, incrementSize);
      string valueColumnName = this.DetermineValueColumnName(parms, dialect);
      return (IDatabaseStructure) new TableStructure(dialect, sequenceName, valueColumnName, initialValue, incrementSize);
    }

    protected bool RequiresPooledSequence(
      int initialValue,
      int incrementSize,
      IOptimizer optimizer)
    {
      int num = optimizer.ApplyIncrementSizeToSourceValues ? incrementSize : 1;
      return initialValue > 1 || num > 1;
    }

    public virtual object Generate(ISessionImplementor session, object obj)
    {
      return this.Optimizer.Generate(this.DatabaseStructure.BuildCallback(session));
    }

    public virtual string GeneratorKey() => this.DatabaseStructure.Name;

    public virtual string[] SqlCreateStrings(NHibernate.Dialect.Dialect dialect)
    {
      return this.DatabaseStructure.SqlCreateStrings(dialect);
    }

    public virtual string[] SqlDropString(NHibernate.Dialect.Dialect dialect)
    {
      return this.DatabaseStructure.SqlDropStrings(dialect);
    }
  }
}
