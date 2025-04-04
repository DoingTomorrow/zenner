// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.TemplateRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.MessageTemplates
{
  internal static class TemplateRenderer
  {
    public static void Render(
      this string template,
      IFormatProvider formatProvider,
      object[] parameters,
      bool forceTemplateRenderer,
      StringBuilder sb,
      out IList<MessageTemplateParameter> messageTemplateParameters)
    {
      int startIndex = 0;
      int length1 = 0;
      messageTemplateParameters = (IList<MessageTemplateParameter>) null;
      int length2 = sb.Length;
      TemplateEnumerator templateEnumerator = new TemplateEnumerator(template);
      while (templateEnumerator.MoveNext())
      {
        if (length1 == 0 && !forceTemplateRenderer && templateEnumerator.Current.MaybePositionalTemplate && sb.Length == length2)
        {
          sb.AppendFormat(formatProvider, template, parameters);
          return;
        }
        Literal literal = templateEnumerator.Current.Literal;
        sb.Append(template, startIndex, literal.Print);
        int num = startIndex + literal.Print;
        if (literal.Skip == (short) 0)
        {
          startIndex = num + 1;
        }
        else
        {
          startIndex = num + (int) literal.Skip;
          Hole hole = templateEnumerator.Current.Hole;
          if (hole.Index != (short) -1 && messageTemplateParameters == null)
          {
            ++length1;
            TemplateRenderer.RenderHole(sb, hole, formatProvider, parameters[(int) hole.Index], true);
          }
          else
          {
            object parameter = parameters[length1];
            if (messageTemplateParameters == null)
            {
              messageTemplateParameters = (IList<MessageTemplateParameter>) new MessageTemplateParameter[parameters.Length];
              if (length1 != 0)
              {
                templateEnumerator = new TemplateEnumerator(template);
                sb.Length = length2;
                length1 = 0;
                startIndex = 0;
                continue;
              }
            }
            messageTemplateParameters[length1++] = new MessageTemplateParameter(hole.Name, parameter, hole.Format, hole.CaptureType);
            TemplateRenderer.RenderHole(sb, hole, formatProvider, parameter);
          }
        }
      }
      if (messageTemplateParameters == null || length1 == messageTemplateParameters.Count)
        return;
      MessageTemplateParameter[] templateParameterArray = new MessageTemplateParameter[length1];
      for (int index = 0; index < templateParameterArray.Length; ++index)
        templateParameterArray[index] = messageTemplateParameters[index];
      messageTemplateParameters = (IList<MessageTemplateParameter>) templateParameterArray;
    }

    public static void Render(
      this Template template,
      StringBuilder sb,
      IFormatProvider formatProvider,
      object[] parameters)
    {
      int startIndex = 0;
      int index = 0;
      foreach (Literal literal in template.Literals)
      {
        sb.Append(template.Value, startIndex, literal.Print);
        int num = startIndex + literal.Print;
        if (literal.Skip == (short) 0)
        {
          startIndex = num + 1;
        }
        else
        {
          startIndex = num + (int) literal.Skip;
          if (template.IsPositional)
          {
            Hole hole = template.Holes[index++];
            TemplateRenderer.RenderHole(sb, hole, formatProvider, parameters[(int) hole.Index], true);
          }
          else
            TemplateRenderer.RenderHole(sb, template.Holes[index], formatProvider, parameters[index++]);
        }
      }
    }

    private static void RenderHole(
      StringBuilder sb,
      Hole hole,
      IFormatProvider formatProvider,
      object value,
      bool legacy = false)
    {
      TemplateRenderer.RenderHole(sb, hole.CaptureType, hole.Format, formatProvider, value, legacy);
    }

    public static void RenderHole(
      StringBuilder sb,
      CaptureType captureType,
      string holeFormat,
      IFormatProvider formatProvider,
      object value,
      bool legacy = false)
    {
      if (value == null)
        sb.Append("NULL");
      else if (captureType == CaptureType.Normal & legacy)
        ValueFormatter.FormatToString(value, holeFormat, formatProvider, sb);
      else
        ValueFormatter.Instance.FormatValue(value, holeFormat, captureType, formatProvider, sb);
    }
  }
}
