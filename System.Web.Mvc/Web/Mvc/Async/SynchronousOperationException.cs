// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.SynchronousOperationException
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Runtime.Serialization;

#nullable disable
namespace System.Web.Mvc.Async
{
  [Serializable]
  public sealed class SynchronousOperationException : HttpException
  {
    public SynchronousOperationException()
    {
    }

    private SynchronousOperationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public SynchronousOperationException(string message)
      : base(message)
    {
    }

    public SynchronousOperationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
