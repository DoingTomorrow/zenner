// Decompiled with JetBrains decompiler
// Type: NLog.Targets.MethodCallTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NLog.Targets
{
  [Target("MethodCall")]
  public sealed class MethodCallTarget : MethodCallTargetBase
  {
    public string ClassName { get; set; }

    public string MethodName { get; set; }

    private MethodInfo Method { get; set; }

    private int NeededParameters { get; set; }

    public MethodCallTarget()
    {
    }

    public MethodCallTarget(string name)
      : this()
    {
      this.Name = name;
    }

    protected override void InitializeTarget()
    {
      base.InitializeTarget();
      if (this.ClassName != null && this.MethodName != null)
      {
        Type type = Type.GetType(this.ClassName);
        if (type != (Type) null)
        {
          this.Method = type.GetMethod(this.MethodName);
          if (this.Method == (MethodInfo) null)
            InternalLogger.Warn<string, string>("Initialize MethodCallTarget, method '{0}' in class '{1}' not found - it should be static", this.MethodName, this.ClassName);
          else
            this.NeededParameters = this.Method.GetParameters().Length;
        }
        else
        {
          InternalLogger.Warn<string>("Initialize MethodCallTarget, class '{0}' not found", this.ClassName);
          this.Method = (MethodInfo) null;
        }
      }
      else
        this.Method = (MethodInfo) null;
    }

    protected override void DoInvoke(object[] parameters)
    {
      if (this.Method != (MethodInfo) null)
      {
        int count = this.NeededParameters - parameters.Length;
        if (count > 0)
        {
          List<object> objectList = new List<object>((IEnumerable<object>) parameters);
          objectList.AddRange(Enumerable.Repeat<object>(Type.Missing, count));
          parameters = objectList.ToArray();
        }
        this.Method.Invoke((object) null, parameters);
      }
      else
        InternalLogger.Trace("No invoke because class/method was not found or set");
    }
  }
}
