// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ReadOptions
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Ionic.Zip
{
  internal class ReadOptions
  {
    public EventHandler<ReadProgressEventArgs> ReadProgress { get; set; }

    public TextWriter StatusMessageWriter { get; set; }

    public Encoding Encoding { get; set; }
  }
}
