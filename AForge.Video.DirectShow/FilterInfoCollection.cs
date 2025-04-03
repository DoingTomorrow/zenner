// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.FilterInfoCollection
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using AForge.Video.DirectShow.Internals;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace AForge.Video.DirectShow
{
  public class FilterInfoCollection : CollectionBase
  {
    public FilterInfoCollection(Guid category) => this.CollectFilters(category);

    public FilterInfo this[int index] => (FilterInfo) this.InnerList[index];

    private void CollectFilters(Guid category)
    {
      object o = (object) null;
      ICreateDevEnum createDevEnum = (ICreateDevEnum) null;
      IEnumMoniker enumMoniker = (IEnumMoniker) null;
      IMoniker[] rgelt = new IMoniker[1];
      try
      {
        o = Activator.CreateInstance(Type.GetTypeFromCLSID(Clsid.SystemDeviceEnum) ?? throw new ApplicationException("Failed creating device enumerator"));
        if (((ICreateDevEnum) o).CreateClassEnumerator(ref category, out enumMoniker, 0) != 0)
          throw new ApplicationException("No devices of the category");
        IntPtr zero = IntPtr.Zero;
        while (enumMoniker.Next(1, rgelt, zero) == 0 && rgelt[0] != null)
        {
          this.InnerList.Add((object) new FilterInfo(rgelt[0]));
          Marshal.ReleaseComObject((object) rgelt[0]);
          rgelt[0] = (IMoniker) null;
        }
        this.InnerList.Sort();
      }
      catch
      {
      }
      finally
      {
        createDevEnum = (ICreateDevEnum) null;
        if (o != null)
          Marshal.ReleaseComObject(o);
        if (enumMoniker != null)
          Marshal.ReleaseComObject((object) enumMoniker);
        if (rgelt[0] != null)
        {
          Marshal.ReleaseComObject((object) rgelt[0]);
          rgelt[0] = (IMoniker) null;
        }
      }
    }
  }
}
