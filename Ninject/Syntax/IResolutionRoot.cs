// Decompiled with JetBrains decompiler
// Type: Ninject.Syntax.IResolutionRoot
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using System;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Syntax
{
  public interface IResolutionRoot : IFluentSyntax
  {
    bool CanResolve(IRequest request);

    bool CanResolve(IRequest request, bool ignoreImplicitBindings);

    IEnumerable<object> Resolve(IRequest request);

    IRequest CreateRequest(
      Type service,
      Func<IBindingMetadata, bool> constraint,
      IEnumerable<IParameter> parameters,
      bool isOptional,
      bool isUnique);
  }
}
