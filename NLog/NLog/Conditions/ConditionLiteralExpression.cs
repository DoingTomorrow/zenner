// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionLiteralExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Globalization;

#nullable disable
namespace NLog.Conditions
{
  internal sealed class ConditionLiteralExpression : ConditionExpression
  {
    public ConditionLiteralExpression(object literalValue) => this.LiteralValue = literalValue;

    public object LiteralValue { get; private set; }

    public override string ToString()
    {
      return this.LiteralValue == null ? "null" : Convert.ToString(this.LiteralValue, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    protected override object EvaluateNode(LogEventInfo context) => this.LiteralValue;
  }
}
