// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.DefaultLinqToHqlGeneratorsRegistry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Functions
{
  public class DefaultLinqToHqlGeneratorsRegistry : ILinqToHqlGeneratorsRegistry
  {
    private readonly Dictionary<MethodInfo, IHqlGeneratorForMethod> registeredMethods = new Dictionary<MethodInfo, IHqlGeneratorForMethod>();
    private readonly Dictionary<MemberInfo, IHqlGeneratorForProperty> registeredProperties = new Dictionary<MemberInfo, IHqlGeneratorForProperty>();
    private readonly List<IRuntimeMethodHqlGenerator> runtimeMethodHqlGenerators = new List<IRuntimeMethodHqlGenerator>();

    public DefaultLinqToHqlGeneratorsRegistry()
    {
      this.RegisterGenerator((IRuntimeMethodHqlGenerator) new StandardLinqExtensionMethodGenerator());
      this.RegisterGenerator((IRuntimeMethodHqlGenerator) new CollectionContainsRuntimeHqlGenerator());
      this.RegisterGenerator((IRuntimeMethodHqlGenerator) new DictionaryItemRuntimeHqlGenerator());
      this.RegisterGenerator((IRuntimeMethodHqlGenerator) new DictionaryContainsKeyRuntimeHqlGenerator());
      this.RegisterGenerator((IRuntimeMethodHqlGenerator) new GenericDictionaryItemRuntimeHqlGenerator());
      this.RegisterGenerator((IRuntimeMethodHqlGenerator) new GenericDictionaryContainsKeyRuntimeHqlGenerator());
      this.RegisterGenerator((IRuntimeMethodHqlGenerator) new ToStringRuntimeMethodHqlGenerator());
      this.RegisterGenerator((IRuntimeMethodHqlGenerator) new LikeGenerator());
      this.RegisterGenerator((IRuntimeMethodHqlGenerator) new GetValueOrDefaultGenerator());
      this.RegisterGenerator((IRuntimeMethodHqlGenerator) new CompareGenerator());
      this.Merge((IHqlGeneratorForMethod) new CompareGenerator());
      this.Merge((IHqlGeneratorForMethod) new StartsWithGenerator());
      this.Merge((IHqlGeneratorForMethod) new EndsWithGenerator());
      this.Merge((IHqlGeneratorForMethod) new ContainsGenerator());
      this.Merge((IHqlGeneratorForMethod) new EqualsGenerator());
      this.Merge((IHqlGeneratorForMethod) new BoolEqualsGenerator());
      this.Merge((IHqlGeneratorForMethod) new ToUpperGenerator());
      this.Merge((IHqlGeneratorForMethod) new ToLowerGenerator());
      this.Merge((IHqlGeneratorForMethod) new SubStringGenerator());
      this.Merge((IHqlGeneratorForMethod) new IndexOfGenerator());
      this.Merge((IHqlGeneratorForMethod) new ReplaceGenerator());
      this.Merge((IHqlGeneratorForProperty) new LengthGenerator());
      this.Merge((IHqlGeneratorForMethod) new TrimGenerator());
      this.Merge((IHqlGeneratorForMethod) new MathGenerator());
      this.Merge((IHqlGeneratorForMethod) new AnyHqlGenerator());
      this.Merge((IHqlGeneratorForMethod) new AllHqlGenerator());
      this.Merge((IHqlGeneratorForMethod) new MinHqlGenerator());
      this.Merge((IHqlGeneratorForMethod) new MaxHqlGenerator());
      this.Merge((IHqlGeneratorForMethod) new CollectionContainsGenerator());
      this.Merge((IHqlGeneratorForProperty) new DateTimePropertiesHqlGenerator());
    }

    protected bool GetRuntimeMethodGenerator(
      MethodInfo method,
      out IHqlGeneratorForMethod methodGenerator)
    {
      methodGenerator = (IHqlGeneratorForMethod) null;
      using (IEnumerator<IRuntimeMethodHqlGenerator> enumerator = this.runtimeMethodHqlGenerators.Where<IRuntimeMethodHqlGenerator>((Func<IRuntimeMethodHqlGenerator, bool>) (typeGenerator => typeGenerator.SupportsMethod(method))).GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          IRuntimeMethodHqlGenerator current = enumerator.Current;
          methodGenerator = current.GetMethodGenerator(method);
          return true;
        }
      }
      return false;
    }

    public virtual bool TryGetGenerator(MethodInfo method, out IHqlGeneratorForMethod generator)
    {
      if (method.IsGenericMethod)
        method = method.GetGenericMethodDefinition();
      return this.registeredMethods.TryGetValue(method, out generator) || this.GetRuntimeMethodGenerator(method, out generator);
    }

    public virtual bool TryGetGenerator(MemberInfo property, out IHqlGeneratorForProperty generator)
    {
      return this.registeredProperties.TryGetValue(property, out generator);
    }

    public virtual void RegisterGenerator(MethodInfo method, IHqlGeneratorForMethod generator)
    {
      this.registeredMethods.Add(method, generator);
    }

    public virtual void RegisterGenerator(MemberInfo property, IHqlGeneratorForProperty generator)
    {
      this.registeredProperties.Add(property, generator);
    }

    public void RegisterGenerator(IRuntimeMethodHqlGenerator generator)
    {
      this.runtimeMethodHqlGenerators.Add(generator);
    }
  }
}
