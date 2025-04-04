// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.TemplateParserException
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog.MessageTemplates
{
  public class TemplateParserException : Exception
  {
    public int Index { get; }

    public string Template { get; }

    public TemplateParserException(string message, int index, string template)
      : base(message)
    {
      this.Index = index;
      this.Template = template;
    }
  }
}
