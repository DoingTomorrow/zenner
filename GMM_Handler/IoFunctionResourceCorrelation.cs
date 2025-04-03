// Decompiled with JetBrains decompiler
// Type: GMM_Handler.IoFunctionResourceCorrelation
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.Collections;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  internal class IoFunctionResourceCorrelation
  {
    private static IoFunctionResourceCorrelation[] IoFuncResCorrelations = new IoFunctionResourceCorrelation[14]
    {
      new IoFunctionResourceCorrelation(InOutFunctions.IO1_Off, new string[0], new string[2]
      {
        "Inp1On",
        "Out1On"
      }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO1_Input, new string[1]
      {
        "Inp1On"
      }, new string[1]{ "Out1On" }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO1_Energy, new string[2]
      {
        "Out1On",
        "EnToOut1"
      }, new string[5]
      {
        "Inp1On",
        "VolToOut1",
        "CEnToOut1",
        "ErrToOut1",
        "Out1Special"
      }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO1_Volume, new string[2]
      {
        "Out1On",
        "VolToOut1"
      }, new string[5]
      {
        "Inp1On",
        "EnToOut1",
        "CEnToOut1",
        "ErrToOut1",
        "Out1Special"
      }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO1_CEnergy, new string[2]
      {
        "Out1On",
        "CEnToOut1"
      }, new string[5]
      {
        "Inp1On",
        "EnToOut1",
        "VolToOut1",
        "ErrToOut1",
        "Out1Special"
      }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO1_Error, new string[2]
      {
        "Out1On",
        "ErrToOut1"
      }, new string[5]
      {
        "Inp1On",
        "EnToOut1",
        "VolToOut1",
        "CEnToOut1",
        "Out1Special"
      }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO1_Special, new string[2]
      {
        "Out1On",
        "Out1Special"
      }, new string[5]
      {
        "Inp1On",
        "EnToOut1",
        "VolToOut1",
        "CEnToOut1",
        "ErrToOut1"
      }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO2_Off, new string[0], new string[2]
      {
        "Inp2On",
        "Out2On"
      }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO2_Input, new string[1]
      {
        "Inp2On"
      }, new string[1]{ "Out2On" }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO2_Energy, new string[2]
      {
        "Out2On",
        "EnToOut2"
      }, new string[5]
      {
        "Inp2On",
        "VolToOut2",
        "CEnToOut2",
        "ErrToOut2",
        "Out2Special"
      }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO2_Volume, new string[2]
      {
        "Out2On",
        "VolToOut2"
      }, new string[5]
      {
        "Inp2On",
        "EnToOut2",
        "CEnToOut2",
        "ErrToOut2",
        "Out2Special"
      }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO2_CEnergy, new string[2]
      {
        "Out2On",
        "CEnToOut2"
      }, new string[5]
      {
        "Inp2On",
        "EnToOut2",
        "VolToOut2",
        "ErrToOut2",
        "Out2Special"
      }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO2_Error, new string[2]
      {
        "Out2On",
        "ErrToOut2"
      }, new string[5]
      {
        "Inp2On",
        "EnToOut2",
        "VolToOut2",
        "CEnToOut2",
        "Out2Special"
      }),
      new IoFunctionResourceCorrelation(InOutFunctions.IO2_Special, new string[2]
      {
        "Out2On",
        "Out2Special"
      }, new string[5]
      {
        "Inp2On",
        "EnToOut2",
        "VolToOut2",
        "CEnToOut2",
        "ErrToOut2"
      })
    };
    private InOutFunctions IoFunction;
    private string[] ThisResourcesAreNeaded;
    private string[] ImpossibleResources;

    internal IoFunctionResourceCorrelation(
      InOutFunctions IoFunction,
      string[] ThisResourcesAreNeaded,
      string[] ImpossibleResources)
    {
      this.IoFunction = IoFunction;
      this.ThisResourcesAreNeaded = ThisResourcesAreNeaded;
      this.ImpossibleResources = ImpossibleResources;
    }

    internal static string GetImpossibleResource(
      InOutFunctions TheIo1Function,
      InOutFunctions TheIo2Function,
      string[] SuppliedResources)
    {
      for (int index1 = 0; index1 < IoFunctionResourceCorrelation.IoFuncResCorrelations.Length; ++index1)
      {
        if (IoFunctionResourceCorrelation.IoFuncResCorrelations[index1].IoFunction == TheIo1Function || IoFunctionResourceCorrelation.IoFuncResCorrelations[index1].IoFunction == TheIo2Function)
        {
          for (int index2 = 0; index2 < IoFunctionResourceCorrelation.IoFuncResCorrelations[index1].ImpossibleResources.Length; ++index2)
          {
            if (!(IoFunctionResourceCorrelation.IoFuncResCorrelations[index1].ImpossibleResources[index2] == ""))
            {
              for (int index3 = 0; index3 < SuppliedResources.Length; ++index3)
              {
                if (IoFunctionResourceCorrelation.IoFuncResCorrelations[index1].ImpossibleResources[index2] == SuppliedResources[index3])
                  return SuppliedResources[index3];
              }
            }
          }
        }
      }
      return (string) null;
    }

    internal static string GetNeadedResources(
      InOutFunctions TheIo1Function,
      InOutFunctions TheIo2Function,
      SortedList AvailableResources)
    {
      string neadedResources = "";
      for (int index1 = 0; index1 < IoFunctionResourceCorrelation.IoFuncResCorrelations.Length; ++index1)
      {
        if (IoFunctionResourceCorrelation.IoFuncResCorrelations[index1].IoFunction == TheIo1Function || IoFunctionResourceCorrelation.IoFuncResCorrelations[index1].IoFunction == TheIo2Function)
        {
          for (int index2 = 0; index2 < IoFunctionResourceCorrelation.IoFuncResCorrelations[index1].ThisResourcesAreNeaded.Length; ++index2)
          {
            if (!(IoFunctionResourceCorrelation.IoFuncResCorrelations[index1].ThisResourcesAreNeaded[index2] == "") && AvailableResources.IndexOfKey((object) IoFunctionResourceCorrelation.IoFuncResCorrelations[index1].ThisResourcesAreNeaded[index2]) < 0)
              neadedResources = neadedResources + IoFunctionResourceCorrelation.IoFuncResCorrelations[index1].ThisResourcesAreNeaded[index2] + ";";
          }
        }
      }
      return neadedResources;
    }

    internal static bool GetNeadedIOFunction(
      string ResString,
      out ulong NeadedIOFunction,
      out ulong NeadedIOFunctionMask)
    {
      NeadedIOFunction = 0UL;
      NeadedIOFunctionMask = 15UL;
      for (int index = 0; index < IoFunctionResourceCorrelation.IoFuncResCorrelations.Length; ++index)
      {
        if (IoFunctionResourceCorrelation.IoFuncResCorrelations[index].ThisResourcesAreNeaded.Length != 0 && IoFunctionResourceCorrelation.IoFuncResCorrelations[index].ThisResourcesAreNeaded[0].Length != 0 && IoFunctionResourceCorrelation.IoFuncResCorrelations[index].ThisResourcesAreNeaded[IoFunctionResourceCorrelation.IoFuncResCorrelations[index].ThisResourcesAreNeaded.Length - 1] == ResString)
        {
          NeadedIOFunction = (ulong) IoFunctionResourceCorrelation.IoFuncResCorrelations[index].IoFunction;
          if (IoFunctionResourceCorrelation.IoFuncResCorrelations[index].IoFunction >= InOutFunctions.IO2_Off)
            NeadedIOFunctionMask = 240UL;
          return true;
        }
      }
      return false;
    }
  }
}
