// Decompiled with JetBrains decompiler
// Type: Ninject.Selection.Heuristics.StandardInjectionHeuristic
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using System;
using System.Reflection;

#nullable disable
namespace Ninject.Selection.Heuristics
{
  public class StandardInjectionHeuristic : 
    NinjectComponent,
    IInjectionHeuristic,
    INinjectComponent,
    IDisposable
  {
    public virtual bool ShouldInject(MemberInfo member)
    {
      Ensure.ArgumentNotNull((object) member, nameof (member));
      PropertyInfo propertyInfo = member as PropertyInfo;
      if (!(propertyInfo != (PropertyInfo) null))
        return ExtensionsForMemberInfo.HasAttribute(member, this.Settings.InjectAttribute);
      bool injectNonPublic = this.Settings.InjectNonPublic;
      MethodInfo setMethod = propertyInfo.GetSetMethod(injectNonPublic);
      return ExtensionsForMemberInfo.HasAttribute(member, this.Settings.InjectAttribute) && setMethod != (MethodInfo) null;
    }
  }
}
