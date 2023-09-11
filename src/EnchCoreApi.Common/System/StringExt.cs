namespace System {
    public static class StringExt {
        public static String SFormat(this String str, params object[] args) {
            return String.Format(str, args);
        }
        public static bool SimilarTo(this string me, string input) {
            return (me.SimilarTo(input, 2) || me.SimilarTo(input, 0.4f));
        }

        public static bool SimilarTo(this string me, string input, int similarChar) {
            if (input.Contains(me) || me.Contains(input))
                return true;
            int count = 0;
            foreach (var c in me) {
                if (input.Contains(c)) {
                    count++;
                    if (count >= similarChar) {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool SimilarTo(this string me, string input, float similarity) {
            if (input.Contains(me) || me.Contains(input))
                return true;
            int count = 0;
            foreach (var c in me) {
                if (input.Contains(c)) {
                    count++;
                    if (count / (float)me.Length >= similarity) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
