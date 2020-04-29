using Coding;
using System;
using System.IO;
 
namespace Sandbox {
    public static class FinancialDataGenerator {
        public static void AsCsv(string outpath, string fileDesc, double priceSeed, 
            double volSeed, DateTime startDate, DateTime endDate) {
            var generator = new Random(DateTime.Now.Millisecond);
            using (var writer = new StreamWriter(Path.Combine(outpath, $"{fileDesc}.csv"))) {
                writer.WriteLine("Date,Open,High,Low,Close,Volume,Adj Close,");
                var dt = endDate.AddWeekdays(0, false);
                while (dt >= startDate) {
                    var t = GenCsvRow(generator, priceSeed, volSeed, dt);
                    writer.WriteLine(t.Item1);
                    priceSeed = t.Item2;
                    dt = dt.AddWeekdays(-1, false);
                }
            }
        }

        private static Tuple<string,double> GenCsvRow(Random r, double open, double volSeed, DateTime dt) {
            var max = open * 1.15;
            var min = open * 0.85;
            var highF = r.NextDouble();
            var high = highF * (max - open) + open;
            var lowF = r.NextDouble();
            var low = lowF * (open - min) + min;
            var closeF = r.NextDouble();
            var close = closeF * (high - low) + low;
            var volMin = (volSeed * 0.5).ToInt();
            var volMax = (volSeed * 1.5).ToInt();
            var vol = r.Next(volMin, volMax);
            Console.WriteLine(close / open);
            return Tuple.Create($"{dt:yyyy-MM-dd},{open:F2},{high:F2},{low:F2},{close:F2},{vol:F0},{close:F2},", 
                close);
        }
    }
}
