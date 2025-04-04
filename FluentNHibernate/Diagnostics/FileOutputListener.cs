// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Diagnostics.FileOutputListener
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.IO;

#nullable disable
namespace FluentNHibernate.Diagnostics
{
  public class FileOutputListener : IDiagnosticListener
  {
    private readonly IDiagnosticResultsFormatter formatter;
    private readonly string outputPath;

    public FileOutputListener(string outputPath)
      : this((IDiagnosticResultsFormatter) new DefaultOutputFormatter(), outputPath)
    {
    }

    public FileOutputListener(IDiagnosticResultsFormatter formatter, string outputPath)
    {
      this.formatter = formatter;
      this.outputPath = outputPath;
    }

    public void Receive(DiagnosticResults results)
    {
      string contents = this.formatter.Format(results);
      string directoryName = Path.GetDirectoryName(this.outputPath);
      if (string.IsNullOrEmpty(directoryName))
        throw new ArgumentException("Output directory is invalid", "outputPath");
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      File.WriteAllText(this.outputPath, contents);
    }
  }
}
