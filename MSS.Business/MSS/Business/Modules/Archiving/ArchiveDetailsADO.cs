// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Archiving.ArchiveDetailsADO
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Archiving;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.Business.Modules.Archiving
{
  public class ArchiveDetailsADO
  {
    public ArchiveDetailsADO()
    {
      this.ArchivedEntities = new List<ArchiveEntity>();
      this.StartTime = DateTime.MinValue;
      this.EndTime = DateTime.MinValue;
      this.ArchiveName = string.Empty;
    }

    public List<ArchiveEntity> ArchivedEntities { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string ArchiveName { get; set; }
  }
}
