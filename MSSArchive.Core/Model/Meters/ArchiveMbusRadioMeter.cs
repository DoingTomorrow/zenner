// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Meters.ArchiveMbusRadioMeter
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.Meters
{
  public class ArchiveMbusRadioMeter
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual Guid MeterId { get; set; }

    public virtual string City { get; set; }

    public virtual string Street { get; set; }

    public virtual string HouseNumber { get; set; }

    public virtual string HouseNumberSupplement { get; set; }

    public virtual string ApartmentNumber { get; set; }

    public virtual string ZipCode { get; set; }

    public virtual string FirstName { get; set; }

    public virtual string LastName { get; set; }

    public virtual string Location { get; set; }

    public virtual string RadioSerialNumber { get; set; }

    [ExcludeProperty]
    public virtual ArchiveInformation ArchiveInformation { get; set; }
  }
}
