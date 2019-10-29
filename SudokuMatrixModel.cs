using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sandbox {
    public sealed class Index : IEquatable<Index> {
        private readonly int row;
        private readonly int column;

        public Index(int row, int column) {
            this.row = row;
            this.column = column;
        }
        

        public override bool Equals(object obj) {
            var ix = obj as Index;
            return Equals(ix);
        }

        public bool Equals(Index ix) {
            return ix != null && Row == ix.Row && Column != ix.Column;
        }

        public override int GetHashCode() {
            return row * 17 + column * 23;
        }

        public override string ToString() {
            return string.Format("[{0},{1]]", Row, Column);
        }

        public int Row => row;

        public int Column => column;
    }

    public sealed class Cell {
        private readonly Index ix;

        public Cell(Index ix) {
            this.ix = ix;
            IsValid = true;
            Value = null;
        }

        public Index Ix => ix;

        public override bool Equals(object obj) {
            return Ix.Equals(obj);
        }

        public override int GetHashCode() {
            return ix.GetHashCode();
        }

        public int? Value { get; set; }

        public bool IsValid { get; set; }
    }

    public sealed class Grid {
        private static IEnumerable<int> targetSet = Enumerable.Range(1, 9);
        private static readonly int targetSum;

        static Grid() {
            targetSum = targetSet.Sum();
        }

        private readonly Dictionary<Index,Cell> matrix = new Dictionary<Index, Cell>();

        public void AddCell(Cell c) {
            matrix[c.Ix] = c;
        }

        public bool CheckCompleteness() {
            var missing = matrix.Select(x => x.Value).Where(x => x.Value == null).ToArray();
            return missing.Length == 0;

        }

        public bool Validate() {
            return matrix.Select(x => x.Value).ToArray().Validate();
        }

        public void InvalidateMatchingCells(Cell c) {
            var matches = matrix.Where(x => x.Value.Value == c.Value);
            foreach (var m in matches) {
                m.Value.IsValid = false;
            }
        }

        public override string ToString() {
            return base.ToString();
        }

        public override bool Equals(object obj) {
            return matrix.Equals(obj);
        }

        public override int GetHashCode() {
            return matrix.GetHashCode();
        }

        public bool IsValid { get; set; }


    }

    public class SudokuMatrixModel {
        private const int CellCount = 9;
        private const int GridCount = 3;

        private readonly Cell[,] matrix = new Cell[CellCount, CellCount];
        private readonly Grid[,] grids = new Grid[GridCount, GridCount];

        public SudokuMatrixModel() {
            for (int i = 0; i < CellCount; i++) {
                for (int j = 0; j < CellCount; j++) {
                    var ix = new Index(i, j);
                    var c = new Cell(ix);
                    matrix[i, j] = c;
                    var gridX = Math.Round(i / 3.0, 0).ToInt();
                    var gridY = Math.Round(j / 3.0, 0).ToInt();
                    var i1 = i % 3;
                    var j1 = j % 3;
                    grids[gridX, gridY].AddCell(c);
                }
            }
        }

        public Cell GetCell(int row, int col) {
            if (row >= matrix.GetLength(0) || col >= matrix.GetLength(1)) {
                throw new Exception($"Location <{row},{col}> is not valid for this matrix");
            }
            return matrix[row, col];
        }

        public void UpdateValue(Cell c, int value) {
            c.Value = value;
        }

        public bool Validate() {
            return ValidateRows() && ValidateColumns() && ValidateGrids();
        }

        public bool CheckCompleteness() {
            var missingValues = grids.Keys.Where(x => x.Value == null).ToArray();
            return missingValues.Length == 0;
        }

        public bool ValidateEdit(Cell editedCell) {
            Grid g;
            if (!grids.TryLookup(editedCell, out g)) {
                throw new Exception("Grid for cell {0} not present in grid collection - Matrix initialization was improperly completed");
            }
            if (editedCell.Value == null) {
                editedCell.IsValid = false;
                return false;
            }
            return ValidateColumn(editedCell) && ValidateRow(editedCell) && ValidateGrid(editedCell);
        }

        private bool ValidateRow(Cell editedCell) {
            var r = matrix.GetRow(editedCell.Ix.Row);
            return r.Validate();
        }

        private bool ValidateRows() {
            var valid = true;
            for (int i = 0; i < CellCount; i++) {
                var rowCells = matrix.GetRow(i);
                if (!rowCells.Validate()) {
                    valid = false;
                }
            }
            return valid;
        }

        private bool ValidateColumn(Cell editedCell) {
            var c = matrix.GetColumn(editedCell.Ix.Column);
            return c.Validate();
        }

        private bool ValidateColumns() {
            var valid = true;
            for (int i = 0; i < CellCount; i++) {
                var colCells = matrix.GetColumn(i);
                if (!colCells.Validate()) {
                    valid = false;
                }
            }
            return valid;
        }

        private bool ValidateGrid(Cell editedCell) {
            var g = grids[editedCell];
            return g.Validate();
        }

        private bool ValidateGrids() {
            var uniqueGrids = grids.Select(x => x.Value).Distinct();
            var valid = true;
            foreach (var item in uniqueGrids) {
                if (!item.Validate()) {
                    valid = false;
                }
            }
            return valid;
        }
    }

    public static class Extensions {
        public static bool Validate(this IEnumerable<Cell> cells, int size = 9) {
            var valid = true;
            for (int i = 0; i < size - 1; i++) {
                var matches = cells.Where(x => x.Value == i + 1).ToArray();
                if (matches.Length > 1) {
                    foreach (var item in matches) {
                        item.IsValid = false;
                        valid = false;
                    }
                }
            }
            return valid;
        }

        public static V LookupOrCreate<K, V>(this Dictionary<K, V> collection, K key, V defaultValue) {
            if (!collection.ContainsKey(key)) {
                collection.Add(key, defaultValue);
                return defaultValue;
            }
            return collection[key];
        }

        public static bool TryLookup<K,V>(this IDictionary<K,V> dict, K key, out V value, bool strict = false) {
            if (dict.TryGetValue(key, out value)) {
                if (strict) {
                    throw new Exception(string.Format("Key {0} not present in dictionary", key));
                }
                value = default(V);
                return false;
            }
            return true;

        }

        public static T[] GetRow<T>(this T[,] matrix, int rowNumber) {
            return Enumerable.Range(0, matrix.GetLength(1))
               .Select(x => matrix[rowNumber, x])
               .ToArray();
        }

        public static T[] GetColumn<T>(this T[,] matrix, int colNumber) {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[x, colNumber])
                    .ToArray();
        }

        public static int ToInt(this double d) {
            return (int)d;
        }
    }
}