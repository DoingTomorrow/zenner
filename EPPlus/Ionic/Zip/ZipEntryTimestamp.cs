// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipEntryTimestamp
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace Ionic.Zip
{
  [Flags]
  internal enum ZipEntryTimestamp
  {
    None = 0,
    DOS = 1,
    Windows = 2,
    Unix = 4,
    InfoZip1 = 8,
  }
}
