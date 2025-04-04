// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.SeparatorLineReader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Exceptions;
using System.Text;

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  internal class SeparatorLineReader(ScriptSplitter splitter) : ScriptReader(splitter)
  {
    private StringBuilder _builder = new StringBuilder();
    private bool _foundGo;
    private bool _gFound;

    private void Reset()
    {
      this._foundGo = false;
      this._gFound = false;
      this._builder = new StringBuilder();
    }

    protected override bool ReadDashDashComment()
    {
      if (!this._foundGo)
      {
        base.ReadDashDashComment();
        return false;
      }
      base.ReadDashDashComment();
      return true;
    }

    protected override void ReadSlashStarComment()
    {
      if (this._foundGo)
        throw new SqlParseException("Incorrect syntax was encountered while parsing GO. Cannot have a slash star /* comment */ after a GO statement.");
      base.ReadSlashStarComment();
    }

    protected override bool ReadNext()
    {
      if (this.EndOfLine)
      {
        if (!this._foundGo)
        {
          this._builder.Append(this.Current);
          this.Splitter.Append(this._builder.ToString());
          this.Splitter.SetParser((ScriptReader) new SeparatorLineReader(this.Splitter));
          return false;
        }
        this.Reset();
        return true;
      }
      if (this.WhiteSpace)
      {
        this._builder.Append(this.Current);
        return false;
      }
      if (!this.CharEquals('g') && !this.CharEquals('o'))
      {
        this.FoundNonEmptyCharacter(this.Current);
        return false;
      }
      if (this.CharEquals('o'))
      {
        if (ScriptReader.CharEquals('g', this.LastChar) && !this._foundGo)
          this._foundGo = true;
        else
          this.FoundNonEmptyCharacter(this.Current);
      }
      if (ScriptReader.CharEquals('g', this.Current))
      {
        if (this._gFound || !char.IsWhiteSpace(this.LastChar) && this.LastChar != char.MinValue)
        {
          this.FoundNonEmptyCharacter(this.Current);
          return false;
        }
        this._gFound = true;
      }
      if (!this.HasNext && this._foundGo)
      {
        this.Reset();
        return true;
      }
      this._builder.Append(this.Current);
      return false;
    }

    private void FoundNonEmptyCharacter(char c)
    {
      this._builder.Append(c);
      this.Splitter.Append(this._builder.ToString());
      this.Splitter.SetParser((ScriptReader) new SqlScriptReader(this.Splitter));
    }
  }
}
