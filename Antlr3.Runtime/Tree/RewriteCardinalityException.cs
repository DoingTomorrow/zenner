// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.RewriteCardinalityException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  public class RewriteCardinalityException : Exception
  {
    private readonly string _elementDescription;

    public RewriteCardinalityException()
    {
    }

    public RewriteCardinalityException(string elementDescription)
      : this(elementDescription, elementDescription)
    {
      this._elementDescription = elementDescription;
    }

    public RewriteCardinalityException(string elementDescription, Exception innerException)
      : this(elementDescription, elementDescription, innerException)
    {
    }

    public RewriteCardinalityException(string message, string elementDescription)
      : base(message)
    {
      this._elementDescription = elementDescription;
    }

    public RewriteCardinalityException(
      string message,
      string elementDescription,
      Exception innerException)
      : base(message, innerException)
    {
      this._elementDescription = elementDescription;
    }

    protected RewriteCardinalityException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._elementDescription = info != null ? info.GetString("ElementDescription") : throw new ArgumentNullException(nameof (info));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      base.GetObjectData(info, context);
      info.AddValue("ElementDescription", (object) this._elementDescription);
    }
  }
}
