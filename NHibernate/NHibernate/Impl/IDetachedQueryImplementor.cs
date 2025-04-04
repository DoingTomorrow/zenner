// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.IDetachedQueryImplementor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Impl
{
  public interface IDetachedQueryImplementor
  {
    void CopyTo(IDetachedQuery destination);

    void SetParametersTo(IDetachedQuery destination);

    void OverrideInfoFrom(IDetachedQueryImplementor origin);

    void OverrideParametersFrom(IDetachedQueryImplementor origin);
  }
}
