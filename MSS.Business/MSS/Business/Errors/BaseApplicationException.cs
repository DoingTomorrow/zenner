// Decompiled with JetBrains decompiler
// Type: MSS.Business.Errors.BaseApplicationException
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;

#nullable disable
namespace MSS.Business.Errors
{
  [Serializable]
  public class BaseApplicationException : Exception
  {
    public BaseApplicationException()
    {
    }

    public BaseApplicationException(string message)
      : base(message)
    {
    }

    public BaseApplicationException(string format, params object[] args)
      : base(string.Format(format, args))
    {
    }

    public BaseApplicationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public BaseApplicationException(string format, Exception innerException, params object[] args)
      : base(string.Format(format, args), innerException)
    {
    }
  }
}
