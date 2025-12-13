using Microsoft.Extensions.Logging;

namespace AdventOfCode.util;

public static class InputOutputUtil
{
    public static void ValidateFileExists<T>(ILogger<T> logger, string filePath)
    {
        // Ensure the file exists before trying to read
        if (File.Exists(filePath)) return;
        logger.LogCritical("File not found.");
        throw new FileNotFoundException("file not found for the given path");
    }
}