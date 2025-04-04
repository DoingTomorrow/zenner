// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AssociatedMetadataProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class AssociatedMetadataProvider : ModelMetadataProvider
  {
    private static void ApplyMetadataAwareAttributes(
      IEnumerable<Attribute> attributes,
      ModelMetadata result)
    {
      foreach (IMetadataAware metadataAware in attributes.OfType<IMetadataAware>())
        metadataAware.OnMetadataCreated(result);
    }

    protected abstract ModelMetadata CreateMetadata(
      IEnumerable<Attribute> attributes,
      Type containerType,
      Func<object> modelAccessor,
      Type modelType,
      string propertyName);

    protected virtual IEnumerable<Attribute> FilterAttributes(
      Type containerType,
      PropertyDescriptor propertyDescriptor,
      IEnumerable<Attribute> attributes)
    {
      return typeof (ViewPage).IsAssignableFrom(containerType) || typeof (ViewUserControl).IsAssignableFrom(containerType) ? attributes.Where<Attribute>((Func<Attribute, bool>) (a => !(a is ReadOnlyAttribute))) : attributes;
    }

    public override IEnumerable<ModelMetadata> GetMetadataForProperties(
      object container,
      Type containerType)
    {
      return !(containerType == (Type) null) ? this.GetMetadataForPropertiesImpl(container, containerType) : throw new ArgumentNullException(nameof (containerType));
    }

    private IEnumerable<ModelMetadata> GetMetadataForPropertiesImpl(
      object container,
      Type containerType)
    {
      foreach (PropertyDescriptor property in this.GetTypeDescriptor(containerType).GetProperties())
      {
        Func<object> modelAccessor = container == null ? (Func<object>) null : AssociatedMetadataProvider.GetPropertyValueAccessor(container, property);
        yield return this.GetMetadataForProperty(modelAccessor, containerType, property);
      }
    }

    public override ModelMetadata GetMetadataForProperty(
      Func<object> modelAccessor,
      Type containerType,
      string propertyName)
    {
      if (containerType == (Type) null)
        throw new ArgumentNullException(nameof (containerType));
      return this.GetMetadataForProperty(modelAccessor, containerType, (!string.IsNullOrEmpty(propertyName) ? this.GetTypeDescriptor(containerType).GetProperties().Find(propertyName, true) : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (propertyName))) ?? throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Common_PropertyNotFound, new object[2]
      {
        (object) containerType.FullName,
        (object) propertyName
      })));
    }

    protected virtual ModelMetadata GetMetadataForProperty(
      Func<object> modelAccessor,
      Type containerType,
      PropertyDescriptor propertyDescriptor)
    {
      IEnumerable<Attribute> attributes = this.FilterAttributes(containerType, propertyDescriptor, propertyDescriptor.Attributes.Cast<Attribute>());
      ModelMetadata metadata = this.CreateMetadata(attributes, containerType, modelAccessor, propertyDescriptor.PropertyType, propertyDescriptor.Name);
      AssociatedMetadataProvider.ApplyMetadataAwareAttributes(attributes, metadata);
      return metadata;
    }

    public override ModelMetadata GetMetadataForType(Func<object> modelAccessor, Type modelType)
    {
      IEnumerable<Attribute> attributes = !(modelType == (Type) null) ? this.GetTypeDescriptor(modelType).GetAttributes().Cast<Attribute>() : throw new ArgumentNullException(nameof (modelType));
      ModelMetadata metadata = this.CreateMetadata(attributes, (Type) null, modelAccessor, modelType, (string) null);
      AssociatedMetadataProvider.ApplyMetadataAwareAttributes(attributes, metadata);
      return metadata;
    }

    private static Func<object> GetPropertyValueAccessor(
      object container,
      PropertyDescriptor property)
    {
      return (Func<object>) (() => property.GetValue(container));
    }

    protected virtual ICustomTypeDescriptor GetTypeDescriptor(Type type)
    {
      return TypeDescriptorHelper.Get(type);
    }
  }
}
