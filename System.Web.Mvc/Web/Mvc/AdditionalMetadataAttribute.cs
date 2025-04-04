// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AdditionalMetadataAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
  public sealed class AdditionalMetadataAttribute : Attribute, IMetadataAware
  {
    private object _typeId = new object();

    public AdditionalMetadataAttribute(string name, object value)
    {
      this.Name = name != null ? name : throw new ArgumentNullException(nameof (name));
      this.Value = value;
    }

    public override object TypeId => this._typeId;

    public string Name { get; private set; }

    public object Value { get; private set; }

    public void OnMetadataCreated(ModelMetadata metadata)
    {
      if (metadata == null)
        throw new ArgumentNullException(nameof (metadata));
      metadata.AdditionalValues[this.Name] = this.Value;
    }
  }
}
