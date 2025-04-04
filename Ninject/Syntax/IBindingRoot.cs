// Decompiled with JetBrains decompiler
// Type: Ninject.Syntax.IBindingRoot
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Planning.Bindings;
using System;

#nullable disable
namespace Ninject.Syntax
{
  public interface IBindingRoot : IFluentSyntax
  {
    IBindingToSyntax<T> Bind<T>();

    IBindingToSyntax<T1, T2> Bind<T1, T2>();

    IBindingToSyntax<T1, T2, T3> Bind<T1, T2, T3>();

    IBindingToSyntax<T1, T2, T3, T4> Bind<T1, T2, T3, T4>();

    IBindingToSyntax<object> Bind(params Type[] services);

    void Unbind<T>();

    void Unbind(Type service);

    IBindingToSyntax<T1> Rebind<T1>();

    IBindingToSyntax<T1, T2> Rebind<T1, T2>();

    IBindingToSyntax<T1, T2, T3> Rebind<T1, T2, T3>();

    IBindingToSyntax<T1, T2, T3, T4> Rebind<T1, T2, T3, T4>();

    IBindingToSyntax<object> Rebind(params Type[] services);

    void AddBinding(IBinding binding);

    void RemoveBinding(IBinding binding);
  }
}
