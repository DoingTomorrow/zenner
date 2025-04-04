// Decompiled with JetBrains decompiler
// Type: Ninject.OptionalAttribute
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;

#nullable disable
namespace Ninject
{
  [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
  public class OptionalAttribute : Attribute
  {
  }
}
