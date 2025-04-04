// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.CommonToken
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  internal class CommonToken : IToken
  {
    protected internal int type;
    protected internal int line;
    protected internal int charPositionInLine = -1;
    protected internal int channel;
    [NonSerialized]
    protected internal ICharStream input;
    protected internal string text;
    protected internal int index = -1;
    protected internal int start;
    protected internal int stop;

    public CommonToken(int type) => this.type = type;

    public CommonToken(ICharStream input, int type, int channel, int start, int stop)
    {
      this.input = input;
      this.type = type;
      this.channel = channel;
      this.start = start;
      this.stop = stop;
    }

    public CommonToken(int type, string text)
    {
      this.type = type;
      this.channel = 0;
      this.text = text;
    }

    public CommonToken(IToken oldToken)
    {
      this.text = oldToken.Text;
      this.type = oldToken.Type;
      this.line = oldToken.Line;
      this.index = oldToken.TokenIndex;
      this.charPositionInLine = oldToken.CharPositionInLine;
      this.channel = oldToken.Channel;
      if (!(oldToken is CommonToken))
        return;
      this.start = ((CommonToken) oldToken).start;
      this.stop = ((CommonToken) oldToken).stop;
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

    public virtual int StartIndex
    {
      get => this.start;
      set => this.start = value;
    }

    public virtual int StopIndex
    {
      get => this.stop;
      set => this.stop = value;
    }

    public virtual int TokenIndex
    {
      get => this.index;
      set => this.index = value;
    }

    public virtual ICharStream InputStream
    {
      get => this.input;
      set => this.input = value;
    }

    public virtual string Text
    {
      get
      {
        if (this.text != null)
          return this.text;
        if (this.input == null)
          return (string) null;
        this.text = this.input.Substring(this.start, this.stop);
        return this.text;
      }
      set => this.text = value;
    }

    public override string ToString()
    {
      string str = string.Empty;
      if (this.channel > 0)
        str = ",channel=" + (object) this.channel;
      string text = this.Text;
      return "[@" + (object) this.TokenIndex + "," + (object) this.start + ":" + (object) this.stop + "='" + (text == null ? "<no text>" : text.Replace("\n", "\\\\n").Replace("\r", "\\\\r").Replace("\t", "\\\\t")) + "',<" + (object) this.type + ">" + str + "," + (object) this.line + ":" + (object) this.CharPositionInLine + "]";
    }
  }
}
