// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.LoadEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Event
{
  public static class LoadEventListener
  {
    public static readonly LoadType Reload = new LoadType(nameof (Get)).SetAllowNulls(false).SetAllowProxyCreation(false).SetCheckDeleted(true).SetNakedEntityReturned(false).SetExactPersister(true);
    public static readonly LoadType Get = new LoadType(nameof (Get)).SetAllowNulls(true).SetAllowProxyCreation(false).SetCheckDeleted(true).SetNakedEntityReturned(false).SetExactPersister(true);
    public static readonly LoadType Load = new LoadType(nameof (Load)).SetAllowNulls(false).SetAllowProxyCreation(true).SetCheckDeleted(true).SetNakedEntityReturned(false);
    public static readonly LoadType ImmediateLoad = new LoadType(nameof (ImmediateLoad)).SetAllowNulls(true).SetAllowProxyCreation(false).SetCheckDeleted(false).SetNakedEntityReturned(true);
    public static readonly LoadType InternalLoadEager = new LoadType(nameof (InternalLoadEager)).SetAllowNulls(false).SetAllowProxyCreation(false).SetCheckDeleted(false).SetNakedEntityReturned(false);
    public static readonly LoadType InternalLoadLazy = new LoadType(nameof (InternalLoadLazy)).SetAllowNulls(false).SetAllowProxyCreation(true).SetCheckDeleted(false).SetNakedEntityReturned(false);
    public static readonly LoadType InternalLoadNullable = new LoadType(nameof (InternalLoadNullable)).SetAllowNulls(true).SetAllowProxyCreation(false).SetCheckDeleted(false).SetNakedEntityReturned(false);
  }
}
