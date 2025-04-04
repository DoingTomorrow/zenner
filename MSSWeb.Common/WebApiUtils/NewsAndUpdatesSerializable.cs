// Decompiled with JetBrains decompiler
// Type: MSSWeb.Common.WebApiUtils.NewsAndUpdatesSerializable
// Assembly: MSSWeb.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3AC43CAD-1714-4F19-BF9F-59E1481A8FBA
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSWeb.Common.dll

using System;
using System.Web.Mvc;

#nullable disable
namespace MSSWeb.Common.WebApiUtils
{
  public class NewsAndUpdatesSerializable
  {
    public int Id { get; set; }

    public string Subject { get; set; }

    [AllowHtml]
    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
  }
}
