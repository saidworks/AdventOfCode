using AdventOfCode.day1;
using Microsoft.Extensions.Logging;

namespace AdventOfCode;

public class Program
{
    public async static Task Main(string[] args)
    {
        
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole()
                .SetMinimumLevel(LogLevel.Information);
        });
        // Day 1
        ILogger<FileReader> logger = loggerFactory.CreateLogger<FileReader>();
        var fileReader = new FileReader(logger);
        fileReader.CountZeroes();
    }
}