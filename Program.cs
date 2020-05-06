using System;
using System.Linq;

namespace Sandbox {
    class Program {
        static void Main(string[] args) {
            /*var path = "C:/Users/jj/git/ml for finance/data";
            var sd = new DateTime(2012, 1, 1);
            var ed = new DateTime(2012, 12, 31);
            FinancialDataGenerator.AsCsv(path, "IBM", 300, 30000000, sd, ed);*/

            /*var results = LeetcodeSandbox.GroupAnagrams(new string[] { "eat", "tea", "tan", "ate", "nat", "bat" });
            foreach (var sList in results) {
                foreach (var item in sList) {
                    Console.Write($"{item} ");
                }
                Console.WriteLine();
            }*/

            var max = LeetcodeSandbox.ComputeMaxSubarraySum(new[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 });
            Console.WriteLine($"Max subarray sum is {max}.");

            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
        }

        static void RunSudoku() {
            var matrix = SudokuMatrixModel.GenerateRandomBoard();
            Console.Write(matrix.ToString());

            var valid = matrix.Validate();
            Console.WriteLine("Matrix is {0}", valid ? "valid" : "invalid!");
            var errors = matrix.GetInvalidCells().OrderBy(x => x.Ix.Row).ThenBy(x => x.Ix.Column);
            foreach (var item in errors) {
                Console.WriteLine("Cell {0} is invalid", item);
            }
            var gridErrors = matrix.GetInvalidGrids().OrderBy(x => x.Ix.Row).ThenBy(x => x.Ix.Column);
            foreach (var item in gridErrors) {
                Console.WriteLine("Grid {0} is invalid", item);
            }
        }
    }
}
