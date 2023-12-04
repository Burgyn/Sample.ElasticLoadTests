using ShellProgressBar;
using System;
using System.Threading.Tasks;

namespace Sample.ElasticLoadTests
{
    static class LoadTest
    {
        public static async Task RunAsync(int iterationCount)
        {
            var elastic = new ElasticHelper();
            var statistics = new Statistics(await elastic.GetDocumentsCountAsync());
            var options = new ProgressBarOptions
            {
                ProgressBarOnBottom = true,
                BackgroundCharacter = '\u2593'
            };
            //using var pbar = new ProgressBar(iterationCount, "Elastic load test", options);

            for (int i = 0; i < iterationCount; i++)
            {
                //pbar.Tick();
                int count = DataGenerator.RandomCount();
                var documents = DataGenerator.GenerateDocuments(count);
                statistics.Add(count);
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var took = await elastic.UploadDocumentsAsync(documents);
                sw.Stop();

                //Thread.Sleep(TimeSpan.FromMilliseconds(1000));

                var batch = statistics.AddIndexedCount(await elastic.GetDocumentsCountAsync());
                //pbar.WriteLine(batch.Dump() + " --- Duration: " + sw.ElapsedMilliseconds + "  ----- Took: " + took);
                Console.WriteLine(batch.Dump() + " --- Duration: " + sw.ElapsedMilliseconds + "  ----- Took: " + took);
            }

            statistics.Dump();
        }
    }
}
