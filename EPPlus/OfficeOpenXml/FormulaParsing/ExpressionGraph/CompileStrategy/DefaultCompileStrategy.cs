// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.CompileStrategy.DefaultCompileStrategy
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph.CompileStrategy
{
  public class DefaultCompileStrategy(Expression expression) : OfficeOpenXml.FormulaParsing.ExpressionGraph.CompileStrategy.CompileStrategy(expression)
  {
    public override Expression Compile() => this._expression.MergeWithNext();
  }
}
