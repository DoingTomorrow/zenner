// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelClientValidationMembershipPasswordRule
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.Mvc
{
  internal class ModelClientValidationMembershipPasswordRule : ModelClientValidationRule
  {
    public ModelClientValidationMembershipPasswordRule(
      string errorMessage,
      int minRequiredPasswordLength,
      int minRequiredNonAlphanumericCharacters,
      string passwordStrengthRegularExpression)
    {
      this.ErrorMessage = errorMessage;
      this.ValidationType = "password";
      if (minRequiredPasswordLength != 0)
        this.ValidationParameters["min"] = (object) minRequiredPasswordLength;
      if (minRequiredNonAlphanumericCharacters != 0)
        this.ValidationParameters["nonalphamin"] = (object) minRequiredNonAlphanumericCharacters;
      if (string.IsNullOrEmpty(passwordStrengthRegularExpression))
        return;
      this.ValidationParameters["regex"] = (object) passwordStrengthRegularExpression;
    }
  }
}
