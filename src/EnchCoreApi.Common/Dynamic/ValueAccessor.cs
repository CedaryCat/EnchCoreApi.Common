using System.Linq.Expressions;
using System.Reflection;

namespace EnchCoreApi.Common.Dynamic
{
    public class ValueAccessor
    {
        private ValueAccessor(string name, Func<object, object?>? getter, Action<object, object?>? setter) {
            Name = name;
            Getter = getter;
            Setter = setter;
        }
        public object? Get(object obj) {
            return (Getter ?? throw new Exception("Can not access getter"))(obj);
        }
        public void Set(object obj, object value) {
            (Setter ?? throw new Exception("Can not access setter"))(obj, value);
        }
        public string Name { get; private set; }
        public Func<object, object?>? Getter { get; private set; }
        public Action<object, object?>? Setter { get; private set; }
        public static ValueAccessor CreateAccessor(FieldInfo field) {
            var tobj = typeof(object);
            var InstanceParam = Expression.Parameter(tobj, "ins");
            var valueParam = Expression.Parameter(tobj, "val");
            var declaringType = field.DeclaringType ?? throw new Exception("DeclaringType is null");
            var obj = declaringType.IsValueType ? Expression.Unbox(InstanceParam, declaringType) : Expression.TypeAs(InstanceParam, declaringType);
            var value = field.FieldType.IsValueType ? Expression.Unbox(valueParam, field.FieldType) : Expression.TypeAs(valueParam, field.FieldType);
            return new ValueAccessor(
                field.Name,
                Expression.Lambda<Func<object, object?>>(Expression.TypeAs(Expression.Field(obj, field), typeof(object)), InstanceParam).Compile(),
                Expression.Lambda<Action<object, object?>>(Expression.Assign(Expression.Field(obj, field), value), InstanceParam, valueParam).Compile()
                );
        }
        public static ValueAccessor CreateAccessor(PropertyInfo prop) {
            var tobj = typeof(object);
            var InstanceParam = Expression.Parameter(tobj);
            var valueParam = Expression.Parameter(tobj);
            var declaringType = prop.DeclaringType ?? throw new Exception("DeclaringType is null");
            var obj = declaringType.IsValueType ? Expression.Unbox(InstanceParam, declaringType) : Expression.TypeAs(InstanceParam, declaringType);
            var value = prop.PropertyType.IsValueType ? Expression.Unbox(valueParam, prop.PropertyType) : Expression.TypeAs(valueParam, prop.PropertyType);

            return new ValueAccessor(
                prop.Name,
                prop.CanRead ? Expression.Lambda<Func<object, object?>>(Expression.TypeAs(Expression.Property(obj, prop), tobj), InstanceParam).Compile() : null,
                prop.CanWrite ? Expression.Lambda<Action<object, object?>>(Expression.Assign(Expression.Property(obj, prop), value), InstanceParam, valueParam).Compile() : null
                );
        }
        public static ValueAccessor CreateAccessor(MemberInfo PropOrField) {
            if (PropOrField is PropertyInfo prop) {
                return CreateAccessor(prop);
            }
            return CreateAccessor((FieldInfo)PropOrField);
        }
    }
    public class ValueAccessor<TMember>
    {
        private ValueAccessor(string name, Func<object, TMember>? getter, Action<object, TMember>? setter) {
            Name = name;
            Getter = getter;
            Setter = setter;
        }
        public TMember Get(object obj) {
            return (Getter ?? throw new Exception("Can not access getter"))(obj);
        }
        public void Set(object obj, TMember value) {
            (Setter ?? throw new Exception("Can not access setter"))(obj, value);
        }
        public string Name { get; private set; }
        public Func<object, TMember>? Getter { get; private set; }
        public Action<object, TMember>? Setter { get; private set; }
        public static ValueAccessor<TMember> CreateAccessor(FieldInfo field) {
            var InstanceParam = Expression.Parameter(typeof(object));
            var valueParam = Expression.Parameter(typeof(TMember));
            var declaringType = field.DeclaringType ?? throw new Exception("DeclaringType is null");
            var obj = declaringType.IsValueType ? Expression.Unbox(InstanceParam, declaringType) : Expression.TypeAs(InstanceParam, declaringType);
            return new ValueAccessor<TMember>(
                field.Name,
                Expression.Lambda<Func<object, TMember>>(Expression.Field(obj, field), InstanceParam).Compile(),
                Expression.Lambda<Action<object, TMember>>(Expression.Assign(Expression.Field(obj, field), valueParam), InstanceParam, valueParam).Compile());
        }
        public static ValueAccessor<TMember> CreateAccessor(PropertyInfo prop) {
            var InstanceParam = Expression.Parameter(typeof(object));
            var valueParam = Expression.Parameter(typeof(TMember));
            var declaringType = prop.DeclaringType ?? throw new Exception("DeclaringType is null");
            var obj = declaringType.IsValueType ? Expression.Unbox(InstanceParam, declaringType) : Expression.TypeAs(InstanceParam, declaringType);
            var value = prop.PropertyType.IsValueType ? Expression.Unbox(valueParam, prop.PropertyType) : Expression.TypeAs(valueParam, prop.PropertyType);
            return new ValueAccessor<TMember>(
                prop.Name,
                prop.CanRead ? Expression.Lambda<Func<object, TMember>>(Expression.Property(obj, prop), InstanceParam).Compile() : null,
                prop.CanWrite ? Expression.Lambda<Action<object, TMember>>(Expression.Assign(Expression.Property(obj, prop), value), InstanceParam, valueParam).Compile() : null);
        }
        public static ValueAccessor<TMember> CreateAccessor(MemberInfo PropOrField) {
            if (PropOrField is PropertyInfo prop) {
                return CreateAccessor(prop);
            }
            return CreateAccessor((FieldInfo)PropOrField);
        }
    }
    public class ValueAccessor<TInstance, TMember>
    {
        private ValueAccessor(string name, Func<TInstance, TMember>? getter, Action<TInstance, TMember>? setter) {
            Name = name;
            Getter = getter;
            Setter = setter;
        }
        public TMember Get(TInstance obj) {
            return (Getter ?? throw new Exception("Can not access getter"))(obj);
        }
        public void Set(TInstance obj, TMember value) {
            (Setter ?? throw new Exception("Can not access setter"))(obj, value);
        }
        public string Name { get; private set; }
        public Func<TInstance, TMember>? Getter { get; private set; }
        public Action<TInstance, TMember>? Setter { get; private set; }
        public static ValueAccessor<TInstance, TMember> CreateAccessor(FieldInfo field) {
            var InstanceParam = Expression.Parameter(typeof(TInstance));
            var valueParam = Expression.Parameter(typeof(TMember));
            //var obj = field.DeclaringType.IsValueType ? Expression.Unbox(InstanceParam, field.DeclaringType) : Expression.TypeAs(InstanceParam, field.DeclaringType);
            //var value = field.FieldType.IsValueType ? Expression.Unbox(valueParam, field.FieldType) : Expression.TypeAs(valueParam, field.FieldType);
            return new ValueAccessor<TInstance, TMember>(
                field.Name,
                Expression.Lambda<Func<TInstance, TMember>>(Expression.Field(InstanceParam, field), InstanceParam).Compile(),
                Expression.Lambda<Action<TInstance, TMember>>(Expression.Assign(Expression.Field(InstanceParam, field), valueParam), InstanceParam, valueParam).Compile());
        }
        public static ValueAccessor<TInstance, TMember> CreateAccessor(PropertyInfo prop) {
            var InstanceParam = Expression.Parameter(typeof(TInstance));
            var valueParam = Expression.Parameter(typeof(TMember));
            //var obj = prop.DeclaringType.IsValueType ? Expression.Unbox(InstanceParam, prop.DeclaringType) : Expression.TypeAs(InstanceParam, prop.DeclaringType);
            //var value = prop.PropertyType.IsValueType ? Expression.Unbox(valueParam, prop.PropertyType) : Expression.TypeAs(valueParam, prop.PropertyType);
            return new ValueAccessor<TInstance, TMember>(
                prop.Name,
                prop.CanRead ? Expression.Lambda<Func<TInstance, TMember>>(Expression.Property(InstanceParam, prop), InstanceParam).Compile() : null,
                prop.CanWrite ? Expression.Lambda<Action<TInstance, TMember>>(Expression.Assign(Expression.Property(InstanceParam, prop), valueParam), InstanceParam, valueParam).Compile() : null);
        }
        public static ValueAccessor<TInstance, TMember> CreateAccessor(MemberInfo PropOrField) {
            if (PropOrField is PropertyInfo prop) {
                return CreateAccessor(prop);
            }
            return CreateAccessor((FieldInfo)PropOrField);
        }
    }
}
