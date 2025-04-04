// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.SingleServiceResolver`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  internal class SingleServiceResolver<TService> : IResolver<TService> where TService : class
  {
    private Lazy<TService> _currentValueFromResolver;
    private Func<TService> _currentValueThunk;
    private TService _defaultValue;
    private Func<IDependencyResolver> _resolverThunk;
    private string _callerMethodName;

    public SingleServiceResolver(
      Func<TService> currentValueThunk,
      TService defaultValue,
      string callerMethodName)
    {
      if (currentValueThunk == null)
        throw new ArgumentNullException(nameof (currentValueThunk));
      if ((object) defaultValue == null)
        throw new ArgumentNullException(nameof (defaultValue));
      this._resolverThunk = (Func<IDependencyResolver>) (() => DependencyResolver.Current);
      this._currentValueFromResolver = new Lazy<TService>(new Func<TService>(this.GetValueFromResolver));
      this._currentValueThunk = currentValueThunk;
      this._defaultValue = defaultValue;
      this._callerMethodName = callerMethodName;
    }

    internal SingleServiceResolver(
      Func<TService> staticAccessor,
      TService defaultValue,
      IDependencyResolver resolver,
      string callerMethodName)
      : this(staticAccessor, defaultValue, callerMethodName)
    {
      if (resolver == null)
        return;
      this._resolverThunk = (Func<IDependencyResolver>) (() => resolver);
    }

    public TService Current
    {
      get
      {
        return this._currentValueFromResolver.Value ?? this._currentValueThunk() ?? this._defaultValue;
      }
    }

    private TService GetValueFromResolver()
    {
      TService service = this._resolverThunk().GetService<TService>();
      return (object) service == null || (object) this._currentValueThunk() == null ? service : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.SingleServiceResolver_CannotRegisterTwoInstances, new object[2]
      {
        (object) typeof (TService).Name.ToString(),
        (object) this._callerMethodName
      }));
    }
  }
}
