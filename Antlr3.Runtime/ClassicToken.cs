// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ClassicToken
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class ClassicToken : IToken
  {
    private string text;
    private int type;
    private int line;
    private int charPositionInLine;
    private int channel;
    private int index;

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

    public string Text
    {
      get => this.text;
      set => this.text = value;
    }

    public int Type
    {
      get => this.type;
      set => this.type = value;
    }

    public int Line
    {
      get => this.line;
      set => this.line = value;
    }

    public int CharPositionInLine
    {
      get => this.charPositionInLine;
      set => this.charPositionInLine = value;
    }

    public int Channel
    {
      get => this.channel;
      set => this.channel = value;
    }

    public int StartIndex
    {
      get => -1;
      set
      {
      }
    }

    public int StopIndex
    {
      get => -1;
      set
      {
      }
    }

    public int TokenIndex
    {
      get => this.index;
      set => this.index = value;
    }

    public ICharStream InputStream
    {
      get => (ICharStream) null;
      set
      {
      }
    }

    public override string ToString()
    {
      string str = "";
      if (this.channel > 0)
        str = ",channel=" + (object) this.channel;
      string text = this.Text;
      return "[@" + (object) this.TokenIndex + ",'" + (text == null ? "<no text>" : text.Replace("\n", "\\\\n").Replace("\r", "\\\\r").Replace("\t", "\\\\t")) + "',<" + (object) this.type + ">" + str + "," + (object) this.line + ":" + (object) this.CharPositionInLine + "]";
    }
  }
}
