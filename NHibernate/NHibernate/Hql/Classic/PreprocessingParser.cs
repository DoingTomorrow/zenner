// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.PreprocessingParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class PreprocessingParser : IParser
  {
    private static readonly ISet<string> operators = (ISet<string>) new HashedSet<string>();
    private static readonly IDictionary<string, string> collectionProps;
    private readonly IDictionary<string, string> replacements;
    private bool quoted;
    private StringBuilder quotedString;
    private readonly ClauseParser parser = new ClauseParser();
    private string lastToken;
    private string currentCollectionProp;

    static PreprocessingParser()
    {
      PreprocessingParser.operators.Add("<=");
      PreprocessingParser.operators.Add(">=");
      PreprocessingParser.operators.Add("=>");
      PreprocessingParser.operators.Add("=<");
      PreprocessingParser.operators.Add("!=");
      PreprocessingParser.operators.Add("<>");
      PreprocessingParser.operators.Add("!#");
      PreprocessingParser.operators.Add("!~");
      PreprocessingParser.operators.Add("!<");
      PreprocessingParser.operators.Add("!>");
      PreprocessingParser.operators.Add("is not");
      PreprocessingParser.operators.Add("not like");
      PreprocessingParser.operators.Add("not in");
      PreprocessingParser.operators.Add("not between");
      PreprocessingParser.operators.Add("not exists");
      PreprocessingParser.collectionProps = (IDictionary<string, string>) new Dictionary<string, string>();
      PreprocessingParser.collectionProps.Add("elements", "elements");
      PreprocessingParser.collectionProps.Add("indices", "indices");
      PreprocessingParser.collectionProps.Add("size", "size");
      PreprocessingParser.collectionProps.Add("maxindex", "maxIndex");
      PreprocessingParser.collectionProps.Add("minindex", "minIndex");
      PreprocessingParser.collectionProps.Add("maxelement", "maxElement");
      PreprocessingParser.collectionProps.Add("minelement", "minElement");
      PreprocessingParser.collectionProps.Add("index", "index");
    }

    public PreprocessingParser(IDictionary<string, string> replacements)
    {
      this.replacements = replacements;
    }

    public void Token(string token, QueryTranslator q)
    {
      if (this.quoted)
        this.quotedString.Append(token);
      if ("'".Equals(token))
      {
        if (this.quoted)
          token = this.quotedString.ToString();
        else
          this.quotedString = new StringBuilder(20).Append(token);
        this.quoted = !this.quoted;
      }
      if (this.quoted || ParserHelper.IsWhitespace(token))
        return;
      string str1;
      if (this.replacements.TryGetValue(token, out str1))
        token = str1;
      if (this.currentCollectionProp != null)
      {
        if ("(".Equals(token))
          return;
        if (")".Equals(token))
        {
          this.currentCollectionProp = (string) null;
          return;
        }
        token = token + (object) '.' + this.currentCollectionProp;
      }
      else
      {
        string str2;
        if (PreprocessingParser.collectionProps.TryGetValue(token.ToLowerInvariant(), out str2))
        {
          this.currentCollectionProp = str2;
          return;
        }
      }
      if (this.lastToken == null)
      {
        this.lastToken = token;
      }
      else
      {
        string token1 = token.Length > 1 ? this.lastToken + (object) ' ' + token : this.lastToken + token;
        if (PreprocessingParser.operators.Contains(token1.ToLowerInvariant()))
        {
          this.parser.Token(token1, q);
          this.lastToken = (string) null;
        }
        else
        {
          this.parser.Token(this.lastToken, q);
          this.lastToken = token;
        }
      }
    }

    public virtual void Start(QueryTranslator q)
    {
      this.quoted = false;
      this.parser.Start(q);
    }

    public virtual void End(QueryTranslator q)
    {
      if (this.lastToken != null)
        this.parser.Token(this.lastToken, q);
      this.parser.End(q);
      this.lastToken = (string) null;
      this.currentCollectionProp = (string) null;
    }
  }
}
