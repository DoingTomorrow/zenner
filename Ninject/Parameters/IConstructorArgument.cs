// Decompiled with JetBrains decompiler
// Type: Ninject.Parameters.IConstructorArgument
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Planning.Targets;
using System;

#nullable disable
namespace Ninject.Parameters
{
  public interface IConstructorArgument : IParameter, IEquatable<IParameter>
  {
    bool AppliesToTarget(IContext context, ITarget target);
  }
}
