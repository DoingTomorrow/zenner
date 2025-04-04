// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.HqlSqlWalkerTreeAdaptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class HqlSqlWalkerTreeAdaptor : ASTTreeAdaptor
  {
    private readonly HqlSqlWalker _walker;

    public HqlSqlWalkerTreeAdaptor(object walker) => this._walker = (HqlSqlWalker) walker;

    public override object Create(IToken payload)
    {
      if (payload == null)
        return base.Create(payload);
      object node;
      switch (payload.Type)
      {
        case 10:
        case 82:
          node = (object) new BetweenOperatorNode(payload);
          break;
        case 12:
          node = (object) new CountNode(payload);
          break;
        case 13:
          node = (object) new DeleteStatement(payload);
          break;
        case 15:
          node = (object) new DotNode(payload);
          break;
        case 17:
        case 27:
          node = (object) new CollectionFunction(payload);
          break;
        case 19:
          node = (object) new UnaryLogicOperatorNode(payload);
          break;
        case 20:
        case 51:
          node = (object) new BooleanLiteralNode(payload);
          break;
        case 22:
          node = (object) new FromClause(payload);
          break;
        case 26:
        case 83:
          node = (object) new InLogicOperatorNode(payload);
          break;
        case 29:
          node = (object) new InsertStatement(payload);
          break;
        case 30:
          node = (object) new IntoClause(payload);
          break;
        case 34:
        case 84:
        case 102:
        case 107:
        case 109:
        case 110:
        case 111:
        case 112:
          node = (object) new BinaryLogicOperatorNode(payload);
          break;
        case 41:
          node = (object) new OrderByClause(payload);
          break;
        case 45:
        case 86:
          node = (object) new QueryNode(payload);
          break;
        case 53:
          node = (object) new UpdateStatement(payload);
          break;
        case 57:
          node = (object) new CaseNode(payload);
          break;
        case 71:
          node = (object) new AggregateNode(payload);
          break;
        case 73:
          node = (object) new ConstructorNode(payload);
          break;
        case 74:
          node = (object) new Case2Node(payload);
          break;
        case 78:
          node = (object) new IndexNode(payload);
          break;
        case 79:
          node = (object) new IsNotNullLogicOperatorNode(payload);
          break;
        case 80:
          node = (object) new IsNullLogicOperatorNode(payload);
          break;
        case 81:
          node = (object) new MethodNode(payload);
          break;
        case 90:
        case 91:
        case 114:
          node = (object) new UnaryArithmeticNode(payload);
          break;
        case 95:
        case 96:
        case 97:
        case 98:
        case 99:
        case 124:
          node = (object) new LiteralNode(payload);
          break;
        case 100:
          node = (object) new JavaConstantNode(payload);
          break;
        case 106:
        case 149:
          node = (object) new ParameterNode(payload);
          break;
        case 115:
        case 116:
        case 117:
        case 118:
        case 119:
        case 120:
        case 121:
          node = (object) new BinaryArithmeticOperatorNode(payload);
          break;
        case 125:
        case 141:
          node = (object) new IdentNode(payload);
          break;
        case 135:
          node = (object) new FromElement(payload);
          break;
        case 136:
          node = (object) new ImpliedFromElement(payload);
          break;
        case 138:
          node = (object) new SelectClause(payload);
          break;
        case 143:
          node = (object) new SqlFragment(payload);
          break;
        case 145:
          node = (object) new SelectExpressionImpl(payload);
          break;
        default:
          node = (object) new SqlNode(payload);
          break;
      }
      this.Initialise(node);
      return node;
    }

    public override object ErrorNode(
      ITokenStream input,
      IToken start,
      IToken stop,
      RecognitionException e)
    {
      return (object) new ASTErrorNode(input, start, stop, e);
    }

    public override object DupNode(object t)
    {
      IASTNode astNode1 = t is IASTNode astNode2 ? (IASTNode) this.Create(astNode2.Token) : throw new NotImplementedException();
      astNode1.Parent = astNode2.Parent;
      return (object) astNode1;
    }

    private void Initialise(object node)
    {
      if (node is IInitializableNode initializableNode)
        initializableNode.Initialize((object) this._walker);
      if (!(node is ISessionFactoryAwareNode factoryAwareNode))
        return;
      factoryAwareNode.SessionFactory = this._walker.SessionFactoryHelper.Factory;
    }
  }
}
