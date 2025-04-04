// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.IInstantiator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Tuple
{
  public interface IInstantiator
  {
    object Instantiate(object id);

    object Instantiate();

    bool IsInstance(object obj);
  }
}
