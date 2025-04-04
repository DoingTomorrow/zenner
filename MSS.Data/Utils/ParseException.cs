// Decompiled with JetBrains decompiler
// Type: MSS.Data.Utils.ParseException
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using System;

#nullable disable
namespace MSS.Data.Utils
{
  public sealed class ParseException : Exception
  {
    private int position;

    public ParseException(string message, int position)
      : base(message)
    {
      this.position = position;
    }

    public int Position => this.position;

    public override string ToString()
    {
      return string.Format("{0} (at index {1})", (object) this.Message, (object) this.position);
    }
  }
}
