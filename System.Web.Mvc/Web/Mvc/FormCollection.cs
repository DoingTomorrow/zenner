// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FormCollection
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Helpers;

#nullable disable
namespace System.Web.Mvc
{
  [FormCollection.FormCollectionBinder]
  public sealed class FormCollection : NameValueCollection, IValueProvider
  {
    public FormCollection()
    {
    }

    public FormCollection(NameValueCollection collection)
    {
      if (collection == null)
        throw new ArgumentNullException(nameof (collection));
      this.Add(collection);
    }

    internal FormCollection(
      ControllerBase controller,
      Func<NameValueCollection> validatedValuesThunk,
      Func<NameValueCollection> unvalidatedValuesThunk)
    {
      this.Add(controller == null || controller.ValidateRequest ? validatedValuesThunk() : unvalidatedValuesThunk());
    }

    public ValueProviderResult GetValue(string name)
    {
      string[] rawValue = name != null ? this.GetValues(name) : throw new ArgumentNullException(nameof (name));
      if (rawValue == null)
        return (ValueProviderResult) null;
      string attemptedValue = this[name];
      return new ValueProviderResult((object) rawValue, attemptedValue, CultureInfo.CurrentCulture);
    }

    public IValueProvider ToValueProvider() => (IValueProvider) this;

    bool IValueProvider.ContainsPrefix(string prefix)
    {
      return ValueProviderUtil.CollectionContainsPrefix((IEnumerable<string>) this.AllKeys, prefix);
    }

    ValueProviderResult IValueProvider.GetValue(string key) => this.GetValue(key);

    private sealed class FormCollectionBinderAttribute : CustomModelBinderAttribute
    {
      private static readonly FormCollection.FormCollectionBinderAttribute.FormCollectionModelBinder _binder = new FormCollection.FormCollectionBinderAttribute.FormCollectionModelBinder();

      public override IModelBinder GetBinder()
      {
        return (IModelBinder) FormCollection.FormCollectionBinderAttribute._binder;
      }

      private sealed class FormCollectionModelBinder : IModelBinder
      {
        public object BindModel(
          ControllerContext controllerContext,
          ModelBindingContext bindingContext)
        {
          if (controllerContext == null)
            throw new ArgumentNullException(nameof (controllerContext));
          return (object) new FormCollection(controllerContext.Controller, (Func<NameValueCollection>) (() => controllerContext.HttpContext.Request.Form), (Func<NameValueCollection>) (() => controllerContext.HttpContext.Request.Unvalidated().Form));
        }
      }
    }
  }
}
