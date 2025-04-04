// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.StandardScopeCallbacks
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using System;

#nullable disable
namespace Ninject.Infrastructure
{
  public class StandardScopeCallbacks
  {
    public static readonly Func<IContext, object> Transient = (Func<IContext, object>) (ctx => (object) null);
    public static readonly Func<IContext, object> Singleton = (Func<IContext, object>) (ctx => (object) ctx.Kernel);
    public static readonly Func<IContext, object> Thread = (Func<IContext, object>) (ctx => (object) System.Threading.Thread.CurrentThread);
  }
}
