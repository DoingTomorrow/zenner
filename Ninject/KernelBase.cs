// Decompiled with JetBrains decompiler
// Type: Ninject.KernelBase
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Activation.Blocks;
using Ninject.Activation.Caching;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Disposal;
using Ninject.Infrastructure.Introspection;
using Ninject.Infrastructure.Language;
using Ninject.Modules;
using Ninject.Parameters;
using Ninject.Planning;
using Ninject.Planning.Bindings;
using Ninject.Planning.Bindings.Resolvers;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject
{
  public abstract class KernelBase : 
    BindingRoot,
    IKernel,
    IBindingRoot,
    IResolutionRoot,
    IFluentSyntax,
    IServiceProvider,
    IDisposableObject,
    IDisposable
  {
    protected readonly object HandleMissingBindingLockObject = new object();
    private readonly Multimap<Type, IBinding> bindings = new Multimap<Type, IBinding>();
    private readonly Multimap<Type, IBinding> bindingCache = new Multimap<Type, IBinding>();
    private readonly Dictionary<string, INinjectModule> modules = new Dictionary<string, INinjectModule>();

    protected KernelBase()
      : this((IComponentContainer) new ComponentContainer(), (INinjectSettings) new NinjectSettings())
    {
    }

    protected KernelBase(params INinjectModule[] modules)
      : this((IComponentContainer) new ComponentContainer(), (INinjectSettings) new NinjectSettings(), modules)
    {
    }

    protected KernelBase(INinjectSettings settings, params INinjectModule[] modules)
      : this((IComponentContainer) new ComponentContainer(), settings, modules)
    {
    }

    protected KernelBase(
      IComponentContainer components,
      INinjectSettings settings,
      params INinjectModule[] modules)
    {
      Ensure.ArgumentNotNull((object) components, nameof (components));
      Ensure.ArgumentNotNull((object) settings, nameof (settings));
      Ensure.ArgumentNotNull((object) modules, nameof (modules));
      this.Settings = settings;
      this.Components = components;
      components.Kernel = (IKernel) this;
      this.AddComponents();
      this.Bind<IKernel>().ToConstant<KernelBase>(this).InTransientScope();
      this.Bind<IResolutionRoot>().ToConstant<KernelBase>(this).InTransientScope();
      if (this.Settings.LoadExtensions)
        this.Load((IEnumerable<string>) this.Settings.ExtensionSearchPatterns);
      this.Load((IEnumerable<INinjectModule>) modules);
    }

    public INinjectSettings Settings { get; private set; }

    public IComponentContainer Components { get; private set; }

    public override void Dispose(bool disposing)
    {
      if (disposing && !this.IsDisposed && this.Components != null)
      {
        this.Components.Get<ICache>().Clear();
        this.Components.Dispose();
      }
      base.Dispose(disposing);
    }

    public override void Unbind(Type service)
    {
      Ensure.ArgumentNotNull((object) service, nameof (service));
      this.bindings.RemoveAll(service);
      lock (this.bindingCache)
        this.bindingCache.Clear();
    }

    public override void AddBinding(IBinding binding)
    {
      Ensure.ArgumentNotNull((object) binding, nameof (binding));
      this.AddBindings((IEnumerable<IBinding>) new IBinding[1]
      {
        binding
      });
    }

    public override void RemoveBinding(IBinding binding)
    {
      Ensure.ArgumentNotNull((object) binding, nameof (binding));
      this.bindings.Remove(binding.Service, binding);
      lock (this.bindingCache)
        this.bindingCache.Clear();
    }

    public bool HasModule(string name)
    {
      Ensure.ArgumentNotNullOrEmpty(name, nameof (name));
      return this.modules.ContainsKey(name);
    }

    public IEnumerable<INinjectModule> GetModules()
    {
      return (IEnumerable<INinjectModule>) this.modules.Values.ToArray<INinjectModule>();
    }

    public void Load(IEnumerable<INinjectModule> m)
    {
      Ensure.ArgumentNotNull((object) m, "modules");
      m = (IEnumerable<INinjectModule>) m.ToList<INinjectModule>();
      foreach (INinjectModule newModule in m)
      {
        if (string.IsNullOrEmpty(newModule.Name))
          throw new NotSupportedException(ExceptionFormatter.ModulesWithNullOrEmptyNamesAreNotSupported());
        INinjectModule existingModule;
        if (this.modules.TryGetValue(newModule.Name, out existingModule))
          throw new NotSupportedException(ExceptionFormatter.ModuleWithSameNameIsAlreadyLoaded(newModule, existingModule));
        newModule.OnLoad((IKernel) this);
        this.modules.Add(newModule.Name, newModule);
      }
      foreach (INinjectModule ninjectModule in m)
        ninjectModule.OnVerifyRequiredModules();
    }

    public void Load(IEnumerable<string> filePatterns)
    {
      this.Components.Get<IModuleLoader>().LoadModules(filePatterns);
    }

    public void Load(IEnumerable<Assembly> assemblies)
    {
      this.Load(assemblies.SelectMany<Assembly, INinjectModule>((Func<Assembly, IEnumerable<INinjectModule>>) (asm => asm.GetNinjectModules())));
    }

    public void Unload(string name)
    {
      Ensure.ArgumentNotNullOrEmpty(name, nameof (name));
      INinjectModule ninjectModule;
      if (!this.modules.TryGetValue(name, out ninjectModule))
        throw new NotSupportedException(ExceptionFormatter.NoModuleLoadedWithTheSpecifiedName(name));
      ninjectModule.OnUnload((IKernel) this);
      this.modules.Remove(name);
    }

    public virtual void Inject(object instance, params IParameter[] parameters)
    {
      Ensure.ArgumentNotNull(instance, nameof (instance));
      Ensure.ArgumentNotNull((object) parameters, nameof (parameters));
      Type type = instance.GetType();
      IPlanner planner = this.Components.Get<IPlanner>();
      IPipeline pipeline = this.Components.Get<IPipeline>();
      Binding binding = new Binding(type);
      IContext context = this.CreateContext(this.CreateRequest(type, (Func<IBindingMetadata, bool>) null, (IEnumerable<IParameter>) parameters, false, false), (IBinding) binding);
      context.Plan = planner.GetPlan(type);
      InstanceReference reference = new InstanceReference()
      {
        Instance = instance
      };
      pipeline.Activate(context, reference);
    }

    public virtual bool Release(object instance)
    {
      Ensure.ArgumentNotNull(instance, nameof (instance));
      return this.Components.Get<ICache>().Release(instance);
    }

    public virtual bool CanResolve(IRequest request)
    {
      Ensure.ArgumentNotNull((object) request, nameof (request));
      return this.GetBindings(request.Service).Any<IBinding>(this.SatifiesRequest(request));
    }

    public virtual bool CanResolve(IRequest request, bool ignoreImplicitBindings)
    {
      Ensure.ArgumentNotNull((object) request, nameof (request));
      return this.GetBindings(request.Service).Any<IBinding>((Func<IBinding, bool>) (binding => (!ignoreImplicitBindings || !binding.IsImplicit) && this.SatifiesRequest(request)(binding)));
    }

    public virtual IEnumerable<object> Resolve(IRequest request)
    {
      Ensure.ArgumentNotNull((object) request, nameof (request));
      IComparer<IBinding> bindingPrecedenceComparer = this.GetBindingPrecedenceComparer();
      IEnumerable<IBinding> source = Enumerable.Empty<IBinding>();
      if (this.CanResolve(request) || this.HandleMissingBinding(request))
        source = this.GetBindings(request.Service).Where<IBinding>(this.SatifiesRequest(request));
      if (!source.Any<IBinding>())
      {
        if (request.IsOptional)
          return Enumerable.Empty<object>();
        throw new ActivationException(ExceptionFormatter.CouldNotResolveBinding(request));
      }
      if (request.IsUnique)
      {
        IEnumerable<IBinding> list = (IEnumerable<IBinding>) source.OrderByDescending<IBinding, IBinding>((Func<IBinding, IBinding>) (b => b), bindingPrecedenceComparer).ToList<IBinding>();
        IBinding model = list.First<IBinding>();
        source = list.TakeWhile<IBinding>((Func<IBinding, bool>) (binding => bindingPrecedenceComparer.Compare(binding, model) == 0));
        if (source.Count<IBinding>() > 1)
        {
          if (request.IsOptional)
            return Enumerable.Empty<object>();
          throw new ActivationException(ExceptionFormatter.CouldNotUniquelyResolveBinding(request));
        }
      }
      if (source.Any<IBinding>((Func<IBinding, bool>) (binding => !binding.IsImplicit)))
        source = source.Where<IBinding>((Func<IBinding, bool>) (binding => !binding.IsImplicit));
      return source.Select<IBinding, object>((Func<IBinding, object>) (binding => this.CreateContext(request, binding).Resolve()));
    }

    public virtual IRequest CreateRequest(
      Type service,
      Func<IBindingMetadata, bool> constraint,
      IEnumerable<IParameter> parameters,
      bool isOptional,
      bool isUnique)
    {
      Ensure.ArgumentNotNull((object) service, nameof (service));
      Ensure.ArgumentNotNull((object) parameters, nameof (parameters));
      return (IRequest) new Request(service, constraint, parameters, (Func<object>) null, isOptional, isUnique);
    }

    public virtual IActivationBlock BeginBlock()
    {
      return (IActivationBlock) new ActivationBlock((IResolutionRoot) this);
    }

    public virtual IEnumerable<IBinding> GetBindings(Type service)
    {
      Ensure.ArgumentNotNull((object) service, nameof (service));
      lock (this.bindingCache)
      {
        if (!this.bindingCache.ContainsKey(service))
          this.Components.GetAll<IBindingResolver>().SelectMany<IBindingResolver, IBinding>((Func<IBindingResolver, IEnumerable<IBinding>>) (resolver => resolver.Resolve(this.bindings, service))).Map<IBinding>((Action<IBinding>) (binding => this.bindingCache.Add(service, binding)));
        return (IEnumerable<IBinding>) this.bindingCache[service];
      }
    }

    protected virtual IComparer<IBinding> GetBindingPrecedenceComparer()
    {
      return (IComparer<IBinding>) new KernelBase.BindingPrecedenceComparer();
    }

    protected virtual Func<IBinding, bool> SatifiesRequest(IRequest request)
    {
      return (Func<IBinding, bool>) (binding => binding.Matches(request) && request.Matches(binding));
    }

    protected abstract void AddComponents();

    [Obsolete]
    protected virtual bool HandleMissingBinding(Type service) => false;

    protected virtual bool HandleMissingBinding(IRequest request)
    {
      Ensure.ArgumentNotNull((object) request, nameof (request));
      if (this.HandleMissingBinding(request.Service))
        return true;
      List<IBinding> series = this.Components.GetAll<IMissingBindingResolver>().Select<IMissingBindingResolver, List<IBinding>>((Func<IMissingBindingResolver, List<IBinding>>) (c => c.Resolve(this.bindings, request).ToList<IBinding>())).FirstOrDefault<List<IBinding>>((Func<List<IBinding>, bool>) (b => b.Any<IBinding>()));
      if (series == null)
        return false;
      lock (this.HandleMissingBindingLockObject)
      {
        if (!this.CanResolve(request))
        {
          series.Map<IBinding>((Action<IBinding>) (binding => binding.IsImplicit = true));
          this.AddBindings((IEnumerable<IBinding>) series);
        }
      }
      return true;
    }

    [Obsolete]
    protected virtual bool TypeIsSelfBindable(Type service)
    {
      return !service.IsInterface && !service.IsAbstract && !service.IsValueType && service != typeof (string) && !service.ContainsGenericParameters;
    }

    protected virtual IContext CreateContext(IRequest request, IBinding binding)
    {
      Ensure.ArgumentNotNull((object) request, nameof (request));
      Ensure.ArgumentNotNull((object) binding, nameof (binding));
      return (IContext) new Context((IKernel) this, request, binding, this.Components.Get<ICache>(), this.Components.Get<IPlanner>(), this.Components.Get<IPipeline>());
    }

    private void AddBindings(IEnumerable<IBinding> bindings)
    {
      bindings.Map<IBinding>((Action<IBinding>) (binding => this.bindings.Add(binding.Service, binding)));
      lock (this.bindingCache)
        this.bindingCache.Clear();
    }

    object IServiceProvider.GetService(Type service) => this.Get(service);

    private class BindingPrecedenceComparer : IComparer<IBinding>
    {
      public int Compare(IBinding x, IBinding y)
      {
        if (x == y)
          return 0;
        return new List<Func<IBinding, bool>>()
        {
          (Func<IBinding, bool>) (b => b != null),
          (Func<IBinding, bool>) (b => b.IsConditional),
          (Func<IBinding, bool>) (b => !b.Service.ContainsGenericParameters),
          (Func<IBinding, bool>) (b => !b.IsImplicit)
        }.Select(func => new{ func = func, xVal = func(x) }).Where(_param1 => _param1.xVal != _param1.func(y)).Select(_param0 => !_param0.xVal ? -1 : 1).FirstOrDefault<int>();
      }
    }
  }
}
