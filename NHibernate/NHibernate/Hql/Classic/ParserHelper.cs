// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.ParserHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public sealed class ParserHelper
  {
    public const string HqlVariablePrefix = ":";
    public const string HqlSeparators = " \n\r\f\t,()=<>&|+-=/*'^![]#~\\;";
    public const string PathSeparators = ".";
    public const string Whitespace = " \n\r\f\t";

    public static bool IsWhitespace(string str) => " \n\r\f\t".IndexOf(str) > -1;

    private ParserHelper()
    {
    }

    public static void Parse(IParser p, string text, string seperators, QueryTranslator q)
    {
      StringTokenizer stringTokenizer = new StringTokenizer(text, seperators, true);
      p.Start(q);
      foreach (string token in stringTokenizer)
        p.Token(token, q);
      p.End(q);
    }
  }
}
