// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.RewriteEmptyStreamException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  public class RewriteEmptyStreamException : RewriteCardinalityException
  {
    public RewriteEmptyStreamException()
    {
    }

    public RewriteEmptyStreamException(string elementDescription)
      : base(elementDescription)
    {
    }

    public RewriteEmptyStreamException(string elementDescription, Exception innerException)
      : base(elementDescription, innerException)
    {
    }

    public RewriteEmptyStreamException(string message, string elementDescription)
      : base(message, elementDescription)
    {
    }

    public RewriteEmptyStreamException(
      string message,
      string elementDescription,
      Exception innerException)
      : base(message, elementDescription, innerException)
    {
    }

    protected RewriteEmptyStreamException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
