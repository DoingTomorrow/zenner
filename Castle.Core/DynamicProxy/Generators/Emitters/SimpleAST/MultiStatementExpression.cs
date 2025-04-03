// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.MultiStatementExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class MultiStatementExpression : Expression
  {
    private readonly List<Statement> statements = new List<Statement>();

    public void AddStatement(Statement statement) => this.statements.Add(statement);

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      this.statements.ForEach((Action<Statement>) (s => s.Emit(member, gen)));
    }
  }
}
