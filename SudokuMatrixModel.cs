using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return ix != null && row == ix.Row && column == ix.Column;
        }

        public override int GetHashCode() {
            return row * 17 + column * 23;
        }

        public override string ToString() {
            return string.Format("[{0},{1}]", Row, Column);
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

        public Tuple<int,int> ComputeGridIndex() {
            return ((int)Math.Truncate(Ix.Row / 3.0)).
                TupleWith((int)Math.Truncate(Ix.Column / 3.0));
        }

        public override bool Equals(object obj) {
            return Ix.Equals(obj);
        }

        public override int GetHashCode() {
            return ix.GetHashCode();
        }

        public override string ToString() {
            return string.Format("<{0}: value={1}>", Ix, Value);
        }

        public int? Value { get; set; }

        public bool IsValid { get; set; }
    }

    public sealed class Grid {
        private readonly Dictionary<Index,Cell> matrix = new Dictionary<Index, Cell>();

        private readonly Index ix;

        public Grid(Index ix) {
            this.ix = ix;
        }

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
            return string.Format($"<ix>");
        }

        public override bool Equals(object obj) {
            return matrix.Equals(obj);
        }

        public override int GetHashCode() {
            return matrix.GetHashCode();
        }

        public bool IsValid { get; set; }

        public Index Ix => ix;
    }

    public class SudokuMatrixModel {
        private const int CellCount = 9;
        private const int GridCount = 3;

        public static SudokuMatrixModel GenerateRandomBoard() {
            var model = new SudokuMatrixModel();
            // add thirty values
            var count = 0;
            var r = new Random();
            var usedSquares = new HashSet<Index>();
            while (true) {
                if (count == 30) break;
                // the max value for Next is EXCLUSIVE
                var nextX = r.Next(0, CellCount);
                var nextY = r.Next(0, CellCount);
                var nextIx = new Index(nextX, nextY);
                if (usedSquares.Contains(nextIx)) {
                    continue;
                }
                var nextVal = r.Next(1, CellCount + 1);
                var c = model.GetCell(nextX, nextY);
                model.UpdateValue(c, nextVal);
                count++;
                usedSquares.Add(nextIx);
                Console.WriteLine($"Set {nextIx} to {nextVal}");
            }
            return model;
        }

        private readonly Cell[,] matrix = new Cell[CellCount, CellCount];
        private readonly Grid[,] grids = new Grid[GridCount, GridCount];

        public SudokuMatrixModel() {
            for (int i = 0; i < CellCount; i++) {
                for (int j = 0; j < CellCount; j++) {
                    var ix = new Index(i, j);
                    var c = new Cell(ix);
                    matrix[i, j] = c;
                    var gridIx = c.ComputeGridIndex();
                    var g = grids[gridIx.Item1, gridIx.Item2];
                    if (g == null) {
                        grids[gridIx.Item1, gridIx.Item1] = g = new Grid(new Index(gridIx.Item1, gridIx.Item2));
                    }
                    g.AddCell(c);
                }
            }
        }

        public override string ToString() {
            var sb = new StringBuilder();
            for (int i = 0; i < CellCount; i++) {
                sb.Append("|");
                for (int j = 0; j < CellCount; j++) {
                    var c = GetCell(i, j);
                    sb.AppendFormat(" {0} |", c.Value == null ? " " : c.Value.ToString());
                }
                sb.Append("\n");
            }
            return sb.ToString();
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
            var missingValues = grids.Cast<Cell>().Where(x => x.Value == null).ToArray();
            return missingValues.Length == 0;
        }

        public bool ValidateEdit(Cell editedCell) {
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
            var loc = editedCell.ComputeGridIndex();
            var g = grids[loc.Item1, loc.Item2];
            return g.Validate();
        }

        private bool ValidateGrids() {
            var valid = true;
            foreach (var item in grids) {
                if (!item.Validate()) {
                    valid = false;
                }

            }
            return valid;
        }

        public IEnumerable<Cell> GetInvalidCells() {
            return matrix.Cast<Cell>().Select(x => x).Where(x => !x.IsValid);
        }

        public IEnumerable<Grid> GetInvalidGrids() {
            return grids.Cast<Grid>().Select(x => x).Where(x => !x.IsValid);
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

        public static Tuple<T,T> TupleWith<T>(this T first, T second) {
            return Tuple.Create(first, second);
        }
    }
}