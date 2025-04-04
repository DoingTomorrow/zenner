// Decompiled with JetBrains decompiler
// Type: NLog.Targets.MethodCallParameter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System;

#nullable disable
namespace NLog.Targets
{
  [NLogConfigurationItem]
  public class MethodCallParameter
  {
    public MethodCallParameter() => this.ParameterType = typeof (string);

    public MethodCallParameter(Layout layout)
    {
      this.ParameterType = typeof (string);
      this.Layout = layout;
    }

    public MethodCallParameter(string parameterName, Layout layout)
    {
      this.ParameterType = typeof (string);
      this.Name = parameterName;
      this.Layout = layout;
    }

    public MethodCallParameter(string name, Layout layout, Type type)
    {
      this.ParameterType = type;
      this.Name = name;
      this.Layout = layout;
    }

    public string Name { get; set; }

    public Type Type
    {
      get => this.ParameterType;
      set => this.ParameterType = value;
    }

    public Type ParameterType { get; set; }

    [RequiredParameter]
    public Layout Layout { get; set; }
  }
}
