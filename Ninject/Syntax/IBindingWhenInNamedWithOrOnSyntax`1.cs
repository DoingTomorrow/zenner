// Decompiled with JetBrains decompiler
// Type: Ninject.Syntax.IBindingWhenInNamedWithOrOnSyntax`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;

#nullable disable
namespace Ninject.Syntax
{
  public interface IBindingWhenInNamedWithOrOnSyntax<T> : 
    IBindingWhenSyntax<T>,
    IBindingInSyntax<T>,
    IBindingNamedSyntax<T>,
    IBindingWithSyntax<T>,
    IBindingOnSyntax<T>,
    IBindingSyntax,
    IHaveBindingConfiguration,
    IHaveKernel,
    IFluentSyntax
  {
  }
}
