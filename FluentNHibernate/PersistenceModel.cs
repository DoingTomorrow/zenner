// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.PersistenceModel
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;

#nullable disable
namespace FluentNHibernate
{
  public class PersistenceModel
  {
    protected readonly IList<IMappingProvider> classProviders = (IList<IMappingProvider>) new List<IMappingProvider>();
    protected readonly IList<IFilterDefinition> filterDefinitions = (IList<IFilterDefinition>) new List<IFilterDefinition>();
    protected readonly IList<IIndeterminateSubclassMappingProvider> subclassProviders = (IList<IIndeterminateSubclassMappingProvider>) new List<IIndeterminateSubclassMappingProvider>();
    protected readonly IList<IExternalComponentMappingProvider> componentProviders = (IList<IExternalComponentMappingProvider>) new List<IExternalComponentMappingProvider>();
    protected readonly IList<IComponentReferenceResolver> componentResolvers = (IList<IComponentReferenceResolver>) new List<IComponentReferenceResolver>()
    {
      (IComponentReferenceResolver) new ComponentMapComponentReferenceResolver()
    };
    private readonly IList<IMappingModelVisitor> visitors = (IList<IMappingModelVisitor>) new List<IMappingModelVisitor>();
    private IEnumerable<HibernateMapping> compiledMappings;
    private ValidationVisitor validationVisitor;
    private IDiagnosticMessageDispatcher diagnosticDispatcher = (IDiagnosticMessageDispatcher) new DefaultDiagnosticMessageDispatcher();
    protected IDiagnosticLogger log = (IDiagnosticLogger) new NullDiagnosticsLogger();

    public IConventionFinder Conventions { get; private set; }

    public bool MergeMappings { get; set; }

    public PairBiDirectionalManyToManySidesDelegate BiDirectionalManyToManyPairer { get; set; }

    public PersistenceModel(IConventionFinder conventionFinder)
    {
      this.BiDirectionalManyToManyPairer = (PairBiDirectionalManyToManySidesDelegate) ((c, o, w) => { });
      this.Conventions = conventionFinder;
      this.visitors.Add((IMappingModelVisitor) new SeparateSubclassVisitor(this.subclassProviders));
      this.visitors.Add((IMappingModelVisitor) new ComponentReferenceResolutionVisitor((IEnumerable<IComponentReferenceResolver>) this.componentResolvers, (IEnumerable<IExternalComponentMappingProvider>) this.componentProviders));
      this.visitors.Add((IMappingModelVisitor) new RelationshipPairingVisitor(this.BiDirectionalManyToManyPairer));
      this.visitors.Add((IMappingModelVisitor) new ManyToManyTableNameVisitor());
      this.visitors.Add((IMappingModelVisitor) new ConventionVisitor(this.Conventions));
      this.visitors.Add((IMappingModelVisitor) new ComponentColumnPrefixVisitor());
      this.visitors.Add((IMappingModelVisitor) new RelationshipKeyPairingVisitor());
      this.visitors.Add((IMappingModelVisitor) (this.validationVisitor = new ValidationVisitor()));
    }

    public PersistenceModel()
      : this((IConventionFinder) new DefaultConventionFinder())
    {
    }

    public void SetLogger(IDiagnosticLogger logger)
    {
      this.log = logger;
      this.Conventions.SetLogger(logger);
    }

    protected void AddMappingsFromThisAssembly()
    {
      this.AddMappingsFromAssembly(PersistenceModel.FindTheCallingAssembly());
    }

    public void AddMappingsFromAssembly(Assembly assembly)
    {
      this.AddMappingsFromSource((ITypeSource) new AssemblyTypeSource(assembly));
    }

    public void AddMappingsFromSource(ITypeSource source)
    {
      source.GetTypes().Where<Type>((Func<Type, bool>) (x => this.IsMappingOf<IMappingProvider>(x) || this.IsMappingOf<IIndeterminateSubclassMappingProvider>(x) || this.IsMappingOf<IExternalComponentMappingProvider>(x) || this.IsMappingOf<IFilterDefinition>(x))).Each<Type>(new Action<Type>(this.Add));
      this.log.LoadedFluentMappingsFromSource(source);
    }

