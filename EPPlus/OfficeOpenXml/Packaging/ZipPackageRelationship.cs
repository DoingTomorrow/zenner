// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Packaging.ZipPackageRelationship
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Packaging
{
  public class ZipPackageRelationship
  {
    public Uri TargetUri { get; internal set; }

    public Uri SourceUri { get; internal set; }

    public string RelationshipType { get; internal set; }

    public TargetMode TargetMode { get; internal set; }

    public string Id { get; internal set; }
  }
}
