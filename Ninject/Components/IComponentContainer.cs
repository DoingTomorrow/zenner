// Decompiled with JetBrains decompiler
// Type: Ninject.Components.IComponentContainer
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Components
{
  public interface IComponentContainer : IDisposable
  {
    IKernel Kernel { get; set; }

    void Add<TComponent, TImplementation>()
      where TComponent : INinjectComponent
      where TImplementation : TComponent, INinjectComponent;

    void RemoveAll<T>() where T : INinjectComponent;

    void RemoveAll(Type component);

    T Get<T>() where T : INinjectComponent;

    IEnumerable<T> GetAll<T>() where T : INinjectComponent;

    object Get(Type component);

    IEnumerable<object> GetAll(Type component);

    void AddTransient<TComponent, TImplementation>()
      where TComponent : INinjectComponent
      where TImplementation : TComponent, INinjectComponent;
  }
}
