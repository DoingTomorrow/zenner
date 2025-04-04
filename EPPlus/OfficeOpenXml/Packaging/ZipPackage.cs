// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Packaging.ZipPackage
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using Ionic.Zip;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Packaging
{
  public class ZipPackage : ZipPackageRelationshipBase
  {
    private Dictionary<string, ZipPackagePart> Parts = new Dictionary<string, ZipPackagePart>();
    internal Dictionary<string, ZipPackage.ContentType> _contentTypes = new Dictionary<string, ZipPackage.ContentType>();
    private OfficeOpenXml.CompressionLevel _compression = OfficeOpenXml.CompressionLevel.Level6;

    internal ZipPackage() => this.AddNew();

    private void AddNew()
    {
      this._contentTypes.Add("xml", new ZipPackage.ContentType("application/xml", true, "xml"));
      this._contentTypes.Add("rels", new ZipPackage.ContentType("application/vnd.openxmlformats-package.relationships+xml", true, "rels"));
    }

    internal ZipPackage(Stream stream)
    {
      bool flag = false;
      if (stream == null || stream.Length == 0L)
      {
        this.AddNew();
      }
      else
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        stream.Seek(0L, SeekOrigin.Begin);
        using (ZipInputStream zipInputStream = new ZipInputStream(stream))
        {
          for (ZipEntry nextEntry = zipInputStream.GetNextEntry(); nextEntry != null; nextEntry = zipInputStream.GetNextEntry())
          {
            if (nextEntry.UncompressedSize > 0L)
            {
              byte[] numArray = new byte[nextEntry.UncompressedSize];
              zipInputStream.Read(numArray, 0, (int) nextEntry.UncompressedSize);
              if (nextEntry.FileName.ToLower() == "[content_types].xml")
              {
                this.AddContentTypes(Encoding.UTF8.GetString(numArray));
                flag = true;
              }
              else if (nextEntry.FileName.ToLower() == "_rels/.rels")
                this.ReadRelation(Encoding.UTF8.GetString(numArray), "");
              else if (nextEntry.FileName.ToLower().EndsWith(".rels"))
                dictionary.Add(this.GetUriKey(nextEntry.FileName), Encoding.UTF8.GetString(numArray));
              else
                this.Parts.Add(this.GetUriKey(nextEntry.FileName), new ZipPackagePart(this, nextEntry)
                {
                  Stream = new MemoryStream(numArray)
                });
            }
          }
          foreach (KeyValuePair<string, ZipPackagePart> part in this.Parts)
          {
            FileInfo fileInfo = new FileInfo(part.Key);
            string key = string.Format("{0}_rels/{1}.rels", (object) part.Key.Substring(0, part.Key.Length - fileInfo.Name.Length), (object) fileInfo.Name);
            if (dictionary.ContainsKey(key))
              part.Value.ReadRelation(dictionary[key], part.Value.Uri.OriginalString);
            if (this._contentTypes.ContainsKey(part.Key))
              part.Value.ContentType = this._contentTypes[part.Key].Name;
            else if (fileInfo.Extension.Length > 1 && this._contentTypes.ContainsKey(fileInfo.Extension.Substring(1)))
              part.Value.ContentType = this._contentTypes[fileInfo.Extension.Substring(1)].Name;
          }
          if (!flag)
            throw new FileFormatException("The file is not an valid Package file. If the file is encrypted, please supply the password in the constructor.");
          if (!flag)
            throw new FileFormatException("The file is not an valid Package file. If the file is encrypted, please supply the password in the constructor.");
          zipInputStream.Close();
        }
      }
    }

    private void AddContentTypes(string xml)
    {
      XmlDocument xmlDoc = new XmlDocument();
      XmlHelper.LoadXmlSafe(xmlDoc, xml, Encoding.UTF8);
      foreach (XmlElement childNode in xmlDoc.DocumentElement.ChildNodes)
      {
        ZipPackage.ContentType contentType = !string.IsNullOrEmpty(childNode.GetAttribute("Extension")) ? new ZipPackage.ContentType(childNode.GetAttribute("ContentType"), true, childNode.GetAttribute("Extension")) : new ZipPackage.ContentType(childNode.GetAttribute("ContentType"), false, childNode.GetAttribute("PartName"));
        this._contentTypes.Add(this.GetUriKey(contentType.Match), contentType);
      }
    }

    internal ZipPackagePart CreatePart(Uri partUri, string contentType)
    {
      return this.CreatePart(partUri, contentType, OfficeOpenXml.CompressionLevel.Level6);
    }

    internal ZipPackagePart CreatePart(
      Uri partUri,
      string contentType,
      OfficeOpenXml.CompressionLevel compressionLevel)
    {
      if (this.PartExists(partUri))
        throw new InvalidOperationException("Part already exist");
      ZipPackagePart part = new ZipPackagePart(this, partUri, contentType, compressionLevel);
      this._contentTypes.Add(this.GetUriKey(part.Uri.OriginalString), new ZipPackage.ContentType(contentType, false, part.Uri.OriginalString));
      this.Parts.Add(this.GetUriKey(part.Uri.OriginalString), part);
      return part;
    }

    internal ZipPackagePart GetPart(Uri partUri)
    {
      if (this.PartExists(partUri))
        return this.Parts[this.GetUriKey(partUri.OriginalString)];
      throw new InvalidOperationException("Part does not exist.");
    }

    internal string GetUriKey(string uri)
    {
      string uriKey = uri;
      if (uriKey[0] != '/')
        uriKey = "/" + uriKey;
      return uriKey;
    }

    internal bool PartExists(Uri partUri)
    {
      return this.Parts.ContainsKey(this.GetUriKey(partUri.OriginalString));
    }

    internal void DeletePart(Uri Uri)
    {
      List<object[]> objArrayList = new List<object[]>();
      foreach (ZipPackagePart zipPackagePart in this.Parts.Values)
      {
        foreach (ZipPackageRelationship relationship in zipPackagePart.GetRelationships())
        {
          if (UriHelper.ResolvePartUri(zipPackagePart.Uri, relationship.TargetUri).OriginalString == Uri.OriginalString)
            objArrayList.Add(new object[2]
            {
              (object) relationship.Id,
              (object) zipPackagePart
            });
        }
      }
      foreach (object[] objArray in objArrayList)
        ((ZipPackageRelationshipBase) objArray[1]).DeleteRelationship(objArray[0].ToString());
      ZipPackageRelationshipCollection relationships = this.GetPart(Uri).GetRelationships();
      while (relationships.Count > 0)
        relationships.Remove(relationships.First<ZipPackageRelationship>().Id);
      this._contentTypes.Remove(this.GetUriKey(Uri.OriginalString));
      this.Parts.Remove(this.GetUriKey(Uri.OriginalString));
    }

    internal MemoryStream Save()
    {
      MemoryStream memoryStream = new MemoryStream();
      Encoding utF8 = Encoding.UTF8;
      ZipOutputStream os = new ZipOutputStream((Stream) memoryStream, true);
      os.CompressionLevel = (Ionic.Zlib.CompressionLevel) this._compression;
      os.PutNextEntry("[Content_Types].xml");
      byte[] bytes = utF8.GetBytes(this.GetContentTypeXml());
      os.Write(bytes, 0, bytes.Length);
      this._rels.WriteZip(os, "_rels\\.rels");
      ZipPackagePart zipPackagePart1 = (ZipPackagePart) null;
      foreach (ZipPackagePart zipPackagePart2 in this.Parts.Values)
      {
        if (zipPackagePart2.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml")
          zipPackagePart2.WriteZip(os);
        else
          zipPackagePart1 = zipPackagePart2;
      }
      zipPackagePart1?.WriteZip(os);
      os.Flush();
      os.Close();
      os.Dispose();
      return memoryStream;
    }

    private string GetContentTypeXml()
    {
      StringBuilder stringBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">");
      foreach (ZipPackage.ContentType contentType in this._contentTypes.Values)
      {
        if (contentType.IsExtension)
          stringBuilder.AppendFormat("<Default ContentType=\"{0}\" Extension=\"{1}\"/>", (object) contentType.Name, (object) contentType.Match);
        else
          stringBuilder.AppendFormat("<Override ContentType=\"{0}\" PartName=\"{1}\" />", (object) contentType.Name, (object) this.GetUriKey(contentType.Match));
      }
      stringBuilder.Append("</Types>");
      return stringBuilder.ToString();
    }

    internal void Flush()
    {
    }

    internal void Close()
    {
    }

    public OfficeOpenXml.CompressionLevel Compression
    {
      get => this._compression;
      set
      {
        foreach (ZipPackagePart zipPackagePart in this.Parts.Values)
        {
          if (zipPackagePart.CompressionLevel == this._compression)
            zipPackagePart.CompressionLevel = value;
        }
        this._compression = value;
      }
    }

    internal class ContentType
    {
      internal string Name;
      internal bool IsExtension;
      internal string Match;

      public ContentType(string name, bool isExtension, string match)
      {
        this.Name = name;
        this.IsExtension = isExtension;
        this.Match = match;
      }
    }
  }
}
