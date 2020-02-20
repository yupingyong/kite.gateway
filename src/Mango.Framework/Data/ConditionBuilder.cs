using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
namespace Mango.Framework.Data
{
    public class ConditionBuilder : ExpressionVisitor
    {

        StringBuilder strBuilder;

        public ConditionBuilder()
        {
        }

        public string Translate(Expression expression)
        {
            this.strBuilder = new StringBuilder();
            this.Visit(expression);
            return this.strBuilder.ToString();
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            strBuilder.Append("(");
            this.Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.AndAlso:
                    strBuilder.Append(" and ");
                    break;
                case ExpressionType.OrElse:
                    strBuilder.Append(" or ");
                    break;
                case ExpressionType.Equal:
                    strBuilder.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    strBuilder.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    strBuilder.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    strBuilder.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    strBuilder.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    strBuilder.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("运算符{0}不支持", b.NodeType));
            }
            if (b.Right.NodeType != ExpressionType.Parameter&& b.Right.NodeType == ExpressionType.MemberAccess)
            {
                LambdaExpression lambda = Expression.Lambda(b.Right);
                var fn = lambda.Compile();
                this.Visit(Expression.Constant(fn.DynamicInvoke(null), b.Right.Type));
            }
            else
            { 
                this.Visit(b.Right);
            }
            strBuilder.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            switch (Type.GetTypeCode(c.Value.GetType()))
            {
                case TypeCode.Boolean:
                    strBuilder.Append(((bool)c.Value) ? 1 : 0);
                    break;
                case TypeCode.String:
                    strBuilder.Append("'");
                    strBuilder.Append(c.Value);
                    strBuilder.Append("'");
                    break;
                case TypeCode.Object:
                    throw new NotSupportedException(string.Format("常量{0}不支持", c.Value));
                default:
                    strBuilder.Append(c.Value);
                    break;
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                strBuilder.Append(m.Member.Name);
                return m;
            }
            else if (m.Expression != null && m.Expression.NodeType == ExpressionType.Constant)
            {
                LambdaExpression lambda = Expression.Lambda(m);
                var fn = lambda.Compile();
                this.Visit(Expression.Constant(fn.DynamicInvoke(null), m.Type));
                return m;
            }
            throw new NotSupportedException(string.Format("成员{0}不支持", m.Member.Name));
        }
    }
}
