// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeEngine
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SqlServerCe
{
  public sealed class SqlCeEngine : IDisposable
  {
    private Hashtable connTokens;
    private string connStr;
    private bool isDisposed;

    public SqlCeEngine()
    {
      NativeMethods.LoadNativeBinaries();
      this.isDisposed = false;
      NativeMethods.DllAddRef();
      this.LocalConnectionString = string.Empty;
    }

    public SqlCeEngine(string connectionString)
      : this()
    {
      this.LocalConnectionString = connectionString;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~SqlCeEngine() => this.Dispose(false);

    private void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.connStr = (string) null;
      this.connTokens = (Hashtable) null;
      NativeMethods.DllRelease();
      this.isDisposed = true;
    }

    public string LocalConnectionString
    {
      get
      {
        if (string.IsNullOrEmpty(this.connStr) || string.IsNullOrEmpty(this.connStr.Trim()))
          this.connStr = string.Empty;
        return this.connStr;
      }
      set
      {
        if (this.isDisposed)
          throw new ObjectDisposedException(nameof (SqlCeEngine));
        this.connTokens = ConStringUtil.ParseConnectionString(ref value);
        this.connStr = value;
      }
    }

    public void Compact(string connectionString)
    {
      this.Repair(SEFIXOPTION.SEFIX_OPTION_COMPACT, connectionString, RepairOption.DeleteCorruptedRows);
    }

    public void Shrink()
    {
      this.Repair(SEFIXOPTION.SEFIX_OPTION_SHRINK, (string) null, RepairOption.DeleteCorruptedRows);
    }

    public void Repair(string connectionString, RepairOption options)
    {
      this.Repair(SEFIXOPTION.SEFIX_OPTION_REPAIR, connectionString, options);
    }

    public bool Verify() => this.Verify(VerifyOption.Default);

    public bool Verify(VerifyOption option)
    {
      try
      {
        switch (option)
        {
          case VerifyOption.Default:
            this.Repair(SEFIXOPTION.SEFIX_OPTION_VERIFY, (string) null, RepairOption.DeleteCorruptedRows);
            break;
          case VerifyOption.Enhanced:
            this.Repair(SEFIXOPTION.SEFIX_OPTION_VERIFYEX, (string) null, RepairOption.DeleteCorruptedRows);
            break;
        }
        return true;
      }
      catch (SqlCeException ex)
      {
        return false;
      }
    }

    private void Repair(SEFIXOPTION option, string dstConnStr, RepairOption repairOption)
    {
      int lcid = 0;
      string str1 = (string) null;
      IntPtr zero = IntPtr.Zero;
      IntPtr num1 = IntPtr.Zero;
      IntPtr num2 = IntPtr.Zero;
      IntPtr num3 = IntPtr.Zero;
      IntPtr num4 = IntPtr.Zero;
      IntPtr num5 = IntPtr.Zero;
      string source1 = (string) null;
      string source2 = (string) null;
      string source3 = (string) null;
      string source4 = (string) null;
      string source5 = (string) null;
      int fEncrypt = 0;
      bool flag1 = false;
      bool flag2 = true;
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeEngine));
      if (this.connTokens != null)
      {
        if (!string.IsNullOrEmpty(this.LocalConnectionString))
        {
          try
          {
            object connToken1 = this.connTokens[(object) "ssce:database password"];
            if (connToken1 != null)
            {
              string str2 = ((string) connToken1).Trim();
              if (str2.Length > 0)
                source4 = str2;
            }
            if (dstConnStr != null)
            {
              Hashtable connectionString = ConStringUtil.ParseConnectionString(ref dstConnStr);
              object obj1 = connectionString[(object) "data source"];
              if (obj1 != null)
              {
                string str3 = ((string) obj1).Trim();
                if (str3.Length > 0)
                  source2 = str3;
              }
              object obj2 = connectionString[(object) "ssce:database password"];
              if (obj2 != null)
              {
                string str4 = ((string) obj2).Trim();
                if (str4.Length > 0)
                  source5 = str4;
              }
              object obj3 = connectionString[(object) "ssce:temp file directory"];
              if (obj3 != null)
                source3 = (string) obj3;
              object obj4 = connectionString[(object) "locale identifier"];
              if (obj4 != null)
                lcid = (int) obj4;
              object obj5 = connectionString[(object) "ssce:encryption mode"];
              if (obj5 != null)
                str1 = (string) obj5;
              object obj6 = connectionString[(object) "ssce:encrypt database"];
              if (obj6 != null)
                fEncrypt = (bool) obj6 ? 1 : 0;
              object obj7 = connectionString[(object) "ssce:case sensitive"];
              if (obj7 != null)
                flag1 = (bool) obj7;
              else
                flag2 = false;
            }
            else
            {
              source2 = (string) null;
              object connToken2 = this.connTokens[(object) "ssce:database password"];
              if (connToken2 != null)
              {
                string str5 = ((string) connToken2).Trim();
                if (str5.Length > 0)
                  source5 = str5;
              }
              object connToken3 = this.connTokens[(object) "ssce:temp file directory"];
              if (connToken3 != null)
                source3 = (string) connToken3;
              object connToken4 = this.connTokens[(object) "locale identifier"];
              if (connToken4 != null)
                lcid = (int) connToken4;
              object connToken5 = this.connTokens[(object) "ssce:encrypt database"];
              if (connToken5 != null)
                fEncrypt = (bool) connToken5 ? 1 : 0;
              object connToken6 = this.connTokens[(object) "ssce:case sensitive"];
              if (connToken6 != null)
                flag1 = (bool) connToken6;
              else
                flag2 = false;
            }
            num2 = NativeMethods.MarshalStringToLPWSTR(source2);
            num3 = NativeMethods.MarshalStringToLPWSTR(source3);
            num4 = NativeMethods.MarshalStringToLPWSTR(source4);
            num5 = NativeMethods.MarshalStringToLPWSTR(source5);
            object connToken7 = this.connTokens[(object) "data source"];
            if (connToken7 != null)
              source1 = (string) connToken7;
            num1 = NativeMethods.MarshalStringToLPWSTR(source1);
            int fSafeRepair = (int) repairOption;
            int dstEncryptionMode = ConStringUtil.MapEncryptionMode(str1);
            int localeFlags = 0;
            if (!flag2)
              localeFlags = -1;
            else if (flag1)
              localeFlags |= 1;
            int hr = NativeMethods.Rebuild(num1, num2, num3, num4, num5, fEncrypt, option, fSafeRepair, lcid, dstEncryptionMode, localeFlags, ref zero);
            if (hr == 0)
              return;
            SqlCeEngine.ProcessResults(zero, hr);
            return;
          }
          finally
          {
            NativeMethods.CoTaskMemFree(num1);
            NativeMethods.CoTaskMemFree(num2);
            NativeMethods.CoTaskMemFree(num3);
            NativeMethods.CoTaskMemFree(num4);
            NativeMethods.CoTaskMemFree(num5);
            NativeMethods.SafeDelete(ref zero);
          }
        }
      }
      throw new InvalidOperationException(Res.GetString("SQLCE_ConnectionStringNotInitialized"));
    }

    public void CreateDatabase()
    {
      int num1 = -1;
      string str1 = (string) null;
      bool flag1 = false;
      IntPtr zero = IntPtr.Zero;
      bool flag2 = false;
      string source1 = (string) null;
      string source2 = (string) null;
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeEngine));
      if (this.connTokens == null || string.IsNullOrEmpty(this.LocalConnectionString))
        throw new InvalidOperationException(Res.GetString("SQLCE_ConnectionStringNotInitialized"));
      IntPtr num2 = IntPtr.Zero;
      MEOPENINFO structure = new MEOPENINFO();
      try
      {
        num2 = NativeMethods.CoTaskMemAlloc(sizeof (MEOPENINFO));
        if (IntPtr.Zero == num2)
          throw new OutOfMemoryException();
        object connToken1 = this.connTokens[(object) "locale identifier"];
        if (connToken1 != null)
          num1 = (int) connToken1;
        object connToken2 = this.connTokens[(object) "ssce:encryption mode"];
        if (connToken2 != null)
          str1 = (string) connToken2;
        object connToken3 = this.connTokens[(object) "data source"];
        if (connToken3 != null)
          source1 = (string) connToken3;
        object connToken4 = this.connTokens[(object) "ssce:database password"];
        if (connToken4 != null)
        {
          string str2 = ((string) connToken4).Trim();
          if (str2.Length > 0)
            source2 = str2;
        }
        object connToken5 = this.connTokens[(object) "ssce:encrypt database"];
        if (connToken5 != null)
          flag1 = (bool) connToken5;
        object connToken6 = this.connTokens[(object) "ssce:case sensitive"];
        if (connToken6 != null)
          flag2 = (bool) connToken6;
        structure.pwszFileName = NativeMethods.MarshalStringToLPWSTR(source1);
        structure.pwszPassword = NativeMethods.MarshalStringToLPWSTR(source2);
        structure.lcidLocale = num1;
        structure.fEncrypt = flag1 ? 1 : 0;
        structure.dwEncryptionMode = ConStringUtil.MapEncryptionMode(str1);
        structure.dwLocaleFlags = 0;
        if (flag2)
          structure.dwLocaleFlags |= 1;
        Marshal.StructureToPtr((object) structure, num2, false);
        int database = NativeMethods.CreateDatabase(num2, ref zero);
        if (database == 0)
          return;
        SqlCeEngine.ProcessResults(zero, database);
      }
      finally
      {
        NativeMethods.CoTaskMemFree(structure.pwszFileName);
        NativeMethods.CoTaskMemFree(structure.pwszPassword);
        NativeMethods.CoTaskMemFree(num2);
        NativeMethods.SafeDelete(ref zero);
      }
    }

    internal static void ProcessResults(IntPtr pError, int hr)
    {
      if (NativeMethods.Failed(hr))
        throw SqlCeException.FillErrorInformation(hr, pError);
    }

    public void Upgrade()
    {
      this.Repair(SEFIXOPTION.SEFIX_OPTION_UPGRADE, (string) null, RepairOption.DeleteCorruptedRows);
    }

    public void Upgrade(string destConnectionString)
    {
      this.Repair(SEFIXOPTION.SEFIX_OPTION_UPGRADE, destConnectionString, RepairOption.DeleteCorruptedRows);
    }
  }
}
