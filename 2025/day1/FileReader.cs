using Microsoft.Extensions.Logging;

namespace AdventOfCode.day1;

public class FileReader(ILogger<FileReader> logger)
{
    
    // C#

    private static readonly string FilePath = Path.Combine("day1", "data.txt");
    private static readonly List<int> Numbers = [];
    private void ReadAndFormatData()
    {

        // Ensure the file exists before trying to read
        if (!File.Exists(FilePath))
        {
            logger.LogCritical("File not found.");
            throw new FileNotFoundException("file not found for the given path");
        }

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
        var count = 0;
        var result = 50;
        foreach (var t in Numbers)
        {
            // if the result was bigger than 0, then become fewer means dial crossed zero and vice versa
            var tmp = result;
            result += t;
            if (result is > 99 or < -99)
            {
                // risk of duplicates 
                var resultCount = Math.Abs(result);
                count += resultCount/100;
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