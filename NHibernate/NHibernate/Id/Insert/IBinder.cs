// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Insert.IBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Data;

#nullable disable
namespace NHibernate.Id.Insert
{
  public interface IBinder
  {
    object Entity { get; }

    void BindValues(IDbCommand cm);
  }
}
