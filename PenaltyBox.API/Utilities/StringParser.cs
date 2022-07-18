using PenaltyBox.API.Models;

namespace PenaltyBox.API.Utilities
{
    public static class StringParser
    {
        public static List<String> ParseString(string? inputString)
        {
            List<String> result = new();
            if (!String.IsNullOrEmpty(inputString))
            {
                result = inputString.Split(',').ToList();
            }
            return result;
        }

        public static List<SeasonType> ParseSeasonType(string? inputString)
        {
            List<SeasonType> result = new();
            if (!String.IsNullOrEmpty(inputString))
            {
                List<string> inputList = inputString.Split(',').ToList();
                foreach (string input in inputList)
                {
                    SeasonType seasonType;
                    var successfulParse = Enum.TryParse(input, true, out seasonType);
                    if (successfulParse)
                    {
                        result.Add(seasonType);
                    }
                }
            }
            return result;
        }
    }
}
