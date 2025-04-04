// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.CompareAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Property)]
  public class CompareAttribute : ValidationAttribute, IClientValidatable
  {
    public CompareAttribute(string otherProperty)
      : base(MvcResources.CompareAttribute_MustMatch)
    {
      this.OtherProperty = otherProperty != null ? otherProperty : throw new ArgumentNullException(nameof (otherProperty));
    }

    public string OtherProperty { get; private set; }

    public string OtherPropertyDisplayName { get; internal set; }

    public override string FormatErrorMessage(string name)
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, this.ErrorMessageString, new object[2]
      {
        (object) name,
        (object) (this.OtherPropertyDisplayName ?? this.OtherProperty)
      });
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      PropertyInfo property = validationContext.ObjectType.GetProperty(this.OtherProperty);
      if (property == (PropertyInfo) null)
        return new ValidationResult(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.CompareAttribute_UnknownProperty, new object[1]
        {
          (object) this.OtherProperty
        }));
      object objB = property.GetValue(validationContext.ObjectInstance, (object[]) null);
      if (object.Equals(value, objB))
        return (ValidationResult) null;
      if (this.OtherPropertyDisplayName == null)
        this.OtherPropertyDisplayName = ModelMetadataProviders.Current.GetMetadataForProperty((Func<object>) (() => validationContext.ObjectInstance), validationContext.ObjectType, this.OtherProperty).GetDisplayName();
      return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
    }

    public static string FormatPropertyForClientValidation(string property)
    {
      if (property == null)
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (property));
      return "*." + property;
    }

    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
      ModelMetadata metadata,
      ControllerContext context)
    {
      if (metadata.ContainerType != (Type) null && this.OtherPropertyDisplayName == null)
        this.OtherPropertyDisplayName = ModelMetadataProviders.Current.GetMetadataForProperty((Func<object>) (() => metadata.Model), metadata.ContainerType, this.OtherProperty).GetDisplayName();
      yield return (ModelClientValidationRule) new ModelClientValidationEqualToRule(this.FormatErrorMessage(metadata.GetDisplayName()), (object) CompareAttribute.FormatPropertyForClientValidation(this.OtherProperty));
    }
  }
}
