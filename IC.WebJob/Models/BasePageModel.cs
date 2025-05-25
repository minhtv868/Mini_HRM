using AutoMapper;
using IC.Application.Interfaces;
using IC.WebJob.Helpers.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IC.WebJob.Models
{
	[CmsAuthorize]
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
	}
}
