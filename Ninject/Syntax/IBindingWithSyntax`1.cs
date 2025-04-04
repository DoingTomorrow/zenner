// Decompiled with JetBrains decompiler
// Type: Ninject.Syntax.IBindingWithSyntax`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Parameters;
using Ninject.Planning.Targets;
using System;

#nullable disable
namespace Ninject.Syntax
{
  public interface IBindingWithSyntax<T> : 
    IBindingSyntax,
    IHaveBindingConfiguration,
    IHaveKernel,
    IFluentSyntax
  {
    IBindingWithOrOnSyntax<T> WithConstructorArgument(string name, object value);

    IBindingWithOrOnSyntax<T> WithConstructorArgument(string name, Func<IContext, object> callback);

    IBindingWithOrOnSyntax<T> WithConstructorArgument(
      string name,
      Func<IContext, ITarget, object> callback);

    IBindingWithOrOnSyntax<T> WithPropertyValue(string name, object value);

    IBindingWithOrOnSyntax<T> WithPropertyValue(string name, Func<IContext, object> callback);

    IBindingWithOrOnSyntax<T> WithPropertyValue(
      string name,
      Func<IContext, ITarget, object> callback);

    IBindingWithOrOnSyntax<T> WithParameter(IParameter parameter);

    IBindingWithOrOnSyntax<T> WithMetadata(string key, object value);
  }
}
