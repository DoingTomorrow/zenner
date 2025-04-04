// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Meters.ArchiveMeterRadioDetails
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.Meters
{
  public class ArchiveMeterRadioDetails
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual Guid MeterId { get; set; }

    public virtual string dgReg1Flag { get; set; }

    public virtual string dgReg1Mode { get; set; }

    public virtual string dgReg1DakonSerNr { get; set; }

    public virtual string dgReg1Signal { get; set; }

    public virtual string dgReg2Flag { get; set; }

    public virtual string dgReg2Mode { get; set; }

    public virtual string dgReg2DakonSerNr { get; set; }

    public virtual string dgReg2Signal { get; set; }

    public virtual string dgReg3Flag { get; set; }

    public virtual string dgReg3Mode { get; set; }

    public virtual string dgReg3DakonSernr { get; set; }

    public virtual string dgReg3Signal { get; set; }

    public virtual string dgMessbereich { get; set; }

    public virtual string dgZaehlerNr { get; set; }

    public virtual string dgRealErfasser { get; set; }

    public virtual string Street { get; set; }

    public virtual string GemSerialNumber { get; set; }

    public virtual string Scenario { get; set; }

    [ExcludeProperty]
    public virtual ArchiveInformation ArchiveInformation { get; set; }
  }
}
