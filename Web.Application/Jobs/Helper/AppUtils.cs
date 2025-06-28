using Web.Shared.Helpers;

namespace WebJob.Helpers
{
    public class AppUtils
    {
        public static long ComputeSha256HashAsLong(string rawData)
        {
            return StringHelper.CreateId(rawData, true, System.Text.Encoding.UTF8);
        }
        public static string UpdateTeamName(string teamName)
        {
            switch (teamName)
            {
                case "Barcelona":
                    return "FC Barcelona";

                default:
                    return teamName;
            }
        }

    }
}
