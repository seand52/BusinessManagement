using Microsoft.AspNetCore.Mvc;

namespace BusinessManagement.Controllers;

public class BusinessManagementController: ControllerBase
{
    protected string GetUserId()
    {
        return User.Claims.First().Value;
    }
    
}