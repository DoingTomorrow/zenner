// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.LinqToHqlGeneratorsRegistryFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Linq.Functions
{
  public sealed class LinqToHqlGeneratorsRegistryFactory
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (LinqToHqlGeneratorsRegistryFactory));

    public static ILinqToHqlGeneratorsRegistry CreateGeneratorsRegistry(
      IDictionary<string, string> properties)
    {
      string name;
      if (!properties.TryGetValue("linqtohql.generatorsregistry", out name))
        return (ILinqToHqlGeneratorsRegistry) new DefaultLinqToHqlGeneratorsRegistry();
      try
      {
        LinqToHqlGeneratorsRegistryFactory.log.Info((object) ("Initializing LinqToHqlGeneratorsRegistry: " + name));
        return (ILinqToHqlGeneratorsRegistry) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(name));
      }
      catch (Exception ex)
      {
        LinqToHqlGeneratorsRegistryFactory.log.Fatal((object) "Could not instantiate LinqToHqlGeneratorsRegistry", ex);
        throw new HibernateException("Could not instantiate LinqToHqlGeneratorsRegistry: " + name, ex);
      }
    }
  }
}
