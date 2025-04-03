// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.FilterInfo
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using AForge.Video.DirectShow.Internals;
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace AForge.Video.DirectShow
{
  public class FilterInfo : IComparable
  {
    public string Name { get; private set; }

    public string MonikerString { get; private set; }

    public FilterInfo(string monikerString)
    {
      this.MonikerString = monikerString;
      this.Name = this.GetName(monikerString);
    }

    internal FilterInfo(IMoniker moniker)
    {
      this.MonikerString = this.GetMonikerString(moniker);
      this.Name = this.GetName(moniker);
    }

    public int CompareTo(object value)
    {
      FilterInfo filterInfo = (FilterInfo) value;
      return filterInfo == null ? 1 : this.Name.CompareTo(filterInfo.Name);
    }

    public static object CreateFilter(string filterMoniker)
    {
      object ppvResult = (object) null;
      IBindCtx ppbc = (IBindCtx) null;
      IMoniker ppmk = (IMoniker) null;
      int pchEaten = 0;
      if (Win32.CreateBindCtx(0, out ppbc) == 0)
      {
        if (Win32.MkParseDisplayName(ppbc, filterMoniker, ref pchEaten, out ppmk) == 0)
        {
          Guid guid = typeof (IBaseFilter).GUID;
          ppmk.BindToObject((IBindCtx) null, (IMoniker) null, ref guid, out ppvResult);
          Marshal.ReleaseComObject((object) ppmk);
        }
        Marshal.ReleaseComObject((object) ppbc);
      }
      return ppvResult;
    }

    private string GetMonikerString(IMoniker moniker)
    {
      string ppszDisplayName;
      moniker.GetDisplayName((IBindCtx) null, (IMoniker) null, out ppszDisplayName);
      return ppszDisplayName;
    }

    private string GetName(IMoniker moniker)
    {
      object ppvObj = (object) null;
      IPropertyBag propertyBag1 = (IPropertyBag) null;
      try
      {
        Guid guid = typeof (IPropertyBag).GUID;
        moniker.BindToStorage((IBindCtx) null, (IMoniker) null, ref guid, out ppvObj);
        IPropertyBag propertyBag2 = (IPropertyBag) ppvObj;
        object pVar = (object) "";
        int errorCode = propertyBag2.Read("FriendlyName", ref pVar, IntPtr.Zero);
        if (errorCode != 0)
          Marshal.ThrowExceptionForHR(errorCode);
        string str = (string) pVar;
        return str != null && str.Length >= 1 ? str : throw new ApplicationException();
      }
      catch (Exception ex)
      {
        return "";
      }
      finally
      {
        propertyBag1 = (IPropertyBag) null;
        if (ppvObj != null)
          Marshal.ReleaseComObject(ppvObj);
      }
    }

    private string GetName(string monikerString)
    {
      IBindCtx ppbc = (IBindCtx) null;
      IMoniker ppmk = (IMoniker) null;
      string name = "";
      int pchEaten = 0;
      if (Win32.CreateBindCtx(0, out ppbc) == 0)
      {
        if (Win32.MkParseDisplayName(ppbc, monikerString, ref pchEaten, out ppmk) == 0)
        {
          name = this.GetName(ppmk);
          Marshal.ReleaseComObject((object) ppmk);
          ppmk = (IMoniker) null;
        }
        Marshal.ReleaseComObject((object) ppbc);
      }
      return name;
    }
  }
}
