using HttpServer;
using System.Linq.Expressions;
using System.Reflection;

namespace EnchCoreApi.Common.Dynamic {
    public class GenericTypeHelper {
        public static TResult DoGenericMethod<TResult>(Expression<Func<TResult>> GeniMethodRef, object[] args, params Type[] generic) {
            return DoGenericMethod<TResult>(((MethodCallExpression)GeniMethodRef.Body).Method, args, generic);
        }
        public static void DoGenericMethod(Expression<Action> GeniMethodRef, object[] args, params Type[] generic) {
            DoGenericMethod(((MethodCallExpression)GeniMethodRef.Body).Method, args, generic);
        }
        public static void DoGenericMethod(MethodInfo method, object[] args, params Type[] generic)
        {
            Expression.Lambda<Action>(Expression.Call(method.GetGenericMethodDefinition().MakeGenericMethod(generic), args.Select(Expression.Constant).ToArray())).Compile().Invoke();
        }
        public static TResult DoGenericMethod<TResult>(MethodInfo method, object[] args, params Type[] generic)
        {
            if (method.ReturnType != typeof(TResult))
            {
                throw new InvalidCastException($"The return type of the function '{method.Name}' does not match the return type specified by this call");
            }
            return Expression.Lambda<Func<TResult>>(Expression.Call(method.GetGenericMethodDefinition().MakeGenericMethod(generic), args.Select(Expression.Constant).ToArray())).Compile().Invoke();
        }
    }
}
