// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.ReferenceEqualWeakReference
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;

#nullable disable
namespace Ninject.Infrastructure
{
  public class ReferenceEqualWeakReference
  {
    private int cashedHashCode;
    private WeakReference weakReference;

    public ReferenceEqualWeakReference(object target)
    {
      this.weakReference = new WeakReference(target);
    }

    public ReferenceEqualWeakReference(object target, bool trackResurrection)
    {
      this.weakReference = new WeakReference(target, trackResurrection);
    }

    public bool IsAlive => this.weakReference.IsAlive;

    public object Target
    {
      get => this.weakReference.Target;
      set => this.weakReference.Target = value;
    }

    public override bool Equals(object obj)
    {
      if (!this.IsAlive)
        return base.Equals(obj);
      switch (obj)
      {
        case ReferenceEqualWeakReference equalWeakReference:
          obj = equalWeakReference.Target;
          if (obj == null)
            return false;
          break;
        case WeakReference weakReference:
          obj = weakReference.Target;
          if (obj == null)
            return false;
          break;
      }
      return object.ReferenceEquals(this.Target, obj);
    }

    public override int GetHashCode()
    {
      object target = this.Target;
      if (target != null)
        this.cashedHashCode = target.GetHashCode();
      return this.cashedHashCode;
    }
  }
}
