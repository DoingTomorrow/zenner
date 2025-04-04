// Decompiled with JetBrains decompiler
// Type: Ninject.StandardKernel
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Activation.Caching;
using Ninject.Activation.Strategies;
using Ninject.Injection;
using Ninject.Modules;
using Ninject.Planning;
using Ninject.Planning.Bindings.Resolvers;
using Ninject.Planning.Strategies;
using Ninject.Selection;
using Ninject.Selection.Heuristics;

#nullable disable
namespace Ninject
{
  public class StandardKernel : KernelBase
  {
    public StandardKernel(params INinjectModule[] modules)
      : base(modules)
    {
    }

    public StandardKernel(INinjectSettings settings, params INinjectModule[] modules)
      : base(settings, modules)
    {
    }

    protected override IKernel KernelInstance => (IKernel) this;

    protected override void AddComponents()
    {
      this.Components.Add<IPlanner, Planner>();
      this.Components.Add<IPlanningStrategy, ConstructorReflectionStrategy>();
      this.Components.Add<IPlanningStrategy, PropertyReflectionStrategy>();
      this.Components.Add<IPlanningStrategy, MethodReflectionStrategy>();
      this.Components.Add<ISelector, Selector>();
      this.Components.Add<IConstructorScorer, StandardConstructorScorer>();
      this.Components.Add<IInjectionHeuristic, StandardInjectionHeuristic>();
      this.Components.Add<IPipeline, Pipeline>();
      this.Components.Add<IActivationStrategy, ActivationCacheStrategy>();
      this.Components.Add<IActivationStrategy, PropertyInjectionStrategy>();
      this.Components.Add<IActivationStrategy, MethodInjectionStrategy>();
      this.Components.Add<IActivationStrategy, InitializableStrategy>();
      this.Components.Add<IActivationStrategy, StartableStrategy>();
      this.Components.Add<IActivationStrategy, BindingActionStrategy>();
      this.Components.Add<IActivationStrategy, DisposableStrategy>();
      this.Components.Add<IBindingResolver, StandardBindingResolver>();
      this.Components.Add<IBindingResolver, OpenGenericBindingResolver>();
      this.Components.Add<IMissingBindingResolver, DefaultValueBindingResolver>();
      this.Components.Add<IMissingBindingResolver, SelfBindingResolver>();
      if (!this.Settings.UseReflectionBasedInjection)
        this.Components.Add<IInjectorFactory, DynamicMethodInjectorFactory>();
      else
        this.Components.Add<IInjectorFactory, ReflectionInjectorFactory>();
      this.Components.Add<ICache, Cache>();
      this.Components.Add<IActivationCache, ActivationCache>();
      this.Components.Add<ICachePruner, GarbageCollectionCachePruner>();
      this.Components.Add<IModuleLoader, ModuleLoader>();
      this.Components.Add<IModuleLoaderPlugin, CompiledModuleLoaderPlugin>();
      this.Components.Add<IAssemblyNameRetriever, AssemblyNameRetriever>();
    }
  }
}
