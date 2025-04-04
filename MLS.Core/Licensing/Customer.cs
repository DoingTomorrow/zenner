// Decompiled with JetBrains decompiler
// Type: MLS.Core.Model.Licensing.Customer
// Assembly: MLS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FC8C31B-A3E8-40E1-84D2-E43683A764BC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MLS.Core.dll

using MLS.Core.Model.CustomersManagement;
using MLS.Core.Model.NewsAndUpdates;
using System;
using System.Collections.Generic;

#nullable disable
namespace MLS.Core.Model.Licensing
{
  public class Customer
  {
    public virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual string CustomerManualNumber { get; set; }

    public virtual string ZIP { get; set; }

    public virtual string City { get; set; }

    public virtual string Street { get; set; }

    public virtual string HouseNumber { get; set; }

    public virtual IList<LicenseBulk> Licenses { get; set; }

    public virtual Customer ParentCustomer { get; set; }

    public virtual string CompanyName { get; set; }

    public virtual Country Country { get; set; }

    public virtual string Email { get; set; }

    public virtual string Language { get; set; }

    public virtual IList<CustomerRole> CustomerRoles { get; set; }

    public virtual IList<NewsCustomer> NewsCustomers { get; set; }

    public virtual string Password { get; set; }

    public virtual string CustomerMSSNumber { get; set; }

    public virtual string CustomerNumber { get; set; }

    public virtual DateTime? RequestTime { get; set; }

    public virtual DateTime? ForgottenPasswordRequestTime { get; set; }

    public virtual bool HasAgreedGDPR { get; set; }

    public virtual int CustomerLicenseType { get; set; }
  }
}
