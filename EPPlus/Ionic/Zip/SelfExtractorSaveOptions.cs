// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.SelfExtractorSaveOptions
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace Ionic.Zip
{
  internal class SelfExtractorSaveOptions
  {
    public SelfExtractorFlavor Flavor { get; set; }

    public string PostExtractCommandLine { get; set; }

    public string DefaultExtractDirectory { get; set; }

    public string IconFile { get; set; }

    public bool Quiet { get; set; }

    public ExtractExistingFileAction ExtractExistingFile { get; set; }

    public bool RemoveUnpackedFilesAfterExecute { get; set; }

    public Version FileVersion { get; set; }

    public string ProductVersion { get; set; }

    public string Copyright { get; set; }

    public string Description { get; set; }

    public string ProductName { get; set; }

    public string SfxExeWindowTitle { get; set; }

    public string AdditionalCompilerSwitches { get; set; }
  }
}
