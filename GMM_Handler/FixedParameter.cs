// Decompiled with JetBrains decompiler
// Type: GMM_Handler.FixedParameter
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;

#nullable disable
namespace GMM_Handler
{
  internal class FixedParameter : LinkBlock
  {
    private const string StandardErrorMessage = "Illegal emergency frame";

    internal FixedParameter(Meter MyMeterIn)
      : base(MyMeterIn, LinkBlockTypes.FixedParameter)
    {
    }

    internal bool LoadEmergencyFrame()
    {
      CodeBlock codeBlock1 = (CodeBlock) null;
      CodeBlock codeBlock2 = (CodeBlock) null;
      IEnumerator enumerator1 = this.MyMeter.MyDisplayCode.AllMenuItems.GetEnumerator();
      try
      {
label_16:
        if (enumerator1.MoveNext())
        {
          IEnumerator enumerator2 = ((MenuItem) enumerator1.Current).DisplayCodeBlocks.GetEnumerator();
          try
          {
            CodeBlock current;
            while (true)
            {
              do
              {
                if (enumerator2.MoveNext())
                {
                  current = (CodeBlock) enumerator2.Current;
                  if (current.CodeSequenceType == CodeBlock.CodeSequenceTypes.Displaycode && current.CodeList.Count == 3 && ((CodeObject) current.CodeList[0]).CodeType == CodeObject.CodeTypes.BYTE && ((CodeObject) current.CodeList[1]).CodeType == CodeObject.CodeTypes.WORD && (((CodeObject) current.CodeList[2]).CodeType == CodeObject.CodeTypes.iPTR || ((CodeObject) current.CodeList[2]).CodeType == CodeObject.CodeTypes.ePTR || ((CodeObject) current.CodeList[2]).CodeType == CodeObject.CodeTypes.WORD))
                    goto label_5;
                }
                else
                  goto label_16;
              }
              while (current.CodeSequenceType != CodeBlock.CodeSequenceTypes.Framecode);
              break;
label_5:
              codeBlock1 = current;
            }
            if (codeBlock1 == null)
              return this.MyMeter.MyHandler.AddErrorPointMessage("Illegal emergency frame");
            if (current.CodeList.Count > 6)
              return this.MyMeter.MyHandler.AddErrorPointMessage("Illegal emergency frame");
            codeBlock2 = current;
            goto label_21;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
        }
      }
      finally
      {
        if (enumerator1 is IDisposable disposable)
          disposable.Dispose();
      }
      return this.MyMeter.MyHandler.AddErrorPointMessage("Illegal emergency frame");
label_21:
      ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Emergency_Dis_Address"]).ValueEprom = ((CodeObject) codeBlock1.CodeList[2]).CodeValueCompiled;
      ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Emergency_Dis_Flags"]).ValueEprom = ((CodeObject) codeBlock1.CodeList[1]).CodeValueCompiled;
      for (int index = 0; index < codeBlock2.CodeList.Count - 1; ++index)
      {
        CodeObject code = (CodeObject) codeBlock2.CodeList[index];
        Parameter parameter = (Parameter) this.MyMeter.AllParameters[(object) ("DefaultFunction.Emergency_Frame_" + (index + 1).ToString())] ?? (Parameter) this.MyMeter.AllParameters[(object) ("DefaultFunction.Emergency_Frame" + index.ToString())];
        if (index == 0)
        {
          if ((code.CodeValueCompiled & 16L) == 0L)
            return this.MyMeter.MyHandler.AddErrorPointMessage("Illegal emergency frame");
          parameter.ValueEprom = code.CodeValueCompiled & -17L;
        }
        else
          parameter.ValueEprom = code.CodeValueCompiled;
      }
      return true;
    }
  }
}
