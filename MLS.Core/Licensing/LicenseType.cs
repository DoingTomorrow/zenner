// Decompiled with JetBrains decompiler
// Type: MLS.Core.Model.Licensing.LicenseType
// Assembly: MLS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FC8C31B-A3E8-40E1-84D2-E43683A764BC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MLS.Core.dll

#nullable disable
namespace MLS.Core.Model.Licensing
{
  public class LicenseType
  {
    public virtual int Id { get; set; }

    public virtual string Type { get; set; }

    public virtual bool IsDefault { get; set; }

    public virtual string FilePath { get; set; }

    public virtual bool IsMobileLicense { get; set; }
  }
}
