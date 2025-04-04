// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.HttpFileCollectionValueProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public sealed class HttpFileCollectionValueProvider(ControllerContext controllerContext) : 
    DictionaryValueProvider<HttpPostedFileBase[]>((IDictionary<string, HttpPostedFileBase[]>) HttpFileCollectionValueProvider.GetHttpPostedFileDictionary(controllerContext), CultureInfo.InvariantCulture)
  {
    private static readonly Dictionary<string, HttpPostedFileBase[]> _emptyDictionary = new Dictionary<string, HttpPostedFileBase[]>();

    private static Dictionary<string, HttpPostedFileBase[]> GetHttpPostedFileDictionary(
      ControllerContext controllerContext)
    {
      HttpFileCollectionBase files = controllerContext.HttpContext.Request.Files;
      if (files.Count == 0)
        return HttpFileCollectionValueProvider._emptyDictionary;
      List<KeyValuePair<string, HttpPostedFileBase>> source = new List<KeyValuePair<string, HttpPostedFileBase>>();
      string[] allKeys = files.AllKeys;
      for (int index = 0; index < files.Count; ++index)
      {
        string key = allKeys[index];
        if (key != null)
        {
          HttpPostedFileBase httpPostedFileBase = HttpPostedFileBaseModelBinder.ChooseFileOrNull(files[index]);
          source.Add(new KeyValuePair<string, HttpPostedFileBase>(key, httpPostedFileBase));
        }
      }
      return source.GroupBy<KeyValuePair<string, HttpPostedFileBase>, string, HttpPostedFileBase>((Func<KeyValuePair<string, HttpPostedFileBase>, string>) (el => el.Key), (Func<KeyValuePair<string, HttpPostedFileBase>, HttpPostedFileBase>) (el => el.Value), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase).ToDictionary<IGrouping<string, HttpPostedFileBase>, string, HttpPostedFileBase[]>((Func<IGrouping<string, HttpPostedFileBase>, string>) (g => g.Key), (Func<IGrouping<string, HttpPostedFileBase>, HttpPostedFileBase[]>) (g => g.ToArray<HttpPostedFileBase>()), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }
  }
}
