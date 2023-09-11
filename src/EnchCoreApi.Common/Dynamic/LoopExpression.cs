using System.Linq.Expressions;

namespace EnchCoreApi.Common.Dynamic {
    public abstract class LoopsExpression {
        public static Expression ForWithDeclaredVaribale(IEnumerable<ParameterExpression> varitables, Expression continueCondition, IEnumerable<Expression> loopBody) {
            var _break = Expression.Label();
            List<Expression> body = new List<Expression>();
            body.Add(Expression.IfThen(Expression.IsFalse(continueCondition), Expression.Break(_break)));
            foreach (var s in loopBody)
                body.Add(s);
            var loops = Expression.Loop(Expression.Block(body.ToArray()), _break);
            return loops;
        }
        public static Expression For(IEnumerable<ParameterExpression> varitables, ParameterExpression counter, Expression continueCondition, Expression increase, IEnumerable<Expression> loopBody) {
            return For(varitables, counter, null, continueCondition, Expression.Constant(increase), loopBody);
        }

        public static Expression For(IEnumerable<ParameterExpression> varitables, ParameterExpression counter, int initCounter, Expression continueCondition, int increase, IEnumerable<Expression> loopBody) {
            return For(varitables, counter, Expression.Constant(initCounter), continueCondition, Expression.Constant(increase), loopBody);
        }
        public static Expression For(IEnumerable<ParameterExpression> varitables, ParameterExpression counter, Expression initCounter, Expression continueCondition, Expression increase, IEnumerable<Expression> loopBody) {
            var _break = Expression.Label();
            List<Expression> body = new List<Expression>();
            body.Add(Expression.IfThen(Expression.IsFalse(continueCondition), Expression.Break(_break)));
            foreach (var s in loopBody)
                body.Add(s);
            body.Add(Expression.AddAssign(counter, increase));
            var loops = Expression.Loop(Expression.Block(body.ToArray()), _break);
            return Expression.Block(varitables, initCounter != null ? Expression.Assign(counter, initCounter) : Expression.Empty(), loops);
        }
    }
}
