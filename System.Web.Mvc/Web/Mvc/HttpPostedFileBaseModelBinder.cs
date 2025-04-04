// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.HttpPostedFileBaseModelBinder
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class HttpPostedFileBaseModelBinder : IModelBinder
  {
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (bindingContext == null)
        throw new ArgumentNullException(nameof (bindingContext));
      return (object) HttpPostedFileBaseModelBinder.ChooseFileOrNull(controllerContext.HttpContext.Request.Files[bindingContext.ModelName]);
    }

    internal static HttpPostedFileBase ChooseFileOrNull(HttpPostedFileBase rawFile)
    {
      if (rawFile == null)
        return (HttpPostedFileBase) null;
      return rawFile.ContentLength == 0 && string.IsNullOrEmpty(rawFile.FileName) ? (HttpPostedFileBase) null : rawFile;
    }
  }
}
