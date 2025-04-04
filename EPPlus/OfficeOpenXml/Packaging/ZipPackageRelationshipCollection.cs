// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Packaging.ZipPackageRelationshipCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using Ionic.Zip;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Text;

#nullable disable
namespace OfficeOpenXml.Packaging
{
  public class ZipPackageRelationshipCollection : IEnumerable<ZipPackageRelationship>, IEnumerable
  {
    protected internal Dictionary<string, ZipPackageRelationship> _rels = new Dictionary<string, ZipPackageRelationship>();

    internal void Add(ZipPackageRelationship item) => this._rels.Add(item.Id, item);

    public IEnumerator<ZipPackageRelationship> GetEnumerator()
    {
      return (IEnumerator<ZipPackageRelationship>) this._rels.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._rels.Values.GetEnumerator();

    internal void Remove(string id) => this._rels.Remove(id);

    internal bool ContainsKey(string id) => this._rels.ContainsKey(id);

    internal ZipPackageRelationship this[string id] => this._rels[id];

    internal ZipPackageRelationshipCollection GetRelationshipsByType(string relationshipType)
    {
      ZipPackageRelationshipCollection relationshipsByType = new ZipPackageRelationshipCollection();
      foreach (ZipPackageRelationship packageRelationship in this._rels.Values)
      {
        if (packageRelationship.RelationshipType == relationshipType)
          relationshipsByType.Add(packageRelationship);
      }
      return relationshipsByType;
    }

    internal void WriteZip(ZipOutputStream os, string fileName)
    {
      StringBuilder stringBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
      foreach (ZipPackageRelationship packageRelationship in this._rels.Values)
        stringBuilder.AppendFormat("<Relationship Id=\"{0}\" Type=\"{1}\" Target=\"{2}\"{3}/>", (object) SecurityElement.Escape(packageRelationship.Id), (object) packageRelationship.RelationshipType, (object) SecurityElement.Escape(packageRelationship.TargetUri.OriginalString), packageRelationship.TargetMode == TargetMode.External ? (object) " TargetMode=\"External\"" : (object) "");
      stringBuilder.Append("</Relationships>");
      os.PutNextEntry(fileName);
      byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
      os.Write(bytes, 0, bytes.Length);
    }

    public int Count => this._rels.Count;
  }
}
