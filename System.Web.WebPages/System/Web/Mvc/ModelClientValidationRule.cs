// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelClientValidationRule
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Web.Mvc
{
  [TypeForwardedFrom("System.Web.Mvc, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
  public class ModelClientValidationRule
  {
    private readonly Dictionary<string, object> _validationParameters = new Dictionary<string, object>();
    private string _validationType;

    public string ErrorMessage { get; set; }

    public IDictionary<string, object> ValidationParameters
    {
      get => (IDictionary<string, object>) this._validationParameters;
    }

    public string ValidationType
    {
      get => this._validationType ?? string.Empty;
      set => this._validationType = value;
    }
  }
}
