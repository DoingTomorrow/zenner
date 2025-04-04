// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.TemplateInfo
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.Mvc
{
  public class TemplateInfo
  {
    private string _htmlFieldPrefix;
    private object _formattedModelValue;
    private HashSet<object> _visitedObjects;

    public object FormattedModelValue
    {
      get => this._formattedModelValue ?? (object) string.Empty;
      set => this._formattedModelValue = value;
    }

    public string HtmlFieldPrefix
    {
      get => this._htmlFieldPrefix ?? string.Empty;
      set => this._htmlFieldPrefix = value;
    }

    public int TemplateDepth => this.VisitedObjects.Count;

    internal HashSet<object> VisitedObjects
    {
      get
      {
        if (this._visitedObjects == null)
          this._visitedObjects = new HashSet<object>();
        return this._visitedObjects;
      }
      set => this._visitedObjects = value;
    }

    public string GetFullHtmlFieldId(string partialFieldName)
    {
      return HtmlHelper.GenerateIdFromName(this.GetFullHtmlFieldName(partialFieldName));
    }

    public string GetFullHtmlFieldName(string partialFieldName)
    {
      return (this.HtmlFieldPrefix + "." + (partialFieldName ?? string.Empty)).Trim('.');
    }

    public bool Visited(ModelMetadata metadata)
    {
      return this.VisitedObjects.Contains(metadata.Model ?? (object) metadata.ModelType);
    }
  }
}
