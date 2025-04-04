// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Deployment.AssemblyUtils
// Assembly: System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 9BAA0F73-1735-489C-B5F8-24E366CE85FF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Deployment.dll

using Microsoft.Internal.Web.Utils;
using Microsoft.Web.Infrastructure;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;

#nullable disable
namespace System.Web.WebPages.Deployment
{
  internal static class AssemblyUtils
  {
    private const string SharedLibPublicKey = "31bf3856ad364e35";
    internal static readonly AssemblyName ThisAssemblyName = new AssemblyName(typeof (AssemblyUtils).Assembly.FullName);
    internal static readonly Version WebPagesV1Version = new Version(1, 0, 0, 0);
    private static readonly string _binFileName = Path.GetFileName(AssemblyUtils.ThisAssemblyName.Name) + ".dll";
    private static readonly Version _mwiVersion = new AssemblyName(typeof (InfrastructureHelper).Assembly.FullName).Version;
    private static readonly AssemblyName _mwiAssemblyName = AssemblyUtils.GetFullName("Microsoft.Web.Infrastructure", AssemblyUtils._mwiVersion);
    private static readonly AssemblyName[] _version1AssemblyList = new AssemblyName[8]
    {
      AssemblyUtils._mwiAssemblyName,
      AssemblyUtils.GetFullName("System.Web.Razor", AssemblyUtils.WebPagesV1Version),
      AssemblyUtils.GetFullName("System.Web.Helpers", AssemblyUtils.WebPagesV1Version),
      AssemblyUtils.GetFullName("System.Web.WebPages", AssemblyUtils.WebPagesV1Version),
      AssemblyUtils.GetFullName("System.Web.WebPages.Administration", AssemblyUtils.WebPagesV1Version),
      AssemblyUtils.GetFullName("System.Web.WebPages.Razor", AssemblyUtils.WebPagesV1Version),
      AssemblyUtils.GetFullName("WebMatrix.Data", AssemblyUtils.WebPagesV1Version),
      AssemblyUtils.GetFullName("WebMatrix.WebData", AssemblyUtils.WebPagesV1Version)
    };
    private static readonly AssemblyName[] _versionCurrentAssemblyList = new AssemblyName[8]
    {
      AssemblyUtils._mwiAssemblyName,
      AssemblyUtils.GetFullName("System.Web.Razor", AssemblyUtils.ThisAssemblyName.Version),
      AssemblyUtils.GetFullName("System.Web.Helpers", AssemblyUtils.ThisAssemblyName.Version),
      AssemblyUtils.GetFullName("System.Web.WebPages", AssemblyUtils.ThisAssemblyName.Version),
      AssemblyUtils.GetFullName("System.Web.WebPages.Administration", AssemblyUtils.ThisAssemblyName.Version),
      AssemblyUtils.GetFullName("System.Web.WebPages.Razor", AssemblyUtils.ThisAssemblyName.Version),
      AssemblyUtils.GetFullName("WebMatrix.Data", AssemblyUtils.ThisAssemblyName.Version),
      AssemblyUtils.GetFullName("WebMatrix.WebData", AssemblyUtils.ThisAssemblyName.Version)
    };

    internal static Version GetMaxWebPagesVersion()
    {
      return AssemblyUtils.GetMaxWebPagesVersion(AssemblyUtils.GetLoadedAssemblies());
    }

    internal static Version GetMaxWebPagesVersion(IEnumerable<AssemblyName> loadedAssemblies)
    {
      return AssemblyUtils.GetWebPagesAssemblies(loadedAssemblies).Max<AssemblyName, Version>((Func<AssemblyName, Version>) (c => c.Version));
    }

    internal static bool IsVersionAvailable(Version version)
    {
      return AssemblyUtils.IsVersionAvailable(AssemblyUtils.GetLoadedAssemblies(), version);
    }

    internal static bool IsVersionAvailable(
      IEnumerable<AssemblyName> loadedAssemblies,
      Version version)
    {
      return AssemblyUtils.GetWebPagesAssemblies(loadedAssemblies).Any<AssemblyName>((Func<AssemblyName, bool>) (c => c.Version == version));
    }

    private static IEnumerable<AssemblyName> GetWebPagesAssemblies(
      IEnumerable<AssemblyName> loadedAssemblies)
    {
      return loadedAssemblies.Where<AssemblyName>((Func<AssemblyName, bool>) (otherName =>
      {
        bool matchVersion = false;
        return AssemblyUtils.NamesMatch(AssemblyUtils.ThisAssemblyName, otherName, matchVersion);
      }));
    }

