// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothWin32Authentication
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Msft;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public class BluetoothWin32Authentication : IDisposable
  {
    public const int NativeErrorSuccess = 0;
    public const int NativeErrorNotAuthenticated = 1244;
    public const int NativeErrorDeviceNotConnected = 1167;
    private bool m_hasKB942567 = true;
    private readonly IntPtr m_radioHandle = IntPtr.Zero;
    private BluetoothAuthenticationRegistrationHandle m_regHandle;
    private NativeMethods.BluetoothAuthenticationCallback m_callback;
    private NativeMethods.BluetoothAuthenticationCallbackEx m_callbackEx;
    private readonly BluetoothAddress m_remoteAddress;
    private readonly string m_pin;
    private readonly EventHandler<BluetoothWin32AuthenticationEventArgs> m_userCallback;

    public BluetoothWin32Authentication(BluetoothAddress remoteAddress, string pin)
    {
      if (remoteAddress == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (remoteAddress));
      if (remoteAddress.ToInt64() == 0L)
        throw new ArgumentNullException(nameof (remoteAddress), "A non-blank address must be specified.");
      if (pin == null)
        throw new ArgumentNullException(nameof (pin));
      this.m_remoteAddress = remoteAddress;
      this.m_pin = pin;
      this.Register(remoteAddress);
    }

    public BluetoothWin32Authentication(
      EventHandler<BluetoothWin32AuthenticationEventArgs> handler)
    {
      this.m_userCallback = handler;
      this.Register(new BluetoothAddress(0L));
    }

    private void Register(BluetoothAddress remoteAddress)
    {
      this.m_callback = new NativeMethods.BluetoothAuthenticationCallback(this.NativeCallback);
      this.m_callbackEx = new NativeMethods.BluetoothAuthenticationCallbackEx(this.NativeCallback);
      BLUETOOTH_DEVICE_INFO pbtdi = new BLUETOOTH_DEVICE_INFO(remoteAddress);
      uint num;
      if (this.m_hasKB942567)
      {
        try
        {
          num = NativeMethods.BluetoothRegisterForAuthenticationEx(ref pbtdi, out this.m_regHandle, this.m_callbackEx, IntPtr.Zero);
        }
        catch (EntryPointNotFoundException ex)
        {
          this.m_hasKB942567 = false;
          num = NativeMethods.BluetoothRegisterForAuthentication(ref pbtdi, out this.m_regHandle, this.m_callback, IntPtr.Zero);
        }
      }
      else
        num = NativeMethods.BluetoothRegisterForAuthentication(ref pbtdi, out this.m_regHandle, this.m_callback, IntPtr.Zero);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (num != 0U)
        throw new Win32Exception(lastWin32Error);
      this.m_regHandle.SetObjectToKeepAlive((object) this.m_callback, (object) this.m_callbackEx);
    }

    private bool NativeCallback(
      IntPtr param,
      ref BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS pAuthCallbackParams)
    {
      BLUETOOTH_DEVICE_INFO deviceInfo = pAuthCallbackParams.deviceInfo;
      BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS? pAuthCallbackParams1 = new BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS?(pAuthCallbackParams);
      this.NativeCallback(pAuthCallbackParams.authenticationMethod, param, ref deviceInfo, true, ref pAuthCallbackParams1);
      return false;
    }

    private bool NativeCallback(IntPtr param, ref BLUETOOTH_DEVICE_INFO bdi)
    {
      BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS? pAuthCallbackParams = new BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS?();
      return this.NativeCallback(BluetoothAuthenticationMethod.Legacy, param, ref bdi, false, ref pAuthCallbackParams);
    }

    private bool NativeCallback(
      BluetoothAuthenticationMethod method,
      IntPtr param,
      ref BLUETOOTH_DEVICE_INFO bdi,
      bool versionEx,
      ref BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS? pAuthCallbackParams)
    {
      int errorCode;
      if (this.m_pin != null)
      {
        string pin = this.m_pin;
        errorCode = !versionEx ? NativeMethods.BluetoothSendAuthenticationResponse(this.m_radioHandle, ref bdi, pin) : this.BluetoothSendAuthenticationResponseExPin(ref bdi, pin);
      }
      else
      {
        switch (method)
        {
          case BluetoothAuthenticationMethod.Legacy:
            BluetoothWin32AuthenticationEventArgs e1 = new BluetoothWin32AuthenticationEventArgs(bdi);
            while (true)
            {
              this.OnAuthentication(e1);
              if (e1.Pin != null)
              {
                if (e1.PreviousNativeErrorCode != 0 || e1.AttemptNumber == 0)
                {
                  if (e1.PreviousNativeErrorCode != 1167)
                  {
                    string pin = e1.Pin;
                    errorCode = !versionEx ? NativeMethods.BluetoothSendAuthenticationResponse(this.m_radioHandle, ref bdi, pin) : this.BluetoothSendAuthenticationResponseExPin(ref bdi, pin);
                    if (errorCode != 0)
                      Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "    BluetoothSendAuthenticationResponse failed: {0}=0x{0:X}", (object) errorCode));
                    BluetoothWin32AuthenticationEventArgs previousEa = e1;
                    if (previousEa.CallbackWithResult)
                      e1 = new BluetoothWin32AuthenticationEventArgs(errorCode, previousEa);
                    else
                      goto label_26;
                  }
                  else
                    goto label_9;
                }
                else
                  goto label_7;
              }
              else
                break;
            }
            errorCode = 0;
            break;
label_7:
            errorCode = 0;
            break;
label_9:
            errorCode = 0;
            break;
          case BluetoothAuthenticationMethod.OutOfBand:
          case BluetoothAuthenticationMethod.NumericComparison:
          case BluetoothAuthenticationMethod.PasskeyNotification:
          case BluetoothAuthenticationMethod.Passkey:
            BluetoothWin32AuthenticationEventArgs e2 = new BluetoothWin32AuthenticationEventArgs(bdi, ref pAuthCallbackParams);
            while (true)
            {
              this.OnAuthentication(e2);
              if (e2.PreviousNativeErrorCode != 0 || e2.AttemptNumber == 0)
              {
                if (e2.PreviousNativeErrorCode != 1167)
                {
                  bool? confirm = e2.Confirm;
                  if (confirm.HasValue)
                  {
                    errorCode = method == BluetoothAuthenticationMethod.OutOfBand ? this.BluetoothSendAuthenticationResponseExOob(ref bdi, confirm, e2) : this.BluetoothSendAuthenticationResponseExNumCompPasskey(ref bdi, confirm, e2);
                    if (errorCode != 0)
                      Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "    BluetoothSendAuthenticationResponseEx failed: {0}=0x{0:X}", (object) errorCode));
                    BluetoothWin32AuthenticationEventArgs previousEa = e2;
                    if (previousEa.CallbackWithResult)
                      e2 = new BluetoothWin32AuthenticationEventArgs(errorCode, previousEa);
                    else
                      goto label_26;
                  }
                  else
                    goto label_20;
                }
                else
                  goto label_18;
              }
              else
                break;
            }
            errorCode = 0;
            break;
