// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.FormulaDependencies
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class FormulaDependencies
  {
    private readonly FormulaDependencyFactory _formulaDependencyFactory;
    private readonly Dictionary<string, FormulaDependency> _dependencies = new Dictionary<string, FormulaDependency>();

    public FormulaDependencies()
      : this(new FormulaDependencyFactory())
    {
    }

    public FormulaDependencies(FormulaDependencyFactory formulaDependencyFactory)
    {
      this._formulaDependencyFactory = formulaDependencyFactory;
    }

    public IEnumerable<KeyValuePair<string, FormulaDependency>> Dependencies
    {
      get => (IEnumerable<KeyValuePair<string, FormulaDependency>>) this._dependencies;
    }

    public void AddFormulaScope(ParsingScope parsingScope)
    {
    }

    public void Clear() => this._dependencies.Clear();
  }
}
