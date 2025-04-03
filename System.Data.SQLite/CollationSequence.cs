// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.CollationSequence
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public struct CollationSequence
  {
    public string Name;
    public CollationTypeEnum Type;
    public CollationEncodingEnum Encoding;
    internal SQLiteFunction _func;

    public int Compare(string s1, string s2)
    {
      return this._func._base.ContextCollateCompare(this.Encoding, this._func._context, s1, s2);
    }

    public int Compare(char[] c1, char[] c2)
    {
      return this._func._base.ContextCollateCompare(this.Encoding, this._func._context, c1, c2);
    }
  }
}
