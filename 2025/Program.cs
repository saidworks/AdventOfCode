using AdventOfCode.day1;
using AdventOfCode.day2;
using AdventOfCode.day3;
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
        // // Day 1
        // ILogger<ZeroCounter> logger = loggerFactory.CreateLogger<ZeroCounter>();
        // var fileReader = new ZeroCounter(logger);
        // fileReader.CountZeroes();
        // // Day 2
        // var logger = loggerFactory.CreateLogger<IdValidator>();
        // var idValidator = new IdValidator(logger);
        // var invalidNumbersSum = idValidator.ReturnInvalidNumbersSum();
        // logger.LogInformation($"Invalid {invalidNumbersSum} invalid numbers sum");
        // Day 3 
        var logger = loggerFactory.CreateLogger<JoltageFinder>();
        var joltageFinder = new JoltageFinder(logger);
        joltageFinder.OrderAndReturnJoltages();
    }
}