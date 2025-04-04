// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Provider`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using System;

#nullable disable
namespace Ninject.Activation
{
  public abstract class Provider<T> : IProvider<T>, IProvider
  {
    public virtual Type Type => typeof (T);

    public object Create(IContext context)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      return (object) this.CreateInstance(context);
    }

    protected abstract T CreateInstance(IContext context);
  }
}
