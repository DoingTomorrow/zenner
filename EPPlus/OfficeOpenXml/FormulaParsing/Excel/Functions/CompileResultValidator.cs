// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.CompileResultValidator
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public abstract class CompileResultValidator
  {
    private static CompileResultValidator _empty;

    public abstract void Validate(object obj);

    public static CompileResultValidator Empty
    {
      get
      {
        return CompileResultValidator._empty ?? (CompileResultValidator._empty = (CompileResultValidator) new EmptyCompileResultValidator());
      }
    }
  }
}
