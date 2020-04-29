using System;
using System.Linq;

namespace Sandbox {
    class Program {
        static void Main(string[] args) {
            var path = "C:/Users/jj/git/ml for finance/data";
            var sd = new DateTime(2012, 1, 1);
            var ed = new DateTime(2012, 12, 31);
            FinancialDataGenerator.AsCsv(path, "IBM", 300, 30000000, sd, ed);
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
