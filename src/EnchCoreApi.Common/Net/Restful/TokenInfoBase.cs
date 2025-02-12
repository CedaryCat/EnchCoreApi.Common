namespace EnchCoreApi.Common.Net.Restful
{
    public class TokenInfoBase
    {
        public TokenInfoBase(string name, string token) {
            UserName = name;
            Token = token;
        }
        public string UserName { get; set; }
        public string Token { get; set; }
        public virtual bool HasPermission(string permission) {
            if (string.IsNullOrWhiteSpace(permission)) {
                return true;
            }
            return false;
        }
        public static TokenInfoBase None => new TokenInfoBase("None", "");
        public override bool Equals(object? obj) {
            if (obj is TokenInfoBase) {
                var token = (TokenInfoBase)obj;
                return token.GetHashCode() == this.GetHashCode();
            }
            return base.Equals(obj);
        }
        public override int GetHashCode() {
            return (string.IsNullOrWhiteSpace(this.Token) ? "" : this.Token).GetHashCode();
        }
        public override string ToString() {
            return $"{UserName}:{Token}";
        }
    }
}
