using System;

namespace YTMusicUploader.Helpers.FuzzyLogic
{
    public static class Levenshtein
    {
        /// <summary>
        /// Convert strings to lowercase before comparing.
        /// </summary>
        public static bool IrgnoreCase = true;

        /// <summary>
        /// Compute the similarity of two strings using the Levenshtein distance.
        /// </summary>
        /// <param name="s">The first string.</param>
        /// <param name="t">The second string.</param>
        /// <returns>A floating point value between 0.0 and 1.0</returns>
        public static float Similarity(string s, string t)
        {
            int maxLen = Math.Max(s.Length, t.Length);

            if (maxLen == 0)
            {
                return 1.0f;
            }

            if (IrgnoreCase)
            {
                s = s.ToLowerInvariant();
                t = t.ToLowerInvariant();
            }

            float dis = Distance(s, t);

            return 1.0f - dis / maxLen;
        }

        private static int Distance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1]; // matrix
            int cost = 0;

            if (n == 0) return m;
            if (m == 0) return n;

            // Initialize.
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;

            // Find min distance.
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    cost = (t.Substring(j - 1, 1) == s.Substring(i - 1, 1) ? 0 : 1);
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
    }
}
