// Decompiled with JetBrains decompiler
// Type: NLog.Internal.SimpleStringReader
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

#nullable disable
namespace NLog.Internal
{
  internal class SimpleStringReader
  {
    private readonly string _text;

    public SimpleStringReader(string text)
    {
      this._text = text;
      this.Position = 0;
    }

    internal int Position { get; set; }

    internal string Text => this._text;

    internal int Peek() => this.Position < this._text.Length ? (int) this._text[this.Position] : -1;

    internal int Read()
    {
      return this.Position < this._text.Length ? (int) this._text[this.Position++] : -1;
    }

    internal string Substring(int startIndex, int endIndex)
    {
      return this._text.Substring(startIndex, endIndex - startIndex);
    }
  }
}
