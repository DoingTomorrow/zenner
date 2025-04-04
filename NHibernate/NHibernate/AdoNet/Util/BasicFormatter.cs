// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.Util.BasicFormatter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Util;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.AdoNet.Util
{
  public class BasicFormatter : IFormatter
  {
    protected const string IndentString = "    ";
    protected const string Initial = "\n    ";
    protected static readonly HashedSet<string> beginClauses = new HashedSet<string>();
    protected static readonly HashedSet<string> dml = new HashedSet<string>();
    protected static readonly HashedSet<string> endClauses = new HashedSet<string>();
    protected static readonly HashedSet<string> logical = new HashedSet<string>();
    protected static readonly HashedSet<string> misc = new HashedSet<string>();
    protected static readonly HashedSet<string> quantifiers = new HashedSet<string>();

    static BasicFormatter()
    {
      BasicFormatter.beginClauses.Add("left");
      BasicFormatter.beginClauses.Add("right");
      BasicFormatter.beginClauses.Add("inner");
      BasicFormatter.beginClauses.Add("outer");
      BasicFormatter.beginClauses.Add("group");
      BasicFormatter.beginClauses.Add("order");
      BasicFormatter.endClauses.Add("where");
      BasicFormatter.endClauses.Add("set");
      BasicFormatter.endClauses.Add("having");
      BasicFormatter.endClauses.Add("join");
      BasicFormatter.endClauses.Add("from");
      BasicFormatter.endClauses.Add("by");
      BasicFormatter.endClauses.Add("join");
      BasicFormatter.endClauses.Add("into");
      BasicFormatter.endClauses.Add("union");
      BasicFormatter.logical.Add("and");
      BasicFormatter.logical.Add("or");
      BasicFormatter.logical.Add("when");
      BasicFormatter.logical.Add("else");
      BasicFormatter.logical.Add("end");
      BasicFormatter.quantifiers.Add("in");
      BasicFormatter.quantifiers.Add("all");
      BasicFormatter.quantifiers.Add("exists");
      BasicFormatter.quantifiers.Add("some");
      BasicFormatter.quantifiers.Add("any");
      BasicFormatter.dml.Add("insert");
      BasicFormatter.dml.Add("update");
      BasicFormatter.dml.Add("delete");
      BasicFormatter.misc.Add("select");
      BasicFormatter.misc.Add("on");
    }

    public virtual string Format(string source)
    {
      return new BasicFormatter.FormatProcess(source).Perform();
    }

    private class FormatProcess
    {
      private readonly List<bool> afterByOrFromOrSelects = new List<bool>();
      private readonly List<int> parenCounts = new List<int>();
      private readonly StringBuilder result = new StringBuilder();
      private readonly IEnumerator<string> tokens;
      private bool afterBeginBeforeEnd;
      private bool afterBetween;
      private bool afterByOrSetOrFromOrSelect;
      private bool afterInsert;
      private bool afterOn;
      private bool beginLine = true;
      private bool endCommandFound;
      private int indent = 1;
      private int inFunction;
      private string lastToken;
      private string lcToken;
      private int parensSinceSelect;
      private string token;

      public FormatProcess(string sql)
      {
        this.tokens = new StringTokenizer(sql, "()+*/-=<>'`\"[],; \n\r\f\t", true).GetEnumerator();
      }

      public string Perform()
      {
        this.result.Append("\n    ");
        while (this.tokens.MoveNext())
        {
          this.token = this.tokens.Current;
          this.lcToken = this.token.ToLowerInvariant();
          if ("'".Equals(this.token))
            this.ExtractStringEnclosedBy("'");
          else if ("\"".Equals(this.token))
            this.ExtractStringEnclosedBy("\"");
          if (this.IsMultiQueryDelimiter(this.token))
            this.StartingNewQuery();
          else if (this.afterByOrSetOrFromOrSelect && ",".Equals(this.token))
            this.CommaAfterByOrFromOrSelect();
          else if (this.afterOn && ",".Equals(this.token))
            this.CommaAfterOn();
          else if ("(".Equals(this.token))
            this.OpenParen();
          else if (")".Equals(this.token))
            this.CloseParen();
          else if (BasicFormatter.beginClauses.Contains(this.lcToken))
            this.BeginNewClause();
          else if (BasicFormatter.endClauses.Contains(this.lcToken))
            this.EndNewClause();
          else if ("select".Equals(this.lcToken))
            this.Select();
          else if (BasicFormatter.dml.Contains(this.lcToken))
            this.UpdateOrInsertOrDelete();
          else if ("values".Equals(this.lcToken))
            this.Values();
          else if ("on".Equals(this.lcToken))
            this.On();
          else if (this.afterBetween && this.lcToken.Equals("and"))
          {
            this.Misc();
            this.afterBetween = false;
          }
          else if (BasicFormatter.logical.Contains(this.lcToken))
            this.Logical();
          else if (BasicFormatter.FormatProcess.IsWhitespace(this.token))
            this.White();
          else
            this.Misc();
          if (!BasicFormatter.FormatProcess.IsWhitespace(this.token))
            this.lastToken = this.lcToken;
        }
        return this.result.ToString();
      }

      private void StartingNewQuery()
      {
        this.Out();
        this.indent = 1;
        this.endCommandFound = true;
        this.Newline();
      }

      private bool IsMultiQueryDelimiter(string delimiter) => ";".Equals(delimiter);

      private void ExtractStringEnclosedBy(string stringDelimiter)
      {
        while (this.tokens.MoveNext())
        {
          string current = this.tokens.Current;
          this.token += current;
          if (stringDelimiter.Equals(current))
            break;
        }
      }

      private void CommaAfterOn()
      {
        this.Out();
        --this.indent;
        this.Newline();
        this.afterOn = false;
        this.afterByOrSetOrFromOrSelect = true;
      }

      private void CommaAfterByOrFromOrSelect()
      {
        this.Out();
        this.Newline();
      }

      private void Logical()
      {
        if ("end".Equals(this.lcToken))
          --this.indent;
        this.Newline();
        this.Out();
        this.beginLine = false;
      }

      private void On()
      {
        ++this.indent;
        this.afterOn = true;
        this.Newline();
        this.Out();
        this.beginLine = false;
      }

      private void Misc()
      {
        this.Out();
        if ("between".Equals(this.lcToken))
          this.afterBetween = true;
        if (this.afterInsert)
        {
          this.Newline();
          this.afterInsert = false;
        }
        else
        {
          this.beginLine = false;
          if (!"case".Equals(this.lcToken))
            return;
          ++this.indent;
        }
      }

      private void White()
      {
        if (this.beginLine)
          return;
        this.result.Append(" ");
      }

      private void UpdateOrInsertOrDelete()
      {
        this.Out();
        ++this.indent;
        this.beginLine = false;
        if ("update".Equals(this.lcToken))
          this.Newline();
        if ("insert".Equals(this.lcToken))
          this.afterInsert = true;
        this.endCommandFound = false;
      }

      private void Select()
      {
        this.Out();
        ++this.indent;
        this.Newline();
        this.parenCounts.Insert(this.parenCounts.Count, this.parensSinceSelect);
        this.afterByOrFromOrSelects.Insert(this.afterByOrFromOrSelects.Count, this.afterByOrSetOrFromOrSelect);
        this.parensSinceSelect = 0;
        this.afterByOrSetOrFromOrSelect = true;
        this.endCommandFound = false;
      }

      private void Out() => this.result.Append(this.token);

      private void EndNewClause()
      {
        if (!this.afterBeginBeforeEnd)
        {
          --this.indent;
          if (this.afterOn)
          {
            --this.indent;
            this.afterOn = false;
          }
          this.Newline();
        }
        this.Out();
        if (!"union".Equals(this.lcToken))
          ++this.indent;
        this.Newline();
        this.afterBeginBeforeEnd = false;
        this.afterByOrSetOrFromOrSelect = "by".Equals(this.lcToken) || "set".Equals(this.lcToken) || "from".Equals(this.lcToken);
      }

      private void BeginNewClause()
      {
        if (!this.afterBeginBeforeEnd)
        {
          if (this.afterOn)
          {
            --this.indent;
            this.afterOn = false;
          }
          --this.indent;
          this.Newline();
        }
        this.Out();
        this.beginLine = false;
        this.afterBeginBeforeEnd = true;
      }

      private void Values()
      {
        --this.indent;
        this.Newline();
        this.Out();
        ++this.indent;
        this.Newline();
      }

      private void CloseParen()
      {
        if (this.endCommandFound)
        {
          this.Out();
        }
        else
        {
          --this.parensSinceSelect;
          if (this.parensSinceSelect < 0)
          {
            --this.indent;
            int parenCount = this.parenCounts[this.parenCounts.Count - 1];
            this.parenCounts.RemoveAt(this.parenCounts.Count - 1);
            this.parensSinceSelect = parenCount;
            bool byOrFromOrSelect = this.afterByOrFromOrSelects[this.afterByOrFromOrSelects.Count - 1];
            this.afterByOrFromOrSelects.RemoveAt(this.afterByOrFromOrSelects.Count - 1);
            this.afterByOrSetOrFromOrSelect = byOrFromOrSelect;
          }
          if (this.inFunction > 0)
          {
            --this.inFunction;
            this.Out();
          }
          else
          {
            if (!this.afterByOrSetOrFromOrSelect)
            {
              --this.indent;
              this.Newline();
            }
            this.Out();
          }
          this.beginLine = false;
        }
      }

      private void OpenParen()
      {
        if (this.endCommandFound)
        {
          this.Out();
        }
        else
        {
          if (BasicFormatter.FormatProcess.IsFunctionName(this.lastToken) || this.inFunction > 0)
            ++this.inFunction;
          this.beginLine = false;
          if (this.inFunction > 0)
          {
            this.Out();
          }
          else
          {
            this.Out();
            if (!this.afterByOrSetOrFromOrSelect)
            {
              ++this.indent;
              this.Newline();
              this.beginLine = true;
            }
          }
          ++this.parensSinceSelect;
        }
      }

      private static bool IsFunctionName(string tok)
      {
        char c = tok[0];
        return (char.IsLetter(c) || c.CompareTo('$') == 0 || c.CompareTo('_') == 0 || '"' == c) && !BasicFormatter.logical.Contains(tok) && !BasicFormatter.endClauses.Contains(tok) && !BasicFormatter.quantifiers.Contains(tok) && !BasicFormatter.dml.Contains(tok) && !BasicFormatter.misc.Contains(tok);
      }

      private static bool IsWhitespace(string token) => " \n\r\f\t".IndexOf(token) >= 0;

      private void Newline()
      {
        this.result.Append("\n");
        for (int index = 0; index < this.indent; ++index)
          this.result.Append("    ");
        this.beginLine = true;
      }
    }
  }
}
