// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelValidatorProviders
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public static class ModelValidatorProviders
  {
    private static readonly ModelValidatorProviderCollection _providers;

    public static ModelValidatorProviderCollection Providers => ModelValidatorProviders._providers;

    static ModelValidatorProviders()
    {
      ModelValidatorProviderCollection providerCollection = new ModelValidatorProviderCollection();
      providerCollection.Add((ModelValidatorProvider) new DataAnnotationsModelValidatorProvider());
      providerCollection.Add((ModelValidatorProvider) new DataErrorInfoModelValidatorProvider());
      providerCollection.Add((ModelValidatorProvider) new ClientDataTypeModelValidatorProvider());
      ModelValidatorProviders._providers = providerCollection;
    }
  }
}
