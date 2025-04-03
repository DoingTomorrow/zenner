// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.ModuleScope
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Internal;
using Castle.DynamicProxy.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;

#nullable disable
namespace Castle.DynamicProxy
{
  public class ModuleScope
  {
    public static readonly string DEFAULT_FILE_NAME = "CastleDynProxy2.dll";
    public static readonly string DEFAULT_ASSEMBLY_NAME = "DynamicProxyGenAssembly2";
    private ModuleBuilder moduleBuilderWithStrongName;
    private ModuleBuilder moduleBuilder;
    private readonly string strongAssemblyName;
    private readonly string weakAssemblyName;
    private readonly string strongModulePath;
    private readonly string weakModulePath;
    private readonly Dictionary<CacheKey, Type> typeCache = new Dictionary<CacheKey, Type>();
    private readonly Lock cacheLock = Lock.Create();
    private readonly object moduleLocker = new object();
    private readonly bool savePhysicalAssembly;
    private readonly bool disableSignedModule;
    private readonly INamingScope namingScope;

    public ModuleScope()
      : this(false, false)
    {
    }

    public ModuleScope(bool savePhysicalAssembly)
      : this(savePhysicalAssembly, false)
    {
    }

    public ModuleScope(bool savePhysicalAssembly, bool disableSignedModule)
      : this(savePhysicalAssembly, disableSignedModule, ModuleScope.DEFAULT_ASSEMBLY_NAME, ModuleScope.DEFAULT_FILE_NAME, ModuleScope.DEFAULT_ASSEMBLY_NAME, ModuleScope.DEFAULT_FILE_NAME)
    {
    }

    public ModuleScope(
      bool savePhysicalAssembly,
      bool disableSignedModule,
      string strongAssemblyName,
      string strongModulePath,
      string weakAssemblyName,
      string weakModulePath)
      : this(savePhysicalAssembly, disableSignedModule, (INamingScope) new Castle.DynamicProxy.Generators.NamingScope(), strongAssemblyName, strongModulePath, weakAssemblyName, weakModulePath)
    {
    }

    public ModuleScope(
      bool savePhysicalAssembly,
      bool disableSignedModule,
      INamingScope namingScope,
      string strongAssemblyName,
      string strongModulePath,
      string weakAssemblyName,
      string weakModulePath)
    {
      this.savePhysicalAssembly = savePhysicalAssembly;
      this.disableSignedModule = disableSignedModule;
      this.namingScope = namingScope;
      this.strongAssemblyName = strongAssemblyName;
      this.strongModulePath = strongModulePath;
      this.weakAssemblyName = weakAssemblyName;
      this.weakModulePath = weakModulePath;
    }

    public INamingScope NamingScope => this.namingScope;

    public Lock Lock => this.cacheLock;

    public Type GetFromCache(CacheKey key)
    {
      Type fromCache;
      this.typeCache.TryGetValue(key, out fromCache);
      return fromCache;
    }

    public void RegisterInCache(CacheKey key, Type type) => this.typeCache[key] = type;

    public static byte[] GetKeyPair()
    {
      using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Castle.DynamicProxy.DynProxy.snk"))
      {
        int count = manifestResourceStream != null ? (int) manifestResourceStream.Length : throw new MissingManifestResourceException("Should have a Castle.DynamicProxy.DynProxy.snk as an embedded resource, so Dynamic Proxy could sign generated assembly");
        byte[] buffer = new byte[count];
        manifestResourceStream.Read(buffer, 0, count);
        return buffer;
      }
    }

    public ModuleBuilder StrongNamedModule => this.moduleBuilderWithStrongName;

    public string StrongNamedModuleName => Path.GetFileName(this.strongModulePath);

    public string StrongNamedModuleDirectory
    {
      get
      {
        string directoryName = Path.GetDirectoryName(this.strongModulePath);
        return string.IsNullOrEmpty(directoryName) ? (string) null : directoryName;
      }
    }

    public ModuleBuilder WeakNamedModule => this.moduleBuilder;

    public string WeakNamedModuleName => Path.GetFileName(this.weakModulePath);

    public string WeakNamedModuleDirectory
    {
      get
      {
        string directoryName = Path.GetDirectoryName(this.weakModulePath);
        return directoryName == string.Empty ? (string) null : directoryName;
      }
    }

    public ModuleBuilder ObtainDynamicModule(bool isStrongNamed)
    {
      return isStrongNamed ? this.ObtainDynamicModuleWithStrongName() : this.ObtainDynamicModuleWithWeakName();
    }

    public ModuleBuilder ObtainDynamicModuleWithStrongName()
    {
      if (this.disableSignedModule)
        throw new InvalidOperationException("Usage of signed module has been disabled. Use unsigned module or enable signed module.");
      lock (this.moduleLocker)
      {
        if (this.moduleBuilderWithStrongName == null)
          this.moduleBuilderWithStrongName = this.CreateModule(true);
        return this.moduleBuilderWithStrongName;
      }
    }

