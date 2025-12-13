using AdventOfCode.util;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.day1;

public class ZeroCounter(ILogger<ZeroCounter> logger)
{
    private static readonly string FilePath = Path.Combine("day1", "data.txt");
    private static readonly List<int> Numbers = [];
    private void ReadAndFormatData()
    {
        
        InputOutputUtil.ValidateFileExists(logger, FilePath);
        // ReadLines is lazy; it yields lines one by one instead of loading the whole file into memory
        foreach (var line in File.ReadLines(FilePath))
        {
            // Skip empty lines to avoid crashes
            if (string.IsNullOrWhiteSpace(line) || line.Length < 2) 
                continue;

            try
            {
                // 1. Extract the main string (everything except the last digits)
                string rotationDirection = line.Length > 3 ? line[..^3] : (line.Length > 2 ? line[..^2] : line[..^1]);

                // 2. Extract the last digits
                string rotationCount = line.Length > 3 ? line[^3..] : (line.Length > 2 ? line[^2..] : line[^1..]);
                
                // 3. Parse to int and convert based on rotation direction
                int numberPart;
                if (rotationDirection.Equals("R"))
                {
                    numberPart = int.Parse(rotationCount);
                }
                else if (rotationDirection.Equals("L"))
                {
                    numberPart = int.Parse(rotationCount) * -1;
                }
                else
                {
                    continue;
                }
                Numbers.Add(numberPart);
            }
            catch (FormatException)
            {
                logger.LogError($"Error parsing line: '{line}'. The last two characters were not numbers.");
            }
        }
        logger.LogInformation($"count of value parsed: {Numbers.Count}");
    }
    
    public int CountZeroes()
    {
        ReadAndFormatData();
        /*
         *  L150
            L50

            L150
            R50

            R150
            L50

            R150
            R50

            L50
            R50

            L50
            L50

            R50
            L50

            R50
            R50
         */
        
        var count = 0;
        var result = 50;
        foreach (var t in Numbers)
        {
            // if the result was bigger than 0, then become fewer means dial crossed zero and vice versa
            var tmp = result;
            result += t;
            if (t < -99 && result > t)
            {
                count++;
            }
            else if (result is > 99 or < -99)
            {
                // risk of duplicates 
                count += Math.Abs(result)/100;
                result %= 100;
            }
        
            else if (result == 0 || ((t > 0 && tmp < 0 && result>0) || (t < 0 && tmp > 0 && result<0)))
            {
                count++;
            }


        }
 

        logger.LogInformation($"The number of zeroes is {count}");
        return count;
    }
    
}