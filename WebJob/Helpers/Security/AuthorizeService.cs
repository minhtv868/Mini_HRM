using Web.Application.Features.IdentityFeatures.SysFunctions.Queries;
using Web.Domain.Entities.Identity;
using MediatR;
using Newtonsoft.Json;
using System.Text;

namespace WebJob.Helpers.Security
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IMediator _mediator;
        public AuthorizeService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public bool HasPermission(int userId, string itemUrl)
        {
            var result = _mediator.Send(new SysFunctionCheckPermissionByUserQuery(userId, itemUrl)).GetAwaiter().GetResult();
            return result.Data;
        }

        public bool HasPermission(string userName, string itemUrl)
        {
            var result = _mediator.Send(new SysFunctionCheckPermissionByUserQuery(userName, itemUrl)).GetAwaiter().GetResult();
            return result.Data;
        }

        public bool HasPermission(string itemUrl, byte[] sessionData)
        {
            List<SysFunctionGetMenuByUserDto> list = JsonConvert.DeserializeObject<List<SysFunctionGetMenuByUserDto>>(Encoding.Unicode.GetString(sessionData));
            var itemActive = list.FirstOrDefault(x => x.IsEnable == true && x.Url != "/" && itemUrl.StartsWith(x.Url, StringComparison.OrdinalIgnoreCase));
            if (itemActive != null) return true;
            return false;
        }
    }
}

