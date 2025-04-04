// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommStRfCommIf
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Threading;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal class WidcommStRfCommIf : IRfCommIf
  {
    private readonly IRfCommIf _child;
    private readonly WidcommPortSingleThreader _st;

    public WidcommStRfCommIf(WidcommBluetoothFactoryBase factory, IRfCommIf child)
    {
      this._st = factory.GetSingleThreader();
      this._child = child;
    }

    public IntPtr PObject => this._child.PObject;

    public void Create()
    {
      this._st.AddCommand<WidcommPortSingleThreader.MiscNoReturnCommand>(new WidcommPortSingleThreader.MiscNoReturnCommand((ThreadStart) (() => this._child.Create()))).WaitCompletion();
    }

    public void Destroy(bool disposing)
    {
      this._st.AddCommand<WidcommPortSingleThreader.MiscNoReturnCommand>(new WidcommPortSingleThreader.MiscNoReturnCommand((ThreadStart) (() => this._child.Destroy(disposing)))).WaitCompletion(disposing);
    }

    public bool ClientAssignScnValue(Guid serviceGuid, int scn)
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<bool>>(new WidcommPortSingleThreader.MiscReturnCommand<bool>((Func<bool>) (() => this._child.ClientAssignScnValue(serviceGuid, scn)))).WaitCompletion();
    }

    public bool SetSecurityLevel(byte[] p_service_name, BTM_SEC securityLevel, bool isServer)
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<bool>>(new WidcommPortSingleThreader.MiscReturnCommand<bool>((Func<bool>) (() => this._child.SetSecurityLevel(p_service_name, securityLevel, isServer)))).WaitCompletion();
    }

    public int GetScn()
    {
      return this._st.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<int>>(new WidcommPortSingleThreader.MiscReturnCommand<int>((Func<int>) (() => this._child.GetScn()))).WaitCompletion();
    }
  }
}
