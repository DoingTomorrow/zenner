// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplateFormatMethodAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog
{
  [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public sealed class MessageTemplateFormatMethodAttribute : Attribute
  {
    public MessageTemplateFormatMethodAttribute(string parameterName)
    {
      this.ParameterName = parameterName;
    }

    public string ParameterName { get; }
  }
}
