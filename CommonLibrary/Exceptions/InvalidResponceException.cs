// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Exceptions.InvalidResponceException
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;

#nullable disable
namespace ZENNER.CommonLibrary.Exceptions
{
  [Serializable]
  public class InvalidResponceException : Exception
  {
    public byte[] Buffer { get; private set; }

    public InvalidResponceException(string message)
      : this(message, (byte[]) null)
    {
    }

    public InvalidResponceException(string message, byte[] buffer)
      : base(message)
    {
      this.Buffer = buffer;
    }

    public InvalidResponceException(Exception innerException, byte[] buffer)
      : base(innerException.Message, innerException)
    {
      this.Buffer = buffer;
    }

    public override string ToString()
    {
      return this.Buffer == null ? base.ToString() : base.ToString() + " Buffer: " + BitConverter.ToString(this.Buffer);
    }
  }
}
