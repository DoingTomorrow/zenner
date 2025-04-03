// Decompiled with JetBrains decompiler
// Type: S3_Handler.LoggerActualData
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class LoggerActualData
  {
    internal uint RefTime;
    internal uint TimeLE_RefTime;
    internal uint TimeGT_RefTime;
    internal int VirtualWriteIndex;
    internal int WriteIndex;
    internal int VirtualReadIndex;
    internal int ReadIndex;
    internal DateTime refTime;
    internal DateTime timeLE_RefTime;
    internal DateTime timeGT_RefTime;

    internal void BuildDateTimeValues()
    {
      this.refTime = ZR_Calendar.Cal_GetDateTime(this.RefTime);
      this.timeLE_RefTime = ZR_Calendar.Cal_GetDateTime(this.TimeLE_RefTime);
      this.timeGT_RefTime = ZR_Calendar.Cal_GetDateTime(this.TimeGT_RefTime);
    }
  }
}
