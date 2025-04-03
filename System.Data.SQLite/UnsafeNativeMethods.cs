// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.UnsafeNativeMethods
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Xml;

#nullable disable
namespace System.Data.SQLite
{
  [SuppressUnmanagedCodeSecurity]
  internal static class UnsafeNativeMethods
  {
    internal const string SQLITE_DLL = "SQLite.Interop.dll";
    private static readonly string DllFileExtension = ".dll";
    private static readonly string ConfigFileExtension = ".config";
    private static readonly string XmlConfigFileName = typeof (UnsafeNativeMethods).Namespace + UnsafeNativeMethods.DllFileExtension + UnsafeNativeMethods.ConfigFileExtension;
    private static readonly object staticSyncRoot = new object();
    private static Dictionary<string, string> processorArchitecturePlatforms;
    private static readonly string PROCESSOR_ARCHITECTURE = nameof (PROCESSOR_ARCHITECTURE);
    internal static string _SQLiteNativeModuleFileName = (string) null;
    private static IntPtr _SQLiteNativeModuleHandle = IntPtr.Zero;

    static UnsafeNativeMethods() => UnsafeNativeMethods.Initialize();

    internal static void Initialize()
    {
      if (UnsafeNativeMethods.GetSettingValue("No_PreLoadSQLite", (string) null) != null)
        return;
      lock (UnsafeNativeMethods.staticSyncRoot)
      {
        if (UnsafeNativeMethods.processorArchitecturePlatforms == null)
        {
          UnsafeNativeMethods.processorArchitecturePlatforms = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
          UnsafeNativeMethods.processorArchitecturePlatforms.Add("x86", "Win32");
          UnsafeNativeMethods.processorArchitecturePlatforms.Add("AMD64", "x64");
          UnsafeNativeMethods.processorArchitecturePlatforms.Add("IA64", "Itanium");
          UnsafeNativeMethods.processorArchitecturePlatforms.Add("ARM", "WinCE");
        }
        if (!(UnsafeNativeMethods._SQLiteNativeModuleHandle == IntPtr.Zero))
          return;
        string baseDirectory = (string) null;
        string processorArchitecture = (string) null;
        UnsafeNativeMethods.SearchForDirectory(ref baseDirectory, ref processorArchitecture);
        UnsafeNativeMethods.PreLoadSQLiteDll(baseDirectory, processorArchitecture, ref UnsafeNativeMethods._SQLiteNativeModuleFileName, ref UnsafeNativeMethods._SQLiteNativeModuleHandle);
      }
    }

    private static string MaybeCombinePath(string path1, string path2)
    {
      return path1 != null ? (path2 != null ? Path.Combine(path1, path2) : path1) : (path2 != null ? path2 : (string) null);
    }

    private static string GetXmlConfigFileName()
    {
      string path1 = UnsafeNativeMethods.MaybeCombinePath(AppDomain.CurrentDomain.BaseDirectory, UnsafeNativeMethods.XmlConfigFileName);
      if (File.Exists(path1))
        return path1;
      string path2 = UnsafeNativeMethods.MaybeCombinePath(UnsafeNativeMethods.GetAssemblyDirectory(), UnsafeNativeMethods.XmlConfigFileName);
      return File.Exists(path2) ? path2 : (string) null;
    }

