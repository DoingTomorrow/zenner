// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.InstanceReference
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;

#nullable disable
namespace Ninject.Activation
{
  public class InstanceReference
  {
    public object Instance { get; set; }

    public bool Is<T>() => this.Instance is T;

    public T As<T>() => (T) this.Instance;

    public void IfInstanceIs<T>(Action<T> action)
    {
      if (!(this.Instance is T))
        return;
      action((T) this.Instance);
    }
  }
}
