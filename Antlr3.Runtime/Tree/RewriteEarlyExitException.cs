// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.RewriteEarlyExitException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  public class RewriteEarlyExitException : RewriteCardinalityException
  {
    public RewriteEarlyExitException()
    {
    }

    public RewriteEarlyExitException(string elementDescription)
      : base(elementDescription)
    {
    }

    public RewriteEarlyExitException(string elementDescription, Exception innerException)
      : base(elementDescription, innerException)
    {
    }

    public RewriteEarlyExitException(string message, string elementDescription)
      : base(message, elementDescription)
    {
    }

    public RewriteEarlyExitException(
      string message,
      string elementDescription,
      Exception innerException)
      : base(message, elementDescription, innerException)
    {
    }

    protected RewriteEarlyExitException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
