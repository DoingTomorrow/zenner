// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DefaultModelBinder
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Http;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class DefaultModelBinder : IModelBinder
  {
    private static string _resourceClassKey;
    private ModelBinderDictionary _binders;

    protected internal ModelBinderDictionary Binders
    {
      get
      {
        if (this._binders == null)
          this._binders = ModelBinders.Binders;
        return this._binders;
      }
      set => this._binders = value;
    }

    public static string ResourceClassKey
    {
      get => DefaultModelBinder._resourceClassKey ?? string.Empty;
      set => DefaultModelBinder._resourceClassKey = value;
    }

    private static void AddValueRequiredMessageToModelState(
      ControllerContext controllerContext,
      ModelStateDictionary modelState,
      string modelStateKey,
      Type elementType,
      object value)
    {
      if (value != null || TypeHelpers.TypeAllowsNullValue(elementType) || !modelState.IsValidField(modelStateKey))
        return;
      modelState.AddModelError(modelStateKey, DefaultModelBinder.GetValueRequiredResource(controllerContext));
    }

    internal void BindComplexElementalModel(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      object model)
    {
      ModelBindingContext modelBindingContext = this.CreateComplexElementalModelBindingContext(controllerContext, bindingContext, model);
      if (!this.OnModelUpdating(controllerContext, modelBindingContext))
        return;
      this.BindProperties(controllerContext, modelBindingContext);
      this.OnModelUpdated(controllerContext, modelBindingContext);
    }

    internal object BindComplexModel(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      object model = bindingContext.Model;
      Type modelType1 = bindingContext.ModelType;
      if (model == null && modelType1.IsArray)
      {
        Type elementType = modelType1.GetElementType();
        Type modelType2 = typeof (List<>).MakeGenericType(elementType);
        object collection = this.CreateModel(controllerContext, bindingContext, modelType2);
        ModelBindingContext bindingContext1 = new ModelBindingContext()
        {
          ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>) (() => collection), modelType2),
          ModelName = bindingContext.ModelName,
          ModelState = bindingContext.ModelState,
          PropertyFilter = bindingContext.PropertyFilter,
          ValueProvider = bindingContext.ValueProvider
        };
        IList list = (IList) this.UpdateCollection(controllerContext, bindingContext1, elementType);
        if (list == null)
          return (object) null;
        Array instance = Array.CreateInstance(elementType, list.Count);
        list.CopyTo(instance, 0);
        return (object) instance;
      }
      if (model == null)
        model = this.CreateModel(controllerContext, bindingContext, modelType1);
      Type genericInterface1 = TypeHelpers.ExtractGenericInterface(modelType1, typeof (IDictionary<,>));
      if (genericInterface1 != (Type) null)
      {
        Type[] genericArguments = genericInterface1.GetGenericArguments();
        Type keyType = genericArguments[0];
        Type valueType = genericArguments[1];
        ModelBindingContext bindingContext2 = new ModelBindingContext()
        {
          ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>) (() => model), modelType1),
          ModelName = bindingContext.ModelName,
          ModelState = bindingContext.ModelState,
          PropertyFilter = bindingContext.PropertyFilter,
          ValueProvider = bindingContext.ValueProvider
        };
        return this.UpdateDictionary(controllerContext, bindingContext2, keyType, valueType);
      }
      Type genericInterface2 = TypeHelpers.ExtractGenericInterface(modelType1, typeof (IEnumerable<>));
      if (genericInterface2 != (Type) null)
      {
        Type genericArgument = genericInterface2.GetGenericArguments()[0];
        if (typeof (ICollection<>).MakeGenericType(genericArgument).IsInstanceOfType(model))
        {
          ModelBindingContext bindingContext3 = new ModelBindingContext()
          {
            ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>) (() => model), modelType1),
            ModelName = bindingContext.ModelName,
            ModelState = bindingContext.ModelState,
            PropertyFilter = bindingContext.PropertyFilter,
            ValueProvider = bindingContext.ValueProvider
          };
          return this.UpdateCollection(controllerContext, bindingContext3, genericArgument);
        }
      }
      this.BindComplexElementalModel(controllerContext, bindingContext, model);
      return model;
    }

    public virtual object BindModel(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      EnsureStackHelper.EnsureStack();
      if (bindingContext == null)
        throw new ArgumentNullException(nameof (bindingContext));
      bool flag1 = false;
      if (!string.IsNullOrEmpty(bindingContext.ModelName) && !bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName))
      {
        if (!bindingContext.FallbackToEmptyPrefix)
          return (object) null;
        bindingContext = new ModelBindingContext()
        {
          ModelMetadata = bindingContext.ModelMetadata,
          ModelState = bindingContext.ModelState,
          PropertyFilter = bindingContext.PropertyFilter,
          ValueProvider = bindingContext.ValueProvider
        };
        flag1 = true;
      }
      if (!flag1)
      {
        bool flag2 = DefaultModelBinder.ShouldPerformRequestValidation(controllerContext, bindingContext);
        ValueProviderResult valueProviderResult = bindingContext.UnvalidatedValueProvider.GetValue(bindingContext.ModelName, !flag2);
        if (valueProviderResult != null)
          return this.BindSimpleModel(controllerContext, bindingContext, valueProviderResult);
      }
      return !bindingContext.ModelMetadata.IsComplexType ? (object) null : this.BindComplexModel(controllerContext, bindingContext);
    }

    private void BindProperties(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      foreach (PropertyDescriptor filteredModelProperty in this.GetFilteredModelProperties(controllerContext, bindingContext))
        this.BindProperty(controllerContext, bindingContext, filteredModelProperty);
    }

    protected virtual void BindProperty(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      PropertyDescriptor propertyDescriptor)
    {
      string subPropertyName = DefaultModelBinder.CreateSubPropertyName(bindingContext.ModelName, propertyDescriptor.Name);
      if (!bindingContext.ValueProvider.ContainsPrefix(subPropertyName))
        return;
      IModelBinder binder = this.Binders.GetBinder(propertyDescriptor.PropertyType);
      object obj = propertyDescriptor.GetValue(bindingContext.Model);
      ModelMetadata modelMetadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
      modelMetadata.Model = obj;
      ModelBindingContext bindingContext1 = new ModelBindingContext()
      {
        ModelMetadata = modelMetadata,
        ModelName = subPropertyName,
        ModelState = bindingContext.ModelState,
        ValueProvider = bindingContext.ValueProvider
      };
      object propertyValue = this.GetPropertyValue(controllerContext, bindingContext1, propertyDescriptor, binder);
      modelMetadata.Model = propertyValue;
      ModelState modelState = bindingContext.ModelState[subPropertyName];
      if (modelState == null || modelState.Errors.Count == 0)
      {
        if (!this.OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, propertyValue))
          return;
        this.SetProperty(controllerContext, bindingContext, propertyDescriptor, propertyValue);
        this.OnPropertyValidated(controllerContext, bindingContext, propertyDescriptor, propertyValue);
      }
      else
      {
        this.SetProperty(controllerContext, bindingContext, propertyDescriptor, propertyValue);
        foreach (ModelError modelError in modelState.Errors.Where<ModelError>((Func<ModelError, bool>) (err => string.IsNullOrEmpty(err.ErrorMessage) && err.Exception != null)).ToList<ModelError>())
        {
          for (Exception exception = modelError.Exception; exception != null; exception = exception.InnerException)
          {
            if (exception is FormatException)
            {
              string displayName = modelMetadata.GetDisplayName();
              string errorMessage = string.Format((IFormatProvider) CultureInfo.CurrentCulture, DefaultModelBinder.GetValueInvalidResource(controllerContext), new object[2]
              {
                (object) modelState.Value.AttemptedValue,
                (object) displayName
              });
              modelState.Errors.Remove(modelError);
              modelState.Errors.Add(errorMessage);
              break;
            }
          }
        }
      }
    }

    internal object BindSimpleModel(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      ValueProviderResult valueProviderResult)
    {
      bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
      if (bindingContext.ModelType.IsInstanceOfType(valueProviderResult.RawValue))
        return valueProviderResult.RawValue;
      if (bindingContext.ModelType != typeof (string) && !bindingContext.ModelType.IsArray)
      {
        Type genericInterface = TypeHelpers.ExtractGenericInterface(bindingContext.ModelType, typeof (IEnumerable<>));
        if (genericInterface != (Type) null)
        {
          object model = this.CreateModel(controllerContext, bindingContext, bindingContext.ModelType);
          Type genericArgument = genericInterface.GetGenericArguments()[0];
          Type destinationType = genericArgument.MakeArrayType();
          object newContents = DefaultModelBinder.ConvertProviderResult(bindingContext.ModelState, bindingContext.ModelName, valueProviderResult, destinationType);
          if (typeof (ICollection<>).MakeGenericType(genericArgument).IsInstanceOfType(model))
            DefaultModelBinder.CollectionHelpers.ReplaceCollection(genericArgument, model, newContents);
          return model;
        }
      }
      return DefaultModelBinder.ConvertProviderResult(bindingContext.ModelState, bindingContext.ModelName, valueProviderResult, bindingContext.ModelType);
    }

    private static bool CanUpdateReadonlyTypedReference(Type type)
    {
      return !type.IsValueType && !type.IsArray && !(type == typeof (string));
    }

    private static object ConvertProviderResult(
      ModelStateDictionary modelState,
      string modelStateKey,
      ValueProviderResult valueProviderResult,
      Type destinationType)
    {
      try
      {
        return valueProviderResult.ConvertTo(destinationType);
      }
      catch (Exception ex)
      {
        modelState.AddModelError(modelStateKey, ex);
        return (object) null;
      }
    }

    internal ModelBindingContext CreateComplexElementalModelBindingContext(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      object model)
    {
      BindAttribute bindAttr = (BindAttribute) this.GetTypeDescriptor(controllerContext, bindingContext).GetAttributes()[typeof (BindAttribute)];
      Predicate<string> predicate = bindAttr != null ? (Predicate<string>) (propertyName => bindAttr.IsPropertyAllowed(propertyName) && bindingContext.PropertyFilter(propertyName)) : bindingContext.PropertyFilter;
      return new ModelBindingContext()
      {
        ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>) (() => model), bindingContext.ModelType),
        ModelName = bindingContext.ModelName,
        ModelState = bindingContext.ModelState,
        PropertyFilter = predicate,
        ValueProvider = bindingContext.ValueProvider
      };
    }

    protected virtual object CreateModel(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      Type modelType)
    {
      Type type = modelType;
      if (modelType.IsGenericType)
      {
        Type genericTypeDefinition = modelType.GetGenericTypeDefinition();
        if (genericTypeDefinition == typeof (IDictionary<,>))
          type = typeof (Dictionary<,>).MakeGenericType(modelType.GetGenericArguments());
        else if (genericTypeDefinition == typeof (IEnumerable<>) || genericTypeDefinition == typeof (ICollection<>) || genericTypeDefinition == typeof (IList<>))
          type = typeof (List<>).MakeGenericType(modelType.GetGenericArguments());
      }
      return Activator.CreateInstance(type);
    }

    protected static string CreateSubIndexName(string prefix, int index)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}[{1}]", new object[2]
      {
        (object) prefix,
        (object) index
      });
    }

    protected static string CreateSubIndexName(string prefix, string index)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}[{1}]", new object[2]
      {
        (object) prefix,
        (object) index
      });
    }

    protected internal static string CreateSubPropertyName(string prefix, string propertyName)
    {
      if (string.IsNullOrEmpty(prefix))
        return propertyName;
      return string.IsNullOrEmpty(propertyName) ? prefix : prefix + "." + propertyName;
    }

    protected IEnumerable<PropertyDescriptor> GetFilteredModelProperties(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      PropertyDescriptorCollection modelProperties = this.GetModelProperties(controllerContext, bindingContext);
      Predicate<string> propertyFilter = bindingContext.PropertyFilter;
      return modelProperties.Cast<PropertyDescriptor>().Where<PropertyDescriptor>((Func<PropertyDescriptor, bool>) (property => DefaultModelBinder.ShouldUpdateProperty(property, propertyFilter)));
    }

    private static void GetIndexes(
      ModelBindingContext bindingContext,
      out bool stopOnIndexNotFound,
      out IEnumerable<string> indexes)
    {
      string subPropertyName = DefaultModelBinder.CreateSubPropertyName(bindingContext.ModelName, "index");
      ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(subPropertyName);
      if (valueProviderResult != null && valueProviderResult.ConvertTo(typeof (string[])) is string[] strArray)
      {
        stopOnIndexNotFound = false;
        indexes = (IEnumerable<string>) strArray;
      }
      else
      {
        stopOnIndexNotFound = true;
        indexes = DefaultModelBinder.GetZeroBasedIndexes();
      }
    }

    protected virtual PropertyDescriptorCollection GetModelProperties(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      return this.GetTypeDescriptor(controllerContext, bindingContext).GetProperties();
    }

    protected virtual object GetPropertyValue(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      PropertyDescriptor propertyDescriptor,
      IModelBinder propertyBinder)
    {
      object objA = propertyBinder.BindModel(controllerContext, bindingContext);
      return bindingContext.ModelMetadata.ConvertEmptyStringToNull && object.Equals(objA, (object) string.Empty) ? (object) null : objA;
    }

    protected virtual ICustomTypeDescriptor GetTypeDescriptor(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      return TypeDescriptorHelper.Get(bindingContext.ModelType);
    }

    private static string GetUserResourceString(
      ControllerContext controllerContext,
      string resourceName)
    {
      string userResourceString = (string) null;
      if (!string.IsNullOrEmpty(DefaultModelBinder.ResourceClassKey) && controllerContext != null && controllerContext.HttpContext != null)
        userResourceString = controllerContext.HttpContext.GetGlobalResourceObject(DefaultModelBinder.ResourceClassKey, resourceName, CultureInfo.CurrentUICulture) as string;
      return userResourceString;
    }

    private static string GetValueInvalidResource(ControllerContext controllerContext)
    {
      return DefaultModelBinder.GetUserResourceString(controllerContext, "PropertyValueInvalid") ?? MvcResources.DefaultModelBinder_ValueInvalid;
    }

    private static string GetValueRequiredResource(ControllerContext controllerContext)
    {
      return DefaultModelBinder.GetUserResourceString(controllerContext, "PropertyValueRequired") ?? MvcResources.DefaultModelBinder_ValueRequired;
    }

    private static IEnumerable<string> GetZeroBasedIndexes()
    {
      int i = 0;
      while (true)
      {
        yield return i.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        ++i;
      }
    }

    protected static bool IsModelValid(ModelBindingContext bindingContext)
    {
      if (bindingContext == null)
        throw new ArgumentNullException(nameof (bindingContext));
      return string.IsNullOrEmpty(bindingContext.ModelName) ? bindingContext.ModelState.IsValid : bindingContext.ModelState.IsValidField(bindingContext.ModelName);
    }

    protected virtual void OnModelUpdated(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (ModelValidationResult validationResult in ModelValidator.GetModelValidator(bindingContext.ModelMetadata, controllerContext).Validate((object) null))
      {
        string subPropertyName = DefaultModelBinder.CreateSubPropertyName(bindingContext.ModelName, validationResult.MemberName);
        if (!dictionary.ContainsKey(subPropertyName))
          dictionary[subPropertyName] = bindingContext.ModelState.IsValidField(subPropertyName);
        if (dictionary[subPropertyName])
          bindingContext.ModelState.AddModelError(subPropertyName, validationResult.Message);
      }
    }

    protected virtual bool OnModelUpdating(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      return true;
    }

    protected virtual void OnPropertyValidated(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      PropertyDescriptor propertyDescriptor,
      object value)
    {
    }

    protected virtual bool OnPropertyValidating(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      PropertyDescriptor propertyDescriptor,
      object value)
    {
      return true;
    }

    protected virtual void SetProperty(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      PropertyDescriptor propertyDescriptor,
      object value)
    {
      ModelMetadata metadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
      metadata.Model = value;
      string subPropertyName = DefaultModelBinder.CreateSubPropertyName(bindingContext.ModelName, metadata.PropertyName);
      if (value == null && bindingContext.ModelState.IsValidField(subPropertyName))
      {
        ModelValidator modelValidator = ModelValidatorProviders.Providers.GetValidators(metadata, controllerContext).Where<ModelValidator>((Func<ModelValidator, bool>) (v => v.IsRequired)).FirstOrDefault<ModelValidator>();
        if (modelValidator != null)
        {
          foreach (ModelValidationResult validationResult in modelValidator.Validate(bindingContext.Model))
            bindingContext.ModelState.AddModelError(subPropertyName, validationResult.Message);
        }
      }
      bool flag = value == null && !TypeHelpers.TypeAllowsNullValue(propertyDescriptor.PropertyType);
      if (!propertyDescriptor.IsReadOnly)
      {
        if (!flag)
        {
          try
          {
            propertyDescriptor.SetValue(bindingContext.Model, value);
          }
          catch (Exception ex)
          {
            if (bindingContext.ModelState.IsValidField(subPropertyName))
              bindingContext.ModelState.AddModelError(subPropertyName, ex);
          }
        }
      }
      if (!flag || !bindingContext.ModelState.IsValidField(subPropertyName))
        return;
      bindingContext.ModelState.AddModelError(subPropertyName, DefaultModelBinder.GetValueRequiredResource(controllerContext));
    }

    private static bool ShouldPerformRequestValidation(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      if (controllerContext == null || controllerContext.Controller == null || bindingContext == null || bindingContext.ModelMetadata == null)
        return true;
      return controllerContext.Controller.ValidateRequest && bindingContext.ModelMetadata.RequestValidationEnabled;
    }

    private static bool ShouldUpdateProperty(
      PropertyDescriptor property,
      Predicate<string> propertyFilter)
    {
      return (!property.IsReadOnly || DefaultModelBinder.CanUpdateReadonlyTypedReference(property.PropertyType)) && propertyFilter(property.Name);
    }

    internal object UpdateCollection(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      Type elementType)
    {
      bool stopOnIndexNotFound;
      IEnumerable<string> indexes;
      DefaultModelBinder.GetIndexes(bindingContext, out stopOnIndexNotFound, out indexes);
      IModelBinder binder = this.Binders.GetBinder(elementType);
      List<object> newContents = new List<object>();
      foreach (string index in indexes)
      {
        string subIndexName = DefaultModelBinder.CreateSubIndexName(bindingContext.ModelName, index);
        if (!bindingContext.ValueProvider.ContainsPrefix(subIndexName))
        {
          if (stopOnIndexNotFound)
            break;
        }
        else
        {
          ModelBindingContext bindingContext1 = new ModelBindingContext()
          {
            ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>) null, elementType),
            ModelName = subIndexName,
            ModelState = bindingContext.ModelState,
            PropertyFilter = bindingContext.PropertyFilter,
            ValueProvider = bindingContext.ValueProvider
          };
          object obj = binder.BindModel(controllerContext, bindingContext1);
          DefaultModelBinder.AddValueRequiredMessageToModelState(controllerContext, bindingContext.ModelState, subIndexName, elementType, obj);
          newContents.Add(obj);
        }
      }
      if (newContents.Count == 0)
        return (object) null;
      object model = bindingContext.Model;
      DefaultModelBinder.CollectionHelpers.ReplaceCollection(elementType, model, (object) newContents);
      return model;
    }

    internal object UpdateDictionary(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      Type keyType,
      Type valueType)
    {
      bool stopOnIndexNotFound;
      IEnumerable<string> indexes;
      DefaultModelBinder.GetIndexes(bindingContext, out stopOnIndexNotFound, out indexes);
      IModelBinder binder1 = this.Binders.GetBinder(keyType);
      IModelBinder binder2 = this.Binders.GetBinder(valueType);
      List<KeyValuePair<object, object>> newContents = new List<KeyValuePair<object, object>>();
      foreach (string index in indexes)
      {
        string subIndexName = DefaultModelBinder.CreateSubIndexName(bindingContext.ModelName, index);
        string subPropertyName1 = DefaultModelBinder.CreateSubPropertyName(subIndexName, "key");
        string subPropertyName2 = DefaultModelBinder.CreateSubPropertyName(subIndexName, "value");
        if (!bindingContext.ValueProvider.ContainsPrefix(subPropertyName1) || !bindingContext.ValueProvider.ContainsPrefix(subPropertyName2))
        {
          if (stopOnIndexNotFound)
            break;
        }
        else
        {
          ModelBindingContext bindingContext1 = new ModelBindingContext()
          {
            ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>) null, keyType),
            ModelName = subPropertyName1,
            ModelState = bindingContext.ModelState,
            ValueProvider = bindingContext.ValueProvider
          };
          object obj = binder1.BindModel(controllerContext, bindingContext1);
          DefaultModelBinder.AddValueRequiredMessageToModelState(controllerContext, bindingContext.ModelState, subPropertyName1, keyType, obj);
          if (keyType.IsInstanceOfType(obj))
            newContents.Add(DefaultModelBinder.CreateEntryForModel(controllerContext, bindingContext, valueType, binder2, subPropertyName2, obj));
        }
      }
      if (newContents.Count == 0 && bindingContext.ValueProvider is IEnumerableValueProvider valueProvider)
      {
        foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) valueProvider.GetKeysFromPrefix(bindingContext.ModelName))
          newContents.Add(DefaultModelBinder.CreateEntryForModel(controllerContext, bindingContext, valueType, binder2, keyValuePair.Value, (object) keyValuePair.Key));
      }
      if (newContents.Count == 0)
        return (object) null;
      object model = bindingContext.Model;
      DefaultModelBinder.CollectionHelpers.ReplaceDictionary(keyType, valueType, model, (object) newContents);
      return model;
    }

    private static KeyValuePair<object, object> CreateEntryForModel(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      Type valueType,
      IModelBinder valueBinder,
      string modelName,
      object modelKey)
    {
      ModelBindingContext bindingContext1 = new ModelBindingContext()
      {
        ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>) null, valueType),
        ModelName = modelName,
        ModelState = bindingContext.ModelState,
        PropertyFilter = bindingContext.PropertyFilter,
        ValueProvider = bindingContext.ValueProvider
      };
      object obj = valueBinder.BindModel(controllerContext, bindingContext1);
      DefaultModelBinder.AddValueRequiredMessageToModelState(controllerContext, bindingContext.ModelState, modelName, valueType, obj);
      return new KeyValuePair<object, object>(modelKey, obj);
    }

    private static class CollectionHelpers
    {
      private static readonly MethodInfo _replaceCollectionMethod = typeof (DefaultModelBinder.CollectionHelpers).GetMethod("ReplaceCollectionImpl", BindingFlags.Static | BindingFlags.NonPublic);
      private static readonly MethodInfo _replaceDictionaryMethod = typeof (DefaultModelBinder.CollectionHelpers).GetMethod("ReplaceDictionaryImpl", BindingFlags.Static | BindingFlags.NonPublic);

      [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
      public static void ReplaceCollection(
        Type collectionType,
        object collection,
        object newContents)
      {
        DefaultModelBinder.CollectionHelpers._replaceCollectionMethod.MakeGenericMethod(collectionType).Invoke((object) null, new object[2]
        {
          collection,
          newContents
        });
      }

      private static void ReplaceCollectionImpl<T>(
        ICollection<T> collection,
        IEnumerable newContents)
      {
        collection.Clear();
        if (newContents == null)
          return;
        foreach (object newContent in newContents)
        {
          if (!(newContent is T obj1))
            obj1 = default (T);
          T obj2 = obj1;
          collection.Add(obj2);
        }
      }

      [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
      public static void ReplaceDictionary(
        Type keyType,
        Type valueType,
        object dictionary,
        object newContents)
      {
        DefaultModelBinder.CollectionHelpers._replaceDictionaryMethod.MakeGenericMethod(keyType, valueType).Invoke((object) null, new object[2]
        {
          dictionary,
          newContents
        });
      }

      private static void ReplaceDictionaryImpl<TKey, TValue>(
        IDictionary<TKey, TValue> dictionary,
        IEnumerable<KeyValuePair<object, object>> newContents)
      {
        dictionary.Clear();
        foreach (KeyValuePair<object, object> newContent in newContents)
        {
          TKey key = (TKey) newContent.Key;
          TValue obj = newContent.Value is TValue ? (TValue) newContent.Value : default (TValue);
          dictionary[key] = obj;
        }
      }
    }
  }
}
