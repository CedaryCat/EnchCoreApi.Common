using System.Linq.Expressions;
using System.Reflection;

namespace EnchCoreApi.Common.Dynamic
{
    public static class Dynamics
    {
        public static MethodInfo GetMethod<TInstance>(Expression<Action<TInstance>> methodReferance) {
            return ((MethodCallExpression)methodReferance.Body).Method;
        }
        public static MethodInfo GetMethod(Expression<Action> methodReferance) {
            return ((MethodCallExpression)methodReferance.Body).Method;
        }
        public static ConstructorInfo GetConstructor<TInstance>(Expression<Func<TInstance>> ctorReferance) {
            return ((NewExpression)ctorReferance.Body).Constructor!;
        }
        public static FieldInfo GetField<TInstance, TField>(Expression<Func<TInstance, TField>> fieldReferance) {
            return (FieldInfo)((MemberExpression)fieldReferance.Body).Member;
        }
        public static FieldInfo GetField<TProperty>(Expression<Func<TProperty>> fieldReferance) {
            return (FieldInfo)((MemberExpression)fieldReferance.Body).Member; ;
        }
        public static PropertyInfo GetProperty<TInstance, TProperty>(Expression<Func<TInstance, TProperty>> propReferance) {
            return (PropertyInfo)((MemberExpression)propReferance.Body).Member;
        }
        public static PropertyInfo GetProperty<TProperty>(Expression<Func<TProperty>> propReferance) {
            return (PropertyInfo)((MemberExpression)propReferance.Body).Member;
        }
    }
}
