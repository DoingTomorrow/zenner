// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.ScriptReader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  internal abstract class ScriptReader
  {
    protected readonly ScriptSplitter Splitter;

    protected ScriptReader(ScriptSplitter splitter) => this.Splitter = splitter;

    public bool ReadNextSection()
    {
      if (this.IsQuote)
      {
        this.ReadQuotedString();
        return false;
      }
      if (this.BeginDashDashComment)
        return this.ReadDashDashComment();
      if (!this.BeginSlashStarComment)
        return this.ReadNext();
      this.ReadSlashStarComment();
      return false;
    }

    protected virtual bool ReadDashDashComment()
    {
      this.Splitter.Append(this.Current);
      while (this.Splitter.Next())
      {
        this.Splitter.Append(this.Current);
        if (this.EndOfLine)
          break;
      }
      this.Splitter.SetParser((ScriptReader) new SeparatorLineReader(this.Splitter));
      return false;
    }

    protected virtual void ReadSlashStarComment()
    {
      if (!this.ReadSlashStarCommentWithResult())
        return;
      this.Splitter.SetParser((ScriptReader) new SeparatorLineReader(this.Splitter));
    }

    private bool ReadSlashStarCommentWithResult()
    {
      this.Splitter.Append(this.Current);
      while (this.Splitter.Next())
      {
        if (this.BeginSlashStarComment)
        {
          this.ReadSlashStarCommentWithResult();
        }
        else
        {
          this.Splitter.Append(this.Current);
          if (this.EndSlashStarComment)
            return true;
        }
      }
      return false;
    }

    protected virtual void ReadQuotedString()
    {
      this.Splitter.Append(this.Current);
      while (this.Splitter.Next())
      {
        this.Splitter.Append(this.Current);
        if (this.IsQuote)
          break;
      }
    }

    protected abstract bool ReadNext();

    protected bool HasNext => this.Splitter.HasNext;

    protected bool WhiteSpace => char.IsWhiteSpace(this.Splitter.Current);

    protected bool EndOfLine => '\n' == this.Splitter.Current;

    protected bool IsQuote => '\'' == this.Splitter.Current;

    protected char Current => this.Splitter.Current;

    protected char LastChar => this.Splitter.LastChar;

    private bool BeginDashDashComment => this.Current == '-' && this.Peek() == '-';

    private bool BeginSlashStarComment => this.Current == '/' && this.Peek() == '*';

    private bool EndSlashStarComment => this.LastChar == '*' && this.Current == '/';

    protected static bool CharEquals(char expected, char actual)
    {
      return (int) char.ToLowerInvariant(expected) == (int) char.ToLowerInvariant(actual);
    }

    protected bool CharEquals(char compare) => ScriptReader.CharEquals(this.Current, compare);

    protected char Peek() => !this.HasNext ? char.MinValue : (char) this.Splitter.Peek();
  }
}
