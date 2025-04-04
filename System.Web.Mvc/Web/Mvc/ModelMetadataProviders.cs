// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelMetadataProviders
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class ModelMetadataProviders
  {
    private static ModelMetadataProviders _instance = new ModelMetadataProviders();
    private ModelMetadataProvider _currentProvider;
    private IResolver<ModelMetadataProvider> _resolver;

    internal ModelMetadataProviders(IResolver<ModelMetadataProvider> resolver = null)
    {
      this._resolver = resolver ?? (IResolver<ModelMetadataProvider>) new SingleServiceResolver<ModelMetadataProvider>((Func<ModelMetadataProvider>) (() => this._currentProvider), (ModelMetadataProvider) new CachedDataAnnotationsModelMetadataProvider(), "ModelMetadataProviders.Current");
    }

    public static ModelMetadataProvider Current
    {
      get => ModelMetadataProviders._instance.CurrentInternal;
      set => ModelMetadataProviders._instance.CurrentInternal = value;
    }

    internal ModelMetadataProvider CurrentInternal
    {
      get => this._resolver.Current;
      set
      {
        this._currentProvider = value ?? (ModelMetadataProvider) new EmptyModelMetadataProvider();
      }
    }
  }
}
