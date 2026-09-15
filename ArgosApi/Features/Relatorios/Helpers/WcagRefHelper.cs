using System.Text.RegularExpressions;

namespace ArgosApi.Features.Relatorios.Helpers
{
    /// <summary>
    /// Normaliza tags WCAG do axe-core (wcag143) e códigos já pontuados (1.4.3).
    /// </summary>
    public static partial class WcagRefHelper
    {
        /// <summary>
        /// Formata referências WCAG e descarta o SC 4.1.1, obsoleto na WCAG 2.2.
        /// </summary>
        public static List<string> Format(IEnumerable<string>? referencias)
        {
            if (referencias is null)
            {
                return [];
            }

            return referencias
                .Select(FormatOne)
                .Where(criterio => criterio is not null)
                .Cast<string>()
                .Distinct()
                .OrderBy(criterio => criterio, Comparer<string>.Create(Compare))
                .ToList();
        }

        /// <summary>
        /// Converte uma tag do axe-core ou um código já pontuado no formato 1.4.3.
        /// </summary>
        public static string? FormatOne(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return null;
            }

            var tag = raw.Trim();
            var formatted = FormattedScRegex().Match(tag);
            string? criterio;
            if (formatted.Success)
            {
                criterio = $"{formatted.Groups[1].Value}.{formatted.Groups[2].Value}.{int.Parse(formatted.Groups[3].Value)}";
            }
            else
            {
                var axe = AxeScTagRegex().Match(tag);
                if (!axe.Success)
                {
                    return null;
                }

                criterio = $"{axe.Groups[1].Value}.{axe.Groups[2].Value}.{int.Parse(axe.Groups[3].Value)}";
            }

            return criterio == "4.1.1" ? null : criterio;
        }

        private static int Compare(string a, string b)
        {
            var aParts = a.Split('.').Select(part => int.TryParse(part, out var n) ? n : 0).ToArray();
            var bParts = b.Split('.').Select(part => int.TryParse(part, out var n) ? n : 0).ToArray();
            for (var i = 0; i < 3; i++)
            {
                var av = i < aParts.Length ? aParts[i] : 0;
                var bv = i < bParts.Length ? bParts[i] : 0;
                if (av != bv)
                {
                    return av - bv;
                }
            }

            return 0;
        }

        [GeneratedRegex(@"^wcag(\d)(\d)(\d+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        private static partial Regex AxeScTagRegex();

        [GeneratedRegex(@"^(\d+)\.(\d+)\.(\d+)$", RegexOptions.CultureInvariant)]
        private static partial Regex FormattedScRegex();
    }
}
