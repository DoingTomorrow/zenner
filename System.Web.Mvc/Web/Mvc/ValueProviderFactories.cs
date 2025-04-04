// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ValueProviderFactories
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public static class ValueProviderFactories
  {
    private static readonly ValueProviderFactoryCollection _factories;

    public static ValueProviderFactoryCollection Factories => ValueProviderFactories._factories;

    static ValueProviderFactories()
    {
      ValueProviderFactoryCollection factoryCollection = new ValueProviderFactoryCollection();
      factoryCollection.Add((ValueProviderFactory) new ChildActionValueProviderFactory());
      factoryCollection.Add((ValueProviderFactory) new FormValueProviderFactory());
      factoryCollection.Add((ValueProviderFactory) new JsonValueProviderFactory());
      factoryCollection.Add((ValueProviderFactory) new RouteDataValueProviderFactory());
      factoryCollection.Add((ValueProviderFactory) new QueryStringValueProviderFactory());
      factoryCollection.Add((ValueProviderFactory) new HttpFileCollectionValueProviderFactory());
      ValueProviderFactories._factories = factoryCollection;
    }
  }
}
