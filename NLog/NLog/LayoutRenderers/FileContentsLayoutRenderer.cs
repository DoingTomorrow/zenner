// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.FileContentsLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("file-contents")]
  public class FileContentsLayoutRenderer : LayoutRenderer
  {
    private string _lastFileName;
    private string _currentFileContents;

    public FileContentsLayoutRenderer()
    {
      this.Encoding = Encoding.Default;
      this._lastFileName = string.Empty;
    }

    [DefaultParameter]
    public Layout FileName { get; set; }

    public Encoding Encoding { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      lock (this)
      {
        string fileName = this.FileName.Render(logEvent);
        if (fileName != this._lastFileName)
        {
          this._currentFileContents = this.ReadFileContents(fileName);
          this._lastFileName = fileName;
        }
      }
      builder.Append(this._currentFileContents);
    }

    private string ReadFileContents(string fileName)
    {
      try
      {
        using (StreamReader streamReader = new StreamReader(fileName, this.Encoding))
          return streamReader.ReadToEnd();
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "Cannot read file contents of '{0}'.", (object) fileName);
        if (!ex.MustBeRethrown())
          return string.Empty;
        throw;
      }
    }
  }
}
