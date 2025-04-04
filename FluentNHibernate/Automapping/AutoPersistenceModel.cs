// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.AutoPersistenceModel
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Visitors;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public class AutoPersistenceModel : PersistenceModel
  {
    private readonly IAutomappingConfiguration cfg;
    private readonly AutoMappingExpressions expressions;
    private readonly AutoMapper autoMapper;
    private readonly List<ITypeSource> sources = new List<ITypeSource>();
    private Func<Type, bool> whereClause;
    private readonly List<AutoMapType> mappingTypes = new List<AutoMapType>();
    private bool autoMappingsCreated;
    private readonly AutoMappingAlterationCollection alterations = new AutoMappingAlterationCollection();
    private readonly List<InlineOverride> inlineOverrides = new List<InlineOverride>();
    private readonly List<Type> ignoredTypes = new List<Type>();
    private readonly List<Type> includedTypes = new List<Type>();

    public AutoPersistenceModel()
    {
      this.expressions = new AutoMappingExpressions();
      this.cfg = (IAutomappingConfiguration) new ExpressionBasedAutomappingConfiguration(this.expressions);
      this.autoMapper = new AutoMapper(this.cfg, (IConventionFinder) this.Conventions, (IEnumerable<InlineOverride>) this.inlineOverrides);
      this.componentResolvers.Add((IComponentReferenceResolver) new AutomappedComponentResolver(this.autoMapper, this.cfg));
    }

    public AutoPersistenceModel(IAutomappingConfiguration cfg)
    {
      this.cfg = cfg;
      this.autoMapper = new AutoMapper(cfg, (IConventionFinder) this.Conventions, (IEnumerable<InlineOverride>) this.inlineOverrides);
      this.componentResolvers.Add((IComponentReferenceResolver) new AutomappedComponentResolver(this.autoMapper, cfg));
    }

    public AutoPersistenceModel AddMappingsFromAssemblyOf<T>()
    {
      return this.AddMappingsFromAssembly(typeof (T).Assembly);
    }

    public AutoPersistenceModel AddMappingsFromAssembly(Assembly assembly)
    {
      this.AddMappingsFromSource((ITypeSource) new AssemblyTypeSource(assembly));
      return this;
    }

    public AutoPersistenceModel AddMappingsFromSource(ITypeSource source)
    {
      base.AddMappingsFromSource(source);
      return this;
    }

    public AutoPersistenceModel Alterations(
      Action<AutoMappingAlterationCollection> alterationDelegate)
    {
      alterationDelegate(this.alterations);
      return this;
    }

    public AutoPersistenceModel UseOverridesFromAssemblyOf<T>()
    {
      return this.UseOverridesFromAssembly(typeof (T).Assembly);
    }

    public AutoPersistenceModel UseOverridesFromAssembly(Assembly assembly)
    {
      this.alterations.Add((IAutoMappingAlteration) new AutoMappingOverrideAlteration(assembly));
      return this;
    }

    public SetupConventionFinder<AutoPersistenceModel> Conventions
    {
      get => new SetupConventionFinder<AutoPersistenceModel>(this, base.Conventions);
    }

    [Obsolete("Depreciated in favour of supplying your own IAutomappingConfiguration instance to AutoMap: AutoMap.AssemblyOf<T>(your_configuration_instance)")]
    public AutoPersistenceModel Setup(Action<AutoMappingExpressions> expressionsAction)
    {
      if (this.HasUserDefinedConfiguration)
        throw new InvalidOperationException("Cannot use Setup method when using a user-defined IAutomappingConfiguration instance.");
      expressionsAction(this.expressions);
      return this;
    }

    public AutoPersistenceModel Where(Func<Type, bool> where)
    {
      if (this.HasUserDefinedConfiguration)
        throw new InvalidOperationException("Cannot use Where method when using a user-defined IAutomappingConfiguration instance.");
      this.whereClause = where;
      return this;
    }

    public override IEnumerable<HibernateMapping> BuildMappings()
    {
      this.CompileMappings();
      return base.BuildMappings();
    }

    private void CompileMappings()
    {
      if (this.autoMappingsCreated)
        return;
      this.alterations.Apply(this);
      foreach (Type type in (IEnumerable<Type>) this.sources.SelectMany<ITypeSource, Type>((Func<ITypeSource, IEnumerable<Type>>) (x => x.GetTypes())).OrderBy<Type, int>((Func<Type, int>) (x => this.InheritanceHierarchyDepth(x))))
      {
        if (!this.cfg.ShouldMap(type))
          this.log.AutomappingSkippedType(type, "Skipped by result of IAutomappingConfiguration.ShouldMap(Type)");
        else if (this.whereClause != null && !this.whereClause(type))
          this.log.AutomappingSkippedType(type, "Skipped by Where clause");
        else if (this.ShouldMap(type))
          this.mappingTypes.Add(new AutoMapType(type));
      }
      this.log.AutomappingCandidateTypes(this.mappingTypes.Select<AutoMapType, Type>((Func<AutoMapType, Type>) (x => x.Type)));
      foreach (AutoMapType mappingType in this.mappingTypes)
      {
        if (!mappingType.IsMapped)
          this.AddMapping(mappingType.Type);
      }
      this.autoMappingsCreated = true;
    }

    private int InheritanceHierarchyDepth(Type type)
    {
      int num = 0;
      Type type1 = type;
      while (type1 != null && type1 != typeof (object))
      {
        type1 = type1.BaseType;
        ++num;
      }
      return num;
    }

    public override void Configure(Configuration configuration)
    {
      this.CompileMappings();
      base.Configure(configuration);
    }

    private void AddMapping(Type type)
    {
      this.log.BeginAutomappingType(type);
      this.Add((IMappingProvider) new PassThroughMappingProvider(this.autoMapper.Map(this.GetTypeToMap(type), this.mappingTypes)));
    }

    private Type GetTypeToMap(Type type)
    {
      while (this.ShouldMapParent(type))
        type = type.BaseType;
      return type;
    }

    private bool ShouldMapParent(Type type)
    {
      return this.ShouldMap(type.BaseType) && !this.cfg.IsConcreteBaseType(type.BaseType);
    }

    private bool ShouldMap(Type type)
    {
      if (this.includedTypes.Contains(type))
        return true;
      if (this.ignoredTypes.Contains(type))
      {
        this.log.AutomappingSkippedType(type, "Skipped by IgnoreBase");
        return false;
      }
      if (type.IsGenericType && this.ignoredTypes.Contains(type.GetGenericTypeDefinition()))
      {
        this.log.AutomappingSkippedType(type, "Skipped by IgnoreBase");
        return false;
      }
      if (type.IsAbstract && this.cfg.AbstractClassIsLayerSupertype(type))
      {
        this.log.AutomappingSkippedType(type, "Skipped by IAutomappingConfiguration.AbstractClassIsLayerSupertype(Type)");
        return false;
      }
      if (this.cfg.IsComponent(type))
      {
        this.log.AutomappingSkippedType(type, "Skipped by IAutomappingConfiguration.IsComponent(Type)");
        return false;
      }
      return type != typeof (object);
    }

    public IMappingProvider FindMapping<T>() => this.FindMapping(typeof (T));

    public IMappingProvider FindMapping(Type type)
    {
      Func<IMappingProvider, Type, bool> finder = (Func<IMappingProvider, Type, bool>) ((provider, expectedType) =>
      {
        Type type1 = provider.GetType();
        if (type1.IsGenericType)
          return type1.GetGenericArguments()[0] == expectedType;
        if (type1.BaseType.IsGenericType && type1.BaseType.GetGenericTypeDefinition() == typeof (ClassMap<>))
          return type1.BaseType.GetGenericArguments()[0] == expectedType;
        return provider is PassThroughMappingProvider && provider.GetClassMapping().Type == expectedType;
      });
      IMappingProvider mapping = this.classProviders.FirstOrDefault<IMappingProvider>((Func<IMappingProvider, bool>) (t => finder(t, type)));
      if (mapping != null)
        return mapping;
      return type.BaseType != typeof (object) && !this.cfg.IsConcreteBaseType(type.BaseType) ? this.FindMapping(type.BaseType) : (IMappingProvider) null;
    }

    public AutoPersistenceModel AddEntityAssembly(Assembly assembly)
    {
      return this.AddTypeSource((ITypeSource) new AssemblyTypeSource(assembly));
    }

    public AutoPersistenceModel AddTypeSource(ITypeSource source)
    {
      this.sources.Add(source);
      return this;
    }

    public AutoPersistenceModel AddFilter<TFilter>() where TFilter : IFilterDefinition
    {
      this.Add(typeof (TFilter));
      return this;
    }

    internal void AddOverride(Type type, Action<object> action)
    {
      this.inlineOverrides.Add(new InlineOverride(type, action));
    }

    public AutoPersistenceModel Override<T>(Action<AutoMapping<T>> populateMap)
    {
      this.inlineOverrides.Add(new InlineOverride(typeof (T), (Action<object>) (x =>
      {
        if (!(x is AutoMapping<T>))
          return;
        populateMap((AutoMapping<T>) x);
      })));
      return this;
    }

    public void Override(Type overrideType)
    {
      Type type = ((IEnumerable<Type>) overrideType.GetInterfaces()).Where<Type>((Func<Type, bool>) (x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IAutoMappingOverride<>) && x.GetGenericArguments().Length > 0)).FirstOrDefault<Type>();
      if (type == null)
        return;
      Type entityType = ((IEnumerable<Type>) type.GetGenericArguments()).First<Type>();
      Type autoMappingType = typeof (AutoMapping<>).MakeGenericType(entityType);
      this.AddOverride(entityType, (Action<object>) (x =>
      {
        if (!x.GetType().IsAssignableFrom(autoMappingType))
          return;
        object instance = Activator.CreateInstance(overrideType);
        typeof (AutoPersistenceModel).GetMethod("OverrideHelper", BindingFlags.Instance | BindingFlags.NonPublic)?.MakeGenericMethod(entityType).Invoke((object) this, new object[2]
        {
          x,
          instance
        });
      }));
    }

    private void OverrideHelper<T>(AutoMapping<T> x, IAutoMappingOverride<T> mappingOverride)
    {
      mappingOverride.Override(x);
    }

    public AutoPersistenceModel OverrideAll(Action<IPropertyIgnorer> alteration)
    {
      this.inlineOverrides.Add(new InlineOverride(typeof (object), (Action<object>) (x =>
      {
        if (!(x is IPropertyIgnorer))
          return;
        alteration((IPropertyIgnorer) x);
      })));
      return this;
    }

    public AutoPersistenceModel IgnoreBase<T>() => this.IgnoreBase(typeof (T));

    public AutoPersistenceModel IgnoreBase(Type baseType)
    {
      this.ignoredTypes.Add(baseType);
      return this;
    }

    public AutoPersistenceModel IncludeBase<T>() => this.IncludeBase(typeof (T));

    public AutoPersistenceModel IncludeBase(Type baseType)
    {
      this.includedTypes.Add(baseType);
      return this;
    }

    protected override string GetMappingFileName() => "AutoMappings.hbm.xml";

    private bool HasUserDefinedConfiguration
    {
      get => !(this.cfg is ExpressionBasedAutomappingConfiguration);
    }
  }
}
