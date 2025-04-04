// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelClientValidationRemoteRule
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace System.Web.Mvc
{
  [TypeForwardedFrom("System.Web.Mvc, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
  public class ModelClientValidationRemoteRule : ModelClientValidationRule
  {
    public ModelClientValidationRemoteRule(
      string errorMessage,
      string url,
      string httpMethod,
      string additionalFields)
    {
      this.ErrorMessage = errorMessage;
      this.ValidationType = "remote";
      this.ValidationParameters[nameof (url)] = (object) url;
      if (!string.IsNullOrEmpty(httpMethod))
        this.ValidationParameters["type"] = (object) httpMethod;
      this.ValidationParameters["additionalfields"] = (object) additionalFields;
    }
  }
}
