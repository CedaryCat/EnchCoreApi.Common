using System.Collections;
using System.Linq.Expressions;

namespace EnchCoreApi.Common.Dynamic
{
    //public abstract class ExpOp
    //{
    //    pub abstract Expression Exp { get;}
    //    public static implicit operator ExpOp(Expression from) => new ExpOp<Expression>(from);
    //    //public static implicit operator Expression(ExpOp from) => from.Exp;
    //    public static ExpOp operator ++(ExpOp exp)
    //    {
    //        return Expression.Increment(exp);
    //    }
    //}

    public class Exps<TExp> : IEnumerable<TExp> where TExp : Expression
    {
        private readonly IEnumerable<TExp>[] exps;
        public Exps(params IEnumerable<TExp>[] exps) {
            this.exps = exps;
        }

        public IEnumerator<TExp> GetEnumerator() {
            foreach (var e in exps) {
                foreach (var i in e) {
                    yield return i;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public class ExpOp<TExp> where TExp : Expression
    {
        public TExp expression;

        //protected override Expression Exp { get => expression;}

        public ExpOp(TExp p) {
            expression = p;
        }
        public static implicit operator ExpOp<TExp>(TExp from) => new ExpOp<TExp>(from);
        public static implicit operator TExp(ExpOp<TExp> from) => from.expression;
        public static ExpOp<BinaryExpression> operator >(ExpOp<TExp> left, Expression right) {
            return Expression.GreaterThan(left, right);
        }
        public static ExpOp<BinaryExpression> operator <(ExpOp<TExp> left, Expression right) {
            return Expression.LessThan(left, right);
        }
        public static ExpOp<BinaryExpression> operator >(Expression left, ExpOp<TExp> right) {
            return Expression.GreaterThan(left, right);
        }
        public static ExpOp<BinaryExpression> operator <(Expression left, ExpOp<TExp> right) {
            return Expression.LessThan(left, right);
        }
        public static ExpOp<BinaryExpression> operator >(ExpOp<TExp> left, ExpOp<TExp> right) {
            return Expression.GreaterThan(left, right);
        }
        public static ExpOp<BinaryExpression> operator <(ExpOp<TExp> left, ExpOp<TExp> right) {
            return Expression.LessThan(left, right);
        }
        public static ExpOp<BinaryExpression> operator >(ExpOp<TExp> left, object right) {
            return Expression.GreaterThan(left, Expression.Constant(right));
        }
        public static ExpOp<BinaryExpression> operator <(ExpOp<TExp> left, object right) {
            return Expression.LessThan(left, Expression.Constant(right));
        }

        public static ExpOp<BinaryExpression> operator >=(ExpOp<TExp> left, Expression right) {
            return Expression.GreaterThanOrEqual(left, right);
        }
        public static ExpOp<BinaryExpression> operator <=(ExpOp<TExp> left, Expression right) {
            return Expression.LessThanOrEqual(left, right);
        }
        public static ExpOp<BinaryExpression> operator >=(Expression left, ExpOp<TExp> right) {
            return Expression.GreaterThanOrEqual(left, right);
        }
        public static ExpOp<BinaryExpression> operator <=(Expression left, ExpOp<TExp> right) {
            return Expression.LessThanOrEqual(left, right);
        }
        public static ExpOp<BinaryExpression> operator >=(ExpOp<TExp> left, ExpOp<TExp> right) {
            return Expression.GreaterThanOrEqual(left, right);
        }
        public static ExpOp<BinaryExpression> operator <=(ExpOp<TExp> left, ExpOp<TExp> right) {
            return Expression.LessThanOrEqual(left, right);
        }
        public static ExpOp<BinaryExpression> operator >=(ExpOp<TExp> left, object right) {
            return Expression.GreaterThanOrEqual(left, Expression.Constant(right));
        }
        public static ExpOp<BinaryExpression> operator <=(ExpOp<TExp> left, object right) {
            return Expression.LessThanOrEqual(left, Expression.Constant(right));
        }

        public static ExpOp<BinaryExpression> operator ==(ExpOp<TExp> left, Expression right) {
            return Expression.Equal(left, right);
        }
        public static ExpOp<BinaryExpression> operator !=(ExpOp<TExp> left, Expression right) {
            return Expression.NotEqual(left, right);
        }
        public static ExpOp<BinaryExpression> operator ==(Expression left, ExpOp<TExp> right) {
            return Expression.Equal(left, right);
        }
        public static ExpOp<BinaryExpression> operator !=(Expression left, ExpOp<TExp> right) {
            return Expression.NotEqual(left, right);
        }
        public static ExpOp<BinaryExpression> operator ==(ExpOp<TExp> left, ExpOp<TExp> right) {
            return Expression.Equal(left, right);
        }
        public static ExpOp<BinaryExpression> operator !=(ExpOp<TExp> left, ExpOp<TExp> right) {
            return Expression.NotEqual(left, right);
        }
        public static ExpOp<BinaryExpression> operator ==(ExpOp<TExp> left, bool right) {
            return Expression.Equal(left, Expression.Constant(right));
        }
        public static ExpOp<BinaryExpression> operator !=(ExpOp<TExp> left, bool right) {
            return Expression.NotEqual(left, Expression.Constant(right));
        }

        public static Expression operator !(ExpOp<TExp> exp) {
            return Expression.IsFalse(exp);
        }

        public static ExpOp<BinaryExpression> operator &(Expression left, ExpOp<TExp> right) {
            return Expression.And(left, right);
        }
        public static ExpOp<BinaryExpression> operator &(ExpOp<TExp> left, Expression right) {
            return Expression.And(left, right);
        }
        public static ExpOp<BinaryExpression> operator &(ExpOp<TExp> left, ExpOp<TExp> right) {
            return Expression.And(left, right);
        }

        public static ExpOp<BinaryExpression> operator |(Expression left, ExpOp<TExp> right) {
            return Expression.Or(left, right);
        }
        public static ExpOp<BinaryExpression> operator |(ExpOp<TExp> left, Expression right) {
            return Expression.Or(left, right);
        }
        public static ExpOp<BinaryExpression> operator |(ExpOp<TExp> left, ExpOp<TExp> right) {
            return Expression.Or(left, right);
        }
        public ExpOp<IndexExpression> this[params ExpOp<TExp>[] indexs] {
            get {
                Expression[] arr = new Expression[indexs.Length];
                for (int i = 0; i < arr.Length; i++) {
                    arr[i] = indexs[i];
                }
                return Expression.ArrayAccess(this, arr);
            }
        }
        public ExpOp<IndexExpression> this[params Expression[] indexs] {
            get {
                return Expression.ArrayAccess(this, indexs);
            }
        }
    }
}
