// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DataAnnotationsModelValidatorProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class DataAnnotationsModelValidatorProvider : AssociatedValidatorProvider
  {
    private static bool _addImplicitRequiredAttributeForValueTypes = true;
    private static ReaderWriterLockSlim _adaptersLock = new ReaderWriterLockSlim();
    internal static DataAnnotationsModelValidationFactory DefaultAttributeFactory = (DataAnnotationsModelValidationFactory) ((metadata, context, attribute) => (ModelValidator) new DataAnnotationsModelValidator(metadata, context, attribute));
    internal static Dictionary<Type, DataAnnotationsModelValidationFactory> AttributeFactories = DataAnnotationsModelValidatorProvider.BuildAttributeFactoriesDictionary();
    internal static DataAnnotationsValidatableObjectAdapterFactory DefaultValidatableFactory = (DataAnnotationsValidatableObjectAdapterFactory) ((metadata, context) => (ModelValidator) new ValidatableObjectAdapter(metadata, context));
    internal static Dictionary<Type, DataAnnotationsValidatableObjectAdapterFactory> ValidatableFactories = new Dictionary<Type, DataAnnotationsValidatableObjectAdapterFactory>();

    public static bool AddImplicitRequiredAttributeForValueTypes
    {
      get => DataAnnotationsModelValidatorProvider._addImplicitRequiredAttributeForValueTypes;
      set
      {
        DataAnnotationsModelValidatorProvider._addImplicitRequiredAttributeForValueTypes = value;
      }
    }

    protected override IEnumerable<ModelValidator> GetValidators(
      ModelMetadata metadata,
      ControllerContext context,
      IEnumerable<Attribute> attributes)
    {
      DataAnnotationsModelValidatorProvider._adaptersLock.EnterReadLock();
      try
      {
        List<ModelValidator> validators = new List<ModelValidator>();
        if (DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes && metadata.IsRequired && !attributes.Any<Attribute>((Func<Attribute, bool>) (a => a is RequiredAttribute)))
          attributes = attributes.Concat<Attribute>((IEnumerable<Attribute>) new RequiredAttribute[1]
          {
            new RequiredAttribute()
          });
        foreach (ValidationAttribute attribute in attributes.OfType<ValidationAttribute>())
        {
          DataAnnotationsModelValidationFactory attributeFactory;
          if (!DataAnnotationsModelValidatorProvider.AttributeFactories.TryGetValue(attribute.GetType(), out attributeFactory))
            attributeFactory = DataAnnotationsModelValidatorProvider.DefaultAttributeFactory;
          validators.Add(attributeFactory(metadata, context, attribute));
        }
        if (typeof (IValidatableObject).IsAssignableFrom(metadata.ModelType))
        {
          DataAnnotationsValidatableObjectAdapterFactory validatableFactory;
          if (!DataAnnotationsModelValidatorProvider.ValidatableFactories.TryGetValue(metadata.ModelType, out validatableFactory))
            validatableFactory = DataAnnotationsModelValidatorProvider.DefaultValidatableFactory;
          validators.Add(validatableFactory(metadata, context));
        }
        return (IEnumerable<ModelValidator>) validators;
      }
      finally
      {
        DataAnnotationsModelValidatorProvider._adaptersLock.ExitReadLock();
      }
    }

    public static void RegisterAdapter(Type attributeType, Type adapterType)
    {
      DataAnnotationsModelValidatorProvider.ValidateAttributeType(attributeType);
      DataAnnotationsModelValidatorProvider.ValidateAttributeAdapterType(adapterType);
      ConstructorInfo constructor = DataAnnotationsModelValidatorProvider.GetAttributeAdapterConstructor(attributeType, adapterType);
      DataAnnotationsModelValidatorProvider._adaptersLock.EnterWriteLock();
      try
      {
        DataAnnotationsModelValidatorProvider.AttributeFactories[attributeType] = (DataAnnotationsModelValidationFactory) ((metadata, context, attribute) => (ModelValidator) constructor.Invoke(new object[3]
        {
          (object) metadata,
          (object) context,
          (object) attribute
        }));
      }
      finally
      {
        DataAnnotationsModelValidatorProvider._adaptersLock.ExitWriteLock();
      }
    }

    public static void RegisterAdapterFactory(
      Type attributeType,
      DataAnnotationsModelValidationFactory factory)
    {
      DataAnnotationsModelValidatorProvider.ValidateAttributeType(attributeType);
      DataAnnotationsModelValidatorProvider.ValidateAttributeFactory(factory);
      DataAnnotationsModelValidatorProvider._adaptersLock.EnterWriteLock();
      try
      {
        DataAnnotationsModelValidatorProvider.AttributeFactories[attributeType] = factory;
      }
      finally
      {
        DataAnnotationsModelValidatorProvider._adaptersLock.ExitWriteLock();
      }
    }

    public static void RegisterDefaultAdapter(Type adapterType)
    {
      DataAnnotationsModelValidatorProvider.ValidateAttributeAdapterType(adapterType);
      ConstructorInfo constructor = DataAnnotationsModelValidatorProvider.GetAttributeAdapterConstructor(typeof (ValidationAttribute), adapterType);
      DataAnnotationsModelValidatorProvider.DefaultAttributeFactory = (DataAnnotationsModelValidationFactory) ((metadata, context, attribute) => (ModelValidator) constructor.Invoke(new object[3]
      {
        (object) metadata,
        (object) context,
        (object) attribute
      }));
    }

    public static void RegisterDefaultAdapterFactory(DataAnnotationsModelValidationFactory factory)
    {
      DataAnnotationsModelValidatorProvider.ValidateAttributeFactory(factory);
      DataAnnotationsModelValidatorProvider.DefaultAttributeFactory = factory;
    }

    private static ConstructorInfo GetAttributeAdapterConstructor(
      Type attributeType,
      Type adapterType)
    {
      ConstructorInfo constructor = adapterType.GetConstructor(new Type[3]
      {
        typeof (ModelMetadata),
        typeof (ControllerContext),
        attributeType
      });
      return !(constructor == (ConstructorInfo) null) ? constructor : throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.DataAnnotationsModelValidatorProvider_ConstructorRequirements, (object) adapterType.FullName, (object) typeof (ModelMetadata).FullName, (object) typeof (ControllerContext).FullName, (object) attributeType.FullName), nameof (adapterType));
    }

    private static void ValidateAttributeAdapterType(Type adapterType)
    {
      if (adapterType == (Type) null)
        throw new ArgumentNullException(nameof (adapterType));
      if (!typeof (ModelValidator).IsAssignableFrom(adapterType))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Common_TypeMustDriveFromType, new object[2]
        {
          (object) adapterType.FullName,
          (object) typeof (ModelValidator).FullName
        }), nameof (adapterType));
    }

    private static void ValidateAttributeType(Type attributeType)
    {
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      if (!typeof (ValidationAttribute).IsAssignableFrom(attributeType))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Common_TypeMustDriveFromType, new object[2]
        {
          (object) attributeType.FullName,
          (object) typeof (ValidationAttribute).FullName
        }), nameof (attributeType));
    }

    private static void ValidateAttributeFactory(DataAnnotationsModelValidationFactory factory)
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
    }

    public static void RegisterValidatableObjectAdapter(Type modelType, Type adapterType)
    {
      DataAnnotationsModelValidatorProvider.ValidateValidatableModelType(modelType);
      DataAnnotationsModelValidatorProvider.ValidateValidatableAdapterType(adapterType);
      ConstructorInfo constructor = DataAnnotationsModelValidatorProvider.GetValidatableAdapterConstructor(adapterType);
      DataAnnotationsModelValidatorProvider._adaptersLock.EnterWriteLock();
      try
      {
        DataAnnotationsModelValidatorProvider.ValidatableFactories[modelType] = (DataAnnotationsValidatableObjectAdapterFactory) ((metadata, context) => (ModelValidator) constructor.Invoke(new object[2]
        {
          (object) metadata,
          (object) context
        }));
      }
      finally
      {
        DataAnnotationsModelValidatorProvider._adaptersLock.ExitWriteLock();
      }
    }

    public static void RegisterValidatableObjectAdapterFactory(
      Type modelType,
      DataAnnotationsValidatableObjectAdapterFactory factory)
    {
      DataAnnotationsModelValidatorProvider.ValidateValidatableModelType(modelType);
      DataAnnotationsModelValidatorProvider.ValidateValidatableFactory(factory);
      DataAnnotationsModelValidatorProvider._adaptersLock.EnterWriteLock();
      try
      {
        DataAnnotationsModelValidatorProvider.ValidatableFactories[modelType] = factory;
      }
      finally
      {
        DataAnnotationsModelValidatorProvider._adaptersLock.ExitWriteLock();
      }
    }

    public static void RegisterDefaultValidatableObjectAdapter(Type adapterType)
    {
      DataAnnotationsModelValidatorProvider.ValidateValidatableAdapterType(adapterType);
      ConstructorInfo constructor = DataAnnotationsModelValidatorProvider.GetValidatableAdapterConstructor(adapterType);
      DataAnnotationsModelValidatorProvider.DefaultValidatableFactory = (DataAnnotationsValidatableObjectAdapterFactory) ((metadata, context) => (ModelValidator) constructor.Invoke(new object[2]
      {
        (object) metadata,
        (object) context
      }));
    }

    public static void RegisterDefaultValidatableObjectAdapterFactory(
      DataAnnotationsValidatableObjectAdapterFactory factory)
    {
      DataAnnotationsModelValidatorProvider.ValidateValidatableFactory(factory);
      DataAnnotationsModelValidatorProvider.DefaultValidatableFactory = factory;
    }

    private static ConstructorInfo GetValidatableAdapterConstructor(Type adapterType)
    {
      ConstructorInfo constructor = adapterType.GetConstructor(new Type[2]
      {
        typeof (ModelMetadata),
        typeof (ControllerContext)
      });
      return !(constructor == (ConstructorInfo) null) ? constructor : throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.DataAnnotationsModelValidatorProvider_ValidatableConstructorRequirements, new object[3]
      {
        (object) adapterType.FullName,
        (object) typeof (ModelMetadata).FullName,
        (object) typeof (ControllerContext).FullName
      }), nameof (adapterType));
    }

    private static void ValidateValidatableAdapterType(Type adapterType)
    {
      if (adapterType == (Type) null)
        throw new ArgumentNullException(nameof (adapterType));
      if (!typeof (ModelValidator).IsAssignableFrom(adapterType))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Common_TypeMustDriveFromType, new object[2]
        {
          (object) adapterType.FullName,
          (object) typeof (ModelValidator).FullName
        }), nameof (adapterType));
    }

    private static void ValidateValidatableModelType(Type modelType)
    {
      if (modelType == (Type) null)
        throw new ArgumentNullException(nameof (modelType));
      if (!typeof (IValidatableObject).IsAssignableFrom(modelType))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Common_TypeMustDriveFromType, new object[2]
        {
          (object) modelType.FullName,
          (object) typeof (IValidatableObject).FullName
        }), nameof (modelType));
    }

    private static void ValidateValidatableFactory(
      DataAnnotationsValidatableObjectAdapterFactory factory)
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
    }

    private static Dictionary<Type, DataAnnotationsModelValidationFactory> BuildAttributeFactoriesDictionary()
    {
      Dictionary<Type, DataAnnotationsModelValidationFactory> dictionary = new Dictionary<Type, DataAnnotationsModelValidationFactory>();
      DataAnnotationsModelValidatorProvider.AddValidationAttributeAdapter(dictionary, typeof (RangeAttribute), (DataAnnotationsModelValidationFactory) ((metadata, context, attribute) => (ModelValidator) new RangeAttributeAdapter(metadata, context, (RangeAttribute) attribute)));
      DataAnnotationsModelValidatorProvider.AddValidationAttributeAdapter(dictionary, typeof (RegularExpressionAttribute), (DataAnnotationsModelValidationFactory) ((metadata, context, attribute) => (ModelValidator) new RegularExpressionAttributeAdapter(metadata, context, (RegularExpressionAttribute) attribute)));
      DataAnnotationsModelValidatorProvider.AddValidationAttributeAdapter(dictionary, typeof (RequiredAttribute), (DataAnnotationsModelValidationFactory) ((metadata, context, attribute) => (ModelValidator) new RequiredAttributeAdapter(metadata, context, (RequiredAttribute) attribute)));
      DataAnnotationsModelValidatorProvider.AddValidationAttributeAdapter(dictionary, typeof (StringLengthAttribute), (DataAnnotationsModelValidationFactory) ((metadata, context, attribute) => (ModelValidator) new StringLengthAttributeAdapter(metadata, context, (StringLengthAttribute) attribute)));
      DataAnnotationsModelValidatorProvider.AddValidationAttributeAdapter(dictionary, ValidationAttributeHelpers.MembershipPasswordAttributeType, (DataAnnotationsModelValidationFactory) ((metadata, context, attribute) => (ModelValidator) new MembershipPasswordAttributeAdapter(metadata, context, attribute)));
      DataAnnotationsModelValidatorProvider.AddValidationAttributeAdapter(dictionary, ValidationAttributeHelpers.CompareAttributeType, (DataAnnotationsModelValidationFactory) ((metadata, context, attribute) => (ModelValidator) new CompareAttributeAdapter(metadata, context, attribute)));
      DataAnnotationsModelValidatorProvider.AddValidationAttributeAdapter(dictionary, ValidationAttributeHelpers.FileExtensionsAttributeType, (DataAnnotationsModelValidationFactory) ((metadata, context, attribute) => (ModelValidator) new FileExtensionsAttributeAdapter(metadata, context, attribute)));
      DataAnnotationsModelValidatorProvider.AddDataTypeAttributeAdapter(dictionary, ValidationAttributeHelpers.CreditCardAttributeType, "creditcard");
      DataAnnotationsModelValidatorProvider.AddDataTypeAttributeAdapter(dictionary, ValidationAttributeHelpers.EmailAddressAttributeType, "email");
      DataAnnotationsModelValidatorProvider.AddDataTypeAttributeAdapter(dictionary, ValidationAttributeHelpers.PhoneAttributeType, "phone");
      DataAnnotationsModelValidatorProvider.AddDataTypeAttributeAdapter(dictionary, ValidationAttributeHelpers.UrlAttributeType, "url");
      return dictionary;
    }

    private static void AddValidationAttributeAdapter(
      Dictionary<Type, DataAnnotationsModelValidationFactory> dictionary,
      Type validataionAttributeType,
      DataAnnotationsModelValidationFactory factory)
    {
      if (!(validataionAttributeType != (Type) null))
        return;
      dictionary.Add(validataionAttributeType, factory);
    }

    private static void AddDataTypeAttributeAdapter(
      Dictionary<Type, DataAnnotationsModelValidationFactory> dictionary,
      Type attributeType,
      string ruleName)
    {
      DataAnnotationsModelValidatorProvider.AddValidationAttributeAdapter(dictionary, attributeType, (DataAnnotationsModelValidationFactory) ((metadata, context, attribute) => (ModelValidator) new DataTypeAttributeAdapter(metadata, context, (DataTypeAttribute) attribute, ruleName)));
    }
  }
}
