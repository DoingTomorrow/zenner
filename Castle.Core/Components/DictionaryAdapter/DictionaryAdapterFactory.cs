// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.DictionaryAdapterFactory
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Xml.XPath;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class DictionaryAdapterFactory : IDictionaryAdapterFactory
  {
    private readonly Dictionary<Type, Type> interfaceToAdapter = new Dictionary<Type, Type>();
    private readonly object typesDictionaryLocker = new object();
    private static readonly ICollection<Type> InfrastructureTypes = (ICollection<Type>) new HashSet<Type>()
    {
      typeof (IEditableObject),
      typeof (IDictionaryEdit),
      typeof (IChangeTracking),
      typeof (IRevertibleChangeTracking),
      typeof (IDictionaryNotify),
      typeof (IDataErrorInfo),
      typeof (IDictionaryValidate),
      typeof (IDictionaryAdapter)
    };
    private static readonly PropertyInfo AdapterGetMeta = typeof (IDictionaryAdapter).GetProperty("Meta");
    private static readonly MethodInfo AdapterGetProperty = typeof (IDictionaryAdapter).GetMethod("GetProperty");
    private static readonly MethodInfo AdapterSetProperty = typeof (IDictionaryAdapter).GetMethod("SetProperty");

    public T GetAdapter<T>(IDictionary dictionary) => (T) this.GetAdapter(typeof (T), dictionary);

    public object GetAdapter(Type type, IDictionary dictionary)
    {
      return this.InternalGetAdapter(type, dictionary, (PropertyDescriptor) null);
    }

    public object GetAdapter(Type type, IDictionary dictionary, PropertyDescriptor descriptor)
    {
      return this.InternalGetAdapter(type, dictionary, descriptor);
    }

    public T GetAdapter<T, R>(IDictionary<string, R> dictionary)
    {
      return (T) this.GetAdapter<R>(typeof (T), dictionary);
    }

    public object GetAdapter<R>(Type type, IDictionary<string, R> dictionary)
    {
      GenericDictionaryAdapter<R> dictionaryAdapter = new GenericDictionaryAdapter<R>(dictionary);
      return this.InternalGetAdapter(type, (IDictionary) dictionaryAdapter, (PropertyDescriptor) null);
    }

    public T GetAdapter<T>(NameValueCollection nameValues)
    {
      return this.GetAdapter<T>((IDictionary) new NameValueCollectionAdapter(nameValues));
    }

    public object GetAdapter(Type type, NameValueCollection nameValues)
    {
      return this.GetAdapter(type, (IDictionary) new NameValueCollectionAdapter(nameValues));
    }

    public T GetAdapter<T>(IXPathNavigable xpathNavigable)
    {
      return (T) this.GetAdapter(typeof (T), xpathNavigable);
    }

    public object GetAdapter(Type type, IXPathNavigable xpathNavigable)
    {
      XPathAdapter xpathAdapter = new XPathAdapter(xpathNavigable);
      return this.GetAdapter(type, (IDictionary) new Hashtable(), new DictionaryDescriptor().AddBehavior((IDictionaryBehavior) XPathBehavior.Instance).AddBehavior((IDictionaryBehavior) xpathAdapter));
    }

    public DictionaryAdapterMeta GetAdapterMeta(Type type)
    {
      return this.GetAdapterMeta(type, (PropertyDescriptor) null);
    }

    public DictionaryAdapterMeta GetAdapterMeta(Type type, PropertyDescriptor descriptor)
    {
      return type.IsInterface ? (DictionaryAdapterMeta) this.InternalGetAdapterType(type, descriptor).InvokeMember("__meta", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField, (Binder) null, (object) null, (object[]) null) : throw new ArgumentException("Only interfaces can be adapted and have metadata");
    }

    private Type InternalGetAdapterType(Type type, PropertyDescriptor descriptor)
    {
      if (!type.IsInterface)
        throw new ArgumentException("Only interfaces can be adapted to a dictionary");
      Type adapterType;
      if (!this.interfaceToAdapter.TryGetValue(type, out adapterType))
      {
        lock (this.typesDictionaryLocker)
        {
          if (!this.interfaceToAdapter.TryGetValue(type, out adapterType))
          {
            AppDomain domain = Thread.GetDomain();
            string adapterAssemblyName = this.GetAdapterAssemblyName(type);
            TypeBuilder typeBuilder = DictionaryAdapterFactory.CreateTypeBuilder(type, domain, adapterAssemblyName);
            Assembly adapterAssembly = this.CreateAdapterAssembly(type, typeBuilder, descriptor);
            adapterType = DictionaryAdapterFactory.CreateAdapterType(type, adapterAssembly);
            this.interfaceToAdapter[type] = adapterType;
          }
        }
      }
      return adapterType;
    }

    private object InternalGetAdapter(
      Type type,
      IDictionary dictionary,
      PropertyDescriptor descriptor)
    {
      Type adapterType = this.InternalGetAdapterType(type, descriptor);
      return this.CreateAdapterInstance(dictionary, descriptor, adapterType);
    }

    private static TypeBuilder CreateTypeBuilder(
      Type type,
      AppDomain appDomain,
      string adapterAssemblyName)
    {
      AssemblyName name = new AssemblyName(adapterAssemblyName);
      ModuleBuilder moduleBuilder = appDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run).DefineDynamicModule(adapterAssemblyName);
      return DictionaryAdapterFactory.CreateAdapterType(type, moduleBuilder);
    }

    private static TypeBuilder CreateAdapterType(Type type, ModuleBuilder moduleBuilder)
    {
      TypeBuilder adapterType = moduleBuilder.DefineType(DictionaryAdapterFactory.GetAdapterFullTypeName(type), TypeAttributes.Public | TypeAttributes.BeforeFieldInit);
      adapterType.AddInterfaceImplementation(type);
      adapterType.SetParent(typeof (DictionaryAdapterBase));
      CustomAttributeBuilder customBuilder1 = new CustomAttributeBuilder(typeof (DictionaryAdapterAttribute).GetConstructor(new Type[1]
      {
        typeof (Type)
      }), (object[]) new Type[1]{ type });
      adapterType.SetCustomAttribute(customBuilder1);
      CustomAttributeBuilder customBuilder2 = new CustomAttributeBuilder(typeof (DebuggerDisplayAttribute).GetConstructor(new Type[1]
      {
        typeof (string)
      }), (object[]) new string[1]
      {
        "Type: {Meta.Type.FullName,nq}"
      });
      adapterType.SetCustomAttribute(customBuilder2);
      return adapterType;
    }

    private Assembly CreateAdapterAssembly(
      Type type,
      TypeBuilder typeBuilder,
      PropertyDescriptor descriptor)
    {
      FieldAttributes attributes = FieldAttributes.Public | FieldAttributes.Static;
      FieldBuilder field = typeBuilder.DefineField("__meta", typeof (DictionaryAdapterMeta), attributes);
      DictionaryAdapterFactory.CreateAdapterConstructor(typeBuilder);
      IDictionaryInitializer[] typeInitializers;
      IDictionaryMetaInitializer[] metaInitializers;
      object[] typeBehaviors;
      Dictionary<string, PropertyDescriptor> propertyDescriptors = DictionaryAdapterFactory.GetPropertyDescriptors(type, out typeInitializers, out metaInitializers, out typeBehaviors);
      DictionaryAdapterFactory.CreateMetaProperty(typeBuilder, DictionaryAdapterFactory.AdapterGetMeta, (FieldInfo) field);
      foreach (KeyValuePair<string, PropertyDescriptor> keyValuePair in propertyDescriptors)
        DictionaryAdapterFactory.CreateAdapterProperty(typeBuilder, keyValuePair.Value);
      typeBuilder.CreateType().InvokeMember("__meta", BindingFlags.Static | BindingFlags.Public | BindingFlags.SetField, (Binder) null, (object) null, (object[]) new DictionaryAdapterMeta[1]
      {
        new DictionaryAdapterMeta(type, typeInitializers, metaInitializers, typeBehaviors, (IDictionary<string, PropertyDescriptor>) propertyDescriptors, descriptor as DictionaryDescriptor, (IDictionaryAdapterFactory) this)
      });
      return typeBuilder.Assembly;
    }

    private static void CreateAdapterConstructor(TypeBuilder typeBuilder)
    {
      ILGenerator ilGenerator = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.Standard, new Type[1]
      {
        typeof (DictionaryAdapterInstance)
      }).GetILGenerator();
      ConstructorInfo constructor = typeof (DictionaryAdapterBase).GetConstructors()[0];
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Ldarg_1);
      ilGenerator.Emit(OpCodes.Call, constructor);
      ilGenerator.Emit(OpCodes.Ret);
    }

    private static void CreateMetaProperty(
      TypeBuilder typeBuilder,
      PropertyInfo prop,
      FieldInfo field)
    {
      MethodAttributes attributes = MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName;
      MethodBuilder methodInfoBody = typeBuilder.DefineMethod("get_" + prop.Name, attributes, prop.PropertyType, (Type[]) null);
      ILGenerator ilGenerator = methodInfoBody.GetILGenerator();
      if (field.IsStatic)
      {
        ilGenerator.Emit(OpCodes.Ldsfld, field);
      }
      else
      {
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldfld, field);
      }
      ilGenerator.Emit(OpCodes.Ret);
      typeBuilder.DefineMethodOverride((MethodInfo) methodInfoBody, prop.GetGetMethod());
    }

    private static void CreateAdapterProperty(
      TypeBuilder typeBuilder,
      PropertyDescriptor descriptor)
    {
      PropertyInfo property = descriptor.Property;
      PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(property.Name, property.Attributes, property.PropertyType, (Type[]) null);
      MethodAttributes propAttribs = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName;
      if (property.CanRead)
        DictionaryAdapterFactory.CreatePropertyGetMethod(typeBuilder, propertyBuilder, descriptor, propAttribs);
      if (!property.CanWrite)
        return;
      DictionaryAdapterFactory.CreatePropertySetMethod(typeBuilder, propertyBuilder, descriptor, propAttribs);
    }

    private static void PreparePropertyMethod(
      PropertyDescriptor descriptor,
      ILGenerator propILGenerator)
    {
      propILGenerator.DeclareLocal(typeof (string));
      propILGenerator.DeclareLocal(typeof (object));
      propILGenerator.Emit(OpCodes.Ldstr, descriptor.PropertyName);
      propILGenerator.Emit(OpCodes.Stloc_0);
    }

    private static void CreatePropertyGetMethod(
      TypeBuilder typeBuilder,
      PropertyBuilder propertyBuilder,
      PropertyDescriptor descriptor,
      MethodAttributes propAttribs)
    {
      MethodBuilder mdBuilder = typeBuilder.DefineMethod("get_" + descriptor.PropertyName, propAttribs, descriptor.PropertyType, (Type[]) null);
      ILGenerator ilGenerator = mdBuilder.GetILGenerator();
      Label label1 = ilGenerator.DefineLabel();
      Label label2 = ilGenerator.DefineLabel();
      Label label3 = ilGenerator.DefineLabel();
      DictionaryAdapterFactory.PreparePropertyMethod(descriptor, ilGenerator);
      LocalBuilder local = ilGenerator.DeclareLocal(descriptor.PropertyType);
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Ldloc_0);
      ilGenerator.Emit(OpCodes.Ldc_I4_0);
      ilGenerator.Emit(OpCodes.Callvirt, DictionaryAdapterFactory.AdapterGetProperty);
      ilGenerator.Emit(OpCodes.Stloc_1);
      ilGenerator.Emit(OpCodes.Ldloc_1);
      ilGenerator.Emit(OpCodes.Brfalse_S, label1);
      ilGenerator.Emit(OpCodes.Ldloc_1);
      ilGenerator.Emit(OpCodes.Unbox_Any, descriptor.PropertyType);
      ilGenerator.Emit(OpCodes.Br_S, label2);
      ilGenerator.MarkLabel(label1);
      ilGenerator.Emit(OpCodes.Ldloca_S, local);
      ilGenerator.Emit(OpCodes.Initobj, descriptor.PropertyType);
      ilGenerator.Emit(OpCodes.Br_S, label3);
      ilGenerator.MarkLabel(label2);
      ilGenerator.Emit(OpCodes.Stloc_S, local);
      ilGenerator.MarkLabel(label3);
      ilGenerator.Emit(OpCodes.Ldloc_S, local);
      ilGenerator.Emit(OpCodes.Ret);
      propertyBuilder.SetGetMethod(mdBuilder);
    }

    private static void CreatePropertySetMethod(
      TypeBuilder typeBuilder,
      PropertyBuilder propertyBuilder,
      PropertyDescriptor descriptor,
      MethodAttributes propAttribs)
    {
      MethodBuilder mdBuilder = typeBuilder.DefineMethod("set_" + descriptor.PropertyName, propAttribs, (Type) null, new Type[1]
      {
        descriptor.PropertyType
      });
      ILGenerator ilGenerator = mdBuilder.GetILGenerator();
      DictionaryAdapterFactory.PreparePropertyMethod(descriptor, ilGenerator);
      ilGenerator.Emit(OpCodes.Ldarg_1);
      if (descriptor.PropertyType.IsValueType)
        ilGenerator.Emit(OpCodes.Box, descriptor.PropertyType);
      ilGenerator.Emit(OpCodes.Stloc_1);
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Ldloc_0);
      ilGenerator.Emit(OpCodes.Ldloca_S, 1);
      ilGenerator.Emit(OpCodes.Callvirt, DictionaryAdapterFactory.AdapterSetProperty);
      ilGenerator.Emit(OpCodes.Pop);
      ilGenerator.Emit(OpCodes.Ret);
      propertyBuilder.SetSetMethod(mdBuilder);
    }

    private static Dictionary<string, PropertyDescriptor> GetPropertyDescriptors(
      Type type,
      out IDictionaryInitializer[] typeInitializers,
      out IDictionaryMetaInitializer[] metaInitializers,
      out object[] typeBehaviors)
    {
      Dictionary<string, PropertyDescriptor> propertyMap = new Dictionary<string, PropertyDescriptor>();
      object[] interfaceBehaviors = typeBehaviors = DictionaryAdapterFactory.ExpandBehaviors(DictionaryAdapterFactory.GetInterfaceBehaviors<object>(type)).ToArray<object>();
      typeInitializers = typeBehaviors.OfType<IDictionaryInitializer>().Prioritize<IDictionaryInitializer>().ToArray<IDictionaryInitializer>();
      metaInitializers = typeBehaviors.OfType<IDictionaryMetaInitializer>().Prioritize<IDictionaryMetaInitializer>().ToArray<IDictionaryMetaInitializer>();
      bool defaultFetch = typeBehaviors.OfType<FetchAttribute>().Select<FetchAttribute, bool>((Func<FetchAttribute, bool>) (b => b.Fetch)).FirstOrDefault<bool>();
      DictionaryAdapterFactory.CollectProperties(type, (Action<PropertyInfo>) (property =>
      {
        object[] array = DictionaryAdapterFactory.ExpandBehaviors(DictionaryAdapterFactory.GetPropertyBehaviors<object>((MemberInfo) property)).ToArray<object>();
        PropertyDescriptor propertyDescriptor1 = new PropertyDescriptor(property, array);
        foreach (IPropertyDescriptorInitializer descriptorInitializer in (IEnumerable<IPropertyDescriptorInitializer>) array.OfType<IPropertyDescriptorInitializer>().OrderBy<IPropertyDescriptorInitializer, int>((Func<IPropertyDescriptorInitializer, int>) (b => b.ExecutionOrder)))
          descriptorInitializer.Initialize(propertyDescriptor1, array);
        propertyDescriptor1.AddKeyBuilders(array.OfType<IDictionaryKeyBuilder>().Prioritize<IDictionaryKeyBuilder>(DictionaryAdapterFactory.GetInterfaceBehaviors<IDictionaryKeyBuilder>(property.ReflectedType)));
        propertyDescriptor1.AddGetters(array.OfType<IDictionaryPropertyGetter>().Prioritize<IDictionaryPropertyGetter>(interfaceBehaviors.OfType<IDictionaryPropertyGetter>()));
        DictionaryAdapterFactory.AddDefaultGetter(propertyDescriptor1);
        propertyDescriptor1.AddSetters(array.OfType<IDictionaryPropertySetter>().Prioritize<IDictionaryPropertySetter>(interfaceBehaviors.OfType<IDictionaryPropertySetter>()));
        bool? nullable = new bool?(array.OfType<FetchAttribute>().Select<FetchAttribute, bool>((Func<FetchAttribute, bool>) (b => b.Fetch)).FirstOrDefault<bool>());
        propertyDescriptor1.Fetch = nullable.GetValueOrDefault(defaultFetch);
        PropertyDescriptor propertyDescriptor2;
        if (propertyMap.TryGetValue(property.Name, out propertyDescriptor2) && propertyDescriptor2.Property.PropertyType == property.PropertyType)
        {
          if (!property.CanRead || !property.CanWrite)
            return;
          propertyMap[property.Name] = propertyDescriptor1;
        }
        else
          propertyMap.Add(property.Name, propertyDescriptor1);
      }));
      return propertyMap;
    }

    private static IEnumerable<T> GetInterfaceBehaviors<T>(Type type) where T : class
    {
      return (IEnumerable<T>) AttributesUtil.GetTypeAttributes<T>(type);
    }

    private static IEnumerable<T> GetPropertyBehaviors<T>(MemberInfo member) where T : class
    {
      return (IEnumerable<T>) member.GetAttributes<T>();
    }

    private static IEnumerable<object> ExpandBehaviors(IEnumerable<object> behaviors)
    {
      return behaviors.SelectMany<object, object>((Func<object, IEnumerable<object>>) (behavior => behavior is IDictionaryBehaviorBuilder ? ((IDictionaryBehaviorBuilder) behavior).BuildBehaviors().Cast<object>() : Enumerable.Repeat<object>(behavior, 1)));
    }

    private static void CollectProperties(Type currentType, Action<PropertyInfo> onProperty)
    {
      List<Type> typeList = new List<Type>();
      typeList.Add(currentType);
      typeList.AddRange((IEnumerable<Type>) currentType.GetInterfaces());
      BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public;
      foreach (Type type in typeList)
      {
        if (!DictionaryAdapterFactory.InfrastructureTypes.Contains(type))
        {
          foreach (PropertyInfo property in type.GetProperties(bindingAttr))
            onProperty(property);
        }
      }
    }

    private static void AddDefaultGetter(PropertyDescriptor descriptor)
    {
      if (descriptor.TypeConverter == null)
        return;
      descriptor.AddGetter((IDictionaryPropertyGetter) new DefaultPropertyGetter(descriptor.TypeConverter));
    }

    private string GetAdapterAssemblyName(Type type)
    {
      return type.Assembly.GetName().Name + "." + DictionaryAdapterFactory.GetSafeTypeFullName(type) + ".DictionaryAdapter";
    }

    private static string GetAdapterFullTypeName(Type type)
    {
      return type.Namespace + "." + DictionaryAdapterFactory.GetAdapterTypeName(type);
    }

    private static string GetAdapterTypeName(Type type)
    {
      return DictionaryAdapterFactory.GetSafeTypeName(type).Substring(1) + "DictionaryAdapter";
    }

    public static string GetSafeTypeFullName(Type type)
    {
      if (type.IsGenericTypeDefinition)
        return type.FullName.Replace("`", "_");
      if (!type.IsGenericType)
        return type.FullName;
      StringBuilder sb = new StringBuilder();
      if (!string.IsNullOrEmpty(type.Namespace))
        sb.Append(type.Namespace).Append(".");
      DictionaryAdapterFactory.AppendGenericTypeName(type, sb);
      return sb.ToString();
    }

    public static string GetSafeTypeName(Type type)
    {
      if (type.IsGenericTypeDefinition)
        return type.Name.Replace("`", "_");
      if (!type.IsGenericType)
        return type.Name;
      StringBuilder sb = new StringBuilder();
      DictionaryAdapterFactory.AppendGenericTypeName(type, sb);
      return sb.ToString();
    }

    private static void AppendGenericTypeName(Type type, StringBuilder sb)
    {
      sb.Append(type.Name.Replace("`", "_"));
      foreach (Type genericArgument in type.GetGenericArguments())
        sb.Append("_").Append(DictionaryAdapterFactory.GetSafeTypeFullName(genericArgument).Replace(".", "_"));
    }

    private static Type CreateAdapterType(Type type, Assembly assembly)
    {
      string adapterFullTypeName = DictionaryAdapterFactory.GetAdapterFullTypeName(type);
      return assembly.GetType(adapterFullTypeName, true);
    }

    private object CreateAdapterInstance(
      IDictionary dictionary,
      PropertyDescriptor descriptor,
      Type adapterType)
    {
      BindingFlags invokeAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField;
      DictionaryAdapterMeta meta = (DictionaryAdapterMeta) adapterType.InvokeMember("__meta", invokeAttr, (Binder) null, (object) null, (object[]) null);
      DictionaryAdapterInstance dictionaryAdapterInstance = new DictionaryAdapterInstance(dictionary, meta, descriptor, (IDictionaryAdapterFactory) this);
      return Activator.CreateInstance(adapterType, (object) dictionaryAdapterInstance);
    }
  }
}
