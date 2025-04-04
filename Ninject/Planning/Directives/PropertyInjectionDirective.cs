// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Directives.PropertyInjectionDirective
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Injection;
using Ninject.Planning.Targets;
using System.Reflection;

#nullable disable
namespace Ninject.Planning.Directives
{
  public class PropertyInjectionDirective : IDirective
  {
    public PropertyInjector Injector { get; private set; }

    public ITarget Target { get; private set; }

    public PropertyInjectionDirective(PropertyInfo member, PropertyInjector injector)
    {
      this.Injector = injector;
      this.Target = this.CreateTarget(member);
    }

    protected virtual ITarget CreateTarget(PropertyInfo propertyInfo)
    {
      return (ITarget) new PropertyTarget(propertyInfo);
    }
  }
}
