// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ValidationAttributeHelpers
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Security;

#nullable disable
namespace System.Web.Mvc
{
  internal static class ValidationAttributeHelpers
  {
    private static Assembly _systemWebAssembly = typeof (Membership).Assembly;
    private static Assembly _systemComponentModelDataAnnotationsAssembly = typeof (DataType).Assembly;
    public static readonly Type MembershipPasswordAttributeType = ValidationAttributeHelpers.FindType(ValidationAttributeHelpers._systemWebAssembly, "System.Web.Security.MembershipPasswordAttribute");
    public static readonly Type CompareAttributeType = ValidationAttributeHelpers.FindType(ValidationAttributeHelpers._systemComponentModelDataAnnotationsAssembly, "System.ComponentModel.DataAnnotations.CompareAttribute");
    public static readonly Type CreditCardAttributeType = ValidationAttributeHelpers.FindType(ValidationAttributeHelpers._systemComponentModelDataAnnotationsAssembly, "System.ComponentModel.DataAnnotations.CreditCardAttribute");
    public static readonly Type EmailAddressAttributeType = ValidationAttributeHelpers.FindType(ValidationAttributeHelpers._systemComponentModelDataAnnotationsAssembly, "System.ComponentModel.DataAnnotations.EmailAddressAttribute");
    public static readonly Type FileExtensionsAttributeType = ValidationAttributeHelpers.FindType(ValidationAttributeHelpers._systemComponentModelDataAnnotationsAssembly, "System.ComponentModel.DataAnnotations.FileExtensionsAttribute");
    public static readonly Type PhoneAttributeType = ValidationAttributeHelpers.FindType(ValidationAttributeHelpers._systemComponentModelDataAnnotationsAssembly, "System.ComponentModel.DataAnnotations.PhoneAttribute");
    public static readonly Type UrlAttributeType = ValidationAttributeHelpers.FindType(ValidationAttributeHelpers._systemComponentModelDataAnnotationsAssembly, "System.ComponentModel.DataAnnotations.UrlAttribute");

    public static Func<ValidationAttribute, TProperty> GetPropertyDelegate<TProperty>(
      Type inputType,
      string propertyName)
    {
      if (inputType == (Type) null)
        return (Func<ValidationAttribute, TProperty>) null;
      ParameterExpression parameterExpression;
      return Expression.Lambda<Func<ValidationAttribute, TProperty>>((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression, inputType), propertyName), parameterExpression).Compile();
    }

    private static Type FindType(Assembly assembly, string typeName)
    {
      try
      {
        return assembly.GetType(typeName, false);
      }
      catch
      {
        return (Type) null;
      }
    }
  }
}
