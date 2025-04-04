// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.StringTokenizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Util
{
  public class StringTokenizer : IEnumerable<string>, IEnumerable
  {
    private const string _defaultDelim = " \t\n\r\f";
    private string _origin;
    private string _delim;
    private bool _returnDelim;

    public StringTokenizer(string str)
    {
      this._origin = str;
      this._delim = " \t\n\r\f";
      this._returnDelim = false;
    }

    public StringTokenizer(string str, string delim)
    {
      this._origin = str;
      this._delim = delim;
      this._returnDelim = true;
    }

    public StringTokenizer(string str, string delim, bool returnDelims)
    {
      this._origin = str;
      this._delim = delim;
      this._returnDelim = returnDelims;
    }

    public IEnumerator<string> GetEnumerator()
    {
      return (IEnumerator<string>) new StringTokenizer.StringTokenizerEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new StringTokenizer.StringTokenizerEnumerator(this);
    }

    private class StringTokenizerEnumerator : IEnumerator<string>, IDisposable, IEnumerator
    {
      private StringTokenizer _stokenizer;
      private int _cursor;
      private string _next;

      public StringTokenizerEnumerator(StringTokenizer stok) => this._stokenizer = stok;

      public string Current => this._next;

      public void Dispose()
      {
      }

      object IEnumerator.Current => (object) this.Current;

      public bool MoveNext()
      {
        this._next = this.GetNext();
        return this._next != null;
      }

      public void Reset() => this._cursor = 0;

      private string GetNext()
      {
        if (this._cursor >= this._stokenizer._origin.Length)
          return (string) null;
        char ch = this._stokenizer._origin[this._cursor];
        if (this._stokenizer._delim.IndexOf(ch) != -1)
        {
          ++this._cursor;
          return this._stokenizer._returnDelim ? ch.ToString() : this.GetNext();
        }
        int num = this._stokenizer._origin.IndexOfAny(this._stokenizer._delim.ToCharArray(), this._cursor);
        if (num == -1)
          num = this._stokenizer._origin.Length;
        string next = this._stokenizer._origin.Substring(this._cursor, num - this._cursor);
        this._cursor = num;
        return next;
      }
    }
  }
}
