// Decompiled with JetBrains decompiler
// Type: WindowsInput.InputSimulator
// Assembly: InputSimulator, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21845CD4-46CC-4FE2-BD83-49CF4563A54D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InputSimulator.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace WindowsInput
{
  public static class InputSimulator
  {
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(
      uint numberOfInputs,
      INPUT[] inputs,
      int sizeOfInputStructure);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern short GetAsyncKeyState(ushort virtualKeyCode);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern short GetKeyState(ushort virtualKeyCode);

    [DllImport("user32.dll")]
    private static extern IntPtr GetMessageExtraInfo();

    public static bool IsKeyDownAsync(VirtualKeyCode keyCode)
    {
      return InputSimulator.GetAsyncKeyState((ushort) keyCode) < (short) 0;
    }

    public static bool IsKeyDown(VirtualKeyCode keyCode)
    {
      return InputSimulator.GetKeyState((ushort) keyCode) < (short) 0;
    }

    public static bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
    {
      return ((int) InputSimulator.GetKeyState((ushort) keyCode) & 1) == 1;
    }

    public static void SimulateKeyDown(VirtualKeyCode keyCode)
    {
      INPUT input = new INPUT()
      {
        Type = 1,
        Data = {
          Keyboard = new KEYBDINPUT()
        }
      };
      input.Data.Keyboard.Vk = (ushort) keyCode;
      input.Data.Keyboard.Scan = (ushort) 0;
      input.Data.Keyboard.Flags = 0U;
      input.Data.Keyboard.Time = 0U;
      input.Data.Keyboard.ExtraInfo = IntPtr.Zero;
      if (InputSimulator.SendInput(1U, new INPUT[1]{ input }, Marshal.SizeOf(typeof (INPUT))) == 0U)
        throw new Exception(string.Format("The key down simulation for {0} was not successful.", (object) keyCode));
    }

    public static void SimulateKeyUp(VirtualKeyCode keyCode)
    {
      INPUT input = new INPUT()
      {
        Type = 1,
        Data = {
          Keyboard = new KEYBDINPUT()
        }
      };
      input.Data.Keyboard.Vk = (ushort) keyCode;
      input.Data.Keyboard.Scan = (ushort) 0;
      input.Data.Keyboard.Flags = 2U;
      input.Data.Keyboard.Time = 0U;
      input.Data.Keyboard.ExtraInfo = IntPtr.Zero;
      if (InputSimulator.SendInput(1U, new INPUT[1]{ input }, Marshal.SizeOf(typeof (INPUT))) == 0U)
        throw new Exception(string.Format("The key up simulation for {0} was not successful.", (object) keyCode));
    }

    public static void SimulateKeyPress(VirtualKeyCode keyCode)
    {
      INPUT input1 = new INPUT()
      {
        Type = 1,
        Data = {
          Keyboard = new KEYBDINPUT()
        }
      };
      input1.Data.Keyboard.Vk = (ushort) keyCode;
      input1.Data.Keyboard.Scan = (ushort) 0;
      input1.Data.Keyboard.Flags = 0U;
      input1.Data.Keyboard.Time = 0U;
      input1.Data.Keyboard.ExtraInfo = IntPtr.Zero;
      INPUT input2 = new INPUT()
      {
        Type = 1,
        Data = {
          Keyboard = new KEYBDINPUT()
        }
      };
      input2.Data.Keyboard.Vk = (ushort) keyCode;
      input2.Data.Keyboard.Scan = (ushort) 0;
      input2.Data.Keyboard.Flags = 2U;
      input2.Data.Keyboard.Time = 0U;
      input2.Data.Keyboard.ExtraInfo = IntPtr.Zero;
      if (InputSimulator.SendInput(2U, new INPUT[2]
      {
        input1,
        input2
      }, Marshal.SizeOf(typeof (INPUT))) == 0U)
        throw new Exception(string.Format("The key press simulation for {0} was not successful.", (object) keyCode));
    }

    public static void SimulateTextEntry(string text)
    {
      byte[] numArray = text.Length <= int.MaxValue ? Encoding.ASCII.GetBytes(text) : throw new ArgumentException(string.Format("The text parameter is too long. It must be less than {0} characters.", (object) (uint) int.MaxValue), nameof (text));
      int length = numArray.Length;
      INPUT[] inputs = new INPUT[length * 2];
      for (int index = 0; index < length; ++index)
      {
        ushort num = (ushort) numArray[index];
        INPUT input1 = new INPUT()
        {
          Type = 1,
          Data = {
            Keyboard = new KEYBDINPUT()
          }
        };
        input1.Data.Keyboard.Vk = (ushort) 0;
        input1.Data.Keyboard.Scan = num;
        input1.Data.Keyboard.Flags = 4U;
        input1.Data.Keyboard.Time = 0U;
        input1.Data.Keyboard.ExtraInfo = IntPtr.Zero;
        INPUT input2 = new INPUT()
        {
          Type = 1,
          Data = {
            Keyboard = new KEYBDINPUT()
          }
        };
        input2.Data.Keyboard.Vk = (ushort) 0;
        input2.Data.Keyboard.Scan = num;
        input2.Data.Keyboard.Flags = 6U;
        input2.Data.Keyboard.Time = 0U;
        input2.Data.Keyboard.ExtraInfo = IntPtr.Zero;
        if (((int) num & 65280) == 57344)
        {
          input1.Data.Keyboard.Flags |= 1U;
          input2.Data.Keyboard.Flags |= 1U;
        }
        inputs[2 * index] = input1;
        inputs[2 * index + 1] = input2;
      }
      InputSimulator.SendInput((uint) (length * 2), inputs, Marshal.SizeOf(typeof (INPUT)));
    }

    public static void SimulateModifiedKeyStroke(
      VirtualKeyCode modifierKeyCode,
      VirtualKeyCode keyCode)
    {
      InputSimulator.SimulateKeyDown(modifierKeyCode);
      InputSimulator.SimulateKeyPress(keyCode);
      InputSimulator.SimulateKeyUp(modifierKeyCode);
    }

    public static void SimulateModifiedKeyStroke(
      IEnumerable<VirtualKeyCode> modifierKeyCodes,
      VirtualKeyCode keyCode)
    {
      if (modifierKeyCodes != null)
        modifierKeyCodes.ToList<VirtualKeyCode>().ForEach((Action<VirtualKeyCode>) (x => InputSimulator.SimulateKeyDown(x)));
      InputSimulator.SimulateKeyPress(keyCode);
      if (modifierKeyCodes == null)
        return;
      modifierKeyCodes.Reverse<VirtualKeyCode>().ToList<VirtualKeyCode>().ForEach((Action<VirtualKeyCode>) (x => InputSimulator.SimulateKeyUp(x)));
    }

    public static void SimulateModifiedKeyStroke(
      VirtualKeyCode modifierKey,
      IEnumerable<VirtualKeyCode> keyCodes)
    {
      InputSimulator.SimulateKeyDown(modifierKey);
      if (keyCodes != null)
        keyCodes.ToList<VirtualKeyCode>().ForEach((Action<VirtualKeyCode>) (x => InputSimulator.SimulateKeyPress(x)));
      InputSimulator.SimulateKeyUp(modifierKey);
    }

    public static void SimulateModifiedKeyStroke(
      IEnumerable<VirtualKeyCode> modifierKeyCodes,
      IEnumerable<VirtualKeyCode> keyCodes)
    {
      if (modifierKeyCodes != null)
        modifierKeyCodes.ToList<VirtualKeyCode>().ForEach((Action<VirtualKeyCode>) (x => InputSimulator.SimulateKeyDown(x)));
      if (keyCodes != null)
        keyCodes.ToList<VirtualKeyCode>().ForEach((Action<VirtualKeyCode>) (x => InputSimulator.SimulateKeyPress(x)));
      if (modifierKeyCodes == null)
        return;
      modifierKeyCodes.Reverse<VirtualKeyCode>().ToList<VirtualKeyCode>().ForEach((Action<VirtualKeyCode>) (x => InputSimulator.SimulateKeyUp(x)));
    }
  }
}
