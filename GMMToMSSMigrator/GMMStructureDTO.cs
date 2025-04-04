// Decompiled with JetBrains decompiler
// Type: GMMToMSSMigrator.GMMStructureDTO
// Assembly: GMMToMSSMigrator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3ACF3C29-B99D-4830-8DFE-AD2278C0306B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMMToMSSMigrator.dll

using System;

#nullable disable
namespace GMMToMSSMigrator
{
  public class GMMStructureDTO
  {
    public int NodeID { get; set; }

    public int ParentID { get; set; }

    public int NodeOrder { get; set; }

    public int LayerID { get; set; }

    public int NodeTypeID { get; set; }

    public string NodeName { get; set; }

    public string NodeDescription { get; set; }

    public string NodeSettings { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string SerialNr { get; set; }

    public DateTime? ProductionDate { get; set; }
  }
}