    public ModuleBuilder ObtainDynamicModuleWithWeakName()
    {
      lock (this.moduleLocker)
      {
        if (this.moduleBuilder == null)
          this.moduleBuilder = this.CreateModule(false);
        return this.moduleBuilder;
      }
    }

    private ModuleBuilder CreateModule(bool signStrongName)
    {
      AssemblyName assemblyName = this.GetAssemblyName(signStrongName);
      string str = signStrongName ? this.StrongNamedModuleName : this.WeakNamedModuleName;
      string dir = signStrongName ? this.StrongNamedModuleDirectory : this.WeakNamedModuleDirectory;
      if (!this.savePhysicalAssembly)
        return AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(str, false);
      AssemblyBuilder assemblyBuilder;
      try
      {
        assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave, dir);
      }
      catch (ArgumentException ex)
      {
        if (signStrongName || ex.StackTrace.Contains("ComputePublicKey"))
          throw new ArgumentException(string.Format("There was an error creating dynamic assembly for your proxies - you don't have permissions required to sing the assembly. To workaround it you can enfornce generating non-signed assembly only when creating {0}. ALternatively ensure that your account has all the required permissions.", (object) this.GetType()), (Exception) ex);
        throw;
      }
      return assemblyBuilder.DefineDynamicModule(str, str, false);
    }

    private AssemblyName GetAssemblyName(bool signStrongName)
    {
      AssemblyName assemblyName = new AssemblyName()
      {
        Name = signStrongName ? this.strongAssemblyName : this.weakAssemblyName
      };
      if (signStrongName)
      {
        byte[] keyPair = ModuleScope.GetKeyPair();
        if (keyPair != null)
          assemblyName.KeyPair = new StrongNameKeyPair(keyPair);
      }
      return assemblyName;
    }

    public string SaveAssembly()
    {
      if (!this.savePhysicalAssembly)
        return (string) null;
      if (this.StrongNamedModule != null && this.WeakNamedModule != null)
        throw new InvalidOperationException("Both a strong-named and a weak-named assembly have been generated.");
      if (this.StrongNamedModule != null)
        return this.SaveAssembly(true);
      return this.WeakNamedModule != null ? this.SaveAssembly(false) : (string) null;
    }

    public string SaveAssembly(bool strongNamed)
    {
      if (!this.savePhysicalAssembly)
        return (string) null;
      AssemblyBuilder builder;
      string assemblyFileName;
      string fullyQualifiedName;
      if (strongNamed)
      {
        builder = this.StrongNamedModule != null ? (AssemblyBuilder) this.StrongNamedModule.Assembly : throw new InvalidOperationException("No strong-named assembly has been generated.");
        assemblyFileName = this.StrongNamedModuleName;
        fullyQualifiedName = this.StrongNamedModule.FullyQualifiedName;
      }
      else
      {
        builder = this.WeakNamedModule != null ? (AssemblyBuilder) this.WeakNamedModule.Assembly : throw new InvalidOperationException("No weak-named assembly has been generated.");
        assemblyFileName = this.WeakNamedModuleName;
        fullyQualifiedName = this.WeakNamedModule.FullyQualifiedName;
      }
      if (File.Exists(fullyQualifiedName))
        File.Delete(fullyQualifiedName);
      this.AddCacheMappings(builder);
      builder.Save(assemblyFileName);
      return fullyQualifiedName;
    }

    private void AddCacheMappings(AssemblyBuilder builder)
    {
      Dictionary<CacheKey, string> mappings;
      using (this.Lock.ForReading())
      {
        mappings = new Dictionary<CacheKey, string>();
        foreach (KeyValuePair<CacheKey, Type> keyValuePair in this.typeCache)
          mappings.Add(keyValuePair.Key, keyValuePair.Value.FullName);
      }
      CacheMappingsAttribute.ApplyTo(builder, mappings);
    }

    public void LoadAssemblyIntoCache(Assembly assembly)
    {
      CacheMappingsAttribute[] mappingsAttributeArray = assembly != null ? (CacheMappingsAttribute[]) assembly.GetCustomAttributes(typeof (CacheMappingsAttribute), false) : throw new ArgumentNullException(nameof (assembly));
      if (mappingsAttributeArray.Length == 0)
        throw new ArgumentException(string.Format("The given assembly '{0}' does not contain any cache information for generated types.", (object) assembly.FullName), nameof (assembly));
      foreach (KeyValuePair<CacheKey, string> deserializedMapping in mappingsAttributeArray[0].GetDeserializedMappings())
      {
        Type type = assembly.GetType(deserializedMapping.Value);
        if (type != null)
          this.RegisterInCache(deserializedMapping.Key, type);
      }
    }

    public TypeBuilder DefineType(bool inSignedModulePreferably, string name, TypeAttributes flags)
    {
      return this.ObtainDynamicModule(!this.disableSignedModule && inSignedModulePreferably).DefineType(name, flags);
    }
  }
}
