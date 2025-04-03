// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.ProbingAdapterResolver
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace AutoMapper.Internal
{
  internal class ProbingAdapterResolver : IAdapterResolver
  {
    private readonly string[] _platformNames;
    private readonly Func<string, Assembly> _assemblyLoader;
    private readonly object _lock = new object();
    private readonly Dictionary<Type, object> _adapters = new Dictionary<Type, object>();
    private Assembly _assembly;
    private bool _probed;

    public ProbingAdapterResolver(params string[] platformNames)
      : this(new Func<string, Assembly>(Assembly.Load), platformNames)
    {
    }

    public ProbingAdapterResolver(
      Func<string, Assembly> assemblyLoader,
      params string[] platformNames)
    {
      this._platformNames = platformNames;
      this._assemblyLoader = assemblyLoader;
    }

    public object Resolve(Type type)
    {
      lock (this._lock)
      {
        object obj;
        if (!this._adapters.TryGetValue(type, out obj))
        {
          obj = ProbingAdapterResolver.ResolveAdapter(this.GetPlatformSpecificAssembly(), type);
          this._adapters.Add(type, obj);
        }
        return obj;
      }
    }

    private static object ResolveAdapter(Assembly assembly, Type interfaceType)
    {
      string name = ProbingAdapterResolver.MakeAdapterTypeName(interfaceType);
      if ((object) assembly != null)
      {
        Type type1 = assembly.GetType(name + "Override");
        if ((object) type1 != null)
          return Activator.CreateInstance(type1);
        Type type2 = assembly.GetType(name);
        if ((object) type2 != null)
          return Activator.CreateInstance(type2);
      }
      Type type = typeof (ProbingAdapterResolver).Assembly.GetType(name);
      return (object) type == null ? (object) null : Activator.CreateInstance(type);
    }

    private static string MakeAdapterTypeName(Type interfaceType)
    {
      return interfaceType.Namespace + "." + interfaceType.Name.Substring(1);
    }

    private Assembly GetPlatformSpecificAssembly()
    {
      if ((object) this._assembly == null && !this._probed)
      {
        this._probed = true;
        this._assembly = this.ProbeForPlatformSpecificAssembly();
      }
      return this._assembly;
    }

    private Assembly ProbeForPlatformSpecificAssembly()
    {
      return ((IEnumerable<string>) this._platformNames).Select<string, Assembly>(new Func<string, Assembly>(this.ProbeForPlatformSpecificAssembly)).FirstOrDefault<Assembly>((Func<Assembly, bool>) (assembly => (object) assembly != null));
    }

    private Assembly ProbeForPlatformSpecificAssembly(string platformName)
    {
      AssemblyName assemblyName = new AssemblyName(this.GetType().Assembly.FullName)
      {
        Name = "AutoMapper." + platformName
      };
      try
      {
        return this._assemblyLoader(assemblyName.ToString());
      }
      catch (FileNotFoundException ex)
      {
      }
      catch (Exception ex1)
      {
        assemblyName.SetPublicKey((byte[]) null);
        assemblyName.SetPublicKeyToken((byte[]) null);
        try
        {
          return this._assemblyLoader(assemblyName.ToString());
        }
        catch (Exception ex2)
        {
          return (Assembly) null;
        }
      }
      return (Assembly) null;
    }
  }
}
