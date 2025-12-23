using AdventOfCode.util;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.day3;

public class JoltageFinderPart2(ILogger<JoltageFinderPart2> logger)
{
    private static readonly string FilePath = Path.Combine("data", "day3.txt");
    private static readonly List<int> HighestJoltages = [];

    public int OrderAndReturnJoltages()
    {
        InputOutputUtil.ValidateFileExists(logger, FilePath);
        // Read file input  
        // split integers in line and store them in list 
            // for each list find the largest possible joltages
            // add them to list of joltages (respect order)
        // sum up all the joltages 
        foreach (var line in File.ReadLines(FilePath))
        {
            // Skip empty lines to avoid crashes
            if (string.IsNullOrWhiteSpace(line)) continue;
            // Sort by Value in descending order, keeping the original index
            var numbersWithIndex = line.Select(c=>  int.Parse(c.ToString())).ToList();
            HighestJoltages.AddRange(Find12HighestNumbers(line));
        }

        var sumOfJoltages = HighestJoltages.Sum();
        logger.LogInformation($"SumOfJoltages: {sumOfJoltages}");
        return sumOfJoltages;
    }
    
    // Part 2 | Find the twelves highest combination to get highest joltage
    // we have the sorted array pairs with index 
    public static List<int> Find12HighestNumbers(string input)
    {
        List<int> highestNumbers = new List<int>(12);
        
        foreach (char digit in input)
        {
            int number = int.Parse(digit.ToString());
            
            if (highestNumbers.Count < 12)
            {
                highestNumbers.Add(number);
                highestNumbers.Sort((a, b) => b.CompareTo(a));
            }
            else if (number > highestNumbers[11])
            {
                highestNumbers[11] = number;
                highestNumbers.Sort((a, b) => b.CompareTo(a));
            }
        }
        
        return highestNumbers;
    }
    
}