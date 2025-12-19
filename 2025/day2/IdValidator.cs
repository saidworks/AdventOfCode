using System.Globalization;
using System.Text.RegularExpressions;
using AdventOfCode.util;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.day2;

/*
 *  Your job is to find all of the invalid IDs that appear in the given ranges. In the above example:

    11-22 has two invalid IDs, 11 and 22.
    95-115 has one invalid ID, 99.
    998-1012 has one invalid ID, 1010.
    1188511880-1188511890 has one invalid ID, 1188511885.
    222220-222224 has one invalid ID, 222222.
    1698522-1698528 contains no invalid IDs.
    446443-446449 has one invalid ID, 446446.
    38593856-38593862 has one invalid ID, 38593859.
    The rest of the ranges contain no invalid IDs.
    Adding up all the invalid IDs in this example produces 1227775554.
 */

public class IdValidator(ILogger<IdValidator> logger)
{
    private static readonly string FilePath = Path.Combine("data", "day2.txt");
    
    public double ReturnInvalidNumbersSum()
    {
        List<double> invalidNumbers = ReturnInvalidIds();
        foreach (var invalidNumber in invalidNumbers)
            logger.LogInformation($"invalid: {invalidNumber}");
        return invalidNumbers.Sum();
    }
    
    private List<double> ReturnInvalidIds()
    {
        InputOutputUtil.ValidateFileExists(logger, FilePath);
        
        /*
         * read file
         *      split entries
         *      then convert values to integer for each side
         * Input entry as range
         *          loop through each element in range
         *          find ones that contain repeated pattern 
         *              split the entry by 2 
         *              see if there is symmetry 
         */ 
        List<double> invalidNumbers = [];
        // ReadLines is lazy; it yields lines one by one instead of loading the whole file into memory
        foreach (var line in File.ReadLines(FilePath))
        {
            // Skip empty lines to avoid crashes
            if (string.IsNullOrWhiteSpace(line) || line.Length < 2) 
                continue;
            List<string> rangeIds = new List<string>(line.Split(','));
            foreach (var rangeId in rangeIds)
            {
                (double lower, double upper) = createRanges(rangeId);
                logger.LogInformation($"range id: {rangeId} lower: {lower} upper: {upper}");
                invalidNumbers.AddRange(CheckRepeatedNumbers(lower, upper));
                

            }
 
        }

        return invalidNumbers;
    }

    private (double,double) createRanges(string range)
    {
        var rangeSplit = range.Split('-');
        double firstNumber = double.Parse(rangeSplit[0]);
        double secondNumber = double.Parse(rangeSplit[1]);
        return (firstNumber, secondNumber);
    }

    private static List<double> CheckRepeatedNumbers(double lower, double upper)
    {
        List<double> invalidNumbers = new List<double>();

        for (double i = lower; i <= upper; i++)
        {
            if (HasRepeatedNumberPattern(i))
            {
                invalidNumbers.Add(i);
            }
        }

        return invalidNumbers;
    }

    private static bool HasRepeatedNumberPattern(double number)
    {
        string numberString = number.ToString(CultureInfo.InvariantCulture);
        int length = numberString.Length;
    
        // Check if the length of the number is zero or negative, though this shouldn't happen
        if (length <= 0)
            return false;

        // Adjusted logic to correctly identify repeated patterns
        for (int i = 1; i <= length / 2; i++)
        {
            if (length % i != 0)
                continue; // Skip lengths that don't divide evenly into the number string length

            string part = numberString.Substring(0, i);
            string repeatedPattern = new string(part.ToCharArray(), 0, i);
        
            // Construct the repeated pattern to match the length of the original number
            string repeatedPatternStr = repeatedPattern;
            while (repeatedPatternStr.Length < length)
                repeatedPatternStr += repeatedPattern;

            // Trim to the original length if needed
            repeatedPatternStr = repeatedPatternStr.Substring(0, length);

            // Check if the number string matches the repeated pattern
            if (numberString == repeatedPatternStr)
                return true;
        }

        return false;
    }
}