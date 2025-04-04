// Decompiled with JetBrains decompiler
// Type: SQLitePCL.IPlatformStorage
// Assembly: SQLitePCL, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 4D61F17D-4F76-4E73-B63C-94DC04208DE1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.dll

#nullable disable
namespace SQLitePCL
{
  public interface IPlatformStorage
  {
    string GetLocalFilePath(string filename);

    string GetTemporaryDirectoryPath();
  }
}
