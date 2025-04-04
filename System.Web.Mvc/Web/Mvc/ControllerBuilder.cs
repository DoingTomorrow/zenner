// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ControllerBuilder
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class ControllerBuilder
  {
    private static ControllerBuilder _instance = new ControllerBuilder();
    private Func<IControllerFactory> _factoryThunk = (Func<IControllerFactory>) (() => (IControllerFactory) null);
    private HashSet<string> _namespaces = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private IResolver<IControllerFactory> _serviceResolver;

    public ControllerBuilder()
      : this((IResolver<IControllerFactory>) null)
    {
    }

    internal ControllerBuilder(IResolver<IControllerFactory> serviceResolver)
    {
      IResolver<IControllerFactory> resolver = serviceResolver;
      if (resolver == null)
        resolver = (IResolver<IControllerFactory>) new SingleServiceResolver<IControllerFactory>((Func<IControllerFactory>) (() => this._factoryThunk()), (IControllerFactory) new DefaultControllerFactory()
        {
          ControllerBuilder = this
        }, "ControllerBuilder.GetControllerFactory");
      this._serviceResolver = resolver;
    }

    public static ControllerBuilder Current => ControllerBuilder._instance;

    public HashSet<string> DefaultNamespaces => this._namespaces;

    public IControllerFactory GetControllerFactory() => this._serviceResolver.Current;

    public void SetControllerFactory(IControllerFactory controllerFactory)
    {
      this._factoryThunk = controllerFactory != null ? (Func<IControllerFactory>) (() => controllerFactory) : throw new ArgumentNullException(nameof (controllerFactory));
    }

    public void SetControllerFactory(Type controllerFactoryType)
    {
      if (controllerFactoryType == (Type) null)
        throw new ArgumentNullException(nameof (controllerFactoryType));
      if (!typeof (IControllerFactory).IsAssignableFrom(controllerFactoryType))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ControllerBuilder_MissingIControllerFactory, new object[1]
        {
          (object) controllerFactoryType
        }), nameof (controllerFactoryType));
      this._factoryThunk = (Func<IControllerFactory>) (() =>
      {
        try
        {
          return (IControllerFactory) Activator.CreateInstance(controllerFactoryType);
        }
        catch (Exception ex)
        {
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ControllerBuilder_ErrorCreatingControllerFactory, new object[1]
          {
            (object) controllerFactoryType
          }), ex);
        }
      });
    }
  }
}
