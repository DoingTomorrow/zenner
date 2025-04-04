// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ByteArrayModelBinder
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class ByteArrayModelBinder : IModelBinder
  {
    public virtual object BindModel(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      if (bindingContext == null)
        throw new ArgumentNullException(nameof (bindingContext));
      ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
      if (valueProviderResult == null)
        return (object) null;
      string attemptedValue = valueProviderResult.AttemptedValue;
      return string.IsNullOrEmpty(attemptedValue) ? (object) null : (object) Convert.FromBase64String(attemptedValue.Replace("\"", string.Empty));
    }
  }
}
