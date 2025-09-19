using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Leagues.DTOs;
using Web.Application.Features.Finance.Leagues.Queries;
using Web.Application.Features.Finance.Matchs.Commands;
using Web.Application.Features.Finance.Matchs.Queries;
using Web.Application.Features.Finance.Sites.DTOs;
using Web.Application.Features.Finance.Sites.Queries;
using Web.Application.Features.Finance.Teams.DTOs;
using Web.Application.Features.Finance.Teams.Queries;
using WebJob.Models;

namespace WebJob.Pages.Finance.Matchs
{
    public class EditModel : BasePageModel
    {
        private readonly IMediator _mediator;
        public List<SiteGetAllByUserDto> SiteList;
        public List<LeagueGetAllDto> LeagueList;
        public List<TeamGetAllDto> TeamList;
        public IValidator<MatchEditCommand> _validator;

        public EditModel(IMediator mediator, IValidator<MatchEditCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }
        [BindProperty]
        public new MatchEditCommand Command { get; set; }
        public async Task<IActionResult> OnGet(short id = 0)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var dataGetById = await Mediator.Send(new MatchGetByIdQuery { MatchId = id });

            if (dataGetById.Data == null)
            {
                return NotFound();
            }
            SiteList = await Mediator.Send(new SiteGetAllByUserQuery());
            LeagueList = await Mediator.Send(new LeagueGetAllQuery());
            TeamList = await Mediator.Send(new TeamGetAllQuery());
            Command = Mapper.Map<MatchEditCommand>(dataGetById.Data);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var resultValidator = await _validator.ValidateAsync(Command);

            if (!resultValidator.IsValid)
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = resultValidator.Errors.Select(x => x.ErrorMessage).ToList()
                };
            }

            var insertResult = await Mediator.Send(Command);

            return new AjaxResult
            {
                Id = insertResult.Data.ToString(),
                Succeeded = insertResult.Succeeded,
                Messages = insertResult.Messages
            };
        }
    }
}
