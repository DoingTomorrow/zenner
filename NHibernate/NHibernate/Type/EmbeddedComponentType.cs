// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.EmbeddedComponentType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Tuple.Component;
using System;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class EmbeddedComponentType(ComponentMetamodel metamodel) : ComponentType(metamodel)
  {
    public override bool IsEmbedded => true;

    public override object Instantiate(object parent, ISessionImplementor session)
    {
      return true ? base.Instantiate(parent, session) : parent;
    }
  }
}
