// Decompiled with JetBrains decompiler
// Type: Ninject.GlobalKernelRegistrationModule`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Modules;

#nullable disable
namespace Ninject
{
  public abstract class GlobalKernelRegistrationModule<TGlobalKernelRegistry> : NinjectModule where TGlobalKernelRegistry : GlobalKernelRegistration
  {
    public override void Load()
    {
      GlobalKernelRegistration.RegisterKernelForType(this.Kernel, typeof (TGlobalKernelRegistry));
    }

    public override void Unload()
    {
      GlobalKernelRegistration.UnregisterKernelForType(this.Kernel, typeof (TGlobalKernelRegistry));
    }
  }
}
