// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Error
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Web.Mvc.Async;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  internal static class Error
  {
    public static InvalidOperationException AsyncActionMethodSelector_CouldNotFindMethod(
      string methodName,
      Type controllerType)
    {
      return new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.AsyncActionMethodSelector_CouldNotFindMethod, new object[2]
      {
        (object) methodName,
        (object) controllerType
      }));
    }

    public static InvalidOperationException AsyncCommon_AsyncResultAlreadyConsumed()
    {
      return new InvalidOperationException(MvcResources.AsyncCommon_AsyncResultAlreadyConsumed);
    }

    public static InvalidOperationException AsyncCommon_ControllerMustImplementIAsyncManagerContainer(
      Type actualControllerType)
    {
      return new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.AsyncCommon_ControllerMustImplementIAsyncManagerContainer, new object[1]
      {
        (object) actualControllerType
      }));
    }

    public static ArgumentException AsyncCommon_InvalidAsyncResult(string parameterName)
    {
      return new ArgumentException(MvcResources.AsyncCommon_InvalidAsyncResult, parameterName);
    }

    public static ArgumentOutOfRangeException AsyncCommon_InvalidTimeout(string parameterName)
    {
      return new ArgumentOutOfRangeException(parameterName, MvcResources.AsyncCommon_InvalidTimeout);
    }

    public static InvalidOperationException ChildActionOnlyAttribute_MustBeInChildRequest(
      ActionDescriptor actionDescriptor)
    {
      return new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ChildActionOnlyAttribute_MustBeInChildRequest, new object[1]
      {
        (object) actionDescriptor.ActionName
      }));
    }

    public static ArgumentException ParameterCannotBeNullOrEmpty(string parameterName)
    {
      return new ArgumentException(MvcResources.Common_NullOrEmpty, parameterName);
    }

    public static InvalidOperationException PropertyCannotBeNullOrEmpty(string propertyName)
    {
      return new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Common_PropertyCannotBeNullOrEmpty, new object[1]
      {
        (object) propertyName
      }));
    }

    public static SynchronousOperationException SynchronizationContextUtil_ExceptionThrown(
      Exception innerException)
    {
      return new SynchronousOperationException(MvcResources.SynchronizationContextUtil_ExceptionThrown, innerException);
    }

    public static InvalidOperationException ViewDataDictionary_WrongTModelType(
      Type valueType,
      Type modelType)
    {
      return new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ViewDataDictionary_WrongTModelType, new object[2]
      {
        (object) valueType,
        (object) modelType
      }));
    }

    public static InvalidOperationException ViewDataDictionary_ModelCannotBeNull(Type modelType)
    {
      return new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ViewDataDictionary_ModelCannotBeNull, new object[1]
      {
        (object) modelType
      }));
    }
  }
}
