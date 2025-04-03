// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RuntimeThread
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  internal class RuntimeThread
  {
    private DeviceCollectorFunctions MyBusFunctions;

    public event RuntimeThread.FunctionDone OnFunctionDone;

    public void FunctionDoneNotify()
    {
      this.MyBusFunctions.RunningFunction = DeviceCollectorFunctions.Functions.NoFunction;
      if (this.OnFunctionDone == null)
        return;
      this.OnFunctionDone((object) this, 0);
    }

    internal void TestloopReadEEProm(ref DeviceCollectorFunctions BusRef)
    {
      this.MyBusFunctions = BusRef;
      while (!this.MyBusFunctions.BreakRequest)
      {
        ((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).Location = 0;
        ((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).StartAddress = 10;
        ((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).NumberOfBytes = 210;
        ((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).ReadMemory();
      }
    }

    internal void TestloopWriteReadEEProm(ref DeviceCollectorFunctions BusRef)
    {
      Random random = new Random((int) SystemValues.DateTimeNow.Ticks);
      byte[] numArray = new byte[2048];
      this.MyBusFunctions = BusRef;
      while (!this.MyBusFunctions.BreakRequest)
      {
        ((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).Location = 0;
        ((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).StartAddress = 0;
        ((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).NumberOfBytes = 2048;
        ByteField byteField = new ByteField(2048);
        for (int index = 0; index < 2048; ++index)
        {
          byte Byte;
          do
          {
            Byte = (byte) random.Next((int) byte.MaxValue);
          }
          while (Byte == (byte) 219);
          numArray[index] = Byte;
          byteField.Add(Byte);
        }
        ((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).DataBuffer = byteField;
        if (!((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).WriteMemory())
        {
          int num1 = (int) MessageBox.Show("EEProm write error", "Error");
        }
        else if (!((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).ReadMemory())
        {
          int num2 = (int) MessageBox.Show("EEProm read error", "Error");
        }
        else
        {
          string text = "";
          if (((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).DataBuffer.Count != 2048)
          {
            text = "Read size to slow";
          }
          else
          {
            for (int index = 0; index < 2048; ++index)
            {
              if ((int) numArray[index] != (int) ((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).DataBuffer.Data[index])
                text = text + index.ToString("x04") + "; w:" + numArray[index].ToString("x02") + "; r:" + ((Serie2MBus) this.MyBusFunctions.MyDeviceList.SelectedDevice).DataBuffer.Data[index].ToString("x02") + "\r\n";
            }
          }
          if (text != "")
          {
            int num3 = (int) MessageBox.Show(text, "Error");
          }
        }
      }
    }

    public delegate void FunctionDone(object sender, int DummyParameter);

    internal delegate void Start(ref DeviceCollectorFunctions BusFkt);
  }
}
