// Decompiled with JetBrains decompiler
// Type: Ninject.Selection.Selector
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using Ninject.Selection.Heuristics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject.Selection
{
  public class Selector : NinjectComponent, ISelector, INinjectComponent, IDisposable
  {
    private const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Public;

    protected virtual BindingFlags Flags
    {
      get
      {
        return !this.Settings.InjectNonPublic ? BindingFlags.Instance | BindingFlags.Public : BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      }
    }

    public IConstructorScorer ConstructorScorer { get; set; }

    public ICollection<IInjectionHeuristic> InjectionHeuristics { get; private set; }

    public Selector(
      IConstructorScorer constructorScorer,
      IEnumerable<IInjectionHeuristic> injectionHeuristics)
    {
      Ensure.ArgumentNotNull((object) constructorScorer, nameof (constructorScorer));
      Ensure.ArgumentNotNull((object) injectionHeuristics, nameof (injectionHeuristics));
      this.ConstructorScorer = constructorScorer;
      this.InjectionHeuristics = (ICollection<IInjectionHeuristic>) injectionHeuristics.ToList<IInjectionHeuristic>();
    }

    public virtual IEnumerable<ConstructorInfo> SelectConstructorsForInjection(Type type)
    {
      Ensure.ArgumentNotNull((object) type, nameof (type));
      ConstructorInfo[] constructors = type.GetConstructors(this.Flags);
      return constructors.Length != 0 ? (IEnumerable<ConstructorInfo>) constructors : (IEnumerable<ConstructorInfo>) null;
    }

    public virtual IEnumerable<PropertyInfo> SelectPropertiesForInjection(Type type)
    {
      Ensure.ArgumentNotNull((object) type, nameof (type));
      List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();
      propertyInfoList.AddRange(((IEnumerable<PropertyInfo>) type.GetProperties(this.Flags)).Select<PropertyInfo, PropertyInfo>((Func<PropertyInfo, PropertyInfo>) (p => p.GetPropertyFromDeclaredType(p, this.Flags))).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => this.InjectionHeuristics.Any<IInjectionHeuristic>((Func<IInjectionHeuristic, bool>) (h => h.ShouldInject((MemberInfo) p))))));
      if (this.Settings.InjectParentPrivateProperties)
      {
        for (Type baseType = type.BaseType; baseType != (Type) null; baseType = baseType.BaseType)
          propertyInfoList.AddRange(this.GetPrivateProperties(type.BaseType));
      }
      return (IEnumerable<PropertyInfo>) propertyInfoList;
    }

    private IEnumerable<PropertyInfo> GetPrivateProperties(Type type)
    {
      return ((IEnumerable<PropertyInfo>) type.GetProperties(this.Flags)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.DeclaringType == type && p.IsPrivate()));
    }

    public virtual IEnumerable<MethodInfo> SelectMethodsForInjection(Type type)
    {
      Ensure.ArgumentNotNull((object) type, nameof (type));
      return ((IEnumerable<MethodInfo>) type.GetMethods(this.Flags)).Where<MethodInfo>((Func<MethodInfo, bool>) (m => this.InjectionHeuristics.Any<IInjectionHeuristic>((Func<IInjectionHeuristic, bool>) (h => h.ShouldInject((MemberInfo) m)))));
    }
  }
}
