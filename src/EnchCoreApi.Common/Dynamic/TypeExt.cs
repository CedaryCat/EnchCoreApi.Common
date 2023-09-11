using System.Linq.Expressions;
using System.Reflection;

namespace EnchCoreApi.Common.Dynamic {
    public static class Dynamics {
        public static MethodInfo GetMethod<TInstance>(Expression<Action<TInstance>> methodReferance) {
            return (methodReferance.Body as MethodCallExpression).Method;
        }
        public static MethodInfo GetMethod(Expression<Action> methodReferance) {
            return (methodReferance.Body as MethodCallExpression).Method;
        }
        public static ConstructorInfo GetConstructor<TInstance>(Expression<Func<TInstance>> ctorReferance) {
            return (ctorReferance.Body as NewExpression).Constructor;
        }
        public static FieldInfo GetField<TInstance, TField>(Expression<Func<TInstance, TField>> fieldReferance) {
            return (fieldReferance.Body as MemberExpression).Member as FieldInfo;
        }
        public static FieldInfo GetField<TProperty>(Expression<Func<TProperty>> fieldReferance) {
            return (fieldReferance.Body as MemberExpression).Member as FieldInfo;
        }
        public static PropertyInfo GetProperty<TInstance, TProperty>(Expression<Func<TInstance, TProperty>> propReferance) {
            return (propReferance.Body as MemberExpression).Member as PropertyInfo;
        }
        public static PropertyInfo GetProperty<TProperty>(Expression<Func<TProperty>> propReferance) {
            return (propReferance.Body as MemberExpression).Member as PropertyInfo;
        }
    }
}
