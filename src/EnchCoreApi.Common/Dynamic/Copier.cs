using System.Linq.Expressions;
using System.Reflection;

namespace EnchCoreApi.Common.Dynamic {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CopyFieldAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CopyIgnoreFieldAttribute : Attribute { }
    public static class Copier<Tobj> {
        /// <summary>
        /// (from,to)
        /// </summary>
        public static Func<Tobj, Tobj, Tobj> AllCopyAction = (f, t) => t;
        /// <summary>
        /// (from,to)
        /// </summary>
        public static Func<Tobj, Tobj, Tobj> FieldCopyAction = (f, t) => t;
        /// <summary>
        /// (from,to)
        /// </summary>
        public static Func<Tobj, Tobj, Tobj> PropertyCopyAction = (f, t) => t;
        /// <summary>
        /// (from,to)
        /// </summary>
        public static Func<Tobj, Tobj, Tobj> AttrOrientedCopyAction = (f, t) => t;
        static Copier() {
            var fields = typeof(Tobj).GetFields();
            var props = typeof(Tobj).GetProperties();

            var from = Expression.Parameter(typeof(Tobj), "from");
            var to = Expression.Parameter(typeof(Tobj), "to");
            List<Expression> fieldCopy = new List<Expression>();
            List<Expression> propCopy = new List<Expression>();
            List<Expression> allcopy = new List<Expression>();
            List<Expression> aoCopy = new List<Expression>();
            for (int i = 0; i < fields.Length; i++) {
                if (fields[i].IsStatic || !fields[i].IsPublic) {
                    continue;
                }
                if (fields[i].GetCustomAttribute<CopyIgnoreFieldAttribute>() is not null) {
                    continue;
                }
                var exp = Expression.Assign(Expression.Field(to, fields[i]), Expression.Field(from, fields[i]));
                fieldCopy.Add(exp);
                allcopy.Add(exp);
                if (fields[i].GetCustomAttribute<CopyFieldAttribute>() is not null) {
                    aoCopy.Add(exp);
                }
            }
            for (int i = 0; i < props.Length; i++) {
                if (props[i].GetCustomAttribute<CopyIgnoreFieldAttribute>() is not null) {
                    continue;
                }
                if (!props[i].CanRead || !props[i].CanWrite || !props[i].GetMethod.IsPublic || !props[i].SetMethod.IsPublic || props[i].GetMethod.IsStatic || props[i].SetMethod.IsStatic) {
                    continue;
                }
                var exp = Expression.Assign(Expression.Property(to, props[i]), Expression.Property(from, props[i]));
                propCopy.Add(exp);
                allcopy.Add(exp);
                if (!(props[i].GetCustomAttribute<CopyFieldAttribute>() is null)) {
                    aoCopy.Add(exp);
                }
            }
            if (propCopy.Count > 0) {
                propCopy.Add(to);
                Expression.Block(propCopy.ToArray());
                PropertyCopyAction = Expression.Lambda<Func<Tobj, Tobj, Tobj>>(Expression.Block(propCopy.ToArray()), from, to).Compile();
            }
            if (fieldCopy.Count > 0) {
                fieldCopy.Add(to);
                Expression.Block(fieldCopy.ToArray());
                FieldCopyAction = Expression.Lambda<Func<Tobj, Tobj, Tobj>>(Expression.Block(fieldCopy.ToArray()), from, to).Compile();
            }
            if (propCopy.Count + fieldCopy.Count > 0) {
                allcopy.Add(to);
                Expression.Block(allcopy.ToArray());
                AllCopyAction = Expression.Lambda<Func<Tobj, Tobj, Tobj>>(Expression.Block(allcopy.ToArray()), from, to).Compile();
            }
            if (aoCopy.Count > 0) {
                aoCopy.Add(to);
                Expression.Block(aoCopy.ToArray());
                AttrOrientedCopyAction = Expression.Lambda<Func<Tobj, Tobj, Tobj>>(Expression.Block(aoCopy.ToArray()), from, to).Compile();
            }
        }
    }
    public static class Copier<TFrom, TTo> {
        public static Func<TFrom, TTo, TTo> AllCopyAction = (f, t) => t;
        public static Func<TFrom, TTo, TTo> FieldCopyAction = (f, t) => t;
        public static Func<TFrom, TTo, TTo> PropertyCopyAction = (f, t) => t;
        public static Func<TFrom, TTo, TTo> AttrOrientedCopyAction = (f, t) => t;
        static Copier() {
            var fromfields = typeof(TFrom).GetFields();
            var fromprops = typeof(TFrom).GetProperties();
            var toType = typeof(TTo);

            var from = Expression.Parameter(typeof(TFrom), "from");
            var to = Expression.Parameter(typeof(TTo), "to");

            List<Expression> fieldCopy = new List<Expression>();
            List<Expression> propCopy = new List<Expression>();
            List<Expression> allcopy = new List<Expression>();
            List<Expression> aoCopy = new List<Expression>();

            for (int i = 0; i < fromfields.Length; i++) {
                if (fromfields[i].IsStatic || !fromfields[i].IsPublic)
                    continue;
                if (fromfields[i].GetCustomAttribute<CopyIgnoreFieldAttribute>() is not null)
                    continue;
                var tofield = toType.GetField(fromfields[i].Name);
                if (tofield == null)
                    continue;
                if (tofield.IsStatic || !tofield.IsPublic)
                    continue;
                if (tofield.GetCustomAttribute<CopyIgnoreFieldAttribute>() is not null)
                    continue;
                if (!tofield.FieldType.IsAssignableFrom(fromfields[i].FieldType))
                    continue;

                var exp = Expression.Assign(Expression.Field(to, tofield), Expression.Field(from, fromfields[i]));
                fieldCopy.Add(exp);
                allcopy.Add(exp);
                if (!(fromfields[i].GetCustomAttribute<CopyFieldAttribute>() is null)) {
                    aoCopy.Add(exp);
                }
            }

            for (int i = 0; i < fromprops.Length; i++) {
                if (fromprops[i].GetCustomAttribute<CopyIgnoreFieldAttribute>() is not null)
                    continue;
                if (!fromprops[i].CanRead || !fromprops[i].GetMethod.IsPublic || !fromprops[i].SetMethod.IsPublic || fromprops[i].GetMethod.IsStatic)
                    continue;
                var toprop = toType.GetProperty(fromprops[i].Name);
                if (toprop == null)
                    continue;
                if (toprop.GetCustomAttribute<CopyIgnoreFieldAttribute>() is not null)
                    continue;
                if (!toprop.CanWrite || !fromprops[i].GetMethod.IsPublic || !fromprops[i].SetMethod.IsPublic || fromprops[i].SetMethod.IsStatic)
                    continue;
                if (!toprop.PropertyType.IsAssignableFrom(fromprops[i].PropertyType))
                    continue;

                var exp = Expression.Assign(Expression.Property(to, toprop), Expression.Property(from, fromprops[i]));
                propCopy.Add(exp);
                allcopy.Add(exp);
                if (!(fromprops[i].GetCustomAttribute<CopyFieldAttribute>() is null)) {
                    aoCopy.Add(exp);
                }
            }
            if (propCopy.Count > 0) {
                propCopy.Add(to);
                Expression.Block(propCopy.ToArray());
                PropertyCopyAction = Expression.Lambda<Func<TFrom, TTo, TTo>>(Expression.Block(propCopy.ToArray()), from, to).Compile();
            }
            if (fieldCopy.Count > 0) {
                fieldCopy.Add(to);
                Expression.Block(fieldCopy.ToArray());
                FieldCopyAction = Expression.Lambda<Func<TFrom, TTo, TTo>>(Expression.Block(fieldCopy.ToArray()), from, to).Compile();
            }
            if (propCopy.Count + fieldCopy.Count > 0) {
                allcopy.Add(to);
                Expression.Block(allcopy.ToArray());
                AllCopyAction = Expression.Lambda<Func<TFrom, TTo, TTo>>(Expression.Block(allcopy.ToArray()), from, to).Compile();
            }
            if (aoCopy.Count > 0) {
                aoCopy.Add(to);
                Expression.Block(aoCopy.ToArray());
                AttrOrientedCopyAction = Expression.Lambda<Func<TFrom, TTo, TTo>>(Expression.Block(aoCopy.ToArray()), from, to).Compile();
            }
        }
    }
}
