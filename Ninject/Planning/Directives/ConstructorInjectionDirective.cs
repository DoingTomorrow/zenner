// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Directives.ConstructorInjectionDirective
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Injection;
using System.Reflection;

#nullable disable
namespace Ninject.Planning.Directives
{
  public class ConstructorInjectionDirective : 
    MethodInjectionDirectiveBase<ConstructorInfo, ConstructorInjector>
  {
    public ConstructorInfo Constructor { get; set; }

    public ConstructorInjectionDirective(ConstructorInfo constructor, ConstructorInjector injector)
      : base(constructor, injector)
    {
      this.Constructor = constructor;
    }
  }
}
