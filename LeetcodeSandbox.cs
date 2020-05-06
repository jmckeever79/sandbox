using Coding;
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

        private static int ComputeMaxCrossingSum(int[] nums, int startIndex, int endIndex, int mid) {
            var sum = 0;
            var leftSum = int.MinValue;
            for (int i = mid;  i >= startIndex; i--) {
                sum += nums[i];
                if (sum > leftSum) {
                    leftSum = sum;
                }
            }

            sum = 0;
            var rightSum = int.MinValue;
            for (int i = mid+1;  i<=endIndex; i++) {
                sum += nums[i];
                if (sum > rightSum) {
                    rightSum = sum;
                }
            }

            return MathUtils.Max(new[] { leftSum, rightSum, leftSum + rightSum });
        }

        private static int ComputeMaxSubarraySumHelper(int[] nums, int startIndex, int endIndex) {
            if (startIndex==endIndex) {
                return nums[startIndex];
            }

            int mid = (startIndex + endIndex) / 2;

            return MathUtils.Max(new[] { ComputeMaxSubarraySumHelper(nums, startIndex, mid),
                ComputeMaxSubarraySumHelper(nums, mid+1, endIndex),
                ComputeMaxCrossingSum(nums, startIndex, endIndex, mid)});
        }

        public static int ComputeMaxSubarraySum(int[] nums) {
            return ComputeMaxSubarraySumHelper(nums, 0, nums.Length - 1);
        }
    }
}
