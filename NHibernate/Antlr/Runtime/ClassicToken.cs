// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ClassicToken
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  internal class ClassicToken : IToken
  {
    protected internal string text;
    protected internal int type;
    protected internal int line;
    protected internal int charPositionInLine;
    protected internal int channel;
    protected internal int index;

    public ClassicToken(int type) => this.type = type;

    public ClassicToken(IToken oldToken)
    {
      this.text = oldToken.Text;
      this.type = oldToken.Type;
      this.line = oldToken.Line;
      this.charPositionInLine = oldToken.CharPositionInLine;
      this.channel = oldToken.Channel;
    }

    public ClassicToken(int type, string text)
    {
      this.type = type;
      this.text = text;
    }

    public ClassicToken(int type, string text, int channel)
    {
      this.type = type;
      this.text = text;
      this.channel = channel;
    }

    public virtual int Type
    {
      get => this.type;
      set => this.type = value;
    }

    public virtual int Line
    {
      get => this.line;
      set => this.line = value;
    }

    public virtual int CharPositionInLine
    {
      get => this.charPositionInLine;
      set => this.charPositionInLine = value;
    }

    public virtual int Channel
    {
      get => this.channel;
      set => this.channel = value;
    }

    public virtual int TokenIndex
    {
      get => this.index;
      set => this.index = value;
    }

    public virtual string Text
    {
      get => this.text;
      set => this.text = value;
    }

    public virtual ICharStream InputStream
    {
      get => (ICharStream) null;
      set
      {
      }
    }

    public override string ToString()
    {
      string str = string.Empty;
      if (this.channel > 0)
        str = ",channel=" + (object) this.channel;
      string text = this.Text;
      return "[@" + (object) this.TokenIndex + ",'" + (text == null ? "<no text>" : text.Replace("\n", "\\\\n").Replace("\r", "\\\\r").Replace("\t", "\\\\t")) + "',<" + (object) this.type + ">" + str + "," + (object) this.line + ":" + (object) this.CharPositionInLine + "]";
    }
  }
}
