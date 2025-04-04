// Decompiled with JetBrains decompiler
// Type: Ninject.Injection.ReflectionInjectorFactory
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using System;
using System.Reflection;

#nullable disable
namespace Ninject.Injection
{
  public class ReflectionInjectorFactory : 
    NinjectComponent,
    IInjectorFactory,
    INinjectComponent,
    IDisposable
  {
    public ConstructorInjector Create(ConstructorInfo constructor)
    {
      return (ConstructorInjector) (args => constructor.Invoke(args));
    }

    public PropertyInjector Create(PropertyInfo property)
    {
      return (PropertyInjector) ((target, value) => property.SetValue(target, value, (object[]) null));
    }

    public MethodInjector Create(MethodInfo method)
    {
      return (MethodInjector) ((target, args) => method.Invoke(target, args));
    }
  }
}
