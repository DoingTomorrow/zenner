// Decompiled with JetBrains decompiler
// Type: ZENNER.ServiceTaskManager
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using GmmDbLib;
using MinomatHandler;
using System;
using System.Collections.Generic;
using System.Reflection;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ZENNER
{
  public sealed class ServiceTaskManager
  {
    public static List<ServiceTask> GetServices(DeviceModel model)
    {
      if (model == null)
        throw new NullReferenceException(nameof (model));
      if (!(model.Name == "Minomat V4 Master") && !(model.Name == "Minomat V4 Slave"))
        return (List<ServiceTask>) null;
      List<ServiceTask> services = new List<ServiceTask>();
      foreach (MethodInfo method in typeof (MinomatV4).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
      {
        if (!method.IsSpecialName && !method.IsVirtual && !method.IsConstructor)
        {
          ParameterInfo[] parameters = method.GetParameters();
          if (parameters == null || parameters.Length == 0)
            services.Add(new ServiceTask()
            {
              Method = method,
              Description = Ot.GetTranslatedLanguageText("STM_", method.ToString())
            });
        }
      }
      return services;
    }
  }
}
