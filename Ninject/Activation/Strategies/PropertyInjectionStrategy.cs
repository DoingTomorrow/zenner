// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Strategies.PropertyInjectionStrategy
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Injection;
using Ninject.Parameters;
using Ninject.Planning.Directives;
using Ninject.Planning.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject.Activation.Strategies
{
  public class PropertyInjectionStrategy : ActivationStrategy
  {
    private const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Public;

    private BindingFlags Flags
    {
      get
      {
        return !this.Settings.InjectNonPublic ? BindingFlags.Instance | BindingFlags.Public : BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      }
    }

    public IInjectorFactory InjectorFactory { get; set; }

    public PropertyInjectionStrategy(IInjectorFactory injectorFactory)
    {
      this.InjectorFactory = injectorFactory;
    }

    public override void Activate(IContext context, InstanceReference reference)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      Ensure.ArgumentNotNull((object) reference, nameof (reference));
      IEnumerable<IParameter> parameters = context.Parameters.Where<IParameter>((Func<IParameter, bool>) (parameter => parameter is PropertyValue));
      IEnumerable<string> source = parameters.Select<IParameter, string>((Func<IParameter, string>) (parameter => parameter.Name));
      foreach (PropertyInjectionDirective injectionDirective in context.Plan.GetAll<PropertyInjectionDirective>())
      {
        PropertyInjectionDirective propertyInjectionDirective = injectionDirective;
        if (!source.Any<string>((Func<string, bool>) (name => object.Equals((object) name, (object) propertyInjectionDirective))))
        {
          object obj = this.GetValue(context, injectionDirective.Target);
          injectionDirective.Injector(reference.Instance, obj);
        }
      }
      this.AssignProperyOverrides(context, reference, parameters);
    }

    private void AssignProperyOverrides(
      IContext context,
      InstanceReference reference,
      IEnumerable<IParameter> propertyValues)
    {
      PropertyInfo[] properties = reference.Instance.GetType().GetProperties(this.Flags);
      foreach (IParameter propertyValue in propertyValues)
      {
        string propertyName = propertyValue.Name;
        PropertyInfo propertyInfo = ((IEnumerable<PropertyInfo>) properties).Where<PropertyInfo>((Func<PropertyInfo, bool>) (property => string.Equals(property.Name, propertyName, StringComparison.Ordinal))).FirstOrDefault<PropertyInfo>();
        PropertyInjectionDirective injectionDirective = !(propertyInfo == (PropertyInfo) null) ? new PropertyInjectionDirective(propertyInfo, this.InjectorFactory.Create(propertyInfo)) : throw new ActivationException(ExceptionFormatter.CouldNotResolvePropertyForValueInjection(context.Request, propertyName));
        object obj = this.GetValue(context, injectionDirective.Target);
        injectionDirective.Injector(reference.Instance, obj);
      }
    }

    public object GetValue(IContext context, ITarget target)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      Ensure.ArgumentNotNull((object) target, nameof (target));
      PropertyValue propertyValue = context.Parameters.OfType<PropertyValue>().Where<PropertyValue>((Func<PropertyValue, bool>) (p => p.Name == target.Name)).SingleOrDefault<PropertyValue>();
      return propertyValue == null ? target.ResolveWithin(context) : propertyValue.GetValue(context, target);
    }
  }
}
