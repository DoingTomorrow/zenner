// Decompiled with JetBrains decompiler
// Type: S3_Handler.RadioSetup
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using HandlerLib;
using System;

#nullable disable
namespace S3_Handler
{
  internal class RadioSetup
  {
    internal RADIO_MODE RadioMode;
    internal string Szenario;
    internal AES_ENCRYPTION_MODE EncryptionMode;

    internal RadioSetup(string listName, byte radioAndEncModeBefore)
    {
      this.EncryptionMode = (AES_ENCRYPTION_MODE) ((int) radioAndEncModeBefore & 15);
      string[] strArray = listName.Split('_');
      this.Szenario = strArray.Length >= 2 && strArray.Length <= 3 ? strArray[1] : throw new Exception("Illegal list name parts at list name: " + listName);
      string str1 = listName;
      RADIO_MODE radioMode = RADIO_MODE.Radio3_Sz0;
      string str2 = radioMode.ToString();
      if (str1 == str2)
      {
        this.RadioMode = RADIO_MODE.Radio3_Sz0;
      }
      else
      {
        string str3 = listName;
        radioMode = RADIO_MODE.Radio3_Sz5;
        string str4 = radioMode.ToString();
        if (str3 == str4)
        {
          this.RadioMode = RADIO_MODE.Radio3_Sz5;
        }
        else
        {
          if (!Enum.TryParse<RADIO_MODE>(strArray[0], out this.RadioMode))
            throw new Exception("Illegal RadioMode at list name: " + listName);
          if (this.RadioMode == RADIO_MODE.Radio3_Sz0 || this.RadioMode == RADIO_MODE.Radio3_Sz5)
            throw new Exception("Illegal RadioMode and Szenario combination at list name: " + listName);
          if (strArray.Length == 3 && !Enum.TryParse<AES_ENCRYPTION_MODE>(strArray[2], out this.EncryptionMode))
            throw new Exception("Illegal EncryptionMode at list name: " + listName);
        }
      }
    }

    internal string GetListName()
    {
      return this.RadioMode == RADIO_MODE.Radio3_Sz0 || this.RadioMode == RADIO_MODE.Radio3_Sz5 ? this.RadioMode.ToString() : this.RadioMode.ToString() + "_" + this.Szenario;
    }

    internal byte GetRadioAndEncryptionMode()
    {
      return (byte) (((int) this.RadioMode << 4) + this.EncryptionMode);
    }
  }
}
