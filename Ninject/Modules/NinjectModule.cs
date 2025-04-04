// Decompiled with JetBrains decompiler
// Type: Ninject.Modules.NinjectModule
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using Ninject.Planning.Bindings;
using Ninject.Syntax;
using System;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Modules
{
  public abstract class NinjectModule : BindingRoot, INinjectModule, IHaveKernel
  {
    protected NinjectModule() => this.Bindings = (ICollection<IBinding>) new List<IBinding>();

    public IKernel Kernel { get; private set; }

    public virtual string Name => this.GetType().FullName;

    public ICollection<IBinding> Bindings { get; private set; }

    protected override IKernel KernelInstance => this.Kernel;

    public void OnLoad(IKernel kernel)
    {
      Ensure.ArgumentNotNull((object) kernel, nameof (kernel));
      this.Kernel = kernel;
      this.Load();
    }

    public void OnUnload(IKernel kernel)
    {
      Ensure.ArgumentNotNull((object) kernel, nameof (kernel));
      this.Unload();
      this.Bindings.Map<IBinding>(new Action<IBinding>(((IBindingRoot) this.Kernel).RemoveBinding));
      this.Kernel = (IKernel) null;
    }

    public void OnVerifyRequiredModules() => this.VerifyRequiredModulesAreLoaded();

    public abstract void Load();

    public virtual void Unload()
    {
    }

    public virtual void VerifyRequiredModulesAreLoaded()
    {
    }

    public override void Unbind(Type service) => this.Kernel.Unbind(service);

    public override void AddBinding(IBinding binding)
    {
      Ensure.ArgumentNotNull((object) binding, nameof (binding));
      this.Kernel.AddBinding(binding);
      this.Bindings.Add(binding);
    }

    public override void RemoveBinding(IBinding binding)
    {
      Ensure.ArgumentNotNull((object) binding, nameof (binding));
      this.Kernel.RemoveBinding(binding);
      this.Bindings.Remove(binding);
    }
  }
}
