// Decompiled with JetBrains decompiler
// Type: MLS.Core.Model.NewsAndUpdates.News
// Assembly: MLS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FC8C31B-A3E8-40E1-84D2-E43683A764BC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MLS.Core.dll

using MLS.Core.Model.Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

#nullable disable
namespace MLS.Core.Model.NewsAndUpdates
{
  public class News
  {
    public virtual int Id { get; set; }

    public virtual DateTime StartDate { get; set; }

    public virtual DateTime EndDate { get; set; }

    public virtual Customer AdminCustomer { get; set; }

    public virtual string Subject { get; set; }

    [StringLength(5000, ErrorMessage = "Maximum length is 5000 characters.")]
    [AllowHtml]
    public virtual string Description { get; set; }

    public virtual IList<NewsCustomer> NewsCustomers { get; set; }
  }
}
