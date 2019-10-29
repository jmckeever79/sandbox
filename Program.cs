using System;

namespace Sandbox {
    class Program {
        static void Main(string[] args) {
            var matrix = new SudokuMatrixModel();
            matrix.GetCell(0, 1).Value = 4;
            matrix.GetCell(1, 5).Value = 7;
            matrix.GetCell(3, 6).Value = 7;
            matrix.GetCell(6, 4).Value = 2;
            Console.Write(matrix.Validate());
        }
    }
}
