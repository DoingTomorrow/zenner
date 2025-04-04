// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Utils.ParseException
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using System;

#nullable disable
namespace MSS.PartialSyncData.Utils
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
