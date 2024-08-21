using System.Linq.Expressions;

namespace EnchCoreApi.Common.Dynamic
{
    public class Constructor
    {
        public Func<object[], object> Creator;
        public Func<object> NoParamsCreator;
        private Constructor(Func<object[], object> func) {
            Creator = func;
        }
        private Constructor(Func<object> func) {
            NoParamsCreator = func;
        }
        public static Constructor Create(Type objType) {
            if (objType.IsValueType) {
                return new Constructor(() => default);
            }
            var finalExp = Expression.New(objType.GetConstructor(new Type[] { }));
            var exp = Expression.Lambda<Func<object>>(finalExp);
            return new Constructor(exp.Compile());
        }
        public static Constructor Create(Type objType, params Type[] paramType) {
            Expression[] innerArgs = new Expression[paramType.Length];
            ParameterExpression arrArg = Expression.Parameter(typeof(object[]), "parameters");
            for (int i = 0; i < paramType.Length; i++) {
                var objT = typeof(object);
                if (paramType[i].IsValueType) {
                    innerArgs[i] = Expression.Unbox(Expression.ArrayIndex(arrArg, Expression.Constant(i)), paramType[i]);
                }
                else {
                    innerArgs[i] = Expression.TypeAs(Expression.ArrayIndex(arrArg, Expression.Constant(i)), paramType[i]);
                }
            }
            var finalExp = Expression.New(objType.GetConstructor(paramType), innerArgs);
            var exp = Expression.Lambda<Func<object[], object>>(finalExp, arrArg);
            return new Constructor(exp.Compile());
        }
        public T GetInstance<T>(params object[] args) {
            return (T)Creator(args);
        }
        public object GetInstance() {
            return NoParamsCreator();
        }
    }
    public class Constructor<Tobj>
    {
        public Func<object[], Tobj> Creator;
        public Func<Tobj> NoParamsCreator;
        private Constructor(Func<object[], Tobj> func) {
            Creator = func;
        }
        private Constructor(Func<Tobj> func) {
            NoParamsCreator = func;
        }
        public static Constructor<Tobj> Create(params Type[] paramType) {
            Expression[] innerArgs = new Expression[paramType.Length];
            ParameterExpression arrArg = Expression.Parameter(typeof(object[]), "parameters");
            for (int i = 0; i < paramType.Length; i++) {
                var objT = typeof(object);
                if (paramType[i].IsValueType) {
                    innerArgs[i] = Expression.Unbox(Expression.ArrayIndex(arrArg, Expression.Constant(i)), paramType[i]);
                }
                else {
                    innerArgs[i] = Expression.TypeAs(Expression.ArrayIndex(arrArg, Expression.Constant(i)), paramType[i]);
                }
            }
            var finalExp = Expression.New(typeof(Tobj).GetConstructor(paramType), innerArgs);
            var exp = Expression.Lambda<Func<object[], Tobj>>(finalExp, arrArg);
            return new Constructor<Tobj>(exp.Compile());
        }
        public static Constructor<Tobj> Create() {
            var type = typeof(Tobj);
            if (type.IsValueType) {
                return new Constructor<Tobj>(() => default);
            }
            var ctor = type.GetConstructor(new Type[] { });
            if (ctor == null) {
                throw new Exception($"{typeof(Tobj).Name} have not a default ctor");
            }
            var finalExp = Expression.New(ctor);
            var exp = Expression.Lambda<Func<Tobj>>(finalExp);
            return new Constructor<Tobj>(exp.Compile());
        }
        public Tobj GetInstance(params object[] args) {
            return Creator(args);
        }
        public Tobj GetInstance() {
            return NoParamsCreator();
        }
    }
    public class ConstructorFunc<TFunc> where TFunc : Delegate
    {
        public TFunc Creator;
        private ConstructorFunc(TFunc func) {
            Creator = func;
        }
        public static ConstructorFunc<TFunc> Create() {
            var genArgs = typeof(TFunc).GetGenericArguments();
            if (typeof(TFunc).Name != "Func") {
                throw new ArgumentException("TFunc is not a Func<T1,T2,,out TResult> type");
            }
            ParameterExpression[] _params = new ParameterExpression[genArgs.Length - 1];
            Type[] args = new Type[genArgs.Length - 1];
            for (int i = 0; i < args.Length - 1; i++) {
                args[i] = genArgs[i];
                _params[i] = Expression.Parameter(args[i]);
            }
            var finalExp = Expression.New(genArgs[genArgs.Length - 1].GetConstructor(args), _params);
            var exp = Expression.Lambda<TFunc>(finalExp, _params);
            return new ConstructorFunc<TFunc>(exp.Compile());
        }
    }
}

