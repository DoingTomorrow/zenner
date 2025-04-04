// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.MembershipPasswordAttributeAdapter
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace System.Web.Mvc
{
  internal class MembershipPasswordAttributeAdapter(
    ModelMetadata metadata,
    ControllerContext context,
    ValidationAttribute attribute) : DataAnnotationsModelValidator(metadata, context, attribute)
  {
    private static Lazy<Func<ValidationAttribute, int>> minRequiredNonAlphanumericCharacters = MembershipPasswordAttributeAdapter.GetLazyPropertyDelegate<int>("MinRequiredNonAlphanumericCharacters");
    private static Lazy<Func<ValidationAttribute, int>> minRequiredPasswordLength = MembershipPasswordAttributeAdapter.GetLazyPropertyDelegate<int>("MinRequiredPasswordLength");
    private static Lazy<Func<ValidationAttribute, string>> passwordStrengthRegularExpression = MembershipPasswordAttributeAdapter.GetLazyPropertyDelegate<string>("PasswordStrengthRegularExpression");

    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
    {
      yield return (ModelClientValidationRule) new ModelClientValidationMembershipPasswordRule(this.ErrorMessage, MembershipPasswordAttributeAdapter.minRequiredPasswordLength.Value(this.Attribute), MembershipPasswordAttributeAdapter.minRequiredNonAlphanumericCharacters.Value(this.Attribute), MembershipPasswordAttributeAdapter.passwordStrengthRegularExpression.Value(this.Attribute));
    }

    private static Lazy<Func<ValidationAttribute, TProperty>> GetLazyPropertyDelegate<TProperty>(
      string propertyName)
    {
      return new Lazy<Func<ValidationAttribute, TProperty>>((Func<Func<ValidationAttribute, TProperty>>) (() => ValidationAttributeHelpers.GetPropertyDelegate<TProperty>(ValidationAttributeHelpers.MembershipPasswordAttributeType, propertyName)));
    }
  }
}