    private static Assembly FindTheCallingAssembly()
    {
      StackTrace stackTrace = new StackTrace(Thread.CurrentThread, false);
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      Assembly theCallingAssembly = (Assembly) null;
      for (int index = 0; index < stackTrace.FrameCount; ++index)
      {
        Assembly assembly = stackTrace.GetFrame(index).GetMethod().DeclaringType.Assembly;
        if (assembly != executingAssembly)
        {
          theCallingAssembly = assembly;
          break;
        }
      }
      return theCallingAssembly;
    }

    public void Add(IMappingProvider provider) => this.classProviders.Add(provider);

    public void Add(IIndeterminateSubclassMappingProvider provider)
    {
      this.subclassProviders.Add(provider);
    }

    public void Add(IFilterDefinition definition) => this.filterDefinitions.Add(definition);

    public void Add(IExternalComponentMappingProvider provider)
    {
      this.componentProviders.Add(provider);
    }

    public void Add(Type type)
    {
      object obj = type.InstantiateUsingParameterlessConstructor();
      switch (obj)
      {
        case IMappingProvider _:
          this.log.FluentMappingDiscovered(type);
          this.Add((IMappingProvider) obj);
          break;
        case IIndeterminateSubclassMappingProvider _:
          this.log.FluentMappingDiscovered(type);
          this.Add((IIndeterminateSubclassMappingProvider) obj);
          break;
        case IFilterDefinition _:
          this.Add((IFilterDefinition) obj);
          break;
        case IExternalComponentMappingProvider _:
          this.log.FluentMappingDiscovered(type);
          this.Add((IExternalComponentMappingProvider) obj);
          break;
        default:
          throw new InvalidOperationException("Unsupported mapping type '" + type.FullName + "'");
      }
    }

    private bool IsMappingOf<T>(Type type)
    {
      return !type.IsGenericType && typeof (T).IsAssignableFrom(type);
    }

    public virtual IEnumerable<HibernateMapping> BuildMappings()
    {
      List<HibernateMapping> mappings = new List<HibernateMapping>();
      if (this.MergeMappings)
        this.BuildSingleMapping(new Action<HibernateMapping>(mappings.Add));
      else
        this.BuildSeparateMappings(new Action<HibernateMapping>(mappings.Add));
      this.ApplyVisitors((IEnumerable<HibernateMapping>) mappings);
      this.log.Flush();
      return (IEnumerable<HibernateMapping>) mappings;
    }

    private void BuildSeparateMappings(Action<HibernateMapping> add)
    {
      foreach (IMappingProvider classProvider in (IEnumerable<IMappingProvider>) this.classProviders)
      {
        HibernateMapping hibernateMapping = classProvider.GetHibernateMapping();
        hibernateMapping.AddClass(classProvider.GetClassMapping());
        add(hibernateMapping);
      }
      foreach (IFilterDefinition filterDefinition in (IEnumerable<IFilterDefinition>) this.filterDefinitions)
      {
        HibernateMapping hibernateMapping = filterDefinition.GetHibernateMapping();
        hibernateMapping.AddFilter(filterDefinition.GetFilterMapping());
        add(hibernateMapping);
      }
    }

    private void BuildSingleMapping(Action<HibernateMapping> add)
    {
      HibernateMapping hibernateMapping = new HibernateMapping();
      foreach (IMappingProvider classProvider in (IEnumerable<IMappingProvider>) this.classProviders)
        hibernateMapping.AddClass(classProvider.GetClassMapping());
      foreach (IFilterDefinition filterDefinition in (IEnumerable<IFilterDefinition>) this.filterDefinitions)
        hibernateMapping.AddFilter(filterDefinition.GetFilterMapping());
      if (hibernateMapping.Classes.Count<ClassMapping>() <= 0)
        return;
      add(hibernateMapping);
    }

    private void ApplyVisitors(IEnumerable<HibernateMapping> mappings)
    {
      foreach (IMappingModelVisitor visitor in (IEnumerable<IMappingModelVisitor>) this.visitors)
        visitor.Visit(mappings);
    }

    private void EnsureMappingsBuilt()
    {
      if (this.compiledMappings != null)
        return;
      this.compiledMappings = this.BuildMappings();
    }

