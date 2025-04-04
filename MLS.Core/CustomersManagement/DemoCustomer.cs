// Decompiled with JetBrains decompiler
// Type: MLS.Core.Model.CustomersManagement.DemoCustomer
// Assembly: MLS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FC8C31B-A3E8-40E1-84D2-E43683A764BC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MLS.Core.dll

using MLS.Core.Model.Licensing;
using System;

#nullable disable
namespace MLS.Core.Model.CustomersManagement
{
  public class DemoCustomer
  {
    public virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual string ZIP { get; set; }

    public virtual string City { get; set; }

    public virtual string Street { get; set; }

    public virtual string HouseNumber { get; set; }

    public virtual string CompanyName { get; set; }

    public virtual Country Country { get; set; }

    public virtual string CustomerMSSNumber { get; set; }

    public virtual string CustomerNumber { get; set; }

    public virtual string Email { get; set; }

    public virtual string Language { get; set; }

    public virtual DateTime RequestTime { get; set; }

    public virtual bool IsActivated { get; set; }

    public virtual bool HasAgreedGDPR { get; set; }

    public virtual int CustomerLicenseType { get; set; }

    public virtual AccountStatusEnum? AccountStatus { get; set; }
  }
}
