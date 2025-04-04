// Decompiled with JetBrains decompiler
// Type: Ninject.Syntax.IBindingWhenSyntax`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using System;

#nullable disable
namespace Ninject.Syntax
{
  public interface IBindingWhenSyntax<T> : 
    IBindingSyntax,
    IHaveBindingConfiguration,
    IHaveKernel,
    IFluentSyntax
  {
    IBindingInNamedWithOrOnSyntax<T> When(Func<IRequest, bool> condition);

    IBindingInNamedWithOrOnSyntax<T> WhenInjectedInto<TParent>();

    IBindingInNamedWithOrOnSyntax<T> WhenInjectedInto(Type parent);

    IBindingInNamedWithOrOnSyntax<T> WhenInjectedExactlyInto<TParent>();

    IBindingInNamedWithOrOnSyntax<T> WhenInjectedExactlyInto(Type parent);

    IBindingInNamedWithOrOnSyntax<T> WhenClassHas<TAttribute>() where TAttribute : Attribute;

    IBindingInNamedWithOrOnSyntax<T> WhenMemberHas<TAttribute>() where TAttribute : Attribute;

    IBindingInNamedWithOrOnSyntax<T> WhenTargetHas<TAttribute>() where TAttribute : Attribute;

    IBindingInNamedWithOrOnSyntax<T> WhenClassHas(Type attributeType);

    IBindingInNamedWithOrOnSyntax<T> WhenMemberHas(Type attributeType);

    IBindingInNamedWithOrOnSyntax<T> WhenTargetHas(Type attributeType);

    IBindingInNamedWithOrOnSyntax<T> WhenParentNamed(string name);

    IBindingInNamedWithOrOnSyntax<T> WhenAnyAnchestorNamed(string name);
  }
}
