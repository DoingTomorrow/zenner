// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.DataCollectors.ArchiveMinomatRadioDetails
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.DataCollectors
{
  public class ArchiveMinomatRadioDetails
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual bool Deleted { get; set; }

    public virtual bool Conflict { get; set; }

    public virtual string NoteId { get; set; }

    public virtual string MinomatType { get; set; }

    public virtual string RadioIdMaster { get; set; }

    public virtual string RegistrationStatus { get; set; }

    public virtual string Name { get; set; }

    public virtual string Location { get; set; }

    public virtual string Entrance { get; set; }

    public virtual string MinomatStatus { get; set; }

    public virtual bool IsConfigured { get; set; }

    public virtual string ParameterSet { get; set; }

    public virtual DateTime? DueDate { get; set; }

    public virtual bool CanRegiesterMoreThanOne { get; set; }

    public virtual int ReservedPlaces { get; set; }

    public virtual DateTime? InstalledOn { get; set; }

    public virtual bool IsSlave { get; set; }

    public virtual DateTime LastConnection { get; set; }

    public virtual string StatusOfTheLastConnection { get; set; }

    public virtual string NetzId { get; set; }

    public virtual string Channel { get; set; }

    public virtual string NrOfReceivedDevices { get; set; }

    public virtual string NrOfAssignedDevices { get; set; }

    public virtual string NrOfRegisteredDevices { get; set; }

    public virtual int NetExtensionMode { get; set; }

    public virtual string InstallatioStatus { get; set; }

    public virtual Guid MinomatId { get; set; }

    [ExcludeProperty]
    public virtual ArchiveInformation ArchiveInformation { get; set; }
  }
}
