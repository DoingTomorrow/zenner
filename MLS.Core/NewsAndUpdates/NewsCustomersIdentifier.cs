// Decompiled with JetBrains decompiler
// Type: MLS.Core.Model.NewsAndUpdates.NewsCustomersIdentifier
// Assembly: MLS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FC8C31B-A3E8-40E1-84D2-E43683A764BC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MLS.Core.dll

using Common.Library.NHibernate.Data;
using System;

#nullable disable
namespace MLS.Core.Model.NewsAndUpdates
{
  [CompositeIdentifier]
  [Serializable]
  public class NewsCustomersIdentifier
  {
    public virtual int NewsId { get; set; }

    public virtual int CustomerId { get; set; }

    public override bool Equals(object obj)
    {
      return obj != null && obj is NewsCustomersIdentifier customersIdentifier && this.NewsId == customersIdentifier.NewsId && this.CustomerId == customersIdentifier.CustomerId;
    }

    public override int GetHashCode()
    {
      return (this.NewsId.ToString() + "_" + (object) this.CustomerId).GetHashCode();
    }
  }
}
