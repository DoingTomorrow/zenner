// Decompiled with JetBrains decompiler
// Type: MSS.Interfaces.ISessionManager
// Assembly: MSS.Interfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 178808BA-C10E-4054-B175-D79F79744EFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Interfaces.dll

using NHibernate;

#nullable disable
namespace MSS.Interfaces
{
  public interface ISessionManager
  {
    ISession OpenSession();

    void CloseSession(ISession session);
  }
}
