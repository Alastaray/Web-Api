
using AspEndpoint;
using BenchmarkDotNet.Attributes;


namespace CutBenchmark
{
    [MemoryDiagnoser]
    [RankColumn]
    public class CutTestingBDN
    {
        
        private Picture picture;
        public CutTestingBDN()
        {
            picture = new Picture(ConfigImage.Path, ConfigImage.Name);                     
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
