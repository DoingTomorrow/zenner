// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Packaging.ZipPackagePart
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using Ionic.Zip;
using System;
using System.IO;

#nullable disable
namespace OfficeOpenXml.Packaging
{
  internal class ZipPackagePart : ZipPackageRelationshipBase, IDisposable
  {
    internal OfficeOpenXml.CompressionLevel CompressionLevel;
    private MemoryStream _stream;
    private string _contentType = "";

    internal ZipPackagePart(ZipPackage package, ZipEntry entry)
    {
      this.Package = package;
      this.Entry = entry;
      this.SaveHandler = (ZipPackagePart.SaveHandlerDelegate) null;
      this.Uri = new Uri(package.GetUriKey(entry.FileName), UriKind.Relative);
    }

    internal ZipPackagePart(
      ZipPackage package,
      Uri partUri,
      string contentType,
      OfficeOpenXml.CompressionLevel compressionLevel)
    {
      this.Package = package;
      this.Uri = partUri;
      this.ContentType = contentType;
      this.CompressionLevel = compressionLevel;
    }

    internal ZipPackage Package { get; set; }

    internal ZipEntry Entry { get; set; }

    internal MemoryStream Stream
    {
      get => this._stream;
      set => this._stream = value;
    }

    internal override ZipPackageRelationship CreateRelationship(
      Uri targetUri,
      TargetMode targetMode,
      string relationshipType)
    {
      ZipPackageRelationship relationship = base.CreateRelationship(targetUri, targetMode, relationshipType);
      relationship.SourceUri = this.Uri;
      return relationship;
    }

    internal MemoryStream GetStream()
    {
      return this.GetStream(FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    internal MemoryStream GetStream(FileMode fileMode)
    {
      return this.GetStream(FileMode.Create, FileAccess.ReadWrite);
    }

    internal MemoryStream GetStream(FileMode fileMode, FileAccess fileAccess)
    {
      if (this._stream == null || fileMode == FileMode.CreateNew || fileMode == FileMode.Create)
        this._stream = new MemoryStream();
      else
        this._stream.Seek(0L, SeekOrigin.Begin);
      return this._stream;
    }

    public string ContentType
    {
      get => this._contentType;
      internal set
      {
        if (!string.IsNullOrEmpty(this._contentType) && this.Package._contentTypes.ContainsKey(this.Package.GetUriKey(this.Uri.OriginalString)))
        {
          this.Package._contentTypes.Remove(this.Package.GetUriKey(this.Uri.OriginalString));
          this.Package._contentTypes.Add(this.Package.GetUriKey(this.Uri.OriginalString), new ZipPackage.ContentType(value, false, this.Uri.OriginalString));
        }
        this._contentType = value;
      }
    }

    public Uri Uri { get; private set; }

    public System.IO.Stream GetZipStream()
    {
      return (System.IO.Stream) new ZipOutputStream((System.IO.Stream) new MemoryStream());
    }

    internal ZipPackagePart.SaveHandlerDelegate SaveHandler { get; set; }

    internal void WriteZip(ZipOutputStream os)
    {
      if (this.SaveHandler == null)
      {
        byte[] array = this.GetStream().ToArray();
        if (array.Length == 0)
          return;
        os.CompressionLevel = (Ionic.Zlib.CompressionLevel) this.CompressionLevel;
        os.PutNextEntry(this.Uri.OriginalString);
        os.Write(array, 0, array.Length);
      }
      else
        this.SaveHandler(os, (Ionic.Zlib.CompressionLevel) this.CompressionLevel, this.Uri.OriginalString);
      if (this._rels.Count > 0)
      {
        string originalString = this.Uri.OriginalString;
        FileInfo fileInfo = new FileInfo(originalString);
        this._rels.WriteZip(os, string.Format("{0}_rels/{1}.rels", (object) originalString.Substring(0, originalString.Length - fileInfo.Name.Length), (object) fileInfo.Name));
      }
    }

    public void Dispose()
    {
      this._stream.Close();
      this._stream.Dispose();
    }

    internal delegate void SaveHandlerDelegate(
      ZipOutputStream stream,
      Ionic.Zlib.CompressionLevel compressionLevel,
      string fileName);
  }
}
