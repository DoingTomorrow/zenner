// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MinomatV4Parameter
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class MinomatV4Parameter : EventArgs
  {
    public SCGiCommand Name { get; set; }

    public object Value { get; set; }

    public TimeSpan Elapsed { get; set; }

    public string ErrorText { get; set; }

    public override string ToString()
    {
      SCGiCommand scGiCommand = this.Name;
      if (this.Name == SCGiCommand.RegisteredSlavesIntegrated || this.Name == SCGiCommand.RegisteredSlavesNotIntegrated)
        scGiCommand = SCGiCommand.RegisteredSlaves;
      StringBuilder stringBuilder = new StringBuilder(string.Format("(0x{0:X4}) ({1, 5}) {2,-30} ", (object) scGiCommand, (object) Util.ElapsedToString(this.Elapsed), (object) this.Name));
      if (this.Value != null)
      {
        if (this.Value is byte[])
          stringBuilder.Append("\n").Append(Util.ByteArrayToHexString(this.Value as byte[]));
        else if (this.Value is List<uint>)
        {
          List<uint> uintList = (List<uint>) this.Value;
          if (uintList.Count > 0)
          {
            stringBuilder.Append("\n");
            int num1 = 1;
            foreach (uint num2 in uintList)
            {
              stringBuilder.Append(num2.ToString());
              if (num1 == 10)
              {
                stringBuilder.Append("\n");
                num1 = 0;
              }
              else
                stringBuilder.Append(", ");
              ++num1;
            }
          }
        }
        else
          stringBuilder.Append(this.Value.ToString());
      }
      else if (string.IsNullOrEmpty(this.ErrorText))
        stringBuilder.Append("null");
      else
        stringBuilder.Append("\n# ERROR: # " + this.ErrorText);
      stringBuilder.Append("\n");
      return stringBuilder.ToString();
    }
  }
}
