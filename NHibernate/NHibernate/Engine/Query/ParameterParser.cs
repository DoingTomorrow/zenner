// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.ParameterParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Engine.Query
{
  public class ParameterParser
  {
    private static readonly int NewLineLength = Environment.NewLine.Length;

    private ParameterParser()
    {
    }

    public static void Parse(string sqlString, ParameterParser.IRecognizer recognizer)
    {
      bool flag1 = sqlString.IndexOf("call") > 0 && sqlString.IndexOf("?") > 0 && sqlString.IndexOf("=") > 0 && sqlString.IndexOf("?") < sqlString.IndexOf("call") && sqlString.IndexOf("=") < sqlString.IndexOf("call");
      bool flag2 = false;
      int length = sqlString.Length;
      bool flag3 = false;
      bool flag4 = false;
      for (int index = 0; index < length; ++index)
      {
        if (index + 1 < length && sqlString.Substring(index, 2) == "/*")
        {
          int num = sqlString.IndexOf("*/", index + 2);
          recognizer.Other(sqlString.Substring(index, num - index + 2));
          index = num + 1;
        }
        else if (flag4 && index + 1 < length && sqlString.Substring(index, 2) == "--")
        {
          int num = sqlString.IndexOf(Environment.NewLine, index + 2);
          string sqlPart;
          if (num == -1)
          {
            num = sqlString.Length;
            sqlPart = sqlString.Substring(index);
          }
          else
            sqlPart = sqlString.Substring(index, num - index + Environment.NewLine.Length);
          recognizer.Other(sqlPart);
          index = num + ParameterParser.NewLineLength - 1;
        }
        else if (index + ParameterParser.NewLineLength - 1 < length && sqlString.Substring(index, ParameterParser.NewLineLength) == Environment.NewLine)
        {
          flag4 = true;
          index += ParameterParser.NewLineLength - 1;
          recognizer.Other(Environment.NewLine);
        }
        else
        {
          flag4 = false;
          char character = sqlString[index];
          if (flag3)
          {
            if ('\'' == character)
              flag3 = false;
            recognizer.Other(character);
          }
          else if ('\'' == character)
          {
            flag3 = true;
            recognizer.Other(character);
          }
          else
          {
            switch (character)
            {
              case ':':
                int num1 = StringHelper.FirstIndexOfChar(sqlString, " \n\r\f\t,()=<>&|+-=/*'^![]#~\\;", index + 1);
                int num2 = num1 < 0 ? sqlString.Length : num1;
                string name = sqlString.Substring(index + 1, num2 - (index + 1));
                recognizer.NamedParameter(name, index);
                index = num2 - 1;
                continue;
              case '?':
                if (index < length - 1 && char.IsDigit(sqlString[index + 1]))
                {
                  int num3 = StringHelper.FirstIndexOfChar(sqlString, " \n\r\f\t,()=<>&|+-=/*'^![]#~\\;", index + 1);
                  int num4 = num3 < 0 ? sqlString.Length : num3;
                  string str = sqlString.Substring(index + 1, num4 - (index + 1));
                  try
                  {
                    int.Parse(str);
                  }
                  catch (FormatException ex)
                  {
                    throw new QueryException("ejb3-style positional param was not an integral ordinal", (Exception) ex);
                  }
                  recognizer.JpaPositionalParameter(str, index);
                  index = num4 - 1;
                  continue;
                }
                if (flag1 && !flag2)
                {
                  flag2 = true;
                  recognizer.OutParameter(index);
                  continue;
                }
                recognizer.OrdinalParameter(index);
                continue;
              default:
                recognizer.Other(character);
                continue;
            }
          }
        }
      }
    }

    public interface IRecognizer
    {
      void OutParameter(int position);

      void OrdinalParameter(int position);

      void NamedParameter(string name, int position);

      void JpaPositionalParameter(string name, int position);

      void Other(char character);

      void Other(string sqlPart);
    }
  }
}
