// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.LoadType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Event
{
  public sealed class LoadType
  {
    private readonly string name;
    private bool nakedEntityReturned;
    private bool allowNulls;
    private bool checkDeleted;
    private bool allowProxyCreation;
    private bool exactPersister;

    internal LoadType(string name)
    {
      this.name = !string.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof (name));
    }

    public string Name => this.name;

    public bool IsAllowNulls => this.allowNulls;

    internal LoadType SetAllowNulls(bool allowNulls)
    {
      this.allowNulls = allowNulls;
      return this;
    }

    public bool IsNakedEntityReturned => this.nakedEntityReturned;

    internal LoadType SetNakedEntityReturned(bool immediateLoad)
    {
      this.nakedEntityReturned = immediateLoad;
      return this;
    }

    public bool IsCheckDeleted => this.checkDeleted;

    internal LoadType SetCheckDeleted(bool checkDeleted)
    {
      this.checkDeleted = checkDeleted;
      return this;
    }

    public bool IsAllowProxyCreation => this.allowProxyCreation;

    internal LoadType SetAllowProxyCreation(bool allowProxyCreation)
    {
      this.allowProxyCreation = allowProxyCreation;
      return this;
    }

    public bool ExactPersister => this.exactPersister;

    internal LoadType SetExactPersister(bool exactPersister)
    {
      this.exactPersister = exactPersister;
      return this;
    }

    public override string ToString() => this.name;
  }
}
