using AspEndpoint;



namespace CutBenchmark
{
    public class CutTestingDefault
    {
        private Picture picture;
        public CutTestingDefault()
        {
            picture = new Picture(ConfigImage.Path, ConfigImage.Name);
        }

        public double TestCut(int count)
        {
            var begin = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                picture.Cut(100);
            }
            return (DateTime.Now - begin).TotalMilliseconds;
        }

        public async Task<double> TestCutAsync(int count)
        {
            var begin = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                await picture.CutAsync(100);
            }
            return (DateTime.Now - begin).TotalMilliseconds;
        }

    }
}
