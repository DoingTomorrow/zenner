// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelPackage
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Encryption;
using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public sealed class ExcelPackage : IDisposable
  {
    internal const bool preserveWhitespace = false;
    internal const string schemaXmlExtension = "application/xml";
    internal const string schemaRelsExtension = "application/vnd.openxmlformats-package.relationships+xml";
    internal const string schemaMain = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
    internal const string schemaRelationships = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
    internal const string schemaDrawings = "http://schemas.openxmlformats.org/drawingml/2006/main";
    internal const string schemaSheetDrawings = "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing";
    internal const string schemaMicrosoftVml = "urn:schemas-microsoft-com:vml";
    internal const string schemaMicrosoftOffice = "urn:schemas-microsoft-com:office:office";
    internal const string schemaMicrosoftExcel = "urn:schemas-microsoft-com:office:excel";
    internal const string schemaChart = "http://schemas.openxmlformats.org/drawingml/2006/chart";
    internal const string schemaHyperlink = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink";
    internal const string schemaComment = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/comments";
    internal const string schemaImage = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image";
    internal const string schemaCore = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";
    internal const string schemaExtended = "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties";
    internal const string schemaCustom = "http://schemas.openxmlformats.org/officeDocument/2006/custom-properties";
    internal const string schemaDc = "http://purl.org/dc/elements/1.1/";
    internal const string schemaDcTerms = "http://purl.org/dc/terms/";
    internal const string schemaDcmiType = "http://purl.org/dc/dcmitype/";
    internal const string schemaXsi = "http://www.w3.org/2001/XMLSchema-instance";
    internal const string schemaVt = "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes";
    internal const string schemaPivotTable = "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotTable+xml";
    internal const string schemaPivotCacheDefinition = "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotCacheDefinition+xml";
    internal const string schemaPivotCacheRecords = "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotCacheRecords+xml";
    internal const string schemaVBA = "application/vnd.ms-office.vbaProject";
    internal const string schemaVBASignature = "application/vnd.ms-office.vbaProjectSignature";
    internal const string contentTypeWorkbookDefault = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml";
    internal const string contentTypeWorkbookMacroEnabled = "application/vnd.ms-excel.sheet.macroEnabled.main+xml";
    internal const string contentTypeSharedString = "application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml";
    public const int MaxColumns = 16384;
    public const int MaxRows = 1048576;
    private Stream _stream;
    internal Dictionary<string, ExcelPackage.ImageInfo> _images = new Dictionary<string, ExcelPackage.ImageInfo>();
    private ZipPackage _package;
    internal ExcelWorkbook _workbook;
    private ExcelEncryption _encryption;
    private FileInfo _file;

    public ExcelPackage()
    {
      this.Init();
      this.ConstructNewFile((Stream) new MemoryStream(), (string) null);
    }

    public ExcelPackage(FileInfo newFile)
    {
      this.Init();
      this.File = newFile;
      this.ConstructNewFile((Stream) new MemoryStream(), (string) null);
    }

    public ExcelPackage(FileInfo newFile, string password)
    {
      this.Init();
      this.File = newFile;
      this.ConstructNewFile((Stream) new MemoryStream(), password);
    }

    public ExcelPackage(FileInfo newFile, FileInfo template)
    {
      this.Init();
      this.File = newFile;
      this.CreateFromTemplate(template, (string) null);
    }

    public ExcelPackage(FileInfo newFile, FileInfo template, string password)
    {
      this.Init();
      this.File = newFile;
      this.CreateFromTemplate(template, password);
    }

    public ExcelPackage(FileInfo template, bool useStream)
    {
      this.Init();
      this.CreateFromTemplate(template, (string) null);
      if (useStream)
        return;
      this.File = new FileInfo(Path.GetTempPath() + Guid.NewGuid().ToString() + ".xlsx");
    }

    public ExcelPackage(FileInfo template, bool useStream, string password)
    {
      this.Init();
      this.CreateFromTemplate(template, password);
      if (useStream)
        return;
      this.File = new FileInfo(Path.GetTempPath() + Guid.NewGuid().ToString() + ".xlsx");
    }

    public ExcelPackage(Stream newStream)
    {
      this.Init();
      if (newStream.Length == 0L)
        this.ConstructNewFile(newStream, (string) null);
      else
        this.Load(newStream);
    }

    public ExcelPackage(Stream newStream, string Password)
    {
      if (!newStream.CanRead || !newStream.CanWrite)
        throw new Exception("The stream must be read/write");
      this.Init();
      if (newStream.Length > 0L)
      {
        this.Load(newStream, Password);
      }
      else
      {
        this._stream = newStream;
        this._package = new ZipPackage(this._stream);
        this.CreateBlankWb();
      }
    }

    public ExcelPackage(Stream newStream, Stream templateStream)
    {
      if (newStream.Length > 0L)
        throw new Exception("The output stream must be empty. Length > 0");
      if (!newStream.CanRead || !newStream.CanWrite)
        throw new Exception("The stream must be read/write");
      this.Init();
      this.Load(templateStream, newStream, (string) null);
    }

    public ExcelPackage(Stream newStream, Stream templateStream, string Password)
    {
      if (newStream.Length > 0L)
        throw new Exception("The output stream must be empty. Length > 0");
      if (!newStream.CanRead || !newStream.CanWrite)
        throw new Exception("The stream must be read/write");
      this.Init();
      this.Load(templateStream, newStream, Password);
    }

    internal ExcelPackage.ImageInfo AddImage(byte[] image) => this.AddImage(image, (Uri) null, "");

    internal ExcelPackage.ImageInfo AddImage(byte[] image, Uri uri, string contentType)
    {
      string key = BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(image)).Replace("-", "");
      lock (this._images)
      {
        if (this._images.ContainsKey(key))
        {
          ++this._images[key].RefCount;
        }
        else
        {
          ZipPackagePart part;
          if (uri == (Uri) null)
          {
            uri = this.GetNewUri(this.Package, "/xl/media/image{0}.jpg");
            part = this.Package.CreatePart(uri, "image/jpeg", CompressionLevel.Level0);
          }
          else
            part = this.Package.CreatePart(uri, contentType, CompressionLevel.Level0);
          part.GetStream(FileMode.Create, FileAccess.Write).Write(image, 0, image.GetLength(0));
          this._images.Add(key, new ExcelPackage.ImageInfo()
          {
            Uri = uri,
            RefCount = 1,
            Hash = key,
            Part = part
          });
        }
      }
      return this._images[key];
    }

    internal ExcelPackage.ImageInfo LoadImage(byte[] image, Uri uri, ZipPackagePart imagePart)
    {
      string key = BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(image)).Replace("-", "");
      if (this._images.ContainsKey(key))
        ++this._images[key].RefCount;
      else
        this._images.Add(key, new ExcelPackage.ImageInfo()
        {
          Uri = uri,
          RefCount = 1,
          Hash = key,
          Part = imagePart
        });
      return this._images[key];
    }

    internal void RemoveImage(string hash)
    {
      lock (this._images)
      {
        if (!this._images.ContainsKey(hash))
          return;
        ExcelPackage.ImageInfo image = this._images[hash];
        --image.RefCount;
        if (image.RefCount != 0)
          return;
        this.Package.DeletePart(image.Uri);
        this._images.Remove(hash);
      }
    }

    internal ExcelPackage.ImageInfo GetImageInfo(byte[] image)
    {
      string key = BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(image)).Replace("-", "");
      return this._images.ContainsKey(key) ? this._images[key] : (ExcelPackage.ImageInfo) null;
    }

    private Uri GetNewUri(ZipPackage package, string sUri)
    {
      int num = 1;
      Uri partUri;
      do
      {
        partUri = new Uri(string.Format(sUri, (object) num++), UriKind.Relative);
      }
      while (package.PartExists(partUri));
      return partUri;
    }

    private void Init() => this.DoAdjustDrawings = true;

    private void CreateFromTemplate(FileInfo template, string password)
    {
      template?.Refresh();
      if (!template.Exists)
        throw new Exception("Passed invalid TemplatePath to Excel Template");
      this._stream = (Stream) new MemoryStream();
      if (password != null)
      {
        this.Encryption.IsEncrypted = true;
        this.Encryption.Password = password;
        this._stream = (Stream) new EncryptedPackageHandler().DecryptPackage(template, this.Encryption);
      }
      else
      {
        byte[] buffer = System.IO.File.ReadAllBytes(template.FullName);
        this._stream.Write(buffer, 0, buffer.Length);
      }
      try
      {
        this._package = new ZipPackage(this._stream);
      }
      catch (Exception ex)
      {
        if (password == null && CompoundDocument.IsStorageFile(template.FullName) == 0)
          throw new Exception("Can not open the package. Package is an OLE compound document. If this is an encrypted package, please supply the password", ex);
        throw ex;
      }
    }

    private void ConstructNewFile(Stream stream, string password)
    {
      this._stream = stream;
      if (this.File != null)
        this.File.Refresh();
      if (this.File != null && this.File.Exists)
      {
        if (password != null)
        {
          EncryptedPackageHandler encryptedPackageHandler = new EncryptedPackageHandler();
          this.Encryption.IsEncrypted = true;
          this.Encryption.Password = password;
          this._stream = (Stream) encryptedPackageHandler.DecryptPackage(this.File, this.Encryption);
        }
        else
          this.ReadFile();
        try
        {
          this._package = new ZipPackage(this._stream);
        }
        catch (Exception ex)
        {
          if (password == null && CompoundDocument.IsStorageFile(this.File.FullName) == 0)
            throw new Exception("Can not open the package. Package is an OLE compound document. If this is an encrypted package, please supply the password", ex);
          throw ex;
        }
      }
      else
      {
        this._package = new ZipPackage(this._stream);
        this.CreateBlankWb();
      }
    }

    private void ReadFile()
    {
      byte[] buffer = System.IO.File.ReadAllBytes(this.File.FullName);
      this._stream.Write(buffer, 0, buffer.Length);
    }

    private void CreateBlankWb()
    {
      XmlDocument workbookXml = this.Workbook.WorkbookXml;
      this._package.CreateRelationship(UriHelper.GetRelativeUri(new Uri("/xl", UriKind.Relative), this.Workbook.WorkbookUri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument");
    }

    public ZipPackage Package => this._package;

    public ExcelEncryption Encryption
    {
      get
      {
        if (this._encryption == null)
          this._encryption = new ExcelEncryption();
        return this._encryption;
      }
    }

    public ExcelWorkbook Workbook
    {
      get
      {
        if (this._workbook == null)
        {
          this._workbook = new ExcelWorkbook(this, this.CreateDefaultNSM());
          this._workbook.GetExternalReferences();
          this._workbook.GetDefinedNames();
        }
        return this._workbook;
      }
    }

    public bool DoAdjustDrawings { get; set; }

    private XmlNamespaceManager CreateDefaultNSM()
    {
      XmlNamespaceManager defaultNsm = new XmlNamespaceManager((XmlNameTable) new NameTable());
      defaultNsm.AddNamespace(string.Empty, "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      defaultNsm.AddNamespace("d", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      defaultNsm.AddNamespace("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
      defaultNsm.AddNamespace("c", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      defaultNsm.AddNamespace("vt", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
      defaultNsm.AddNamespace("xp", "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties");
      defaultNsm.AddNamespace("ctp", "http://schemas.openxmlformats.org/officeDocument/2006/custom-properties");
      defaultNsm.AddNamespace("cp", "http://schemas.openxmlformats.org/package/2006/metadata/core-properties");
      defaultNsm.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
      defaultNsm.AddNamespace("dcterms", "http://purl.org/dc/terms/");
      defaultNsm.AddNamespace("dcmitype", "http://purl.org/dc/dcmitype/");
      defaultNsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
      return defaultNsm;
    }

    internal void SavePart(Uri uri, XmlDocument xmlDoc)
    {
      ZipPackagePart part = this._package.GetPart(uri);
      xmlDoc.Save((Stream) part.GetStream(FileMode.Create, FileAccess.Write));
    }

    internal void SaveWorkbook(Uri uri, XmlDocument xmlDoc)
    {
      ZipPackagePart part = this._package.GetPart(uri);
      if (this.Workbook.VbaProject == null)
      {
        if (part.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml")
          part = this._package.CreatePart(uri, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", this.Compression);
      }
      else if (part.ContentType != "application/vnd.ms-excel.sheet.macroEnabled.main+xml")
      {
        ZipPackageRelationshipCollection relationships = part.GetRelationships();
        this._package.DeletePart(uri);
        part = this.Package.CreatePart(uri, "application/vnd.ms-excel.sheet.macroEnabled.main+xml");
        foreach (ZipPackageRelationship packageRelationship in relationships)
        {
          this.Package.DeleteRelationship(packageRelationship.Id);
          part.CreateRelationship(packageRelationship.TargetUri, packageRelationship.TargetMode, packageRelationship.RelationshipType);
        }
      }
      xmlDoc.Save((Stream) part.GetStream(FileMode.Create, FileAccess.Write));
    }

    public void Dispose()
    {
      if (this._package == null)
        return;
      if (this.Stream != null && (this.Stream.CanRead || this.Stream.CanWrite))
        this.Stream.Close();
      this._package.Close();
      this._stream.Dispose();
      this._workbook.Dispose();
      this._package = (ZipPackage) null;
      this._images = (Dictionary<string, ExcelPackage.ImageInfo>) null;
      this._file = (FileInfo) null;
      this._workbook = (ExcelWorkbook) null;
      this._stream = (Stream) null;
      this._workbook = (ExcelWorkbook) null;
    }

    public void Save()
    {
      try
      {
        this.Workbook.Save();
        if (this.File == null)
        {
          this._stream = (Stream) this._package.Save();
          this._package.Close();
        }
        else
        {
          if (System.IO.File.Exists(this.File.FullName))
          {
            try
            {
              System.IO.File.Delete(this.File.FullName);
            }
            catch (Exception ex)
            {
              throw new Exception(string.Format("Error overwriting file {0}", (object) this.File.FullName), ex);
            }
          }
          if (this.Stream is MemoryStream)
          {
            this._package.Close();
            this._stream = (Stream) this._package.Save();
            FileStream fileStream = new FileStream(this.File.FullName, FileMode.Create);
            if (this.Encryption.IsEncrypted)
            {
              MemoryStream memoryStream = new EncryptedPackageHandler().EncryptPackage(((MemoryStream) this.Stream).ToArray(), this.Encryption);
              fileStream.Write(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
            }
            else
              fileStream.Write(((MemoryStream) this.Stream).GetBuffer(), 0, (int) this.Stream.Length);
            fileStream.Close();
          }
          else
            System.IO.File.WriteAllBytes(this.File.FullName, this.GetAsByteArray(false));
        }
      }
      catch (Exception ex)
      {
        if (this.File == null)
          throw ex;
        throw new InvalidOperationException(string.Format("Error saving file {0}", (object) this.File.FullName), ex);
      }
    }

    public void Save(string password)
    {
      this.Encryption.Password = password;
      this.Save();
    }

    public void SaveAs(FileInfo file)
    {
      this.File = file;
      this.Save();
    }

    public void SaveAs(FileInfo file, string password)
    {
      this.File = file;
      this.Encryption.Password = password;
      this.Save();
    }

    public void SaveAs(Stream OutputStream)
    {
      this.File = (FileInfo) null;
      this.Save();
      if (this.Encryption.IsEncrypted)
      {
        byte[] numArray = new byte[this.Stream.Length];
        long position = this.Stream.Position;
        this.Stream.Seek(0L, SeekOrigin.Begin);
        this.Stream.Read(numArray, 0, (int) this.Stream.Length);
        ExcelPackage.CopyStream((Stream) new EncryptedPackageHandler().EncryptPackage(numArray, this.Encryption), ref OutputStream);
      }
      else
        ExcelPackage.CopyStream(this._stream, ref OutputStream);
    }

    public void SaveAs(Stream OutputStream, string password)
    {
      this.Encryption.Password = password;
      this.SaveAs(OutputStream);
    }

    public FileInfo File
    {
      get => this._file;
      set => this._file = value;
    }

    public Stream Stream => this._stream;

    public CompressionLevel Compression
    {
      get => this.Package.Compression;
      set => this.Package.Compression = value;
    }

    internal XmlDocument GetXmlFromUri(Uri uri)
    {
      XmlDocument xmlDoc = new XmlDocument();
      ZipPackagePart part = this._package.GetPart(uri);
      XmlHelper.LoadXmlSafe(xmlDoc, (Stream) part.GetStream());
      return xmlDoc;
    }

    public byte[] GetAsByteArray() => this.GetAsByteArray(true);

    public byte[] GetAsByteArray(string password)
    {
      if (password != null)
        this.Encryption.Password = password;
      return this.GetAsByteArray(true);
    }

    internal byte[] GetAsByteArray(bool save)
    {
      if (save)
      {
        this.Workbook.Save();
        this._package.Close();
        this._stream = (Stream) this._package.Save();
      }
      byte[] asByteArray = new byte[this.Stream.Length];
      long position = this.Stream.Position;
      this.Stream.Seek(0L, SeekOrigin.Begin);
      this.Stream.Read(asByteArray, 0, (int) this.Stream.Length);
      if (this.Encryption.IsEncrypted)
        asByteArray = new EncryptedPackageHandler().EncryptPackage(asByteArray, this.Encryption).ToArray();
      this.Stream.Seek(position, SeekOrigin.Begin);
      this.Stream.Close();
      return asByteArray;
    }

    public void Load(Stream input) => this.Load(input, (Stream) new MemoryStream(), (string) null);

    public void Load(Stream input, string Password)
    {
      this.Load(input, (Stream) new MemoryStream(), Password);
    }

    private void Load(Stream input, Stream output, string Password)
    {
      if (this._package != null)
      {
        this._package.Close();
        this._package = (ZipPackage) null;
      }
      if (this._stream != null)
      {
        this._stream.Close();
        this._stream.Dispose();
        this._stream = (Stream) null;
      }
      if (Password != null)
      {
        Stream outputStream = (Stream) new MemoryStream();
        ExcelPackage.CopyStream(input, ref outputStream);
        EncryptedPackageHandler encryptedPackageHandler = new EncryptedPackageHandler();
        this.Encryption.Password = Password;
        this._stream = (Stream) encryptedPackageHandler.DecryptPackage((MemoryStream) outputStream, this.Encryption);
      }
      else
      {
        this._stream = output;
        ExcelPackage.CopyStream(input, ref this._stream);
      }
      try
      {
        this._package = new ZipPackage(this._stream);
      }
      catch (Exception ex)
      {
        EncryptedPackageHandler encryptedPackageHandler = new EncryptedPackageHandler();
        if (Password == null && CompoundDocument.IsStorageILockBytes(CompoundDocument.GetLockbyte((MemoryStream) this._stream)) == 0)
          throw new Exception("Can not open the package. Package is an OLE compound document. If this is an encrypted package, please supply the password", ex);
        throw ex;
      }
      this._workbook = (ExcelWorkbook) null;
    }

    private static void CopyStream(Stream inputStream, ref Stream outputStream)
    {
      if (!inputStream.CanRead)
        throw new Exception("Can not read from inputstream");
      if (!outputStream.CanWrite)
        throw new Exception("Can not write to outputstream");
      if (inputStream.CanSeek)
        inputStream.Seek(0L, SeekOrigin.Begin);
      int count1 = 8096;
      byte[] buffer = new byte[count1];
      for (int count2 = inputStream.Read(buffer, 0, count1); count2 > 0; count2 = inputStream.Read(buffer, 0, count1))
        outputStream.Write(buffer, 0, count2);
      outputStream.Flush();
    }

    internal class ImageInfo
    {
      internal string Hash { get; set; }

      internal Uri Uri { get; set; }

      internal int RefCount { get; set; }

      internal ZipPackagePart Part { get; set; }
    }
  }
}