    protected virtual string GetMappingFileName() => "FluentMappings.hbm.xml";

    private string DetermineMappingFileName(HibernateMapping mapping)
    {
      if (this.MergeMappings)
        return this.GetMappingFileName();
      return mapping.Classes.Count<ClassMapping>() > 0 ? mapping.Classes.First<ClassMapping>().Type.FullName + ".hbm.xml" : "filter-def." + mapping.Filters.First<FilterDefinitionMapping>().Name + ".hbm.xml";
    }

    public void WriteMappingsTo(string folder)
    {
      this.WriteMappingsTo((Func<HibernateMapping, XmlTextWriter>) (mapping => new XmlTextWriter(Path.Combine(folder, this.DetermineMappingFileName(mapping)), Encoding.Default)), true);
    }

    public void WriteMappingsTo(TextWriter writer)
    {
      this.WriteMappingsTo((Func<HibernateMapping, XmlTextWriter>) (_ => new XmlTextWriter(writer)), false);
    }

    private void WriteMappingsTo(
      Func<HibernateMapping, XmlTextWriter> writerBuilder,
      bool shouldDispose)
    {
      this.EnsureMappingsBuilt();
      foreach (HibernateMapping compiledMapping in this.compiledMappings)
      {
        XmlDocument xmlDocument = new MappingXmlSerializer().Serialize(compiledMapping);
        XmlTextWriter w = (XmlTextWriter) null;
        try
        {
          w = writerBuilder(compiledMapping);
          w.Formatting = Formatting.Indented;
          xmlDocument.WriteTo((XmlWriter) w);
        }
        finally
        {
          if (shouldDispose && w != null)
            w.Close();
        }
      }
    }

    public virtual void Configure(Configuration cfg)
    {
      this.EnsureMappingsBuilt();
      foreach (HibernateMapping mapping in this.compiledMappings.Where<HibernateMapping>((Func<HibernateMapping, bool>) (m => m.Classes.Count<ClassMapping>() == 0)))
      {
        XmlDocument doc = new MappingXmlSerializer().Serialize(mapping);
        cfg.AddDocument(doc);
      }
      foreach (HibernateMapping mapping in this.compiledMappings.Where<HibernateMapping>((Func<HibernateMapping, bool>) (m => m.Classes.Count<ClassMapping>() > 0)))
      {
        XmlDocument doc = new MappingXmlSerializer().Serialize(mapping);
        if (cfg.GetClassMapping(mapping.Classes.First<ClassMapping>().Type) == null)
          cfg.AddDocument(doc);
      }
    }

    public bool ContainsMapping(Type type)
    {
      return this.classProviders.Any<IMappingProvider>((Func<IMappingProvider, bool>) (x => x.GetType() == type)) || this.filterDefinitions.Any<IFilterDefinition>((Func<IFilterDefinition, bool>) (x => x.GetType() == type)) || this.subclassProviders.Any<IIndeterminateSubclassMappingProvider>((Func<IIndeterminateSubclassMappingProvider, bool>) (x => x.GetType() == type)) || this.componentProviders.Any<IExternalComponentMappingProvider>((Func<IExternalComponentMappingProvider, bool>) (x => x.GetType() == type));
    }

    public bool ValidationEnabled
    {
      get => this.validationVisitor.Enabled;
      set => this.validationVisitor.Enabled = value;
    }

    internal void ImportProviders(PersistenceModel model)
    {
      model.classProviders.Each<IMappingProvider>((Action<IMappingProvider>) (x =>
      {
        if (this.classProviders.Contains(x))
          return;
        this.classProviders.Add(x);
      }));
      model.subclassProviders.Each<IIndeterminateSubclassMappingProvider>((Action<IIndeterminateSubclassMappingProvider>) (x =>
      {
        if (this.subclassProviders.Contains(x))
          return;
        this.subclassProviders.Add(x);
      }));
      model.componentProviders.Each<IExternalComponentMappingProvider>((Action<IExternalComponentMappingProvider>) (x =>
      {
        if (this.componentProviders.Contains(x))
          return;
        this.componentProviders.Add(x);
      }));
    }
  }
}
