// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FieldValidationMetadata
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace System.Web.Mvc
{
  public class FieldValidationMetadata
  {
    private readonly Collection<ModelClientValidationRule> _validationRules = new Collection<ModelClientValidationRule>();
    private string _fieldName;

    public string FieldName
    {
      get => this._fieldName ?? string.Empty;
      set => this._fieldName = value;
    }

    public bool ReplaceValidationMessageContents { get; set; }

    public string ValidationMessageId { get; set; }

    public ICollection<ModelClientValidationRule> ValidationRules
    {
      get => (ICollection<ModelClientValidationRule>) this._validationRules;
    }
  }
}
