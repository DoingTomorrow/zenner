// Decompiled with JetBrains decompiler
// Type: MLS.Core.Model.Licensing.LicenseBulk
// Assembly: MLS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FC8C31B-A3E8-40E1-84D2-E43683A764BC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MLS.Core.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MLS.Core.Model.Licensing
{
  public class LicenseBulk
  {
    public virtual int Id { get; set; }

    public virtual LicenseType LicenseType { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual int Available { get; set; }

    public virtual int Total { get; set; }

    public virtual int AvailableReplacements { get; set; }

    public virtual int TotalReplacements { get; set; }

    public virtual IList<License> IndividualLicenses { get; set; }

    public virtual string Reason { get; set; }

    public virtual DateTime? ValidityStartDate { get; set; }

    public virtual string FilePath { get; set; }
  }
}
