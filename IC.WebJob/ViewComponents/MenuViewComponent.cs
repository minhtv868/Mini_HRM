using IC.Application.Features.IdentityFeatures.SysFunctions.Queries;
using IC.Domain.Entities.Identity;
using IC.WebJob.Helpers.Configs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace IC.WebJob.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public MenuViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }
        public IViewComponentResult Invoke(int maxPriority, bool isDone)
        {
            List<SysFunctionGetMenuByUserDto> items;
            if (HttpContext.Session.TryGetValue(KeyConfig.ListSysMenuItemKey, out byte[] sessionData))
            {
                items = JsonConvert.DeserializeObject<List<SysFunctionGetMenuByUserDto>>(Encoding.Unicode.GetString(sessionData));
            }
            else
            {
                //Lấy ds Menu theo quyền của user đăng nhập
                var result = _mediator.Send(new SysFunctionGetMenuByUserQuery(HttpContext.User.Identity.Name)).GetAwaiter().GetResult();
                items = result.Data;
                //Cache để performance tốt hơn
                if (AppConfig.AppSettings.CacheSysMenu)
                {
                    HttpContext.Session.Set(KeyConfig.ListSysMenuItemKey, Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(items)));
                }
            }
            //Setup trạng thái active của menu theo đường dẫn request
            var requestPath = HttpContext.Request.Path.Value;

            //MenuHelper.SetMenuActive(items, requestPath);
            return View(items);
        }
    }
}
