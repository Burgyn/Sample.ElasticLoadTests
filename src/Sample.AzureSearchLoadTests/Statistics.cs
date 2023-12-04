using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Sample.ElasticLoadTests
{
    public class Statistics
    {
        private int _actualBatch = 0;
        private long _totalCount = 0;
        private readonly List<Batch> _batches = new();
        private int _errorSequence = 0;
        private Dictionary<int, int> _errorGroups = new();

        public Statistics(long initTotalCount)
        {
            _totalCount = initTotalCount;
        }

        public void Add(int actualBatch)
        {
            _actualBatch += actualBatch;
            _totalCount += actualBatch;
        }

        public Batch AddIndexedCount(long indexedCount)
        {
            Batch batch = new(DateTime.Now, _totalCount, _actualBatch, indexedCount);

            _batches.Add(batch);
            _actualBatch = 0;

            ErrorSequence(batch);

            return batch;
        }

        private void ErrorSequence(Batch batch)
        {
            if (batch.MissingDataCount > 0)
            {
                _errorSequence++;
            }
            else
            {
                int count = _errorGroups.ContainsKey(_errorSequence) ? _errorGroups[_errorSequence] : 0;
                _errorGroups[_errorSequence] = ++count;
                _errorSequence = 0;
            }
        }

        public void Dump()
        {
            using var writer = new StreamWriter("result.csv");
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" });

            csv.WriteRecords(_batches);

            Console.WriteLine();
            Console.WriteLine("Error groups:");
            foreach (var item in _errorGroups.OrderBy(p => p.Key))
            {
                Console.WriteLine($"Sequence: '{item.Key}' - {item.Value}");
            }
        }

        public record Batch(DateTime Time, long TotalUploadedCount, int ActualUploadedCount, long TotalIndexedCount)
        {
            public long MissingDataCount => TotalUploadedCount - TotalIndexedCount;

            public long ActualIndexedCount => ActualUploadedCount - MissingDataCount;

            public string Dump()
                => $"Total uploaded count: {TotalUploadedCount}, Total indexed count: {TotalIndexedCount}, Actual uploaded count: {ActualUploadedCount}, Actual indexed count: {ActualIndexedCount}, Missing data count {MissingDataCount}";
        }
    }
}
