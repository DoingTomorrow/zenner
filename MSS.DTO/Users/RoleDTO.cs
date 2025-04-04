// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Users.RoleDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;

#nullable disable
namespace MSS.DTO.Users
{
  public class RoleDTO
  {
    public virtual Guid Id { get; set; }

    public virtual string Name { get; set; }

    public virtual bool IsStandard { get; set; }
  }
}
