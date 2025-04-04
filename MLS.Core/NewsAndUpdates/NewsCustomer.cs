// Decompiled with JetBrains decompiler
// Type: MLS.Core.Model.NewsAndUpdates.NewsCustomer
// Assembly: MLS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FC8C31B-A3E8-40E1-84D2-E43683A764BC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MLS.Core.dll

using MLS.Core.Model.Licensing;

#nullable disable
namespace MLS.Core.Model.NewsAndUpdates
{
  public class NewsCustomer
  {
    private News _news;
    private Customer _Customer;

    public NewsCustomer() => this.Id = new NewsCustomersIdentifier();

    public virtual NewsCustomersIdentifier Id { get; set; }

    public virtual News News
    {
      get => this._news;
      set
      {
        this._news = value;
        this.Id.NewsId = value != null ? value.Id : 0;
      }
    }

    public virtual Customer Customer
    {
      get => this._Customer;
      set
      {
        this._Customer = value;
        this.Id.CustomerId = value != null ? value.Id : 0;
      }
    }
  }
}
