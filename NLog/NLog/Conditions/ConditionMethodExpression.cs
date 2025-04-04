// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionMethodExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.Conditions
{
  internal sealed class ConditionMethodExpression : ConditionExpression
  {
    private readonly bool _acceptsLogEvent;
    private readonly string _conditionMethodName;
    private readonly ReflectionHelpers.LateBoundMethod _lateBoundMethod;
    private readonly object[] _lateBoundMethodDefaultParameters;

    public ConditionMethodExpression(
      string conditionMethodName,
      MethodInfo methodInfo,
      IEnumerable<ConditionExpression> methodParameters)
    {
      this.MethodInfo = methodInfo;
      this._conditionMethodName = conditionMethodName;
      this.MethodParameters = (IList<ConditionExpression>) new List<ConditionExpression>(methodParameters).AsReadOnly();
      ParameterInfo[] parameters = this.MethodInfo.GetParameters();
      if (parameters.Length != 0 && parameters[0].ParameterType == typeof (LogEventInfo))
        this._acceptsLogEvent = true;
      int count1 = this.MethodParameters.Count;
      if (this._acceptsLogEvent)
        ++count1;
      int num1 = 0;
      int num2 = 0;
      foreach (ParameterInfo parameterInfo in parameters)
      {
        if (parameterInfo.IsOptional)
          ++num2;
        else
          ++num1;
      }
      if (count1 < num1 || count1 > parameters.Length)
      {
        string message;
        if (num2 > 0)
          message = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Condition method '{0}' requires between {1} and {2} parameters, but passed {3}.", (object) conditionMethodName, (object) num1, (object) parameters.Length, (object) count1);
        else
          message = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Condition method '{0}' requires {1} parameters, but passed {2}.", new object[3]
          {
            (object) conditionMethodName,
            (object) num1,
            (object) count1
          });
        InternalLogger.Error(message);
        throw new ConditionParseException(message);
      }
      this._lateBoundMethod = ReflectionHelpers.CreateLateBoundMethod(this.MethodInfo);
      if (parameters.Length > this.MethodParameters.Count)
      {
        this._lateBoundMethodDefaultParameters = new object[parameters.Length - this.MethodParameters.Count];
        for (int count2 = this.MethodParameters.Count; count2 < parameters.Length; ++count2)
        {
          ParameterInfo parameterInfo = parameters[count2];
          this._lateBoundMethodDefaultParameters[count2 - this.MethodParameters.Count] = parameterInfo.DefaultValue;
        }
      }
      else
        this._lateBoundMethodDefaultParameters = (object[]) null;
    }

    public MethodInfo MethodInfo { get; private set; }

    public IList<ConditionExpression> MethodParameters { get; private set; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this._conditionMethodName);
      stringBuilder.Append("(");
      string str = string.Empty;
      for (int index = 0; index < this.MethodParameters.Count; ++index)
      {
        ConditionExpression methodParameter = this.MethodParameters[index];
        stringBuilder.Append(str);
        stringBuilder.Append((object) methodParameter);
        str = ", ";
      }
      stringBuilder.Append(")");
      return stringBuilder.ToString();
    }

    protected override object EvaluateNode(LogEventInfo context)
    {
      int num = this._acceptsLogEvent ? 1 : 0;
      int length = this._lateBoundMethodDefaultParameters != null ? this._lateBoundMethodDefaultParameters.Length : 0;
      object[] arguments = new object[this.MethodParameters.Count + num + length];
      for (int index = 0; index < this.MethodParameters.Count; ++index)
      {
        ConditionExpression methodParameter = this.MethodParameters[index];
        arguments[index + num] = methodParameter.Evaluate(context);
      }
      if (this._acceptsLogEvent)
        arguments[0] = (object) context;
      if (this._lateBoundMethodDefaultParameters != null)
      {
        for (int index = this._lateBoundMethodDefaultParameters.Length - 1; index >= 0; --index)
          arguments[arguments.Length - index - 1] = this._lateBoundMethodDefaultParameters[index];
      }
      return this._lateBoundMethod((object) null, arguments);
    }
  }
}
