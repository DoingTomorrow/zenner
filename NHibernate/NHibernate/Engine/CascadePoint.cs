// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.CascadePoint
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Engine
{
  public enum CascadePoint
  {
    AfterEvict = 0,
    AfterLock = 0,
    AfterUpdate = 0,
    BeforeFlush = 0,
    BeforeMerge = 0,
    BeforeRefresh = 0,
    AfterInsertBeforeDelete = 1,
    BeforeInsertAfterDelete = 2,
    AfterInsertBeforeDeleteViaCollection = 3,
  }
}
