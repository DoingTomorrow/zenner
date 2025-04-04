// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Razor.HostingEnvironmentWrapper
// Assembly: System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C1ADC2BA-BB9B-44AC-BD15-1738CDE0D481
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Razor.dll

using System.Web.Hosting;

#nullable disable
namespace System.Web.WebPages.Razor
{
  internal sealed class HostingEnvironmentWrapper : IHostingEnvironment
  {
    public string MapPath(string virtualPath) => HostingEnvironment.MapPath(virtualPath);
  }
}
