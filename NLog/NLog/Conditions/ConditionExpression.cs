// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;

#nullable disable
namespace NLog.Conditions
{
  [NLogConfigurationItem]
  [ThreadAgnostic]
  public abstract class ConditionExpression
  {
    public static implicit operator ConditionExpression(string conditionExpressionText)
    {
      return ConditionParser.ParseExpression(conditionExpressionText);
    }

    public object Evaluate(LogEventInfo context)
    {
      try
      {
        return this.EvaluateNode(context);
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "Exception occurred when evaluating condition");
        if (!ex.MustBeRethrownImmediately())
          throw new ConditionEvaluationException("Exception occurred when evaluating condition", ex);
        throw;
      }
    }

    public abstract override string ToString();

    protected abstract object EvaluateNode(LogEventInfo context);
  }
}
