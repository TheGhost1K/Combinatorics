using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Exporters;

var config = DefaultConfig.Instance
    .AddJob(Job.ShortRun.WithWarmupCount(3).WithIterationCount(10))
    .AddColumn(RankColumn.Arabic)
    .AddLogger(ConsoleLogger.Default)
    .AddExporter(MarkdownExporter.GitHub)
    .WithOptions(ConfigOptions.DisableOptimizationsValidator);

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);