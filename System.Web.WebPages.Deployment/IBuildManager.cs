// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Deployment.IBuildManager
// Assembly: System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 9BAA0F73-1735-489C-B5F8-24E366CE85FF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Deployment.dll

using System.IO;

#nullable disable
namespace System.Web.WebPages.Deployment
{
  internal interface IBuildManager
  {
    Stream CreateCachedFile(string fileName);

    Stream ReadCachedFile(string fileName);
  }
}