    private static string GetSettingValueViaXmlConfigFile(
      string fileName,
      string name,
      string @default,
      bool expand)
    {
      try
      {
        if (fileName == null || name == null)
          return @default;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(fileName);
        if (xmlDocument.SelectSingleNode(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "/configuration/appSettings/add[@key='{0}']", (object) name)) is XmlElement xmlElement)
        {
          string name1 = (string) null;
          if (xmlElement.HasAttribute("value"))
            name1 = xmlElement.GetAttribute("value");
          if (expand && !string.IsNullOrEmpty(name1))
            name1 = Environment.ExpandEnvironmentVariables(name1);
          if (name1 != null)
            return name1;
        }
      }
      catch (Exception ex)
      {
        try
        {
          Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to get setting \"{0}\" value from XML configuration file \"{1}\": {2}", (object) name, (object) fileName, (object) ex));
        }
        catch
        {
        }
      }
      return @default;
    }

    internal static string GetSettingValue(string name, string @default)
    {
      if (Environment.GetEnvironmentVariable("No_SQLiteGetSettingValue") != null || name == null)
        return @default;
      bool expand = true;
      if (Environment.GetEnvironmentVariable("No_Expand") != null)
        expand = false;
      else if (Environment.GetEnvironmentVariable(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "No_Expand_{0}", (object) name)) != null)
        expand = false;
      string name1 = Environment.GetEnvironmentVariable(name);
      if (expand && !string.IsNullOrEmpty(name1))
        name1 = Environment.ExpandEnvironmentVariables(name1);
      if (name1 != null)
        return name1;
      return Environment.GetEnvironmentVariable("No_SQLiteXmlConfigFile") != null ? @default : UnsafeNativeMethods.GetSettingValueViaXmlConfigFile(UnsafeNativeMethods.GetXmlConfigFileName(), name, @default, expand);
    }

    private static string ListToString(IList<string> list)
    {
      if (list == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in (IEnumerable<string>) list)
      {
        if (str != null)
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(' ');
          stringBuilder.Append(str);
        }
      }
      return stringBuilder.ToString();
    }

    private static int CheckForArchitecturesAndPlatforms(string directory, ref List<string> matches)
    {
      int num = 0;
      if (matches == null)
        matches = new List<string>();
      lock (UnsafeNativeMethods.staticSyncRoot)
      {
        if (!string.IsNullOrEmpty(directory))
        {
          if (UnsafeNativeMethods.processorArchitecturePlatforms != null)
          {
            foreach (KeyValuePair<string, string> architecturePlatform in UnsafeNativeMethods.processorArchitecturePlatforms)
            {
              if (Directory.Exists(UnsafeNativeMethods.MaybeCombinePath(directory, architecturePlatform.Key)))
              {
                matches.Add(architecturePlatform.Key);
                ++num;
              }
              string path2 = architecturePlatform.Value;
              if (path2 != null && Directory.Exists(UnsafeNativeMethods.MaybeCombinePath(directory, path2)))
              {
                matches.Add(path2);
                ++num;
              }
            }
          }
        }
      }
      return num;
    }

    private static bool CheckAssemblyCodeBase(Assembly assembly, ref string fileName)
    {
      try
      {
        if (assembly == (Assembly) null)
          return false;
        string codeBase = assembly.CodeBase;
        if (string.IsNullOrEmpty(codeBase))
          return false;
        string localPath = new Uri(codeBase).LocalPath;
        if (!File.Exists(localPath))
          return false;
        string directoryName = Path.GetDirectoryName(localPath);
        if (File.Exists(UnsafeNativeMethods.MaybeCombinePath(directoryName, UnsafeNativeMethods.XmlConfigFileName)))
        {
          fileName = localPath;
          return true;
        }
        List<string> matches = (List<string>) null;
        if (UnsafeNativeMethods.CheckForArchitecturesAndPlatforms(directoryName, ref matches) <= 0)
          return false;
        fileName = localPath;
        return true;
      }
      catch (Exception ex)
      {
        try
        {
          Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to check code base for currently executing assembly: {0}", (object) ex));
        }
        catch
        {
        }
      }
      return false;
    }

    private static string GetAssemblyDirectory()
    {
      try
      {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        if (executingAssembly == (Assembly) null)
          return (string) null;
        string fileName = (string) null;
        if (!UnsafeNativeMethods.CheckAssemblyCodeBase(executingAssembly, ref fileName))
          fileName = executingAssembly.Location;
        if (string.IsNullOrEmpty(fileName))
          return (string) null;
        string directoryName = Path.GetDirectoryName(fileName);
        return string.IsNullOrEmpty(directoryName) ? (string) null : directoryName;
      }
      catch (Exception ex)
      {
        try
        {
          Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to get directory for currently executing assembly: {0}", (object) ex));
        }
        catch
        {
        }
      }
      return (string) null;
    }

    internal static string GetNativeLibraryFileNameOnly()
    {
      return UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_LibraryFileNameOnly", (string) null) ?? "SQLite.Interop.dll";
    }

    private static bool SearchForDirectory(
      ref string baseDirectory,
      ref string processorArchitecture)
    {
      if (UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_NoSearchForDirectory", (string) null) != null)
        return false;
      string libraryFileNameOnly = UnsafeNativeMethods.GetNativeLibraryFileNameOnly();
      if (libraryFileNameOnly == null)
        return false;
      string[] strArray1 = new string[2]
      {
        UnsafeNativeMethods.GetAssemblyDirectory(),
        AppDomain.CurrentDomain.BaseDirectory
      };
      string[] strArray2 = new string[2]
      {
        UnsafeNativeMethods.GetProcessorArchitecture(),
        UnsafeNativeMethods.GetPlatformName((string) null)
      };
      foreach (string path1 in strArray1)
      {
        if (path1 != null)
        {
          foreach (string path2 in strArray2)
          {
            if (path2 != null && File.Exists(UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(UnsafeNativeMethods.MaybeCombinePath(path1, path2), libraryFileNameOnly))))
            {
              baseDirectory = path1;
              processorArchitecture = path2;
              return true;
            }
          }
        }
      }
      return false;
    }

    private static string GetBaseDirectory()
    {
      string settingValue = UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_BaseDirectory", (string) null);
      if (settingValue != null)
        return settingValue;
      if (UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_UseAssemblyDirectory", (string) null) != null)
      {
        string assemblyDirectory = UnsafeNativeMethods.GetAssemblyDirectory();
        if (assemblyDirectory != null)
          return assemblyDirectory;
      }
      return AppDomain.CurrentDomain.BaseDirectory;
    }

    private static string FixUpDllFileName(string fileName)
    {
      return !string.IsNullOrEmpty(fileName) && HelperMethods.IsWindows() && !fileName.EndsWith(UnsafeNativeMethods.DllFileExtension, StringComparison.OrdinalIgnoreCase) ? fileName + UnsafeNativeMethods.DllFileExtension : fileName;
    }

    private static string GetProcessorArchitecture()
    {
      string settingValue = UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_ProcessorArchitecture", (string) null);
      if (settingValue != null)
        return settingValue;
      string a = UnsafeNativeMethods.GetSettingValue(UnsafeNativeMethods.PROCESSOR_ARCHITECTURE, (string) null);
      if (IntPtr.Size == 4 && string.Equals(a, "AMD64", StringComparison.OrdinalIgnoreCase))
        a = "x86";
      return a;
    }

    private static string GetPlatformName(string processorArchitecture)
    {
      if (processorArchitecture == null)
        processorArchitecture = UnsafeNativeMethods.GetProcessorArchitecture();
      if (string.IsNullOrEmpty(processorArchitecture))
        return (string) null;
      lock (UnsafeNativeMethods.staticSyncRoot)
      {
        if (UnsafeNativeMethods.processorArchitecturePlatforms == null)
          return (string) null;
        string platformName;
        if (UnsafeNativeMethods.processorArchitecturePlatforms.TryGetValue(processorArchitecture, out platformName))
          return platformName;
      }
      return (string) null;
    }

    private static bool PreLoadSQLiteDll(
      string baseDirectory,
      string processorArchitecture,
      ref string nativeModuleFileName,
      ref IntPtr nativeModuleHandle)
    {
      if (baseDirectory == null)
        baseDirectory = UnsafeNativeMethods.GetBaseDirectory();
      if (baseDirectory == null)
        return false;
      string libraryFileNameOnly = UnsafeNativeMethods.GetNativeLibraryFileNameOnly();
      if (libraryFileNameOnly == null || File.Exists(UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(baseDirectory, libraryFileNameOnly))))
        return false;
      if (processorArchitecture == null)
        processorArchitecture = UnsafeNativeMethods.GetProcessorArchitecture();
      if (processorArchitecture == null)
        return false;
      string str = UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(UnsafeNativeMethods.MaybeCombinePath(baseDirectory, processorArchitecture), libraryFileNameOnly));
      if (!File.Exists(str))
      {
        string platformName = UnsafeNativeMethods.GetPlatformName(processorArchitecture);
        if (platformName == null)
          return false;
        str = UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(UnsafeNativeMethods.MaybeCombinePath(baseDirectory, platformName), libraryFileNameOnly));
        if (!File.Exists(str))
          return false;
      }
      try
      {
        try
        {
          Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader is trying to load native SQLite library \"{0}\"...", (object) str));
        }
        catch
        {
        }
        nativeModuleFileName = str;
        nativeModuleHandle = NativeLibraryHelper.LoadLibrary(str);
        return nativeModuleHandle != IntPtr.Zero;
      }
      catch (Exception ex)
      {
        try
        {
          int lastWin32Error = Marshal.GetLastWin32Error();
          Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to load native SQLite library \"{0}\" (getLastError = {1}): {2}", (object) str, (object) lastWin32Error, (object) ex));
        }
        catch
        {
        }
      }
      return false;
    }

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_bind_parameter_name_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_database_name_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_database_name16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_decltype_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_decltype16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_name_interop(IntPtr stmt, int index, ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_name16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_origin_name_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_origin_name16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_table_name_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_table_name16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_text_interop(IntPtr stmt, int index, ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_text16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_errmsg_interop(IntPtr db, ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_prepare_interop(
      IntPtr db,
      IntPtr pSql,
      int nBytes,
      ref IntPtr stmt,
      ref IntPtr ptrRemain,
      ref int nRemain);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_table_column_metadata_interop(
      IntPtr db,
      byte[] dbName,
      byte[] tblName,
      byte[] colName,
      ref IntPtr ptrDataType,
      ref IntPtr ptrCollSeq,
      ref int notNull,
      ref int primaryKey,
      ref int autoInc,
      ref int dtLen,
      ref int csLen);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_value_text_interop(IntPtr p, ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_value_text16_interop(IntPtr p, ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern int sqlite3_malloc_size_interop(IntPtr p);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr interop_libversion();

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr interop_sourceid();

    [DllImport("SQLite.Interop.dll")]
    internal static extern int interop_compileoption_used(IntPtr zOptName);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr interop_compileoption_get(int N);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_close_interop(IntPtr db);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_create_function_interop(
      IntPtr db,
      byte[] strName,
      int nArgs,
      int nType,
      IntPtr pvUser,
      SQLiteCallback func,
      SQLiteCallback fstep,
      SQLiteFinalCallback ffinal,
      int needCollSeq);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_finalize_interop(IntPtr stmt);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_backup_finish_interop(IntPtr backup);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_blob_close_interop(IntPtr blob);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_open_interop(
      byte[] utf8Filename,
      byte[] vfsName,
      SQLiteOpenFlagsEnum flags,
      int extFuncs,
      ref IntPtr db);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_open16_interop(
      byte[] utf8Filename,
      byte[] vfsName,
      SQLiteOpenFlagsEnum flags,
      int extFuncs,
      ref IntPtr db);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_reset_interop(IntPtr stmt);

    [DllImport("SQLite.Interop.dll")]
    internal static extern int sqlite3_changes_interop(IntPtr db);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_context_collseq_interop(
      IntPtr context,
      ref int type,
      ref int enc,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern int sqlite3_context_collcompare_interop(
      IntPtr context,
      byte[] p1,
      int p1len,
      byte[] p2,
      int p2len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_cursor_rowid_interop(
      IntPtr stmt,
      int cursor,
      ref long rowid);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_index_column_info_interop(
      IntPtr db,
      byte[] catalog,
      byte[] IndexName,
      byte[] ColumnName,
      ref int sortOrder,
      ref int onError,
      ref IntPtr Collation,
      ref int colllen);

    [DllImport("SQLite.Interop.dll")]
    internal static extern int sqlite3_table_cursor_interop(IntPtr stmt, int db, int tableRootPage);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_libversion();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_libversion_number();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_sourceid();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_compileoption_used(IntPtr zOptName);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_compileoption_get(int N);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_enable_shared_cache(int enable);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_enable_load_extension(IntPtr db, int enable);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_load_extension(
      IntPtr db,
      byte[] fileName,
      byte[] procName,
      ref IntPtr pError);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_overload_function(
      IntPtr db,
      IntPtr zName,
      int nArgs);

    [DllImport("SQLite.Interop.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_win32_set_directory(uint type, string value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_win32_reset_heap();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_win32_compact_heap(ref uint largest);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_malloc(int n);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_realloc(IntPtr p, int n);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_free(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_open_v2(
      byte[] utf8Filename,
      ref IntPtr db,
      SQLiteOpenFlagsEnum flags,
      byte[] vfsName);

    [DllImport("SQLite.Interop.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_open16(string fileName, ref IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_interrupt(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern long sqlite3_last_insert_rowid(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_changes(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern long sqlite3_memory_used();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern long sqlite3_memory_highwater(int resetFlag);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_shutdown();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_busy_timeout(IntPtr db, int ms);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_clear_bindings(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_blob(
      IntPtr stmt,
      int index,
      byte[] value,
      int nSize,
      IntPtr nTransient);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_double(
      IntPtr stmt,
      int index,
      double value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_int(IntPtr stmt, int index, int value);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_bind_int", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_uint(IntPtr stmt, int index, uint value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_int64(IntPtr stmt, int index, long value);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_bind_int64", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_uint64(IntPtr stmt, int index, ulong value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_null(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_text(
      IntPtr stmt,
      int index,
      byte[] value,
      int nlen,
      IntPtr pvReserved);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_bind_parameter_count(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_bind_parameter_index(IntPtr stmt, byte[] strName);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_column_count(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_step(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_stmt_readonly(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern double sqlite3_column_double(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_column_int(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern long sqlite3_column_int64(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_column_blob(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_column_bytes(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_column_bytes16(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern TypeAffinity sqlite3_column_type(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_create_collation(
      IntPtr db,
      byte[] strName,
      int nType,
      IntPtr pvUser,
      SQLiteCollation func);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_aggregate_count(IntPtr context);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_value_blob(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_value_bytes(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_value_bytes16(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern double sqlite3_value_double(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_value_int(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern long sqlite3_value_int64(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern TypeAffinity sqlite3_value_type(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_blob(
      IntPtr context,
      byte[] value,
      int nSize,
      IntPtr pvReserved);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_double(IntPtr context, double value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_error(IntPtr context, byte[] strErr, int nLen);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_error_code(IntPtr context, SQLiteErrorCode value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_error_toobig(IntPtr context);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_error_nomem(IntPtr context);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_value(IntPtr context, IntPtr value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_zeroblob(IntPtr context, int nLen);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_int(IntPtr context, int value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_int64(IntPtr context, long value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_null(IntPtr context);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_text(
      IntPtr context,
      byte[] value,
      int nLen,
      IntPtr pvReserved);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_aggregate_context(IntPtr context, int nBytes);

    [DllImport("SQLite.Interop.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_text16(
      IntPtr stmt,
      int index,
      string value,
      int nlen,
      IntPtr pvReserved);

    [DllImport("SQLite.Interop.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_error16(IntPtr context, string strName, int nLen);

    [DllImport("SQLite.Interop.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_text16(
      IntPtr context,
      string strName,
      int nLen,
      IntPtr pvReserved);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_key(IntPtr db, byte[] key, int keylen);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_rekey(IntPtr db, byte[] key, int keylen);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_progress_handler(
      IntPtr db,
      int ops,
      SQLiteProgressCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_set_authorizer(
      IntPtr db,
      SQLiteAuthorizerCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_update_hook(
      IntPtr db,
      SQLiteUpdateCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_commit_hook(
      IntPtr db,
      SQLiteCommitCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_trace(IntPtr db, SQLiteTraceCallback func, IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_config", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_config_none(SQLiteConfigOpsEnum op);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_config", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_config_int(SQLiteConfigOpsEnum op, int value);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_config", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_config_log(
      SQLiteConfigOpsEnum op,
      SQLiteLogCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_db_config", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_db_config_int_refint(
      IntPtr db,
      SQLiteConfigDbOpsEnum op,
      int value,
      ref int result);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_rollback_hook(
      IntPtr db,
      SQLiteRollbackCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_db_handle(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_db_release_memory(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_db_filename(IntPtr db, IntPtr dbName);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_db_readonly(IntPtr db, IntPtr dbName);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_db_filename", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_db_filename_bytes(IntPtr db, byte[] dbName);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_next_stmt(IntPtr db, IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_exec(
      IntPtr db,
      byte[] strSql,
      IntPtr pvCallback,
      IntPtr pvParam,
      ref IntPtr errMsg);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_release_memory(int nBytes);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_get_autocommit(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_extended_result_codes(IntPtr db, int onoff);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_errcode(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_extended_errcode(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_errstr(SQLiteErrorCode rc);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_log(SQLiteErrorCode iErrCode, byte[] zFormat);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_file_control(
      IntPtr db,
      byte[] zDbName,
      int op,
      IntPtr pArg);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_backup_init(
      IntPtr destDb,
      byte[] zDestName,
      IntPtr sourceDb,
      byte[] zSourceName);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_backup_step(IntPtr backup, int nPage);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_backup_remaining(IntPtr backup);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_backup_pagecount(IntPtr backup);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_blob_close(IntPtr blob);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_blob_bytes(IntPtr blob);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_blob_open(
      IntPtr db,
      byte[] dbName,
      byte[] tblName,
      byte[] colName,
      long rowId,
      int flags,
      ref IntPtr ptrBlob);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_blob_read(
      IntPtr blob,
      [MarshalAs(UnmanagedType.LPArray)] byte[] buffer,
      int count,
      int offset);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_blob_reopen(IntPtr blob, long rowId);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_blob_write(
      IntPtr blob,
      [MarshalAs(UnmanagedType.LPArray)] byte[] buffer,
      int count,
      int offset);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_declare_vtab(IntPtr db, IntPtr zSQL);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_mprintf(IntPtr format, __arglist);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_create_disposable_module(
      IntPtr db,
      IntPtr name,
      ref UnsafeNativeMethods.sqlite3_module module,
      IntPtr pClientData,
      UnsafeNativeMethods.xDestroyModule xDestroy);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_dispose_module(IntPtr pModule);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xCreate(
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xConnect(
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xBestIndex(IntPtr pVtab, IntPtr pIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xDisconnect(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xDestroy(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xOpen(IntPtr pVtab, ref IntPtr pCursor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xClose(IntPtr pCursor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xFilter(
      IntPtr pCursor,
      int idxNum,
      IntPtr idxStr,
      int argc,
      IntPtr argv);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xNext(IntPtr pCursor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xEof(IntPtr pCursor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xColumn(IntPtr pCursor, IntPtr pContext, int index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xRowId(IntPtr pCursor, ref long rowId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xUpdate(IntPtr pVtab, int argc, IntPtr argv, ref long rowId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xBegin(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xSync(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xCommit(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xRollback(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xFindFunction(
      IntPtr pVtab,
      int nArg,
      IntPtr zName,
      ref SQLiteCallback callback,
      ref IntPtr pUserData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xRename(IntPtr pVtab, IntPtr zNew);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xSavepoint(IntPtr pVtab, int iSavepoint);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xRelease(IntPtr pVtab, int iSavepoint);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xRollbackTo(IntPtr pVtab, int iSavepoint);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void xDestroyModule(IntPtr pClientData);

    internal struct sqlite3_module
    {
      public int iVersion;
      public UnsafeNativeMethods.xCreate xCreate;
      public UnsafeNativeMethods.xConnect xConnect;
      public UnsafeNativeMethods.xBestIndex xBestIndex;
      public UnsafeNativeMethods.xDisconnect xDisconnect;
      public UnsafeNativeMethods.xDestroy xDestroy;
      public UnsafeNativeMethods.xOpen xOpen;
      public UnsafeNativeMethods.xClose xClose;
      public UnsafeNativeMethods.xFilter xFilter;
      public UnsafeNativeMethods.xNext xNext;
      public UnsafeNativeMethods.xEof xEof;
      public UnsafeNativeMethods.xColumn xColumn;
      public UnsafeNativeMethods.xRowId xRowId;
      public UnsafeNativeMethods.xUpdate xUpdate;
      public UnsafeNativeMethods.xBegin xBegin;
      public UnsafeNativeMethods.xSync xSync;
      public UnsafeNativeMethods.xCommit xCommit;
      public UnsafeNativeMethods.xRollback xRollback;
      public UnsafeNativeMethods.xFindFunction xFindFunction;
      public UnsafeNativeMethods.xRename xRename;
      public UnsafeNativeMethods.xSavepoint xSavepoint;
      public UnsafeNativeMethods.xRelease xRelease;
      public UnsafeNativeMethods.xRollbackTo xRollbackTo;
    }

    internal struct sqlite3_vtab
    {
      public IntPtr pModule;
      public int nRef;
      public IntPtr zErrMsg;
    }

    internal struct sqlite3_vtab_cursor
    {
      public IntPtr pVTab;
    }

    internal struct sqlite3_index_constraint
    {
      public int iColumn;
      public SQLiteIndexConstraintOp op;
      public byte usable;
      public int iTermOffset;

      public sqlite3_index_constraint(SQLiteIndexConstraint constraint)
        : this()
      {
        if (constraint == null)
          return;
        this.iColumn = constraint.iColumn;
        this.op = constraint.op;
        this.usable = constraint.usable;
        this.iTermOffset = constraint.iTermOffset;
      }
    }

    internal struct sqlite3_index_orderby
    {
      public int iColumn;
      public byte desc;

      public sqlite3_index_orderby(SQLiteIndexOrderBy orderBy)
        : this()
      {
        if (orderBy == null)
          return;
        this.iColumn = orderBy.iColumn;
        this.desc = orderBy.desc;
      }
    }

    internal struct sqlite3_index_constraint_usage
    {
      public int argvIndex;
      public byte omit;

      public sqlite3_index_constraint_usage(SQLiteIndexConstraintUsage constraintUsage)
        : this()
      {
        if (constraintUsage == null)
          return;
        this.argvIndex = constraintUsage.argvIndex;
        this.omit = constraintUsage.omit;
      }
    }

    internal struct sqlite3_index_info
    {
      public int nConstraint;
      public IntPtr aConstraint;
      public int nOrderBy;
      public IntPtr aOrderBy;
      public IntPtr aConstraintUsage;
      public int idxNum;
      public string idxStr;
      public int needToFreeIdxStr;
      public int orderByConsumed;
      public double estimatedCost;
      public long estimatedRows;
      public SQLiteIndexFlags idxFlags;
      public long colUsed;
    }
  }
}
