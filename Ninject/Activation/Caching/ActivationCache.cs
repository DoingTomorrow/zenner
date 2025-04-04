// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Caching.ActivationCache
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using Ninject.Infrastructure;
using System;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Activation.Caching
{
  public class ActivationCache : 
    NinjectComponent,
    IActivationCache,
    INinjectComponent,
    IDisposable,
    IPruneable
  {
    private readonly HashSet<object> activatedObjects = new HashSet<object>();
    private readonly HashSet<object> deactivatedObjects = new HashSet<object>();

    public ActivationCache(ICachePruner cachePruner) => cachePruner.Start((IPruneable) this);

    public int ActivatedObjectCount => this.activatedObjects.Count;

    public int DeactivatedObjectCount => this.deactivatedObjects.Count;

    public void Clear()
    {
      lock (this.activatedObjects)
        this.activatedObjects.Clear();
      lock (this.deactivatedObjects)
        this.deactivatedObjects.Clear();
    }

    public void AddActivatedInstance(object instance)
    {
      lock (this.activatedObjects)
        this.activatedObjects.Add((object) new ReferenceEqualWeakReference(instance));
    }

    public void AddDeactivatedInstance(object instance)
    {
      lock (this.deactivatedObjects)
        this.deactivatedObjects.Add((object) new ReferenceEqualWeakReference(instance));
    }

    public bool IsActivated(object instance) => this.activatedObjects.Contains(instance);

    public bool IsDeactivated(object instance) => this.deactivatedObjects.Contains(instance);

    public void Prune()
    {
      lock (this.activatedObjects)
        ActivationCache.RemoveDeadObjects(this.activatedObjects);
      lock (this.deactivatedObjects)
        ActivationCache.RemoveDeadObjects(this.deactivatedObjects);
    }

    private static void RemoveDeadObjects(HashSet<object> objects)
    {
      objects.RemoveWhere((Predicate<object>) (reference => !((ReferenceEqualWeakReference) reference).IsAlive));
    }
  }
}
