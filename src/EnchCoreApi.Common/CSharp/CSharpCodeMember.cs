namespace EnchCoreApi.Common.CSharp
{
    public abstract class CSCMember
    {
        public CSCMember(string accessModifier, string modifier, string name) {
            AccessModifier = accessModifier;
            Modifier = modifier;
            Name = name;
        }
        public CSCMember(string accessModifier, string modifier, string name, params string[] attribute) : this(accessModifier, modifier, name) {
            Attribute = attribute;
        }
        public string AccessModifier { get; set; }
        public string Modifier { get; set; }
        public string Name { get; set; }
        public string[] Attribute { get; set; }
        public abstract string[] GetCode();
    }
    public class CSCNestedClass : CSCMember
    {
        public CSCNestedClass(string accessModifier, string modifier, string name, string? baseTypeName, params CSCMember[] members) : base(accessModifier, modifier, name) {
            Members = new List<CSCMember>(members);
            BaseTypeName = baseTypeName;
        }
        public CSCNestedClass(string accessModifier, string modifier, string name, string? baseTypeName, string[] textContent) : base(accessModifier, modifier, name) {
            BaseTypeName = baseTypeName;
            TextContent = textContent;
        }

        private readonly string[]? TextContent;
        public string? BaseTypeName { get; set; }

        private readonly List<CSCMember> Members = new List<CSCMember>();
        public override string[] GetCode() {
            int i = 0;
            if (TextContent != null) {
                var result = new string[(Attribute?.Length ?? 0) + 2 + TextContent.Length];
                if (Attribute is not null) {
                    foreach (var a in Attribute) {
                        result[i++] = $"[{a}]";
                    }
                }
                result[i++] = $"{(AccessModifier is null ? "" : $"{AccessModifier} ")}{(Modifier is null ? "" : $"{Modifier} ")}class {Name}{(BaseTypeName is null ? "" : $" : {BaseTypeName}")}";
                result[i++] = "{";
                foreach (var line in TextContent) {
                    result[i++] = line;
                }
                result[i++] = "}";
                return result;
            }
            else {
                var members = Members.Select(m => m.GetCode()).ToArray();
                var length = members.Sum(m => m.Length);
                var result = new string[(Attribute?.Length ?? 0) + 2 + length];
                if (Attribute is not null) {
                    foreach (var a in Attribute) {
                        result[i++] = $"[{a}]";
                    }
                }
                result[i++] = $"{(AccessModifier is null ? "" : $"{AccessModifier} ")}{(Modifier is null ? "" : $"{Modifier} ")}class {Name}{(BaseTypeName is null ? "" : $" : {BaseTypeName}")}";
                result[i++] = "{";
                foreach (var m in members) {
                    foreach (var line in m) {
                        result[i++] = line;
                    }
                }
                result[i++] = "}";
                return result;
            }
        }
    }

    public abstract class ValueMember : CSCMember
    {
        public Type Type { get; set; }
        public ValueMember(string accessModifier, string modifier, Type type, string name) : base(accessModifier, modifier, name) {
            Type = type;
        }
        public ValueMember(string accessModifier, string modifier, Type type, string name, params string[] attribute) : base(accessModifier, modifier, name, attribute) {
            Type = type;
        }
    }

    public class CSCField : ValueMember
    {
        public CSCField(string accessModifier, string modifier, Type type, string name) : base(accessModifier, modifier, type, name) {
        }
        public CSCField(string accessModifier, string modifier, Type type, string name, params string[] attribute) : base(accessModifier, modifier, type, name, attribute) {
        }
        public string[]? InitialValue { get; set; }
        public override string[] GetCode() {
            int i = 0;
            var result = new string[(Attribute?.Length ?? 0) + (InitialValue?.Length ?? 0)];
            if (Attribute != null) {
                foreach (var a in Attribute) {
                    result[i++] = $"[{a}]";
                }
            }
            if (InitialValue == null) {
                result[i++] = $"{(AccessModifier is null ? "" : $"{AccessModifier} ")}{(Modifier is null ? "" : $"{Modifier} ")}global::{Type.FullName} {Name};";
            }
            else {
                int i2 = 0;
                foreach (var v in InitialValue) {
                    if (i2 == 0) {
                        result[i++] = $"{(AccessModifier is null ? "" : $"{AccessModifier} ")}{(Modifier is null ? "" : $"{Modifier} ")}global::{Type.FullName} {Name} = {v}";
                    }
                    else {
                        result[i++] = v;
                    }
                    i2++;
                }
                result[InitialValue.Length - 1] += ";";
            }
            return result;
        }
    }

    public class CSCProperty : ValueMember
    {
        public CSCProperty(string accessModifier, string modifier, string name, Type type) : base(accessModifier, modifier, type, name) {
        }
        public CSCProperty(string accessModifier, string modifier, string name, Type type, params string[] attribute) : base(accessModifier, modifier, type, name, attribute) {
        }
        public bool DefaultAccessor => GetAccessorContent is null && SetAccessorContent is null;
        public string[] GetAccessorContent { get; set; }
        public string[] SetAccessorContent { get; set; }
        public override string[] GetCode() {
            int i = 0;
            var result = new string[(Attribute?.Length ?? 0) + (DefaultAccessor ? 1 : 7) + (GetAccessorContent?.Length ?? 0) + (SetAccessorContent?.Length ?? 0)];
            if (Attribute is not null) {
                foreach (var a in Attribute) {
                    result[i++] = $"[{a}]";
                }
            }
            if (DefaultAccessor) {
                result[i++] = $"{(AccessModifier is null ? "" : $"{AccessModifier} ")}{(Modifier is null ? "" : $"{Modifier} ")}global::{Type.FullName} {Name}" + " { get; set; }";
            }
            else {
                result[i++] = $"{(AccessModifier is null ? "" : $"{AccessModifier} ")}{(Modifier is null ? "" : $"{Modifier} ")}global::{Type.FullName} {Name}";
                result[i++] = "{";
                result[i++] = "    get{";
                if (GetAccessorContent is not null) {
                    foreach (var g in GetAccessorContent) {
                        result[i++] = "        " + g;
                    }
                }
                result[i++] = "    }";
                result[i++] = "    set{";
                if (SetAccessorContent is not null) {
                    foreach (var s in SetAccessorContent) {
                        result[i++] = "        " + s;
                    }
                }
                result[i++] = "    }";
                result[i++] = "}";
            }
            return result;
        }
    }

    public class CSCMethod : ValueMember
    {
        public CSCMethod(string accessModifier, string modifier, Type? type, string name, string parameters) : base(accessModifier, modifier, type, name) {
            Parameters = parameters;
        }
        public CSCMethod(string accessModifier, string modifier, Type? type, string name, string parameters, params string[] attribute) : base(accessModifier, modifier, type, name, attribute) {
            Parameters = parameters;
        }
        public string Parameters { get; set; }
        public string[] MethodContent { get; set; }
        public override string[] GetCode() {
            int i = 0;
            var result = new string[(Attribute?.Length ?? 0) + (MethodContent?.Length ?? 0) + (MethodContent is null ? 1 : 3)];
            if (Attribute is not null) {
                foreach (var a in Attribute) {
                    result[i++] = $"[{a}]";
                }
            }
            if (MethodContent is not null) {
                result[i++] = $"{(AccessModifier is null ? "" : $"{AccessModifier} ")}{(Modifier is null ? "" : $"{Modifier} ")}{(Type is null ? "void" : Type.FullName)} {Name} ({Parameters ?? ""})";
                result[i++] = "{";
                foreach (var v in MethodContent) {
                    result[i++] = "    " + v;
                }
                result[i++] = "}";
            }
            else {
                result[i++] = $"{(AccessModifier is null ? "" : $"{AccessModifier} ")}{(Modifier is null ? "" : $"{Modifier} ")}{(Type is null ? "void" : Type.FullName)} {Name} ({Parameters ?? ""}) " + "{ }";
            }
            return result;
        }
    }
}
