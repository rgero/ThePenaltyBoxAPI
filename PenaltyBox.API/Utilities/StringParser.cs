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
    }
}
