// Decompiled with JetBrains decompiler
// Type: NHibernate.AssertionFailure
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class AssertionFailure : ApplicationException
  {
    private const string DefaultMessage = "An AssertionFailure occurred - this may indicate a bug in NHibernate or in your custom types.";

    public AssertionFailure()
      : base(string.Empty)
    {
      LoggerProvider.LoggerFor(typeof (AssertionFailure)).Error((object) "An AssertionFailure occurred - this may indicate a bug in NHibernate or in your custom types.");
    }

    public AssertionFailure(string message)
      : base(message)
    {
      LoggerProvider.LoggerFor(typeof (AssertionFailure)).Error((object) "An AssertionFailure occurred - this may indicate a bug in NHibernate or in your custom types.", (Exception) this);
    }

    public AssertionFailure(string message, Exception innerException)
      : base(message, innerException)
    {
      LoggerProvider.LoggerFor(typeof (AssertionFailure)).Error((object) "An AssertionFailure occurred - this may indicate a bug in NHibernate or in your custom types.", innerException);
    }

    protected AssertionFailure(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
