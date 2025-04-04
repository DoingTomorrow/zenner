// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.EpplusNameValueProvider
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  public class EpplusNameValueProvider : INameValueProvider
  {
    private ExcelDataProvider _excelDataProvider;
    private ExcelNamedRangeCollection _values;

    public EpplusNameValueProvider(ExcelDataProvider excelDataProvider)
    {
      this._excelDataProvider = excelDataProvider;
      this._values = this._excelDataProvider.GetWorkbookNameValues();
    }

    public virtual bool IsNamedValue(string key)
    {
      return this._values != null && this._values.ContainsKey(key);
    }

    public virtual object GetNamedValue(string key) => (object) this._values[key];

    public virtual void Reload() => this._values = this._excelDataProvider.GetWorkbookNameValues();
  }
}
