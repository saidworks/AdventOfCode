using AdventOfCode.util;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.day3;

public class JoltageFinder(ILogger<JoltageFinder> logger)
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
            // if second-highest number is before the highest  
        // sum up all the joltages 
        foreach (var line in File.ReadLines(FilePath))
        {
            // Skip empty lines to avoid crashes
            if (string.IsNullOrWhiteSpace(line)) continue;
            // Sort by Value in descending order, keeping the original index
            var numbersWithIndex = line.Select((c, index) => new { Value = int.Parse(c.ToString()), Index = index }).ToList();

            var sortedWithIndex = numbersWithIndex.OrderByDescending(pair => pair.Value).ThenBy(pair=>pair.Index).ToList();
          
            // Extract the top two, including their indices
            var topTwo = sortedWithIndex.Take(2).Select(pair => (pair.Value, pair.Index)).ToList();
            var alternativeNum = sortedWithIndex.Where(pair => pair.Index > topTwo[0].Index).Select(pair => (pair.Value, pair.Index)).ToList();
            var joltage = 0;

            if (topTwo[0].Index < topTwo[1].Index)
            {
                joltage = topTwo[0].Value * 10 + topTwo[1].Value;
                logger.LogInformation($"Joltage: {joltage}");
                HighestJoltages.Add(joltage);
                
            }
            else if (topTwo[0].Index >= topTwo[1].Index || topTwo[0].Index == sortedWithIndex.Count)
            {
                joltage = AltJoltageBigger(topTwo, alternativeNum, topTwo[1].Value*10 + topTwo[0].Value );
                logger.LogInformation($"Joltage: {joltage}");
                HighestJoltages.Add(joltage);
            }
            
            
            
        }
        
        var sumOfJoltages = HighestJoltages.Sum();
        logger.LogInformation($"SumOfJoltages: {sumOfJoltages}");
        return sumOfJoltages;
    }
    
    // Idea: Verify if combination of two other digits is bigger than the highest
    private int AltJoltageBigger(List<(int Value, int Index)> topTwo, List<(int Value, int Index)>  alternativeNum, int joltage)
    {
        int maxAlternativeJoltage = 0;

        foreach (var pair in alternativeNum)
        {
            int alternativeJoltage = topTwo[0].Value * 10 + pair.Value;
            if (alternativeJoltage > maxAlternativeJoltage)
            {
                maxAlternativeJoltage = alternativeJoltage;
            }
        }

        return Math.Max(joltage, maxAlternativeJoltage);
    }
    
}