// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.RemoteAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc.Properties;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Property)]
  public class RemoteAttribute : ValidationAttribute, IClientValidatable
  {
    private string _additionalFields;
    private string[] _additonalFieldsSplit = new string[0];

    protected RemoteAttribute()
      : base(MvcResources.RemoteAttribute_RemoteValidationFailed)
    {
      this.RouteData = new RouteValueDictionary();
    }

    public RemoteAttribute(string routeName)
      : this()
    {
      this.RouteName = !string.IsNullOrWhiteSpace(routeName) ? routeName : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (routeName));
    }

    public RemoteAttribute(string action, string controller)
      : this(action, controller, (string) null)
    {
    }

    public RemoteAttribute(string action, string controller, string areaName)
      : this()
    {
      if (string.IsNullOrWhiteSpace(action))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (action));
      this.RouteData[nameof (controller)] = !string.IsNullOrWhiteSpace(controller) ? (object) controller : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (controller));
      this.RouteData[nameof (action)] = (object) action;
      if (string.IsNullOrWhiteSpace(areaName))
        return;
      this.RouteData["area"] = (object) areaName;
    }

    public string HttpMethod { get; set; }

    public string AdditionalFields
    {
      get => this._additionalFields ?? string.Empty;
      set
      {
        this._additionalFields = value;
        this._additonalFieldsSplit = AuthorizeAttribute.SplitString(value);
      }
    }

    protected RouteValueDictionary RouteData { get; private set; }

    protected string RouteName { get; set; }

    protected virtual RouteCollection Routes => RouteTable.Routes;

    public string FormatAdditionalFieldsForClientValidation(string property)
    {
      string str = !string.IsNullOrEmpty(property) ? RemoteAttribute.FormatPropertyForClientValidation(property) : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (property));
      foreach (string property1 in this._additonalFieldsSplit)
        str = str + "," + RemoteAttribute.FormatPropertyForClientValidation(property1);
      return str;
    }

    public static string FormatPropertyForClientValidation(string property)
    {
      if (string.IsNullOrEmpty(property))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (property));
      return "*." + property;
    }

    protected virtual string GetUrl(ControllerContext controllerContext)
    {
      return (this.Routes.GetVirtualPathForArea(controllerContext.RequestContext, this.RouteName, this.RouteData) ?? throw new InvalidOperationException(MvcResources.RemoteAttribute_NoUrlFound)).VirtualPath;
    }

    public override string FormatErrorMessage(string name)
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, this.ErrorMessageString, new object[1]
      {
        (object) name
      });
    }

    public override bool IsValid(object value) => true;

    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
      ModelMetadata metadata,
      ControllerContext context)
    {
      yield return (ModelClientValidationRule) new ModelClientValidationRemoteRule(this.FormatErrorMessage(metadata.GetDisplayName()), this.GetUrl(context), this.HttpMethod, this.FormatAdditionalFieldsForClientValidation(metadata.PropertyName));
    }
  }
}
