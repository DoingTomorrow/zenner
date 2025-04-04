// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.LayoutParser
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Config;
using NLog.Internal;
using NLog.LayoutRenderers;
using NLog.LayoutRenderers.Wrappers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.Layouts
{
  internal static class LayoutParser
  {
    internal static LayoutRenderer[] CompileLayout(
      ConfigurationItemFactory configurationItemFactory,
      SimpleStringReader sr,
      bool isNested,
      out string text)
    {
      List<LayoutRenderer> layoutRendererList = new List<LayoutRenderer>();
      StringBuilder literalBuf = new StringBuilder();
      int position1 = sr.Position;
      int ch1;
      while ((ch1 = sr.Peek()) != -1)
      {
        if (isNested)
        {
          if (ch1 == 92)
          {
            sr.Read();
            int ch2 = sr.Peek();
            if (LayoutParser.EndOfLayout(ch2))
            {
              sr.Read();
              literalBuf.Append((char) ch2);
              continue;
            }
            literalBuf.Append('\\');
            continue;
          }
          if (LayoutParser.EndOfLayout(ch1))
            break;
        }
        sr.Read();
        if (ch1 == 36 && sr.Peek() == 123)
        {
          LayoutParser.AddLiteral(literalBuf, layoutRendererList);
          LayoutRenderer layoutRenderer = LayoutParser.ParseLayoutRenderer(configurationItemFactory, sr);
          if (LayoutParser.CanBeConvertedToLiteral(layoutRenderer))
            layoutRenderer = LayoutParser.ConvertToLiteral(layoutRenderer);
          layoutRendererList.Add(layoutRenderer);
        }
        else
          literalBuf.Append((char) ch1);
      }
      LayoutParser.AddLiteral(literalBuf, layoutRendererList);
      int position2 = sr.Position;
      LayoutParser.MergeLiterals(layoutRendererList);
      text = sr.Substring(position1, position2);
      return layoutRendererList.ToArray();
    }

    private static void AddLiteral(StringBuilder literalBuf, List<LayoutRenderer> result)
    {
      if (literalBuf.Length <= 0)
        return;
      result.Add((LayoutRenderer) new LiteralLayoutRenderer(literalBuf.ToString()));
      literalBuf.Length = 0;
    }

    private static bool EndOfLayout(int ch) => ch == 125 || ch == 58;

    private static string ParseLayoutRendererName(SimpleStringReader sr)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num;
      while ((num = sr.Peek()) != -1 && num != 58 && num != 125)
      {
        stringBuilder.Append((char) num);
        sr.Read();
      }
      return stringBuilder.ToString();
    }

    private static string ParseParameterName(SimpleStringReader sr)
    {
      int num1 = 0;
      StringBuilder stringBuilder = new StringBuilder();
      int num2;
      while ((num2 = sr.Peek()) != -1 && (num2 != 61 && num2 != 125 && num2 != 58 || num1 != 0))
      {
        if (num2 == 36)
        {
          sr.Read();
          stringBuilder.Append('$');
          if (sr.Peek() == 123)
          {
            stringBuilder.Append('{');
            ++num1;
            sr.Read();
          }
        }
        else
        {
          if (num2 == 125)
            --num1;
          if (num2 == 92)
          {
            sr.Read();
            stringBuilder.Append((char) sr.Read());
          }
          else
          {
            stringBuilder.Append((char) num2);
            sr.Read();
          }
        }
      }
      return stringBuilder.ToString();
    }

    private static string ParseParameterValue(SimpleStringReader sr)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num;
      while ((num = sr.Peek()) != -1)
      {
        switch (num)
        {
          case 58:
          case 125:
            goto label_17;
          case 92:
            sr.Read();
            char ch = (char) sr.Peek();
            switch (ch)
            {
              case '"':
              case '\'':
              case ':':
              case '\\':
              case '{':
              case '}':
                sr.Read();
                stringBuilder.Append(ch);
                continue;
              case '0':
                sr.Read();
                stringBuilder.Append(char.MinValue);
                continue;
              case 'U':
                sr.Read();
                char unicode1 = LayoutParser.GetUnicode(sr, 8);
                stringBuilder.Append(unicode1);
                continue;
              case 'a':
                sr.Read();
                stringBuilder.Append('\a');
                continue;
              case 'b':
                sr.Read();
                stringBuilder.Append('\b');
                continue;
              case 'f':
                sr.Read();
                stringBuilder.Append('\f');
                continue;
              case 'n':
                sr.Read();
                stringBuilder.Append('\n');
                continue;
              case 'r':
                sr.Read();
                stringBuilder.Append('\r');
                continue;
              case 't':
                sr.Read();
                stringBuilder.Append('\t');
                continue;
              case 'u':
                sr.Read();
                char unicode2 = LayoutParser.GetUnicode(sr, 4);
                stringBuilder.Append(unicode2);
                continue;
              case 'v':
                sr.Read();
                stringBuilder.Append('\v');
                continue;
              case 'x':
                sr.Read();
                char unicode3 = LayoutParser.GetUnicode(sr, 4);
                stringBuilder.Append(unicode3);
                continue;
              default:
                continue;
            }
          default:
            stringBuilder.Append((char) num);
            sr.Read();
            continue;
        }
      }
label_17:
      return stringBuilder.ToString();
    }

    private static char GetUnicode(SimpleStringReader sr, int maxDigits)
    {
      int unicode = 0;
      for (int index = 0; index < maxDigits; ++index)
      {
        int num1 = sr.Peek();
        int num2;
        if (num1 >= 48 && num1 <= 57)
          num2 = num1 - 48;
        else if (num1 >= 97 && num1 <= 102)
          num2 = num1 - 97 + 10;
        else if (num1 >= 65 && num1 <= 70)
          num2 = num1 - 65 + 10;
        else
          break;
        sr.Read();
        unicode = unicode * 16 + num2;
      }
      return (char) unicode;
    }

    private static LayoutRenderer ParseLayoutRenderer(
      ConfigurationItemFactory configurationItemFactory,
      SimpleStringReader stringReader)
    {
      stringReader.Read();
      string layoutRendererName = LayoutParser.ParseLayoutRendererName(stringReader);
      LayoutRenderer layoutRenderer1 = LayoutParser.GetLayoutRenderer(configurationItemFactory, layoutRendererName);
      Dictionary<Type, LayoutRenderer> dictionary = new Dictionary<Type, LayoutRenderer>();
      List<LayoutRenderer> orderedWrappers = new List<LayoutRenderer>();
      for (int index = stringReader.Read(); index != -1 && index != 125; index = stringReader.Read())
      {
        string str = LayoutParser.ParseParameterName(stringReader).Trim();
        if (stringReader.Peek() == 61)
        {
          stringReader.Read();
          LayoutRenderer layoutRenderer2 = layoutRenderer1;
          PropertyInfo result1;
          Type result2;
          if (!PropertyHelper.TryGetPropertyInfo((object) layoutRenderer1, str, out result1) && configurationItemFactory.AmbientProperties.TryGetDefinition(str, out result2))
          {
            LayoutRenderer instance;
            if (!dictionary.TryGetValue(result2, out instance))
            {
              instance = configurationItemFactory.AmbientProperties.CreateInstance(str);
              dictionary[result2] = instance;
              orderedWrappers.Add(instance);
            }
            if (!PropertyHelper.TryGetPropertyInfo((object) instance, str, out result1))
              result1 = (PropertyInfo) null;
            else
              layoutRenderer2 = instance;
          }
          if (result1 == (PropertyInfo) null)
            LayoutParser.ParseParameterValue(stringReader);
          else if (typeof (Layout).IsAssignableFrom(result1.PropertyType))
          {
            SimpleLayout simpleLayout = new SimpleLayout();
            string text;
            LayoutRenderer[] renderers = LayoutParser.CompileLayout(configurationItemFactory, stringReader, true, out text);
            simpleLayout.SetRenderers(renderers, text);
            result1.SetValue((object) layoutRenderer2, (object) simpleLayout, (object[]) null);
          }
          else if (typeof (ConditionExpression).IsAssignableFrom(result1.PropertyType))
          {
            ConditionExpression expression = ConditionParser.ParseExpression(stringReader, configurationItemFactory);
            result1.SetValue((object) layoutRenderer2, (object) expression, (object[]) null);
          }
          else
          {
            string parameterValue = LayoutParser.ParseParameterValue(stringReader);
            PropertyHelper.SetPropertyFromString((object) layoutRenderer2, str, parameterValue, configurationItemFactory);
          }
        }
        else
          LayoutParser.SetDefaultPropertyValue(configurationItemFactory, layoutRenderer1, str);
      }
      return LayoutParser.ApplyWrappers(configurationItemFactory, layoutRenderer1, orderedWrappers);
    }

    private static LayoutRenderer GetLayoutRenderer(
      ConfigurationItemFactory configurationItemFactory,
      string name)
    {
      try
      {
        return configurationItemFactory.LayoutRenderers.CreateInstance(name);
      }
      catch (Exception ex)
      {
        if (((int) LogManager.ThrowConfigExceptions ?? (LogManager.ThrowExceptions ? 1 : 0)) != 0)
        {
          throw;
        }
        else
        {
          object[] objArray = new object[1]{ (object) name };
          InternalLogger.Error(ex, "Error parsing layout {0} will be ignored.", objArray);
          return (LayoutRenderer) new LiteralLayoutRenderer(string.Empty);
        }
      }
    }

    private static void SetDefaultPropertyValue(
      ConfigurationItemFactory configurationItemFactory,
      LayoutRenderer layoutRenderer,
      string parameterName)
    {
      PropertyInfo result;
      if (PropertyHelper.TryGetPropertyInfo((object) layoutRenderer, string.Empty, out result))
      {
        if (typeof (SimpleLayout) == result.PropertyType)
        {
          result.SetValue((object) layoutRenderer, (object) new SimpleLayout(parameterName), (object[]) null);
        }
        else
        {
          string str = parameterName;
          PropertyHelper.SetPropertyFromString((object) layoutRenderer, result.Name, str, configurationItemFactory);
        }
      }
      else
        InternalLogger.Warn<string>("{0} has no default property", layoutRenderer.GetType().FullName);
    }

    private static LayoutRenderer ApplyWrappers(
      ConfigurationItemFactory configurationItemFactory,
      LayoutRenderer lr,
      List<LayoutRenderer> orderedWrappers)
    {
      for (int index = orderedWrappers.Count - 1; index >= 0; --index)
      {
        WrapperLayoutRendererBase orderedWrapper = (WrapperLayoutRendererBase) orderedWrappers[index];
        InternalLogger.Trace<string, string>("Wrapping {0} with {1}", lr.GetType().Name, orderedWrapper.GetType().Name);
        if (LayoutParser.CanBeConvertedToLiteral(lr))
          lr = LayoutParser.ConvertToLiteral(lr);
        orderedWrapper.Inner = (Layout) new SimpleLayout(new LayoutRenderer[1]
        {
          lr
        }, string.Empty, configurationItemFactory);
        lr = (LayoutRenderer) orderedWrapper;
      }
      return lr;
    }

    private static bool CanBeConvertedToLiteral(LayoutRenderer lr)
    {
      object[] objArray = new object[1]{ (object) lr };
      foreach (IRenderable reachableObject in ObjectGraphScanner.FindReachableObjects<IRenderable>(true, objArray))
      {
        if (!(reachableObject.GetType() == typeof (SimpleLayout)) && !reachableObject.GetType().IsDefined(typeof (AppDomainFixedOutputAttribute), false))
          return false;
      }
      return true;
    }

    private static void MergeLiterals(List<LayoutRenderer> list)
    {
      int index = 0;
      while (index + 1 < list.Count)
      {
        LiteralLayoutRenderer literalLayoutRenderer1 = list[index] as LiteralLayoutRenderer;
        LiteralLayoutRenderer literalLayoutRenderer2 = list[index + 1] as LiteralLayoutRenderer;
        if (literalLayoutRenderer1 != null && literalLayoutRenderer2 != null)
        {
          literalLayoutRenderer1.Text += literalLayoutRenderer2.Text;
          list.RemoveAt(index + 1);
        }
        else
          ++index;
      }
    }

    private static LayoutRenderer ConvertToLiteral(LayoutRenderer renderer)
    {
      return (LayoutRenderer) new LiteralLayoutRenderer(renderer.Render(LogEventInfo.CreateNullEvent()));
    }
  }
}
