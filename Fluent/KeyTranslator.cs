// Decompiled with JetBrains decompiler
// Type: Fluent.KeyTranslator
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;

#nullable disable
namespace Fluent
{
  internal static class KeyTranslator
  {
    private static readonly Dictionary<int, IntPtr> keyboardLayoutHandlers = new Dictionary<int, IntPtr>();
    private static readonly byte[] keyboardState = new byte[(int) byte.MaxValue];
    private static IntPtr[] existingLayouts;

    public static char? KeyToChar(Key key, CultureInfo cultureInfo)
    {
      if (KeyTranslator.existingLayouts == null)
      {
        int keyboardLayoutList = NativeMethods.GetKeyboardLayoutList(0, (IntPtr[]) null);
        KeyTranslator.existingLayouts = new IntPtr[keyboardLayoutList];
        NativeMethods.GetKeyboardLayoutList(keyboardLayoutList, KeyTranslator.existingLayouts);
      }
      int num1 = KeyInterop.VirtualKeyFromKey(key);
      IntPtr num2;
      if (!KeyTranslator.keyboardLayoutHandlers.TryGetValue(cultureInfo.LCID, out num2))
      {
        num2 = NativeMethods.LoadKeyboardLayout(cultureInfo.LCID.ToString("x8", (IFormatProvider) CultureInfo.InvariantCulture), 128U);
        KeyTranslator.keyboardLayoutHandlers.Add(cultureInfo.LCID, num2);
      }
      uint wScanCode = NativeMethods.MapVirtualKeyEx((uint) num1, 2U, num2);
      StringBuilder pwszBuff = new StringBuilder(10);
      int unicodeEx = NativeMethods.ToUnicodeEx((uint) num1, wScanCode, KeyTranslator.keyboardState, pwszBuff, 10, 0U, num2);
      if (!((IEnumerable<IntPtr>) KeyTranslator.existingLayouts).Contains<IntPtr>(num2))
      {
        NativeMethods.UnloadKeyboardLayout(num2);
        KeyTranslator.keyboardLayoutHandlers.Remove(cultureInfo.LCID);
      }
      return pwszBuff.Length < 1 || unicodeEx < 1 ? new char?() : new char?(pwszBuff[0]);
    }
  }
}
