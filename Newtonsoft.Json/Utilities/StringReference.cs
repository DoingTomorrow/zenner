// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StringReference
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Utilities
{
  internal struct StringReference(char[] chars, int startIndex, int length)
  {
    private readonly char[] _chars = chars;
    private readonly int _startIndex = startIndex;
    private readonly int _length = length;

    public char this[int i] => this._chars[i];

    public char[] Chars => this._chars;

    public int StartIndex => this._startIndex;

    public int Length => this._length;

    public override string ToString() => new string(this._chars, this._startIndex, this._length);
  }
}
