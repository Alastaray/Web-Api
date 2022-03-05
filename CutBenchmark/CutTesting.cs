
using AspEndpoint;
using BenchmarkDotNet.Attributes;


namespace CutBenchmark
{
    [MemoryDiagnoser]
    [RankColumn]
    public class CutTesting
    {
        public const string path = "C:\\";
        public const string name = "test.jpg";
        public Picture picture;
        public CutTesting()
        {
            picture = new Picture(path, name);                     
        }
        [Benchmark]
        [IterationCount(100)]
        public void TestCut()
        {
            picture.Cut(100);
        }
        [Benchmark]
        [IterationCount(100)]
        public async Task TestCutAsync()
        {
            await picture.CutAsync(100);
        }
        
    }
}
