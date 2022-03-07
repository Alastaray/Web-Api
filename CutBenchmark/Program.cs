using AspEndpoint;
using BenchmarkDotNet.Running;
using CutBenchmark;

Picture picture = new Picture(ConfigImage.Path, ConfigImage.Name);
await picture.DownloadAsync("https://www.imgonline.com.ua/examples/bee-on-daisy.jpg");
CutTestingDefault test = new CutTestingDefault();
Console.WriteLine($"TestCut = {test.TestCut(100)}ms");
Console.WriteLine($"TestCutAsync = {await test.TestCutAsync(100)}ms\n\n");
BenchmarkRunner.Run<CutTestingBDN>();

