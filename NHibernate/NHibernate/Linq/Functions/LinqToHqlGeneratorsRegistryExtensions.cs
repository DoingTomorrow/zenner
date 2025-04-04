// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.LinqToHqlGeneratorsRegistryExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Functions
{
  public static class LinqToHqlGeneratorsRegistryExtensions
  {
    public static void Merge(
      this ILinqToHqlGeneratorsRegistry registry,
      IHqlGeneratorForMethod generator)
    {
      if (registry == null)
        throw new ArgumentNullException(nameof (registry));
      if (generator == null)
        throw new ArgumentNullException(nameof (generator));
      Array.ForEach<MethodInfo>(generator.SupportedMethods.ToArray<MethodInfo>(), (Action<MethodInfo>) (method => registry.RegisterGenerator(method, generator)));
    }

    public static void Merge(
      this ILinqToHqlGeneratorsRegistry registry,
      IHqlGeneratorForProperty generator)
    {
      if (registry == null)
        throw new ArgumentNullException(nameof (registry));
      if (generator == null)
        throw new ArgumentNullException(nameof (generator));
      Array.ForEach<MemberInfo>(generator.SupportedProperties.ToArray<MemberInfo>(), (Action<MemberInfo>) (property => registry.RegisterGenerator(property, generator)));
    }
  }
}
