// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FormContext
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Web.Script.Serialization;

#nullable disable
namespace System.Web.Mvc
{
  public class FormContext
  {
    private readonly Dictionary<string, FieldValidationMetadata> _fieldValidators = new Dictionary<string, FieldValidationMetadata>();
    private readonly Dictionary<string, bool> _renderedFields = new Dictionary<string, bool>();

    public IDictionary<string, FieldValidationMetadata> FieldValidators
    {
      get => (IDictionary<string, FieldValidationMetadata>) this._fieldValidators;
    }

    public string FormId { get; set; }

    public bool ReplaceValidationSummary { get; set; }

    public string ValidationSummaryId { get; set; }

    public string GetJsonValidationMetadata()
    {
      JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
      SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>()
      {
        {
          "Fields",
          (object) this.FieldValidators.Values
        },
        {
          "FormId",
          (object) this.FormId
        }
      };
      if (!string.IsNullOrEmpty(this.ValidationSummaryId))
        sortedDictionary["ValidationSummaryId"] = (object) this.ValidationSummaryId;
      sortedDictionary["ReplaceValidationSummary"] = (object) this.ReplaceValidationSummary;
      return scriptSerializer.Serialize((object) sortedDictionary);
    }

    public FieldValidationMetadata GetValidationMetadataForField(string fieldName)
    {
      return this.GetValidationMetadataForField(fieldName, false);
    }

    public FieldValidationMetadata GetValidationMetadataForField(
      string fieldName,
      bool createIfNotFound)
    {
      if (string.IsNullOrEmpty(fieldName))
        throw Error.ParameterCannotBeNullOrEmpty(nameof (fieldName));
      FieldValidationMetadata metadataForField;
      if (!this.FieldValidators.TryGetValue(fieldName, out metadataForField) && createIfNotFound)
      {
        metadataForField = new FieldValidationMetadata()
        {
          FieldName = fieldName
        };
        this.FieldValidators[fieldName] = metadataForField;
      }
      return metadataForField;
    }

    public bool RenderedField(string fieldName)
    {
      bool flag;
      this._renderedFields.TryGetValue(fieldName, out flag);
      return flag;
    }

    public void RenderedField(string fieldName, bool value)
    {
      this._renderedFields[fieldName] = value;
    }
  }
}
