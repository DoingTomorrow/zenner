// Decompiled with JetBrains decompiler
// Type: Excel.Core.ZipWorker
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using Excel.Log;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

#nullable disable
namespace Excel.Core
{
  public class ZipWorker : IDisposable
  {
    private const string TMP = "TMP_Z";
    private const string FOLDER_xl = "xl";
    private const string FOLDER_worksheets = "worksheets";
    private const string FILE_sharedStrings = "sharedStrings.{0}";
    private const string FILE_styles = "styles.{0}";
    private const string FILE_workbook = "workbook.{0}";
    private const string FILE_sheet = "sheet{0}.{1}";
    private const string FOLDER_rels = "_rels";
    private const string FILE_rels = "workbook.{0}.rels";
    private byte[] buffer;
    private bool disposed;
    private bool _isCleaned;
    private string _tempPath;
    private string _tempEnv;
    private string _exceptionMessage;
    private string _xlPath;
    private string _format = "xml";
    private bool _isValid;

    public bool IsValid => this._isValid;

    public string TempPath => this._tempPath;

    public string ExceptionMessage => this._exceptionMessage;

    public ZipWorker() => this._tempEnv = Path.GetTempPath();

    public bool Extract(Stream fileStream)
    {
      // ISSUE: unable to decompile the method.
    }

    public Stream GetSharedStringsStream()
    {
      return ZipWorker.GetStream(Path.Combine(this._xlPath, string.Format("sharedStrings.{0}", (object) this._format)));
    }

    public Stream GetStylesStream()
    {
      return ZipWorker.GetStream(Path.Combine(this._xlPath, string.Format("styles.{0}", (object) this._format)));
    }

    public Stream GetWorkbookStream()
    {
      return ZipWorker.GetStream(Path.Combine(this._xlPath, string.Format("workbook.{0}", (object) this._format)));
    }

    public Stream GetWorksheetStream(int sheetId)
    {
      return ZipWorker.GetStream(Path.Combine(Path.Combine(this._xlPath, "worksheets"), string.Format("sheet{0}.{1}", (object) sheetId, (object) this._format)));
    }

    public Stream GetWorksheetStream(string sheetPath)
    {
      if (sheetPath.StartsWith("/xl/"))
        sheetPath = sheetPath.Substring(4);
      return ZipWorker.GetStream(Path.Combine(this._xlPath, sheetPath));
    }

    public Stream GetWorkbookRelsStream()
    {
      return ZipWorker.GetStream(Path.Combine(this._xlPath, Path.Combine("_rels", string.Format("workbook.{0}.rels", (object) this._format))));
    }

    private void CleanFromTemp(bool catchIoError)
    {
      if (string.IsNullOrEmpty(this._tempPath))
        return;
      this._isCleaned = true;
      try
      {
        if (!Directory.Exists(this._tempPath))
          return;
        Directory.Delete(this._tempPath, true);
      }
      catch (IOException ex)
      {
        if (catchIoError)
          return;
        throw;
      }
    }

    private void ExtractZipEntry(ZipFile zipFile, ZipEntry entry)
    {
      if (!entry.IsCompressionMethodSupported() || string.IsNullOrEmpty(entry.Name))
        return;
      string path1 = Path.Combine(this._tempPath, entry.Name);
      string path2 = entry.IsDirectory ? path1 : Path.GetDirectoryName(Path.GetFullPath(path1));
      if (!Directory.Exists(path2))
        Directory.CreateDirectory(path2);
      if (!entry.IsFile)
        return;
      using (FileStream fileStream = File.Create(path1))
      {
        if (this.buffer == null)
          this.buffer = new byte[4096];
        using (Stream inputStream = zipFile.GetInputStream(entry))
        {
          int count;
          while ((count = inputStream.Read(this.buffer, 0, this.buffer.Length)) > 0)
            fileStream.Write(this.buffer, 0, count);
        }
        fileStream.Flush();
      }
    }

    private void NewTempPath()
    {
      string str = Guid.NewGuid().ToString("N");
      this._tempPath = Path.Combine(this._tempEnv, "TMP_Z" + DateTime.Now.ToFileTimeUtc().ToString() + str);
      this._isCleaned = false;
      LogManager.Log<ZipWorker>(this).Debug("Using temp path {0}", (object) this._tempPath);
      Directory.CreateDirectory(this._tempPath);
    }

    private bool CheckFolderTree()
    {
      this._xlPath = Path.Combine(this._tempPath, "xl");
      return Directory.Exists(this._xlPath) && Directory.Exists(Path.Combine(this._xlPath, "worksheets")) && File.Exists(Path.Combine(this._xlPath, "workbook.{0}")) && File.Exists(Path.Combine(this._xlPath, "styles.{0}"));
    }

    private static Stream GetStream(string filePath)
    {
      return File.Exists(filePath) ? (Stream) File.Open(filePath, FileMode.Open, FileAccess.Read) : (Stream) null;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing && !this._isCleaned)
        this.CleanFromTemp(false);
      this.buffer = (byte[]) null;
      this.disposed = true;
    }

    ~ZipWorker() => this.Dispose(false);
  }
}
