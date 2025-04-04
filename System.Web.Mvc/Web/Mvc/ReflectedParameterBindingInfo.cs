// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ReflectedParameterBindingInfo
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  internal class ReflectedParameterBindingInfo : ParameterBindingInfo
  {
    private readonly ParameterInfo _parameterInfo;
    private ICollection<string> _exclude = (ICollection<string>) new string[0];
    private ICollection<string> _include = (ICollection<string>) new string[0];
    private string _prefix;

    public ReflectedParameterBindingInfo(ParameterInfo parameterInfo)
    {
      this._parameterInfo = parameterInfo;
      this.ReadSettingsFromBindAttribute();
    }

    public override IModelBinder Binder
    {
      get
      {
        return ModelBinders.GetBinderFromAttributes((ICustomAttributeProvider) this._parameterInfo, (Func<string>) (() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ReflectedParameterBindingInfo_MultipleConverterAttributes, new object[2]
        {
          (object) this._parameterInfo.Name,
          (object) this._parameterInfo.Member
        })));
      }
    }

    public override ICollection<string> Exclude => this._exclude;

    public override ICollection<string> Include => this._include;

    public override string Prefix => this._prefix;

    private void ReadSettingsFromBindAttribute()
    {
      BindAttribute customAttribute = (BindAttribute) Attribute.GetCustomAttribute(this._parameterInfo, typeof (BindAttribute));
      if (customAttribute == null)
        return;
      this._exclude = (ICollection<string>) new ReadOnlyCollection<string>((IList<string>) AuthorizeAttribute.SplitString(customAttribute.Exclude));
      this._include = (ICollection<string>) new ReadOnlyCollection<string>((IList<string>) AuthorizeAttribute.SplitString(customAttribute.Include));
      this._prefix = customAttribute.Prefix;
    }
  }
}
