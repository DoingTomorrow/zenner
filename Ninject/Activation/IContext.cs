// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.IContext
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Parameters;
using Ninject.Planning;
using Ninject.Planning.Bindings;
using System;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Activation
{
  public interface IContext
  {
    IKernel Kernel { get; }

    IRequest Request { get; }

    IBinding Binding { get; }

    IPlan Plan { get; set; }

    ICollection<IParameter> Parameters { get; }

    Type[] GenericArguments { get; }

    bool HasInferredGenericArguments { get; }

    IProvider GetProvider();

    object GetScope();

    object Resolve();
  }
}
