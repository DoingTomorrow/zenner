// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.ICacheAssembler
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;

#nullable disable
namespace NHibernate.Type
{
  public interface ICacheAssembler
  {
    object Disassemble(object value, ISessionImplementor session, object owner);

    object Assemble(object cached, ISessionImplementor session, object owner);

    void BeforeAssemble(object cached, ISessionImplementor session);
  }
}
