// Decompiled with JetBrains decompiler
// Type: GMM_Handler.MeterResource
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.Collections;

#nullable disable
namespace GMM_Handler
{
  public class MeterResource
  {
    public readonly string Name;
    public readonly ushort SuppliedFromFunction;
    private bool Busy;
    public ArrayList UsedFromFunctions = new ArrayList();

    public MeterResource(string TheResourceName, ushort SuppliedFrom)
    {
      this.Name = TheResourceName;
      this.SuppliedFromFunction = SuppliedFrom;
      this.Busy = false;
    }

    public MeterResource Clone() => new MeterResource(this.Name, this.SuppliedFromFunction);

    public bool AddUsing(ushort FromFunction, bool Exclusive)
    {
      if (this.Busy)
        return false;
      this.UsedFromFunctions.Add((object) FromFunction);
      if (Exclusive)
        this.Busy = true;
      return true;
    }

    public bool IsBusy() => this.Busy;

    internal static bool GetNeadedIOFunction(
      string[] SuppliedResList,
      out ulong NeadedIOFunction,
      out ulong NeadedIOFunctionMask)
    {
      NeadedIOFunction = 0UL;
      NeadedIOFunctionMask = 0UL;
      for (int index = 0; index < SuppliedResList.Length; ++index)
      {
        string suppliedRes = SuppliedResList[index];
        char[] chArray = new char[1]{ '|' };
        foreach (string str in suppliedRes.Split(chArray))
        {
          string ResString = str.Trim();
          if (ResString.Length > 0 && IoFunctionResourceCorrelation.GetNeadedIOFunction(ResString, out NeadedIOFunction, out NeadedIOFunctionMask))
            return true;
        }
      }
      return false;
    }
  }
}
