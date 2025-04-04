// Decompiled with JetBrains decompiler
// Type: Ninject.Selection.Heuristics.StandardConstructorScorer
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Planning.Directives;
using Ninject.Planning.Targets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Selection.Heuristics
{
  public class StandardConstructorScorer : 
    NinjectComponent,
    IConstructorScorer,
    INinjectComponent,
    IDisposable
  {
    public virtual int Score(IContext context, ConstructorInjectionDirective directive)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      Ensure.ArgumentNotNull((object) directive, "constructor");
      if (ExtensionsForMemberInfo.HasAttribute(directive.Constructor, this.Settings.InjectAttribute))
        return int.MaxValue;
      int num = 1;
      foreach (ITarget target in directive.Targets)
      {
        if (this.ParameterExists(context, target))
          ++num;
        else if (this.BindingExists(context, target))
        {
          ++num;
        }
        else
        {
          ++num;
          if (num > 0)
            num += int.MinValue;
        }
      }
      return num;
    }

    protected virtual bool BindingExists(IContext context, ITarget target)
    {
      Type targetType = this.GetTargetType(target);
      return context.Kernel.GetBindings(targetType).Any<IBinding>((Func<IBinding, bool>) (b => !b.IsImplicit)) || target.HasDefaultValue;
    }

    private Type GetTargetType(ITarget target)
    {
      Type targetType = target.Type;
      if (targetType.IsArray)
        targetType = targetType.GetElementType();
      if (targetType.IsGenericType && ((IEnumerable<Type>) targetType.GetInterfaces()).Any<Type>((Func<Type, bool>) (type => type == typeof (IEnumerable))))
        targetType = targetType.GetGenericArguments()[0];
      return targetType;
    }

    protected virtual bool ParameterExists(IContext context, ITarget target)
    {
      return context.Parameters.OfType<IConstructorArgument>().Any<IConstructorArgument>((Func<IConstructorArgument, bool>) (parameter => parameter.AppliesToTarget(context, target)));
    }
  }
}
