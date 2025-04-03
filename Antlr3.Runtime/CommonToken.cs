// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.CommonToken
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class CommonToken : IToken
  {
    private int type;
    private int line;
    private int charPositionInLine = -1;
    private int channel;
    [NonSerialized]
    private ICharStream input;
    private string text;
    private int index = -1;
    private int start;
    private int stop;

    public CommonToken()
    {
    }

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
      this.input = oldToken.InputStream;
      if (!(oldToken is CommonToken))
        return;
      this.start = ((CommonToken) oldToken).start;
      this.stop = ((CommonToken) oldToken).stop;
    }

    public string Text
    {
      get
      {
        if (this.text != null)
          return this.text;
        if (this.input == null)
          return (string) null;
        return this.start <= this.stop && this.stop < this.input.Count ? this.input.Substring(this.start, this.stop - this.start + 1) : "<EOF>";
      }
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
      get => this.start;
      set => this.start = value;
    }

    public int StopIndex
    {
      get => this.stop;
      set => this.stop = value;
    }

    public int TokenIndex
    {
      get => this.index;
      set => this.index = value;
    }

    public ICharStream InputStream
    {
      get => this.input;
      set => this.input = value;
    }

    public override string ToString()
    {
      string str = "";
      if (this.channel > 0)
        str = ",channel=" + (object) this.channel;
      string text = this.Text;
      return "[@" + (object) this.TokenIndex + "," + (object) this.start + ":" + (object) this.stop + "='" + (text == null ? "<no text>" : Regex.Replace(Regex.Replace(Regex.Replace(text, "\n", "\\\\n"), "\r", "\\\\r"), "\t", "\\\\t")) + "',<" + (object) this.type + ">" + str + "," + (object) this.line + ":" + (object) this.CharPositionInLine + "]";
    }

    [System.Runtime.Serialization.OnSerializing]
    internal void OnSerializing(StreamingContext context)
    {
      if (this.text != null)
        return;
      this.text = this.Text;
    }
  }
}
