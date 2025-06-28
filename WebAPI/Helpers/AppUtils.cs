using System.Text;

namespace WebAPI.Helpers
{
    public static class AppUtils
    {

        public static async Task<string> PostRequestAsync(string uri, string postParams)
        {
            string resultVar = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    StringContent httpContent = new StringContent(postParams, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(uri, httpContent);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        resultVar = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception e)
            {
                // ignored
            }

            return resultVar;
        }
        public static string GetDayOfWeekVi(int dayOfWeek)
        {
            string retval = "";
            switch (dayOfWeek)
            {
                case 0: retval = "Chủ Nhật"; break;
                case 1: retval = "Thứ Hai"; break;
                case 2: retval = "Thứ Ba"; break;
                case 3: retval = "Thứ Tư"; break;
                case 4: retval = "Thứ Năm"; break;
                case 5: retval = "Thứ Sáu"; break;
                case 6: retval = "Thứ Bảy"; break;
            }
            return retval;
        }
    }
}
