using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        var set = new HashSet<string>(words);
        var result = new List<string>();

        foreach (var w in set)
        {
            if (w.Length != 2) continue;
            if (w[0] == w[1]) continue;

            var rev = new string(new[] { w[1], w[0] });
            if (set.Contains(rev) && string.Compare(w, rev, StringComparison.Ordinal) < 0)
            {
                result.Add($"{w} & {rev}");
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var line in File.ReadLines(filename))
        {
            var trimmed = line?.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;

            var fields = SplitCsv(trimmed);
            if (fields.Length < 4) continue;

            var degree = fields[3].Trim();
            if (degree.Length == 0) continue;

            degrees[degree] = degrees.TryGetValue(degree, out var c) ? c + 1 : 1;
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  
    /// Ignore spaces and cases. Use a dictionary to solve.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        if (word1 == null || word2 == null) return false;

        var a = word1.Replace(" ", string.Empty).ToLowerInvariant();
        var b = word2.Replace(" ", string.Empty).ToLowerInvariant();

        if (a.Length != b.Length) return false;

        var counts = new Dictionary<char, int>();
        foreach (var ch in a)
        {
            counts[ch] = counts.TryGetValue(ch, out var c) ? c + 1 : 1;
        }

        foreach (var ch in b)
        {
            if (!counts.TryGetValue(ch, out var c)) return false;
            if (c == 1) counts.Remove(ch);
            else counts[ch] = c - 1;
        }

        return counts.Count == 0;
    }

    /// <summary>
    /// Fetches earthquake data (USGS daily JSON feed) 
    /// and returns ["Place - Mag X.YY", ...].
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);
        if (featureCollection?.Features == null) return Array.Empty<string>();

        return featureCollection.Features
            .Where(f => f?.Properties != null)
            .Select(f =>
            {
                var place = f!.Properties!.Place ?? "Unknown location";
                var mag = f.Properties.Mag.HasValue ? f.Properties.Mag.Value.ToString("0.##") : "N/A";
                return $"{place} - Mag {mag}";
            })
            .ToArray();
    }

    // --- helpers ---
    private static string[] SplitCsv(string s)
    {
        var list = new List<string>();
        var sb = new StringBuilder();
        bool inQ = false;

        for (int i = 0; i < s.Length; i++)
        {
            char ch = s[i];
            if (ch == '\"')
            {
                if (inQ && i + 1 < s.Length && s[i + 1] == '\"') { sb.Append('\"'); i++; }
                else inQ = !inQ;
            }
            else if (ch == ',' && !inQ)
            {
                list.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(ch);
            }
        }
        list.Add(sb.ToString());
        return list.ToArray();
    }
}
