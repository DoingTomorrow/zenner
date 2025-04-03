// Decompiled with JetBrains decompiler
// Type: S3_Handler.TdcStatusData
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System.Collections.Generic;

#nullable disable
namespace S3_Handler
{
  internal class TdcStatusData
  {
    internal SortedList<string, uint> statusParameterValues = new SortedList<string, uint>();
    internal static S3_ParameterNames[] statusParameterList = new S3_ParameterNames[7]
    {
      S3_ParameterNames.lastTdcErrorTime,
      S3_ParameterNames.lastTdcHwError,
      S3_ParameterNames.lastTdcStatusError,
      S3_ParameterNames.numberchangedTdcHwErrors,
      S3_ParameterNames.numberchangedTdcStatusErrors,
      S3_ParameterNames.tdcStatusErrorFlagsOld,
      S3_ParameterNames.tdcHwErrorFlagsOld
    };
  }
}
