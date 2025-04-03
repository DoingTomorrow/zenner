// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.NHibernateNLogFactory
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using NHibernate;
using NLog;
using System;

#nullable disable
namespace MSS.Business.Utils
{
  public class NHibernateNLogFactory : ILoggerFactory
  {
    public IInternalLogger LoggerFor(Type type)
    {
      return (IInternalLogger) new NLogLogger(LogManager.GetLogger(type.FullName));
    }

    public IInternalLogger LoggerFor(string keyName)
    {
      return (IInternalLogger) new NLogLogger(LogManager.GetLogger(keyName));
    }
  }
}
