// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Bluetooth.Msft;
using InTheHand.Net.Sockets;
using System;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public sealed class BluetoothWin32AuthenticationEventArgs : EventArgs
  {
    public const string SixDigitsFormatString = "D6";
    private const string ErrorMessageSendingAnotherPinIsDisallowed_ = "It is disallowed to send another PIN response in this case.";
    private readonly BluetoothDeviceInfo m_device;
    private readonly BluetoothAuthenticationMethod _authMethod;
    private readonly BluetoothAuthenticationRequirements _authReq;
    private readonly BluetoothIoCapability _authIoCapa;
    private readonly int? _numberOrPasskey;
    private int? _responseNumberOrPasskey;
    private string m_pin;
    private bool? _confirmSsp;
    private byte[] _oobC;
    private byte[] _oobR;
    private bool m_callbackWithResult;
    private readonly int m_attemptNumber;
    private readonly int m_errorCode;

    public BluetoothWin32AuthenticationEventArgs()
    {
    }

    public BluetoothWin32AuthenticationEventArgs(BluetoothDeviceInfo device)
    {
      this.m_device = device != null ? device : throw new ArgumentNullException(nameof (device));
      this._authMethod = BluetoothAuthenticationMethod.Legacy;
      this._authReq = BluetoothAuthenticationRequirements.MITMProtectionNotDefined;
      this._authIoCapa = BluetoothIoCapability.Undefined;
      this._numberOrPasskey = new int?();
    }

    internal BluetoothWin32AuthenticationEventArgs(BLUETOOTH_DEVICE_INFO device)
      : this(new BluetoothDeviceInfo((IBluetoothDeviceInfo) new WindowsBluetoothDeviceInfo(device)))
    {
    }

    internal BluetoothWin32AuthenticationEventArgs(
      BLUETOOTH_DEVICE_INFO device,
      ref BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS? authCallbackParams)
      : this(device)
    {
      this._authMethod = authCallbackParams.Value.authenticationMethod;
      this._authReq = authCallbackParams.Value.authenticationRequirements;
      this._authIoCapa = authCallbackParams.Value.ioCapability;
      this._numberOrPasskey = new int?(checked ((int) authCallbackParams.Value.Numeric_Value_Passkey));
    }

    internal BluetoothWin32AuthenticationEventArgs(
      int errorCode,
      BluetoothWin32AuthenticationEventArgs previousEa)
    {
      this.m_device = previousEa != null ? previousEa.Device : throw new ArgumentNullException(nameof (previousEa));
      this._authIoCapa = previousEa._authIoCapa;
      this._authMethod = previousEa._authMethod;
      this._authReq = previousEa._authReq;
      this._numberOrPasskey = previousEa._numberOrPasskey;
      this.m_attemptNumber = previousEa.AttemptNumber + 1;
      this.m_errorCode = errorCode;
    }

    public BluetoothDeviceInfo Device => this.m_device;

    public BluetoothAuthenticationRequirements AuthenticationRequirements => this._authReq;

    public BluetoothIoCapability IoCapability => this._authIoCapa;

    public BluetoothAuthenticationMethod AuthenticationMethod => this._authMethod;

    public bool? JustWorksNumericComparison
    {
      get
      {
        if (this.AuthenticationMethod != BluetoothAuthenticationMethod.NumericComparison)
          return new bool?();
        switch (this.AuthenticationRequirements)
        {
          case BluetoothAuthenticationRequirements.MITMProtectionNotRequired:
          case BluetoothAuthenticationRequirements.MITMProtectionNotRequiredBonding:
          case BluetoothAuthenticationRequirements.MITMProtectionNotRequiredGeneralBonding:
            return new bool?(true);
          case BluetoothAuthenticationRequirements.MITMProtectionRequired:
          case BluetoothAuthenticationRequirements.MITMProtectionRequiredBonding:
          case BluetoothAuthenticationRequirements.MITMProtectionRequiredGeneralBonding:
            return new bool?(false);
          default:
            return new bool?(false);
        }
      }
    }

    public int? NumberOrPasskey => this._numberOrPasskey;

    public string NumberOrPasskeyAsString
    {
      get
      {
        return this.NumberOrPasskey.HasValue ? this.NumberOrPasskey.Value.ToString("D6").Insert(3, " ") : (string) null;
      }
    }

    public string Pin
    {
      get => this.m_pin;
      set => this.m_pin = value;
    }

    public bool? Confirm
    {
      get => this._confirmSsp;
      set => this._confirmSsp = value;
    }

    public int? ResponseNumberOrPasskey
    {
      get => this._responseNumberOrPasskey;
      set => this._responseNumberOrPasskey = value;
    }

    public void ConfirmOob(byte[] c, byte[] r)
    {
      if (c != null && c.Length != 16)
        throw new ArgumentException("OOB C and R value must be length 16 (or not supplied).", nameof (c));
      if (r != null && r.Length != 16)
        throw new ArgumentException("OOB C and R value must be length 16 (or not supplied).", nameof (r));
      this._oobC = c;
      this._oobR = r;
      this.Clone<byte>(ref this._oobC);
      this.Clone<byte>(ref this._oobR);
      this._confirmSsp = new bool?(true);
    }

    private void Clone<T>(ref T[] arr) where T : struct
    {
      if (arr == null)
        return;
      arr = (T[]) arr.Clone();
    }

    internal byte[] OobC => this._oobC;

    internal byte[] OobR => this._oobR;

    public bool CallbackWithResult
    {
      get => this.m_callbackWithResult;
      set => this.m_callbackWithResult = value;
    }

    public int AttemptNumber => this.m_attemptNumber;

    public int PreviousNativeErrorCode => this.m_errorCode;

    [CLSCompliant(false)]
    public uint PreviousNativeErrorCodeAsUnsigned => (uint) this.m_errorCode;

    public bool CannotSendAnotherResponse => this.m_errorCode == 1167;
  }
}
