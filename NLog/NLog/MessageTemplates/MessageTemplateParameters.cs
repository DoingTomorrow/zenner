// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.MessageTemplateParameters
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NLog.MessageTemplates
{
  public sealed class MessageTemplateParameters : IEnumerable<MessageTemplateParameter>, IEnumerable
  {
    private readonly IList<MessageTemplateParameter> _parameters;

    public IEnumerator<MessageTemplateParameter> GetEnumerator()
    {
      return this._parameters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._parameters.GetEnumerator();

    public MessageTemplateParameter this[int index] => this._parameters[index];

    public int Count => this._parameters.Count;

    public bool IsPositional { get; }

    internal bool IsValidTemplate { get; }

    internal MessageTemplateParameters(string message, object[] parameters)
    {
      bool flag = parameters != null && parameters.Length != 0;
      bool isPositional = flag;
      bool isValidTemplate = !flag;
      this._parameters = flag ? MessageTemplateParameters.ParseMessageTemplate(message, parameters, out isPositional, out isValidTemplate) : (IList<MessageTemplateParameter>) ArrayHelper.Empty<MessageTemplateParameter>();
      this.IsPositional = isPositional;
      this.IsValidTemplate = isValidTemplate;
    }

    internal MessageTemplateParameters(
      IList<MessageTemplateParameter> templateParameters,
      string message,
      object[] parameters)
    {
      this._parameters = (IList<MessageTemplateParameter>) ((object) templateParameters ?? (object) ArrayHelper.Empty<MessageTemplateParameter>());
      if (parameters == null || this._parameters.Count == parameters.Length)
        return;
      this.IsValidTemplate = false;
    }

    private static IList<MessageTemplateParameter> ParseMessageTemplate(
      string template,
      object[] parameters,
      out bool isPositional,
      out bool isValidTemplate)
    {
      isPositional = true;
      isValidTemplate = true;
      List<MessageTemplateParameter> messageTemplate = new List<MessageTemplateParameter>(parameters.Length);
      try
      {
        short holeIndex = 0;
        TemplateEnumerator templateEnumerator = new TemplateEnumerator(template);
        while (templateEnumerator.MoveNext())
        {
          if (templateEnumerator.Current.Literal.Skip != (short) 0)
          {
            Hole hole = templateEnumerator.Current.Hole;
            if (hole.Index != (short) -1 & isPositional)
            {
              ++holeIndex;
              object holeValueSafe = MessageTemplateParameters.GetHoleValueSafe(parameters, hole.Index);
              messageTemplate.Add(new MessageTemplateParameter(hole.Name, holeValueSafe, hole.Format, hole.CaptureType));
            }
            else
            {
              if (isPositional)
              {
                isPositional = false;
                if (holeIndex != (short) 0)
                {
                  templateEnumerator = new TemplateEnumerator(template);
                  holeIndex = (short) 0;
                  messageTemplate.Clear();
                  continue;
                }
              }
              object holeValueSafe = MessageTemplateParameters.GetHoleValueSafe(parameters, holeIndex);
              messageTemplate.Add(new MessageTemplateParameter(hole.Name, holeValueSafe, hole.Format, hole.CaptureType));
              ++holeIndex;
            }
          }
        }
        if (messageTemplate.Count != parameters.Length)
          isValidTemplate = false;
        return (IList<MessageTemplateParameter>) messageTemplate;
      }
      catch (Exception ex)
      {
        isValidTemplate = false;
        InternalLogger.Warn(ex, "Error when parsing a message.");
        return (IList<MessageTemplateParameter>) messageTemplate;
      }
    }

    private static object GetHoleValueSafe(object[] parameters, short holeIndex)
    {
      return parameters.Length <= (int) holeIndex ? (object) null : parameters[(int) holeIndex];
    }
  }
}
