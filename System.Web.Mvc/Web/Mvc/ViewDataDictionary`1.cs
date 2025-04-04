// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewDataDictionary`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class ViewDataDictionary<TModel> : ViewDataDictionary
  {
    public ViewDataDictionary()
      : base((object) default (TModel))
    {
    }

    public ViewDataDictionary(TModel model)
      : base((object) model)
    {
    }

    public ViewDataDictionary(ViewDataDictionary viewDataDictionary)
      : base(viewDataDictionary)
    {
    }

    public TModel Model
    {
      get => (TModel) base.Model;
      set => this.SetModel((object) value);
    }

    public override ModelMetadata ModelMetadata
    {
      get
      {
        return base.ModelMetadata ?? (base.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>) null, typeof (TModel)));
      }
      set => base.ModelMetadata = value;
    }

    protected override void SetModel(object value)
    {
      if (!TypeHelpers.IsCompatibleObject<TModel>(value))
        throw value != null ? Error.ViewDataDictionary_WrongTModelType(value.GetType(), typeof (TModel)) : Error.ViewDataDictionary_ModelCannotBeNull(typeof (TModel));
      base.SetModel((object) (TModel) value);
    }
  }
}
