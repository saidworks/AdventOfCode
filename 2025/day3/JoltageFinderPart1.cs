using AdventOfCode.util;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.day3;

public class JoltageFinderPart1(ILogger<JoltageFinderPart1> logger)
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

            var sortedWithIndex = numbersWithIndex.OrderByDescending(pair => pair.Value).ThenBy(pair=>pair.Index).Select(pair => (pair.Value, pair.Index)).ToList();
          
            // Extract the top two, including their indices
            var topTwo = sortedWithIndex.Take(2).Select(pair => (pair.Value, pair.Index)).ToList();
            var joltage = 0;

            if (topTwo[0].Index < topTwo[1].Index)
            {
                joltage = topTwo[0].Value * 10 + topTwo[1].Value;
                logger.LogInformation($"Joltage: {joltage}");
                HighestJoltages.Add(joltage);
                
            }
            else if (topTwo[0].Index >= topTwo[1].Index || topTwo[0].Index == sortedWithIndex.Count)
            {
                joltage = AltJoltageBigger(topTwo, sortedWithIndex, topTwo[1].Value*10 + topTwo[0].Value );
                logger.LogInformation($"Joltage: {joltage}");
                HighestJoltages.Add(joltage);
            }
            string test = FindHighestVoltage(sortedWithIndex);
            logger.LogInformation($"Joltage: {test}");
        }
        

        var sumOfJoltages = HighestJoltages.Sum();
        logger.LogInformation($"SumOfJoltages: {sumOfJoltages}");
        return sumOfJoltages;
    }
    
    // Idea: Verify if combination of two other digits is bigger than the highest
    private int AltJoltageBigger(List<(int Value, int Index)> topTwo, List<(int Value, int Index)>  sortedWithIndex, int joltage)
    {
        int maxAlternativeJoltage = 0;
        var alternativeNum = sortedWithIndex.Where(pair => pair.Index > topTwo[0].Index).ToList();

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
    
    // Part 2 | Find the twelves highest combination to get highest joltage
    // we have the sorted array pairs with index 
    public static string FindHighestVoltage(List<(int Value, int Index)> sortedJoltages)
    {
        // Create a list to store the selected digits
        List<char> selectedDigits = new List<char>();

        // Iterate over the sortedJoltages list and select the highest digits while preserving their original order
        foreach (var pair in sortedJoltages)
        {
            if (selectedDigits.Count < 12)
            {
                selectedDigits.Add((char)(pair.Value + '0'));
            }
            else if (pair.Index > selectedDigits.Last() - '0')
            {
                selectedDigits.Remove(selectedDigits.Last());
                selectedDigits.Add((char)(pair.Value + '0'));
            }
        }

        // Create a new string from the selected digits
        return new string(selectedDigits.ToArray());
    }
    
}