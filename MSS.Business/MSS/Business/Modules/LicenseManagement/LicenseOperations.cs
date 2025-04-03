// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.LicenseManagement.LicenseOperations
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Modules.UsersManagement;
using PlugInLib;
using System;

#nullable disable
namespace MSS.Business.Modules.LicenseManagement
{
  [ComponentPath("")]
  internal class LicenseOperations : LicensePlugIn
  {
    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("", "", "", "", (string[]) null, Enum.GetNames(typeof (OperationEnum)), (object) null);
    }
  }
}