    internal static Version GetVersionFromBin(
      string binDirectory,
      IFileSystem fileSystem,
      Func<string, AssemblyName> getAssemblyNameThunk = null)
    {
      string path = Path.Combine(binDirectory, AssemblyUtils._binFileName);
      if (fileSystem.FileExists(path))
      {
        try
        {
          getAssemblyNameThunk = getAssemblyNameThunk ?? new Func<string, AssemblyName>(AssemblyName.GetAssemblyName);
          AssemblyName right = getAssemblyNameThunk(path);
          bool matchVersion = false;
          if (AssemblyUtils.NamesMatch(AssemblyUtils.ThisAssemblyName, right, matchVersion))
            return right.Version;
        }
        catch (BadImageFormatException ex)
        {
        }
        catch (SecurityException ex)
        {
        }
        catch (FileLoadException ex)
        {
        }
      }
      return (Version) null;
    }

    internal static bool NamesMatch(AssemblyName left, AssemblyName right, bool matchVersion)
    {
      if (!object.Equals((object) left.Name, (object) right.Name) || !object.Equals((object) left.CultureInfo, (object) right.CultureInfo) || !((IEnumerable<byte>) left.GetPublicKeyToken()).SequenceEqual<byte>((IEnumerable<byte>) right.GetPublicKeyToken()))
        return false;
      return !matchVersion || object.Equals((object) left.Version, (object) right.Version);
    }

    internal static IEnumerable<AssemblyName> GetLoadedAssemblies()
    {
      return (IEnumerable<AssemblyName>) ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Select<Assembly, AssemblyName>(new Func<Assembly, AssemblyName>(AssemblyUtils.GetAssemblyName)).ToList<AssemblyName>();
    }

    internal static IEnumerable<AssemblyName> GetAssembliesForVersion(Version version)
    {
      return version == AssemblyUtils.WebPagesV1Version ? (IEnumerable<AssemblyName>) AssemblyUtils._version1AssemblyList : (IEnumerable<AssemblyName>) AssemblyUtils._versionCurrentAssemblyList;
    }

    private static AssemblyName GetAssemblyName(Assembly assembly)
    {
      return new AssemblyName(assembly.FullName);
    }

    private static AssemblyName GetFullName(string name, Version version, string publicKeyToken)
    {
      return new AssemblyName(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}, Version={1}, Culture=neutral, PublicKeyToken={2}", new object[3]
      {
        (object) name,
        (object) version,
        (object) publicKeyToken
      }));
    }

    internal static AssemblyName GetFullName(string name, Version version)
    {
      return AssemblyUtils.GetFullName(name, version, "31bf3856ad364e35");
    }

    public static IDictionary<string, Version> GetAssembliesMatchingOtherVersions(
      IDictionary<string, IEnumerable<string>> references)
    {
      IEnumerable<AssemblyName> webPagesAssemblies = AssemblyUtils.GetAssembliesForVersion(AssemblyUtils.ThisAssemblyName.Version);
      return references == null || webPagesAssemblies == null || !webPagesAssemblies.Any<AssemblyName>() ? (IDictionary<string, Version>) new Dictionary<string, Version>(0) : (IDictionary<string, Version>) references.Select(item => new
      {
        item = item,
        matchedVersion = AssemblyUtils.GetMatchingVersion(webPagesAssemblies, item.Value)
      }).Where(_param0 => _param0.matchedVersion != (Version) null).Select(_param0 => new KeyValuePair<string, Version>(_param0.item.Key, _param0.matchedVersion)).ToDictionary<KeyValuePair<string, Version>, string, Version>((Func<KeyValuePair<string, Version>, string>) (k => k.Key), (Func<KeyValuePair<string, Version>, Version>) (k => k.Value));
    }

    private static Version GetMatchingVersion(
      IEnumerable<AssemblyName> webPagesAssemblies,
      IEnumerable<string> references)
    {
      return webPagesAssemblies.SelectMany((Func<AssemblyName, IEnumerable<string>>) (webPagesAssembly => references), (webPagesAssembly, referenceName) => new
      {
        webPagesAssembly = webPagesAssembly,
        referenceName = referenceName
      }).Select(_param0 => new
      {
        \u003C\u003Eh__TransparentIdentifier13 = _param0,
        referencedAssembly = new AssemblyName(_param0.referenceName)
      }).Where(_param0 =>
      {
        bool matchVersion = false;
        return AssemblyUtils.NamesMatch(_param0.\u003C\u003Eh__TransparentIdentifier13.webPagesAssembly, _param0.referencedAssembly, matchVersion) && _param0.\u003C\u003Eh__TransparentIdentifier13.webPagesAssembly.Version != _param0.referencedAssembly.Version;
      }).Select(_param0 => _param0.referencedAssembly.Version).FirstOrDefault<Version>();
    }
  }
}
