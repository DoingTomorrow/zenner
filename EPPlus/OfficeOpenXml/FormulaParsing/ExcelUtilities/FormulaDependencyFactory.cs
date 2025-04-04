// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.FormulaDependencyFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class FormulaDependencyFactory
  {
    public virtual FormulaDependency Create(ParsingScope scope) => new FormulaDependency(scope);
  }
}
