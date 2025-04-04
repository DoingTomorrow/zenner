// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Model.DataWSAccounts.DataWSAccountIdentifier
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using Common.Library.NHibernate.Data;
using System;

#nullable disable
namespace MSS.MDMCommunication.Business.Model.DataWSAccounts
{
  [CompositeIdentifier]
  [Serializable]
  public class DataWSAccountIdentifier
  {
    public virtual int Enterprise_ID { get; set; }

    public virtual string Account_ID { get; set; }

    public override bool Equals(object obj)
    {
      return obj != null && obj is DataWSAccountIdentifier accountIdentifier && this.Enterprise_ID == accountIdentifier.Enterprise_ID && this.Account_ID == accountIdentifier.Account_ID;
    }

    public override int GetHashCode()
    {
      return (this.Enterprise_ID.ToString() + "_" + this.Account_ID).GetHashCode();
    }
  }
}
