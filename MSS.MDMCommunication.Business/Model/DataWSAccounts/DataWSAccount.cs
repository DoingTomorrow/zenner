// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Model.DataWSAccounts.DataWSAccount
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

#nullable disable
namespace MSS.MDMCommunication.Business.Model.DataWSAccounts
{
  public class DataWSAccount
  {
    public virtual DataWSAccountIdentifier Id { get; set; }

    public virtual string Password { get; set; }

    public virtual string Description { get; set; }

    public virtual byte IsActive { get; set; }
  }
}
