using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Application.Features.Finance.Sites.DTOs;
using Web.Application.Features.Finance.Sites.Queries;
using Web.Application.Interfaces;
using WebJob.Filters;
using WebJob.Helpers.Security;

namespace WebJob.Models
{
    [CmsAuthorize]
    [SitePermission]
    public class BasePageModel : PageModel
    {
        private ISender _mediator = null;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        private IMapper _mapper = null;
        protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();

        private ICurrentUserService _currentUserService = null;
        protected ICurrentUserService CurrentUserService => _currentUserService ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();


        public BaseCommand Command;

        public PagingInput PagingInput;
        public List<SiteGetAllByUserDto> SiteList;
        public async Task<int?> GetOrSetSiteIdAsync()
        {
            var siteList = await Mediator.Send(new SiteGetAllByUserQuery());
            if (siteList?.Any() != true)
                return null;

            var sessionSiteId = HttpContext.Session.GetInt32("SiteId");

            if (sessionSiteId.HasValue)
                return sessionSiteId.Value;

            var firstSiteId = siteList.First().SiteId;
            HttpContext.Session.SetInt32("SiteId", firstSiteId);
            return firstSiteId;
        }


    }
}
