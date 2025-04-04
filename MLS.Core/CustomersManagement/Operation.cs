// Decompiled with JetBrains decompiler
// Type: MLS.Core.Model.CustomersManagement.Operation
// Assembly: MLS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FC8C31B-A3E8-40E1-84D2-E43683A764BC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MLS.Core.dll

using System.Collections.Generic;

#nullable disable
namespace MLS.Core.Model.CustomersManagement
{
  public class Operation
  {
    public virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual string Description { get; set; }

    public virtual IList<RoleOperation> RoleOperations { get; set; }
  }
}
