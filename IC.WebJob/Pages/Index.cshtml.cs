
using IC.Application.Features.IdentityFeatures.SysFunctions.Commands;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string htmlAdsPreview { set; get; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public IActionResult OnGetAsync(string previewAds = "")
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception for Request Home");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostFavoriteFunctionAsync(int sysFunctionId = 0, string handler = "")
        {
            if (sysFunctionId <= 0)
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = new List<string> { $"Chức năng Id <b>{sysFunctionId}</b> không tồn tại." }
                };
            }

            var favoriteFunctionResult = await Mediator.Send(new SysFunctionAssignUserCommand { SysFunctionId = sysFunctionId, UserId = CurrentUserService.UserId });

            return new AjaxResult
            {
                Id = sysFunctionId.ToString(),
                Succeeded = favoriteFunctionResult.Succeeded,
                Messages = favoriteFunctionResult.Messages
            };
        }
        public async Task<IActionResult> OnGetSystemUpdateAsync()
        {
            //var data = await Mediator.Send(new CloudSettingNotifyMaintenanceCMSQuery());
            //if (data.Succeeded && data.Data != null && data.Data.IsActive)
            //{
            //    return new AjaxResult
            //    {
            //        Succeeded = data.Succeeded,
            //        Messages = data.Messages,
            //        Data = data.Data
            //    };
            //}
            return new AjaxResult
            {
                Succeeded = true,
                Messages = new List<string>() { ""},
                Data = null
            };
        }
    }
}