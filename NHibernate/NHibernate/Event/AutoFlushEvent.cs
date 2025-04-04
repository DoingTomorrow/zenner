// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.AutoFlushEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class AutoFlushEvent : FlushEvent
  {
    private ISet<string> querySpaces;
    private bool flushRequired;

    public AutoFlushEvent(ISet<string> querySpaces, IEventSource source)
      : base(source)
    {
      this.querySpaces = querySpaces;
    }

    public ISet<string> QuerySpaces
    {
      get => this.querySpaces;
      set => this.querySpaces = value;
    }

    public bool FlushRequired
    {
      get => this.flushRequired;
      set => this.flushRequired = value;
    }
  }
}
