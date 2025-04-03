// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.CAUUID
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [ComVisible(false)]
  internal struct CAUUID
  {
    public int cElems;
    public IntPtr pElems;

    public Guid[] ToGuidArray()
    {
      Guid[] guidArray = new Guid[this.cElems];
      for (int index = 0; index < this.cElems; ++index)
      {
        IntPtr ptr = new IntPtr(this.pElems.ToInt64() + (long) (index * Marshal.SizeOf(typeof (Guid))));
        guidArray[index] = (Guid) Marshal.PtrToStructure(ptr, typeof (Guid));
      }
      return guidArray;
    }
  }
}
