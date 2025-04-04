// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelBinderAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
  public sealed class ModelBinderAttribute : CustomModelBinderAttribute
  {
    public ModelBinderAttribute(Type binderType)
    {
      if (binderType == (Type) null)
        throw new ArgumentNullException(nameof (binderType));
      this.BinderType = typeof (IModelBinder).IsAssignableFrom(binderType) ? binderType : throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ModelBinderAttribute_TypeNotIModelBinder, new object[1]
      {
        (object) binderType.FullName
      }), nameof (binderType));
    }

    public Type BinderType { get; private set; }

    public override IModelBinder GetBinder()
    {
      try
      {
        return (IModelBinder) Activator.CreateInstance(this.BinderType);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ModelBinderAttribute_ErrorCreatingModelBinder, new object[1]
        {
          (object) this.BinderType.FullName
        }), ex);
      }
    }
  }
}
