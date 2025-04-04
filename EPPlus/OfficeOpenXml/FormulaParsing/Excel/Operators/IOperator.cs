// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Operators.IOperator
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Operators
{
  public interface IOperator
  {
    OfficeOpenXml.FormulaParsing.Excel.Operators.Operators Operator { get; }

    CompileResult Apply(CompileResult left, CompileResult right);

    int Precedence { get; }
  }
}
