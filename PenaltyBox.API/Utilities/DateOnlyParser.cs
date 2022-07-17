namespace PenaltyBox.API.Utilities
{
    public static class DateParser
    {
        public static DateTime ParseString(string? dateString, DateTime defaultValue)
        {
            DateTime targetDay;
            if (!String.IsNullOrEmpty(dateString))
            {
                try
                {
                    targetDay = DateTime.Parse(dateString).Date;
                }
                catch
                {
                    targetDay = defaultValue;
                }
            }
            else
            {
                targetDay = defaultValue;
            }

            return targetDay;
        }
    }
}

