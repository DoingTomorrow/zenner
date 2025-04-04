// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelBinders
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Data.Linq;
using System.Reflection;
using System.Threading;

#nullable disable
namespace System.Web.Mvc
{
  public static class ModelBinders
  {
    private static readonly ModelBinderDictionary _binders = ModelBinders.CreateDefaultBinderDictionary();

    public static ModelBinderDictionary Binders => ModelBinders._binders;

    internal static IModelBinder GetBinderFromAttributes(
      Type type,
      Func<string> errorMessageAccessor)
    {
      return ModelBinders.GetBinderFromAttributesImpl(TypeDescriptorHelper.Get(type).GetAttributes().OfType<CustomModelBinderAttribute>().ToArray<CustomModelBinderAttribute>(), errorMessageAccessor);
    }

    internal static IModelBinder GetBinderFromAttributes(
      ICustomAttributeProvider element,
      Func<string> errorMessageAccessor)
    {
      return ModelBinders.GetBinderFromAttributesImpl((CustomModelBinderAttribute[]) element.GetCustomAttributes(typeof (CustomModelBinderAttribute), true), errorMessageAccessor);
    }

    private static IModelBinder GetBinderFromAttributesImpl(
      CustomModelBinderAttribute[] attrs,
      Func<string> errorMessageAccessor)
    {
      if (attrs == null)
        return (IModelBinder) null;
      switch (attrs.Length)
      {
        case 0:
          return (IModelBinder) null;
        case 1:
          return attrs[0].GetBinder();
        default:
          throw new InvalidOperationException(errorMessageAccessor());
      }
    }

    private static ModelBinderDictionary CreateDefaultBinderDictionary()
    {
      return new ModelBinderDictionary()
      {
        {
          typeof (HttpPostedFileBase),
          (IModelBinder) new HttpPostedFileBaseModelBinder()
        },
        {
          typeof (byte[]),
          (IModelBinder) new ByteArrayModelBinder()
        },
        {
          typeof (Binary),
          (IModelBinder) new LinqBinaryModelBinder()
        },
        {
          typeof (CancellationToken),
          (IModelBinder) new CancellationTokenModelBinder()
        }
      };
    }
  }
}
