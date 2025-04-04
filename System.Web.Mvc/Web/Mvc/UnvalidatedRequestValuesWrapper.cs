// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.UnvalidatedRequestValuesWrapper
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Specialized;

#nullable disable
namespace System.Web.Mvc
{
  internal sealed class UnvalidatedRequestValuesWrapper : IUnvalidatedRequestValues
  {
    private readonly System.Web.Helpers.UnvalidatedRequestValues _unvalidatedValues;

    public UnvalidatedRequestValuesWrapper(System.Web.Helpers.UnvalidatedRequestValues unvalidatedValues)
    {
      this._unvalidatedValues = unvalidatedValues;
    }

    public NameValueCollection Form => this._unvalidatedValues.Form;

    public NameValueCollection QueryString => this._unvalidatedValues.QueryString;

    public string this[string key] => this._unvalidatedValues[key];
  }
}
