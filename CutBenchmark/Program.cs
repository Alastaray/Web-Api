using AspEndpoint;
using BenchmarkDotNet.Running;
using CutBenchmark;

Picture picture = new Picture(CutTesting.path, CutTesting.name);
await picture.DownloadAsync("https://www.imgonline.com.ua/examples/bee-on-daisy.jpg");
BenchmarkRunner.Run<CutTesting>();


