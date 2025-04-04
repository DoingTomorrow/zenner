// Decompiled with JetBrains decompiler
// Type: NLog.Targets.MethodCallTargetBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace NLog.Targets
{
  public abstract class MethodCallTargetBase : Target
  {
    protected MethodCallTargetBase()
    {
      this.Parameters = (IList<MethodCallParameter>) new List<MethodCallParameter>();
    }

    [ArrayParameter(typeof (MethodCallParameter), "parameter")]
    public IList<MethodCallParameter> Parameters { get; private set; }

    protected override void Write(AsyncLogEventInfo logEvent)
    {
      object[] parameters = new object[this.Parameters.Count];
      for (int index = 0; index < parameters.Length; ++index)
      {
        MethodCallParameter parameter = this.Parameters[index];
        string str = this.RenderLogEvent(parameter.Layout, logEvent.LogEvent);
        parameters[index] = Convert.ChangeType((object) str, parameter.ParameterType, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      this.DoInvoke(parameters, logEvent);
    }

    protected virtual void DoInvoke(object[] parameters, AsyncLogEventInfo logEvent)
    {
      this.DoInvoke(parameters, logEvent.Continuation);
    }

    protected virtual void DoInvoke(object[] parameters, AsyncContinuation continuation)
    {
      try
      {
        this.DoInvoke(parameters);
        continuation((Exception) null);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrown())
          throw;
        else
          continuation(ex);
      }
    }

    protected abstract void DoInvoke(object[] parameters);
  }
}
