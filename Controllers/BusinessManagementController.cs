using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BusinessManagement.Controllers;

public class BusinessManagementController: ControllerBase
{
    // TODO: When all controllers are refactored we can add the mediator in the base controller
    // protected readonly IMediator _mediator;
    // public BusinessManagementController (IMediator mediator)
    // {
    //     _mediator = mediator;
    // }
    protected string GetUserId()
    {
        return User.Claims.First().Value;
    }
    
}