label_18:
            errorCode = 0;
            break;
label_20:
            errorCode = 0;
            break;
          default:
            errorCode = 0;
            break;
        }
      }
label_26:
      if (errorCode != 0)
        Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "BluetoothSendAuthenticationResponse failed: {0}=0x{0:X}", (object) errorCode));
      return true;
    }

    private int BluetoothSendAuthenticationResponseExPin(ref BLUETOOTH_DEVICE_INFO bdi, string pin)
    {
      BLUETOOTH_AUTHENTICATE_RESPONSE__PIN_INFO pauthResponse = new BLUETOOTH_AUTHENTICATE_RESPONSE__PIN_INFO();
      pauthResponse.authMethod = BluetoothAuthenticationMethod.Legacy;
      pauthResponse.bthAddressRemote = bdi.Address;
      pauthResponse.pinInfo.pin = new byte[16];
      byte[] bytes = Encoding.UTF8.GetBytes(pin);
      int length = Math.Min(16, bytes.Length);
      Array.Copy((Array) bytes, (Array) pauthResponse.pinInfo.pin, length);
      pauthResponse.pinInfo.pinLength = length;
      return NativeMethods.BluetoothSendAuthenticationResponseEx(this.m_radioHandle, ref pauthResponse);
    }

    private int BluetoothSendAuthenticationResponseExNumCompPasskey(
      ref BLUETOOTH_DEVICE_INFO bdi,
      bool? confirm,
      BluetoothWin32AuthenticationEventArgs e)
    {
      if (!confirm.HasValue)
        return 0;
      BLUETOOTH_AUTHENTICATE_RESPONSE__NUMERIC_COMPARISON_PASSKEY_INFO pauthResponse = new BLUETOOTH_AUTHENTICATE_RESPONSE__NUMERIC_COMPARISON_PASSKEY_INFO();
      pauthResponse.negativeResponse = (byte) 1;
      pauthResponse.authMethod = e.AuthenticationMethod;
      if (e.AuthenticationMethod != BluetoothAuthenticationMethod.NumericComparison && e.AuthenticationMethod != BluetoothAuthenticationMethod.Passkey && e.AuthenticationMethod != BluetoothAuthenticationMethod.PasskeyNotification)
        return 1244;
      pauthResponse.bthAddressRemote = bdi.Address;
      ref bool? local = ref confirm;
      bool valueOrDefault = local.GetValueOrDefault();
      if (local.HasValue)
      {
        switch (valueOrDefault)
        {
          case false:
            break;
          case true:
            pauthResponse.negativeResponse = (byte) 0;
            if (e.ResponseNumberOrPasskey.HasValue)
            {
              pauthResponse.numericComp_passkey = checked ((uint) e.ResponseNumberOrPasskey.Value);
              goto label_9;
            }
            else
              goto label_9;
          default:
            goto label_9;
        }
      }
      pauthResponse.negativeResponse = (byte) 1;
label_9:
      return NativeMethods.BluetoothSendAuthenticationResponseEx(this.m_radioHandle, ref pauthResponse);
    }

    private int BluetoothSendAuthenticationResponseExOob(
      ref BLUETOOTH_DEVICE_INFO bdi,
      bool? confirm,
      BluetoothWin32AuthenticationEventArgs e)
    {
      if (!confirm.HasValue)
        return 0;
      BLUETOOTH_AUTHENTICATE_RESPONSE__OOB_DATA_INFO pauthResponse = new BLUETOOTH_AUTHENTICATE_RESPONSE__OOB_DATA_INFO();
      pauthResponse.negativeResponse = (byte) 1;
      pauthResponse.authMethod = e.AuthenticationMethod;
      if (e.AuthenticationMethod != BluetoothAuthenticationMethod.OutOfBand)
        return 1244;
      pauthResponse.bthAddressRemote = bdi.Address;
      ref bool? local = ref confirm;
      bool valueOrDefault = local.GetValueOrDefault();
      if (local.HasValue)
      {
        switch (valueOrDefault)
        {
          case false:
            break;
          case true:
            pauthResponse.negativeResponse = (byte) 0;
            if (e.OobC != null)
              pauthResponse.oobInfo.C = e.OobC;
            if (e.OobR != null)
            {
              pauthResponse.oobInfo.R = e.OobR;
              goto label_11;
            }
            else
              goto label_11;
          default:
            goto label_11;
        }
      }
      pauthResponse.negativeResponse = (byte) 1;
label_11:
      return NativeMethods.BluetoothSendAuthenticationResponseEx(this.m_radioHandle, ref pauthResponse);
    }

    protected virtual void OnAuthentication(BluetoothWin32AuthenticationEventArgs e)
    {
      if (this.m_userCallback == null)
        return;
      this.m_userCallback((object) this, e);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.m_regHandle == null)
        return;
      this.m_regHandle.Dispose();
    }
  }
}
