using System.Text.Json;

namespace EnchCoreApi.Common.Native {

    public class RidGraph {
        public Dictionary<string, string[]> _ridParents { get; } = new();

        public RidGraph() {
            using var stream = typeof(RidGraph).Assembly.GetManifestResourceStream("Native\\RuntimeIdentifierGraph.json")!;
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            LoadRidGraph(json);
        }

        private void LoadRidGraph(string json) {
            using var doc = JsonDocument.Parse(json);
            foreach (var ridNode in doc.RootElement.GetProperty("runtimes").EnumerateObject()) {
                var rid = ridNode.Name;
                var imports = ridNode.Value.GetProperty("#import").EnumerateArray()
                    .Select(e => e.GetString()!)
                    .ToArray();
                _ridParents[rid] = imports;
            }
        }

        public IEnumerable<string> ExpandRuntimeIdentifier(string rid) {
            var visited = new HashSet<string>();
            var queue = new Queue<string>();
            queue.Enqueue(rid);

            while (queue.Count > 0) {
                var current = queue.Dequeue();
                if (!visited.Add(current)) continue;

                yield return current;

                if (_ridParents.TryGetValue(current, out var parents)) {
                    foreach (var parent in parents) {
                        if (!visited.Contains(parent)) {
                            queue.Enqueue(parent);
                        }
                    }
                }
            }
        }
    }
}
