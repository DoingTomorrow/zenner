// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ANTLRStringStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;

#nullable disable
namespace Antlr.Runtime
{
  internal class ANTLRStringStream : ICharStream, IIntStream
  {
    protected internal char[] data;
    protected int n;
    protected internal int p;
    protected internal int line = 1;
    protected internal int charPositionInLine;
    protected internal int markDepth;
    protected internal IList markers;
    protected int lastMarker;
    protected string name;

    protected ANTLRStringStream()
    {
    }

    public ANTLRStringStream(string input)
    {
      this.data = input.ToCharArray();
      this.n = input.Length;
    }

    public ANTLRStringStream(char[] data, int numberOfActualCharsInArray)
    {
      this.data = data;
      this.n = numberOfActualCharsInArray;
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

    public virtual void Reset()
    {
      this.p = 0;
      this.line = 1;
      this.charPositionInLine = 0;
      this.markDepth = 0;
    }

    public virtual void Consume()
    {
      if (this.p >= this.n)
        return;
      ++this.charPositionInLine;
      if (this.data[this.p] == '\n')
      {
        ++this.line;
        this.charPositionInLine = 0;
      }
      ++this.p;
    }

    public virtual int LA(int i)
    {
      if (i == 0)
        return 0;
      if (i < 0)
      {
        ++i;
        if (this.p + i - 1 < 0)
          return -1;
      }
      return this.p + i - 1 >= this.n ? -1 : (int) this.data[this.p + i - 1];
    }

    public virtual int LT(int i) => this.LA(i);

    public virtual int Index() => this.p;

    [Obsolete("Please use property Count instead.")]
    public virtual int Size() => this.Count;

    public virtual int Count => this.n;

    public virtual int Mark()
    {
      if (this.markers == null)
      {
        this.markers = (IList) new ArrayList();
        this.markers.Add((object) null);
      }
      ++this.markDepth;
      CharStreamState charStreamState;
      if (this.markDepth >= this.markers.Count)
      {
        charStreamState = new CharStreamState();
        this.markers.Add((object) charStreamState);
      }
      else
        charStreamState = (CharStreamState) this.markers[this.markDepth];
      charStreamState.p = this.p;
      charStreamState.line = this.line;
      charStreamState.charPositionInLine = this.charPositionInLine;
      this.lastMarker = this.markDepth;
      return this.markDepth;
    }

    public virtual void Rewind(int m)
    {
      CharStreamState marker = (CharStreamState) this.markers[m];
      this.Seek(marker.p);
      this.line = marker.line;
      this.charPositionInLine = marker.charPositionInLine;
      this.Release(m);
    }

    public virtual void Rewind() => this.Rewind(this.lastMarker);

    public virtual void Release(int marker)
    {
      this.markDepth = marker;
      --this.markDepth;
    }

    public virtual void Seek(int index)
    {
      if (index <= this.p)
      {
        this.p = index;
      }
      else
      {
        while (this.p < index)
          this.Consume();
      }
    }

    public virtual string Substring(int start, int stop)
    {
      return new string(this.data, start, stop - start + 1);
    }

    public virtual string SourceName
    {
      get => this.name;
      set => this.name = value;
    }
  }
}
