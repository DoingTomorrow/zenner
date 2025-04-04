// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.IdentifierGeneratorFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Id.Enhanced;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Id
{
  public sealed class IdentifierGeneratorFactory
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (IdentifierGeneratorFactory));
    private static readonly Dictionary<string, System.Type> idgenerators = new Dictionary<string, System.Type>();
    public static readonly object ShortCircuitIndicator = new object();
    public static readonly object PostInsertIndicator = new object();

    public static object GetGeneratedIdentity(
      IDataReader rs,
      IType type,
      ISessionImplementor session)
    {
      if (!rs.Read())
        throw new HibernateException("The database returned no natively generated identity value");
      object generatedIdentity = IdentifierGeneratorFactory.Get(rs, type, session);
      if (IdentifierGeneratorFactory.log.IsDebugEnabled)
        IdentifierGeneratorFactory.log.Debug((object) ("Natively generated identity: " + generatedIdentity));
      return generatedIdentity;
    }

    public static object Get(IDataReader rs, IType type, ISessionImplementor session)
    {
      try
      {
        return type.NullSafeGet(rs, rs.GetName(0), session, (object) null);
      }
      catch (Exception ex)
      {
        throw new IdentifierGenerationException("could not retrieve identifier value", ex);
      }
    }

    static IdentifierGeneratorFactory()
    {
      IdentifierGeneratorFactory.idgenerators.Add("uuid.hex", typeof (UUIDHexGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("uuid.string", typeof (UUIDStringGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("hilo", typeof (TableHiLoGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("assigned", typeof (Assigned));
      IdentifierGeneratorFactory.idgenerators.Add("counter", typeof (CounterGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("increment", typeof (IncrementGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("sequence", typeof (SequenceGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("seqhilo", typeof (SequenceHiLoGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("vm", typeof (CounterGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("foreign", typeof (ForeignGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("guid", typeof (GuidGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("guid.comb", typeof (GuidCombGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("guid.native", typeof (NativeGuidGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("select", typeof (SelectGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("sequence-identity", typeof (SequenceIdentityGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("trigger-identity", typeof (TriggerIdentityGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("enhanced-sequence", typeof (SequenceStyleGenerator));
      IdentifierGeneratorFactory.idgenerators.Add("enhanced-table", typeof (NHibernate.Id.Enhanced.TableGenerator));
    }

    private IdentifierGeneratorFactory()
    {
    }

    public static IIdentifierGenerator Create(
      string strategy,
      IType type,
      IDictionary<string, string> parms,
      NHibernate.Dialect.Dialect dialect)
    {
      try
      {
        IIdentifierGenerator instance = (IIdentifierGenerator) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(IdentifierGeneratorFactory.GetIdentifierGeneratorClass(strategy, dialect));
        if (instance is IConfigurable configurable)
          configurable.Configure(type, parms, dialect);
        return instance;
      }
      catch (IdentifierGenerationException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new MappingException("could not instantiate id generator: " + strategy, ex);
      }
    }

    public static object CreateNumber(long value, System.Type type)
    {
      if (type == typeof (byte))
        return (object) (byte) value;
      if (type == typeof (sbyte))
        return (object) (sbyte) value;
      if (type == typeof (short))
        return (object) (short) value;
      if (type == typeof (ushort))
        return (object) (ushort) value;
      if (type == typeof (int))
        return (object) (int) value;
      if (type == typeof (uint))
        return (object) (uint) value;
      if (type == typeof (long))
        return (object) value;
      if (type == typeof (ulong))
        return (object) (ulong) value;
      if (type == typeof (Decimal))
        return (object) (Decimal) value;
      try
      {
        return Convert.ChangeType((object) value, type);
      }
      catch (Exception ex)
      {
        throw new IdentifierGenerationException("could not convert generated value to type " + (object) type, ex);
      }
    }

    public static System.Type GetIdentifierGeneratorClass(string strategy, NHibernate.Dialect.Dialect dialect)
    {
      System.Type identifierGeneratorClass;
      if ("native".Equals(strategy))
        identifierGeneratorClass = dialect.NativeIdentifierGeneratorClass;
      else if ("identity".Equals(strategy))
        identifierGeneratorClass = dialect.IdentityStyleIdentifierGeneratorClass;
      else
        IdentifierGeneratorFactory.idgenerators.TryGetValue(strategy, out identifierGeneratorClass);
      try
      {
        if (identifierGeneratorClass == null)
          identifierGeneratorClass = ReflectHelper.ClassForName(strategy);
      }
      catch (Exception ex)
      {
        throw new IdentifierGenerationException("Could not interpret id generator strategy: " + strategy);
      }
      return identifierGeneratorClass;
    }
  }
}
