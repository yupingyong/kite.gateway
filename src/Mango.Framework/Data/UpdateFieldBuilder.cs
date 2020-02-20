using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
namespace Mango.Framework.Data
{
    public class UpdateFieldBuilder : ExpressionVisitor
    {
        StringBuilder strBuilder;
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
            //strBuilder.Append("(");
            this.Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.Equal:
                    strBuilder.Append("=");
                    break;
                case ExpressionType.AndAlso:
                    strBuilder.Append(",");
                    break;
                case ExpressionType.Add:
                    strBuilder.Append("+");
                    break;
                case ExpressionType.Subtract:
                    strBuilder.Append("-");
                    break;
                default:
                    throw new NotSupportedException(string.Format("运算符{0}不支持", b.NodeType));
            }
            this.Visit(b.Right);
            //strBuilder.Append(")");
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
            throw new NotSupportedException(string.Format("成员{0}不支持", m.Member.Name));
        }
    }
}
