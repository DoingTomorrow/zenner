// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.MissingTokenException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  internal class MissingTokenException : MismatchedTokenException
  {
    private object inserted;

    public MissingTokenException()
    {
    }

    public MissingTokenException(int expecting, IIntStream input, object inserted)
      : base(expecting, input)
    {
      this.inserted = inserted;
    }

    public int MissingType => this.Expecting;

    public object Inserted
    {
      get => this.inserted;
      set => this.inserted = value;
    }

    public override string ToString()
    {
      return this.inserted != null && this.token != null ? "MissingTokenException(inserted " + this.inserted + " at " + this.token.Text + ")" : (this.token != null ? "MissingTokenException(at " + this.token.Text + ")" : nameof (MissingTokenException));
    }
  }
}
