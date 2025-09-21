using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Web.Application.Features.Finance.Matchs.Commands;
using Web.Domain.Enums;

namespace Web.Application.Helpers
{
    public class LivescoreResponse
    {
        [JsonPropertyName("Stages")]
        public List<Stage> Stages { get; set; } = new();
    }

    public class Stage
    {
        [JsonPropertyName("Sid")]
        public string Sid { get; set; } = string.Empty;

        [JsonPropertyName("Snm")]
        public string Snm { get; set; } = string.Empty; // Stage name

        [JsonPropertyName("Scd")]
        public string Scd { get; set; } = string.Empty; // Stage code

        [JsonPropertyName("Events")]
        public List<MatchEvent> Events { get; set; } = new();
    }

    public class MatchEvent
    {
        [JsonPropertyName("Eid")]
        public string Eid { get; set; } = string.Empty; // Event ID

        [JsonPropertyName("Pid")]
        public int Pid { get; set; } // Period ID

        [JsonPropertyName("Tr1")]
        public string Tr1 { get; set; } = string.Empty; // Team 1 result

        [JsonPropertyName("Tr2")]
        public string Tr2 { get; set; } = string.Empty; // Team 2 result

        [JsonPropertyName("T1")]
        public List<Team> T1 { get; set; } = new(); // Team 1

        [JsonPropertyName("T2")]
        public List<Team> T2 { get; set; } = new(); // Team 2

        [JsonPropertyName("Epr")]
        public int Epr { get; set; } // Event period

        [JsonPropertyName("Eps")]
        public string Eps { get; set; } = string.Empty; // Event status

        [JsonPropertyName("Esid")]
        public int Esid { get; set; } // Event status ID

        [JsonPropertyName("Esd")]
        public long Esd { get; set; } // Event start date (timestamp)

        [JsonPropertyName("Et")]
        public int Et { get; set; } // Event type
    }

    public class Team
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; } = string.Empty;

        [JsonPropertyName("Nm")]
        public string Nm { get; set; } = string.Empty; // Name

        [JsonPropertyName("Img")]
        public string Img { get; set; } = string.Empty; // Image URL
    }
    public static class FootballDataHelper
    {
        public static List<MatchCreateOrEditCommand> ParseLivescoreData(string jsonData)
        {
            var commands = new List<MatchCreateOrEditCommand>();

            try
            {
                if (string.IsNullOrEmpty(jsonData))
                    return commands;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var response = JsonSerializer.Deserialize<LivescoreResponse>(jsonData, options);

                if (response?.Stages == null)
                    return commands;

                foreach (var stage in response.Stages)
                {
                    if (stage.Events == null) continue;

                    foreach (var matchEvent in stage.Events)
                    {
                        var command = CreateMatchCommand(stage, matchEvent);
                        if (command != null)
                        {
                            commands.Add(command);
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                // Log JSON parsing error
                throw new ArgumentException($"Invalid JSON format: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Log general parsing error
                throw new Exception($"Error parsing football data: {ex.Message}", ex);
            }

            return commands;
        }

        private static MatchCreateOrEditCommand CreateMatchCommand(Stage stage, MatchEvent matchEvent)
        {
            try
            {
                var team1 = matchEvent.T1?.FirstOrDefault();
                var team2 = matchEvent.T2?.FirstOrDefault();

                if (team1 == null || team2 == null)
                    return null;

                var homeScore = ParseScore(matchEvent.Tr1);
                var awayScore = ParseScore(matchEvent.Tr2);

                return new MatchCreateOrEditCommand
                {
                    LSMatchId = int.Parse(matchEvent.Eid),
                    EstimateStartTime = DateTime.ParseExact(matchEvent.Esd.ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                    HomeId = short.Parse(team1.ID),
                    AwayId = short.Parse(team2.ID),
                    HomeName = team1.Nm,
                    AwayName = team2.Nm,
                    HomeLogoPath = team1.Img,
                    AwayLogoPath = team2.Img,
                    HomeGoals = ConvertToByte(homeScore),
                    AwayGoals = ConvertToByte(awayScore),
                    LeagueName = stage.Snm,
                    Status = (byte)StatusEnum.Active,
                    TimePlaying = matchEvent.Eps,
                    //MatchStatusId = matchEvent.Esid
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static int ParseScore(string scoreStr)
        {
            if (string.IsNullOrEmpty(scoreStr))
                return 0;

            return int.TryParse(scoreStr, out int score) ? score : 0;
        }

        private static DateTime ConvertTimestamp(long timestamp)
        {
            try
            {
                // Assuming timestamp is in milliseconds
                return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
            }
            catch
            {
                return DateTime.UtcNow;
            }
        }

        private static short? ConvertToShort(int value)
        {
            if (value <= 0 || value > short.MaxValue)
                return null;

            return (short)value;
        }

        private static byte? ConvertToByte(int value)
        {
            if (value < 0 || value > byte.MaxValue)
                return null;

            return (byte)value;
        }

        private static bool? DetermineIsLive(string matchStatus, int statusId)
        {
            // Logic để xác định trận đấu có đang live không
            // Có thể dựa vào status string hoặc status ID
            if (string.IsNullOrEmpty(matchStatus))
                return false;

            var liveStatuses = new[] { "LIVE", "HT", "1H", "2H" }; // Tùy chỉnh theo API
            return liveStatuses.Any(s => matchStatus.Contains(s, StringComparison.OrdinalIgnoreCase));
        }

        private static bool? DetermineIsHot(MatchEvent matchEvent)
        {
            // Logic để xác định trận đấu có hot không
            // Có thể dựa vào thời gian, đội bóng, hoặc các yếu tố khác

            // Ví dụ: Trận đấu trong 24h tới hoặc đang live
            var matchTime = ConvertTimestamp(matchEvent.Esd);
            var now = DateTime.UtcNow;
            var timeDiff = matchTime - now;

            // Hot nếu trận đấu trong vòng 24h hoặc đang diễn ra
            return timeDiff.TotalHours <= 24 && timeDiff.TotalHours >= -2;
        }
    }
}
