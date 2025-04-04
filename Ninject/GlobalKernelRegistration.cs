// Decompiled with JetBrains decompiler
// Type: Ninject.GlobalKernelRegistration
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace Ninject
{
  public abstract class GlobalKernelRegistration
  {
    private static readonly ReaderWriterLock kernelRegistrationsLock = new ReaderWriterLock();
    private static readonly IDictionary<Type, GlobalKernelRegistration.Registration> kernelRegistrations = (IDictionary<Type, GlobalKernelRegistration.Registration>) new Dictionary<Type, GlobalKernelRegistration.Registration>();

    internal static void RegisterKernelForType(IKernel kernel, Type type)
    {
      GlobalKernelRegistration.Registration registrationForType = GlobalKernelRegistration.GetRegistrationForType(type);
      registrationForType.KernelLock.AcquireWriterLock(-1);
      try
      {
        registrationForType.Kernels.Add(new WeakReference((object) kernel));
      }
      finally
      {
        registrationForType.KernelLock.ReleaseWriterLock();
      }
    }

    internal static void UnregisterKernelForType(IKernel kernel, Type type)
    {
      GlobalKernelRegistration.Registration registrationForType = GlobalKernelRegistration.GetRegistrationForType(type);
      GlobalKernelRegistration.RemoveKernels(registrationForType, registrationForType.Kernels.Where<WeakReference>((Func<WeakReference, bool>) (reference => reference.Target == kernel || !reference.IsAlive)));
    }

    protected void MapKernels(Action<IKernel> action)
    {
      bool flag = false;
      GlobalKernelRegistration.Registration registrationForType = GlobalKernelRegistration.GetRegistrationForType(this.GetType());
      registrationForType.KernelLock.AcquireReaderLock(-1);
      try
      {
        foreach (WeakReference kernel in (IEnumerable<WeakReference>) registrationForType.Kernels)
        {
          if (kernel.Target is IKernel target)
            action(target);
          else
            flag = true;
        }
      }
      finally
      {
        registrationForType.KernelLock.ReleaseReaderLock();
      }
      if (!flag)
        return;
      GlobalKernelRegistration.RemoveKernels(registrationForType, registrationForType.Kernels.Where<WeakReference>((Func<WeakReference, bool>) (reference => !reference.IsAlive)));
    }

    private static void RemoveKernels(
      GlobalKernelRegistration.Registration registration,
      IEnumerable<WeakReference> references)
    {
      registration.KernelLock.AcquireWriterLock(-1);
      try
      {
        foreach (WeakReference weakReference in references.ToArray<WeakReference>())
          registration.Kernels.Remove(weakReference);
      }
      finally
      {
        registration.KernelLock.ReleaseWriterLock();
      }
    }

    private static GlobalKernelRegistration.Registration GetRegistrationForType(Type type)
    {
      GlobalKernelRegistration.kernelRegistrationsLock.AcquireReaderLock(-1);
      try
      {
        GlobalKernelRegistration.Registration registration;
        return GlobalKernelRegistration.kernelRegistrations.TryGetValue(type, out registration) ? registration : GlobalKernelRegistration.CreateNewRegistration(type);
      }
      finally
      {
        GlobalKernelRegistration.kernelRegistrationsLock.ReleaseReaderLock();
      }
    }

    private static GlobalKernelRegistration.Registration CreateNewRegistration(Type type)
    {
      LockCookie writerLock = GlobalKernelRegistration.kernelRegistrationsLock.UpgradeToWriterLock(-1);
      try
      {
        GlobalKernelRegistration.Registration newRegistration1;
        if (GlobalKernelRegistration.kernelRegistrations.TryGetValue(type, out newRegistration1))
          return newRegistration1;
        GlobalKernelRegistration.Registration newRegistration2 = new GlobalKernelRegistration.Registration();
        GlobalKernelRegistration.kernelRegistrations.Add(type, newRegistration2);
        return newRegistration2;
      }
      finally
      {
        GlobalKernelRegistration.kernelRegistrationsLock.DowngradeFromWriterLock(ref writerLock);
      }
    }

    private class Registration
    {
      public Registration()
      {
        this.KernelLock = new ReaderWriterLock();
        this.Kernels = (IList<WeakReference>) new List<WeakReference>();
      }

      public ReaderWriterLock KernelLock { get; private set; }

      public IList<WeakReference> Kernels { get; private set; }
    }
  }
}
