// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.EncabulatorData
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using System.Text;

#nullable disable
namespace PDCL2_Handler
{
  public class EncabulatorData
  {
    private readonly byte[] frame;

    public EncabulatorData(byte[] frame) => this.frame = frame;

    public override string ToString()
    {
      string str1 = Encoding.ASCII.GetString(this.frame.SubArray<byte>(1, 8));
      string str2 = Encoding.ASCII.GetString(this.frame.SubArray<byte>(9, 6));
      string str3 = Encoding.ASCII.GetString(this.frame.SubArray<byte>(15, 4));
      string str4 = Encoding.ASCII.GetString(this.frame.SubArray<byte>(19, 8));
      string str5 = Encoding.ASCII.GetString(this.frame.SubArray<byte>(27, 4));
      string str6 = Encoding.ASCII.GetString(this.frame.SubArray<byte>(31, 8));
      string str7 = Encoding.ASCII.GetString(this.frame.SubArray<byte>(39, 2));
      return str1.Insert(2, ".").Insert(5, ".") + " " + str2.Insert(2, ":").Insert(5, ":") + "   " + str3 + "   " + str4 + "   " + str5 + "   " + str6 + "   " + str7;
    }
  }
}
