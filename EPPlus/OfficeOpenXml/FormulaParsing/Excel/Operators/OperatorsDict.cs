// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Operators.OperatorsDict
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Operators
{
  public class OperatorsDict : Dictionary<string, IOperator>
  {
    private static IDictionary<string, IOperator> _instance;

    public OperatorsDict()
    {
      this.Add("+", Operator.Plus);
      this.Add("-", Operator.Minus);
      this.Add("*", Operator.Multiply);
      this.Add("/", Operator.Divide);
      this.Add("^", Operator.Exp);
      this.Add("=", Operator.Eq);
      this.Add(">", Operator.GreaterThan);
      this.Add(">=", Operator.GreaterThanOrEqual);
      this.Add("<", Operator.LessThan);
      this.Add("<=", Operator.LessThanOrEqual);
      this.Add("<>", Operator.NotEqualsTo);
      this.Add("&", Operator.Concat);
    }

    public static IDictionary<string, IOperator> Instance
    {
      get
      {
        if (OperatorsDict._instance == null)
          OperatorsDict._instance = (IDictionary<string, IOperator>) new OperatorsDict();
        return OperatorsDict._instance;
      }
    }
  }
}
