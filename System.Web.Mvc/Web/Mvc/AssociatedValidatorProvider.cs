// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AssociatedValidatorProvider
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
  public abstract class AssociatedValidatorProvider : ModelValidatorProvider
  {
    protected virtual ICustomTypeDescriptor GetTypeDescriptor(Type type)
    {
      return TypeDescriptorHelper.Get(type);
    }

    public override sealed IEnumerable<ModelValidator> GetValidators(
      ModelMetadata metadata,
      ControllerContext context)
    {
      if (metadata == null)
        throw new ArgumentNullException(nameof (metadata));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      return metadata.ContainerType != (Type) null && !string.IsNullOrEmpty(metadata.PropertyName) ? this.GetValidatorsForProperty(metadata, context) : this.GetValidatorsForType(metadata, context);
    }

    protected abstract IEnumerable<ModelValidator> GetValidators(
      ModelMetadata metadata,
      ControllerContext context,
      IEnumerable<Attribute> attributes);

    private IEnumerable<ModelValidator> GetValidatorsForProperty(
      ModelMetadata metadata,
      ControllerContext context)
    {
      return this.GetValidators(metadata, context, (this.GetTypeDescriptor(metadata.ContainerType).GetProperties().Find(metadata.PropertyName, true) ?? throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Common_PropertyNotFound, new object[2]
      {
        (object) metadata.ContainerType.FullName,
        (object) metadata.PropertyName
      }), nameof (metadata))).Attributes.OfType<Attribute>());
    }

    private IEnumerable<ModelValidator> GetValidatorsForType(
      ModelMetadata metadata,
      ControllerContext context)
    {
      return this.GetValidators(metadata, context, this.GetTypeDescriptor(metadata.ModelType).GetAttributes().Cast<Attribute>());
    }
  }
}
