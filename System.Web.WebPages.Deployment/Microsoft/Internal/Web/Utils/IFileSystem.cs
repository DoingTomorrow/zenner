// Decompiled with JetBrains decompiler
// Type: Microsoft.Internal.Web.Utils.IFileSystem
// Assembly: System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 9BAA0F73-1735-489C-B5F8-24E366CE85FF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Deployment.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Microsoft.Internal.Web.Utils
{
  internal interface IFileSystem
  {
    bool FileExists(string path);

    Stream ReadFile(string path);

    Stream OpenFile(string path);

    IEnumerable<string> EnumerateFiles(string root);
  }
}
