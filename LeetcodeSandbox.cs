using System.Collections.Generic;
using System.Linq;

namespace Sandbox {
    public static class LeetcodeSandbox {
        public static int ComputeMaxProfit(int[] prices) {
            var ct = prices.Length;
            var profit = 0;
            var sellPx = prices[ct - 1];
            for (int i = ct - 2; i > 0; i--) {
                var currPx = prices[i];
                var priorPx = prices[i - 1];
                if (currPx == sellPx) {
                    continue;
                }
                if (currPx > sellPx) {
                    sellPx = currPx;
                } else if (priorPx > currPx && currPx < sellPx) {
                    profit += sellPx - currPx;
                    sellPx = priorPx;
                }
            }
            var firstPx = prices[0];
            if (firstPx < sellPx) {
                profit += sellPx - firstPx;
            }
            return profit;
        }

        public static IList<IList<string>> GroupAnagrams(string[] strs) {
            var d = new Dictionary<string, List<string>>();
            foreach (var s in strs) {
                var ordered = new string(s.OrderBy(x => x).ToArray());
                List<string> anagrams;
                if (!d.TryGetValue(ordered, out anagrams)) {
                    d[ordered] = anagrams = new List<string>();
                }
                anagrams.Add(s);
            }
            var result = new List<IList<string>>();
            foreach (var item in d) {
                result.Add(item.Value);
            }
            return result;
        }
    }
}
