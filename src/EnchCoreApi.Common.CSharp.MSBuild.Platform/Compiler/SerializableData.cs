using EnchCoreApi.Common.Dynamic;
using System.Text;

namespace EnchCoreApi.Common.CSharp.MSBuild.Platform.Compiler {
    public abstract class SerializableData {
        static SerializableData() {
        }

        protected abstract void Serialize(BinaryWriter writer);

        protected abstract void Deserialize(BinaryReader reader);
        public string GetBase64() {
            using var stream = new MemoryStream();
            using var w = new BinaryWriter(stream, Encoding.UTF8);
            Serialize(w);
            var bs = stream.ToArray();

            using var br = new BinaryReader(new MemoryStream(bs), Encoding.UTF8);

            var al = bs.Length;
            var t = Convert.ToBase64String(bs);
            var sl = t.Length;
            return t;
        }
        public void FromBase64(string base64) {
            var arr = Convert.FromBase64String(base64);
            using var stream = new MemoryStream(arr);
            using var r = new BinaryReader(stream, Encoding.UTF8);
            try {
                Deserialize(r);
            }
            catch {
                var ins = this;
            }
        }
    }
}
