// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.FunctionNameProvider
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class FunctionNameProvider : IFunctionNameProvider
  {
    private FunctionNameProvider()
    {
    }

    public static FunctionNameProvider Empty => new FunctionNameProvider();

    public virtual bool IsFunctionName(string name) => false;
  }
}
