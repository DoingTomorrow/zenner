// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.ScriptSplitter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  public class ScriptSplitter : IEnumerable<string>, IEnumerable
  {
    private readonly TextReader _reader;
    private StringBuilder _builder = new StringBuilder();
    private char _current;
    private char _lastChar;
    private ScriptReader _scriptReader;

    public ScriptSplitter(string script)
    {
      this._reader = (TextReader) new StringReader(script);
      this._scriptReader = (ScriptReader) new SeparatorLineReader(this);
    }

    internal bool HasNext => this._reader.Peek() != -1;

    internal char Current => this._current;

    internal char LastChar => this._lastChar;

    public IEnumerator<string> GetEnumerator()
    {
      while (this.Next())
      {
        if (this.Split())
        {
          string script = this._builder.ToString().Trim();
          if (script.Length > 0)
            yield return script;
          this.Reset();
        }
      }
      if (this._builder.Length > 0)
      {
        string scriptRemains = this._builder.ToString().Trim();
        if (scriptRemains.Length > 0)
          yield return scriptRemains;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    internal bool Next()
    {
      if (!this.HasNext)
        return false;
      this._lastChar = this._current;
      this._current = (char) this._reader.Read();
      return true;
    }

    internal int Peek() => this._reader.Peek();

    private bool Split() => this._scriptReader.ReadNextSection();

    internal void SetParser(ScriptReader newReader) => this._scriptReader = newReader;

    internal void Append(string text) => this._builder.Append(text);

    internal void Append(char c) => this._builder.Append(c);

    private void Reset()
    {
      this._current = this._lastChar = char.MinValue;
      this._builder = new StringBuilder();
    }
  }
}
