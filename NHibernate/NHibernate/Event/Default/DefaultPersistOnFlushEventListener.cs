// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultPersistOnFlushEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultPersistOnFlushEventListener : DefaultPersistEventListener
  {
    protected override CascadingAction CascadeAction => CascadingAction.PersistOnFlush;
  }
}
