// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.EntityState
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Event.Default
{
  public enum EntityState
  {
    Undefined = -1, // 0xFFFFFFFF
    Persistent = 0,
    Transient = 1,
    Detached = 2,
    Deleted = 3,
  }
}
