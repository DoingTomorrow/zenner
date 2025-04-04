// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Packaging.ZipPackageRelationshipBase
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Packaging
{
  public abstract class ZipPackageRelationshipBase
  {
    protected ZipPackageRelationshipCollection _rels = new ZipPackageRelationshipCollection();
    protected internal int maxRId = 1;

    internal void DeleteRelationship(string id)
    {
      this._rels.Remove(id);
      this.UpdateMaxRId(id, ref this.maxRId);
    }

    protected void UpdateMaxRId(string id, ref int maxRId)
    {
      int result;
      if (!id.StartsWith("rId") || !int.TryParse(id.Substring(3), out result) || result != maxRId - 1)
        return;
      --maxRId;
    }

    internal virtual ZipPackageRelationship CreateRelationship(
      Uri targetUri,
      TargetMode targetMode,
      string relationshipType)
    {
      ZipPackageRelationship relationship = new ZipPackageRelationship();
      relationship.TargetUri = targetUri;
      relationship.TargetMode = targetMode;
      relationship.RelationshipType = relationshipType;
      relationship.Id = "rId" + this.maxRId++.ToString();
      this._rels.Add(relationship);
      return relationship;
    }

    internal bool RelationshipExists(string id) => this._rels.ContainsKey(id);

    internal ZipPackageRelationshipCollection GetRelationshipsByType(string schema)
    {
      return this._rels.GetRelationshipsByType(schema);
    }

    internal ZipPackageRelationshipCollection GetRelationships() => this._rels;

    internal ZipPackageRelationship GetRelationship(string id) => this._rels[id];

    internal void ReadRelation(string xml, string source)
    {
      XmlDocument xmlDoc = new XmlDocument();
      XmlHelper.LoadXmlSafe(xmlDoc, xml, Encoding.UTF8);
      foreach (XmlElement childNode in xmlDoc.DocumentElement.ChildNodes)
      {
        ZipPackageRelationship packageRelationship = new ZipPackageRelationship();
        packageRelationship.Id = childNode.GetAttribute("Id");
        packageRelationship.RelationshipType = childNode.GetAttribute("Type");
        packageRelationship.TargetMode = childNode.GetAttribute("TargetMode").ToLower() == "external" ? TargetMode.External : TargetMode.Internal;
        try
        {
          packageRelationship.TargetUri = new Uri(childNode.GetAttribute("Target"), UriKind.RelativeOrAbsolute);
        }
        catch
        {
          packageRelationship.TargetUri = new Uri(Uri.EscapeUriString("Invalid:URI " + childNode.GetAttribute("Target")), UriKind.RelativeOrAbsolute);
        }
        if (!string.IsNullOrEmpty(source))
          packageRelationship.SourceUri = new Uri(source, UriKind.Relative);
        int result;
        if (packageRelationship.Id.ToLower().StartsWith("rid") && int.TryParse(packageRelationship.Id.Substring(3), out result) && result >= this.maxRId && result < 2147473647)
          this.maxRId = result + 1;
        this._rels.Add(packageRelationship);
      }
    }
  }
}
