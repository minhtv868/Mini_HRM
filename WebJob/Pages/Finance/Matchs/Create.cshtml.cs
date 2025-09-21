using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Leagues.DTOs;
using Web.Application.Features.Finance.Leagues.Queries;
using Web.Application.Features.Finance.Matchs.Commands;
using Web.Application.Features.Finance.Teams.DTOs;
using Web.Application.Features.Finance.Teams.Queries;
using WebJob.Models;

namespace WebJob.Pages.Finance.Matchs
{
    public class CreateModel : BasePageModel
    {
        public List<LeagueGetAllDto> LeagueList;
        public List<TeamGetAllDto> TeamList;
        public IValidator<MatchCreateCommand> _validator;

        public CreateModel(IMediator mediator, IValidator<MatchCreateCommand> validator)
        {
            _validator = validator;
        }
        [BindProperty]
        public new MatchCreateCommand Command { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Command = new MatchCreateCommand();
            LeagueList = await Mediator.Send(new LeagueGetAllQuery());
            TeamList = await Mediator.Send(new TeamGetAllQuery());
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
