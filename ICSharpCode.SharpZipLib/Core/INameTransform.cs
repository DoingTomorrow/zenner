﻿// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Core.INameTransform
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

#nullable disable
namespace ICSharpCode.SharpZipLib.Core
{
  public interface INameTransform
  {
    string TransformFile(string name);

    string TransformDirectory(string name);
  }
}
