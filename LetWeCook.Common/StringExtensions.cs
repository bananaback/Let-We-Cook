namespace LetWeCook.Common
{
    public static class StringExtensions
    {
        public static int LevenshteinDistance(this string source, string target)
        {
            if (string.IsNullOrEmpty(source)) return target?.Length ?? 0;
            if (string.IsNullOrEmpty(target)) return source.Length;

            var lengthA = source.Length;
            var lengthB = target.Length;
            var matrix = new int[lengthA + 1, lengthB + 1];

            for (int i = 0; i <= lengthA; matrix[i, 0] = i++) { }
            for (int j = 0; j <= lengthB; matrix[0, j] = j++) { }

            for (int i = 1; i <= lengthA; i++)
            {
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = target[j - 1] == source[i - 1] ? 0 : 1;
                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }

            return matrix[lengthA, lengthB];
        }
    }
}
