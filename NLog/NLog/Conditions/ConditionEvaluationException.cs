// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionEvaluationException
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NLog.Conditions
{
  [Serializable]
  public class ConditionEvaluationException : Exception
  {
    public ConditionEvaluationException()
    {
    }

    public ConditionEvaluationException(string message)
      : base(message)
    {
    }

    public ConditionEvaluationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected ConditionEvaluationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
