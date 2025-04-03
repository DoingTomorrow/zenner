// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.FieldReference
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  [DebuggerDisplay("{fieldbuilder.Name} ({fieldbuilder.FieldType})")]
  public class FieldReference : Castle.DynamicProxy.Generators.Emitters.SimpleAST.Reference
  {
    private readonly FieldInfo field;
    private readonly FieldBuilder fieldbuilder;
    private readonly bool isStatic;

    public FieldReference(FieldInfo field)
    {
      this.field = field;
      if ((field.Attributes & FieldAttributes.Static) == FieldAttributes.PrivateScope)
        return;
      this.isStatic = true;
      this.owner = (Castle.DynamicProxy.Generators.Emitters.SimpleAST.Reference) null;
    }

    public FieldReference(FieldBuilder fieldbuilder)
    {
      this.fieldbuilder = fieldbuilder;
      this.field = (FieldInfo) fieldbuilder;
      if ((fieldbuilder.Attributes & FieldAttributes.Static) == FieldAttributes.PrivateScope)
        return;
      this.isStatic = true;
      this.owner = (Castle.DynamicProxy.Generators.Emitters.SimpleAST.Reference) null;
    }

    public FieldBuilder Fieldbuilder => this.fieldbuilder;

    public FieldInfo Reference => this.field;

    public override void LoadReference(ILGenerator gen)
    {
      if (this.isStatic)
        gen.Emit(OpCodes.Ldsfld, this.Reference);
      else
        gen.Emit(OpCodes.Ldfld, this.Reference);
    }

    public override void StoreReference(ILGenerator gen)
    {
      if (this.isStatic)
        gen.Emit(OpCodes.Stsfld, this.Reference);
      else
        gen.Emit(OpCodes.Stfld, this.Reference);
    }

    public override void LoadAddressOfReference(ILGenerator gen)
    {
      if (this.isStatic)
        gen.Emit(OpCodes.Ldsflda, this.Reference);
      else
        gen.Emit(OpCodes.Ldflda, this.Reference);
    }
  }
}
