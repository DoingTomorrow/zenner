// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ComHelper
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Ionic.Zip
{
  [ClassInterface(ClassInterfaceType.AutoDispatch)]
  [Guid("ebc25cf6-9120-4283-b972-0e5520d0000F")]
  [ComVisible(true)]
  internal class ComHelper
  {
    public bool IsZipFile(string filename) => ZipFile.IsZipFile(filename);

    public bool IsZipFileWithExtract(string filename) => ZipFile.IsZipFile(filename, true);

    public bool CheckZip(string filename) => ZipFile.CheckZip(filename);

    public bool CheckZipPassword(string filename, string password)
    {
      return ZipFile.CheckZipPassword(filename, password);
    }

    public void FixZipDirectory(string filename) => ZipFile.FixZipDirectory(filename);

    public string GetZipLibraryVersion() => ZipFile.LibraryVersion.ToString();
  }
}
