using Awv.Automation.Generation.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Awv.Automation.Generation
{
    public class Sequence : IRNG
    {
        public int IntegerIndex { get; set; } = 0;
        public int DoubleIndex { get; set; } = 0;
        public List<int> Integers { get; set; } = new List<int>();
        public List<double> Doubles { get; set; } = new List<double>();

        public Sequence()
        {

        }

        public Sequence(string base64)
        {
            LoadBase64(base64);
        }

        public Sequence(params int[] int16s)
        {
            Integers = new List<int>(int16s);
        }

        public Sequence(params double[] doubles)
        {
            Doubles = new List<double>(doubles);
        }

        public Sequence(IEnumerable<int> int16s, IEnumerable<double> doubles)
        {
            Integers = new List<int>(int16s);
            Doubles = new List<double>(doubles);
        }

        public void LoadBase64(string base64)
        {
            var bytes = Convert.FromBase64String(base64);

            using (var ms = new MemoryStream(bytes))
            using (var reader = new BinaryReader(ms))
            {
                ms.Seek(0, SeekOrigin.Begin);
                var intCount = reader.ReadInt32();
                var doubleCount = reader.ReadInt32();

                for (var i = 0; i < intCount; i++)
                    Integers.Add(reader.ReadInt32());
                for (var i = 0; i < doubleCount; i++)
                    Doubles.Add(reader.ReadDouble());
            }
        }

        public string ToBase64()
        {
            var base64 = new StringBuilder();
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(Integers.Count);
                writer.Write(Doubles.Count);
                Integers.ForEach(val => writer.Write(val));
                Doubles.ForEach(val => writer.Write(val));

                ms.Seek(0, SeekOrigin.Begin);

                var bytes = new byte[ms.Length];

                ms.Read(bytes, 0, bytes.Length);

                base64.Append(Convert.ToBase64String(bytes));
            }

            return base64.ToString();
        }

        public int Next()
            => Integers[IntegerIndex++ % Integers.Count];

        public int Next(int maxValue)
        {
            if (maxValue <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxValue), $"'{nameof(maxValue)}' must be greater than zero.");

            return Next(0, maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException(nameof(minValue), $"'{nameof(minValue)}' cannot be greater than {nameof(maxValue)}.");


            var range = maxValue - minValue;
            var value = Next() % maxValue;

            if (value < minValue)
                value += range;

            return value;
        }

        public double NextDouble()
            => Doubles[DoubleIndex++ % Doubles.Count];
    }
}
