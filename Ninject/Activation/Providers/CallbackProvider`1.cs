// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Providers.CallbackProvider`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using System;

#nullable disable
namespace Ninject.Activation.Providers
{
  public class CallbackProvider<T> : Provider<T>
  {
    public Func<IContext, T> Method { get; private set; }

    public CallbackProvider(Func<IContext, T> method)
    {
      Ensure.ArgumentNotNull((object) method, nameof (method));
      this.Method = method;
    }

    protected override T CreateInstance(IContext context) => this.Method(context);
  }
}
