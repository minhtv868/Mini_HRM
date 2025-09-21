using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging;
using Web.Domain.Entities.Finance;

public record GoldPriceCrawlJob : IRequest { }

internal class GoldPriceCrawlJobHandler : IRequestHandler<GoldPriceCrawlJob>
{
    private readonly ILogger<GoldPriceCrawlJobHandler> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public GoldPriceCrawlJobHandler(
        ILogger<GoldPriceCrawlJobHandler> logger,
        IHttpClientFactory httpClientFactory
     )
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task Handle(GoldPriceCrawlJob command, CancellationToken cancellationToken)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
            client.DefaultRequestHeaders.Referrer = new Uri("https://www.google.com/");

            var html = await client.GetStringAsync("https://giavang.net", cancellationToken);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var table = doc.DocumentNode.SelectSingleNode("//table[@id='tbl']");
            if (table == null)
            {
                _logger.LogWarning("Không tìm thấy bảng dữ liệu.");
                return;
            }

            var rows = table.SelectNodes(".//tr");
            if (rows == null || rows.Count < 2)
            {
                _logger.LogWarning("Không tìm thấy hàng dữ liệu.");
                return;
            }

            var now = DateTime.Now;

            foreach (var row in rows.Skip(1)) // Bỏ dòng tiêu đề
            {
                var cells = row.SelectNodes("td");
                if (cells == null || cells.Count < 4) continue;

                var brand = cells[0].InnerText.Trim();
                var location = cells[1].InnerText.Trim();
                var buyText = cells[2].InnerText.Trim().Replace(".", "").Replace(",", "");
                var sellText = cells[3].InnerText.Trim().Replace(".", "").Replace(",", "");

                if (!decimal.TryParse(buyText, out var buy)) continue;
                if (!decimal.TryParse(sellText, out var sell)) continue;

                var gold = new GoldPrice
                {
                    Type = "Vàng miếng",
                    Brand = brand,
                    Location = location,
                    BuyPrice = buy,
                    SellPrice = sell,
                    Source = "https://giavang.net",
                    CrDateTime = now,
                    UpdDateTime = now
                };
            }

            _logger.LogInformation("Đã lưu giá vàng thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi crawl giá vàng.");
        }
    }

}
