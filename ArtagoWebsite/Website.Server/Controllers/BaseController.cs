using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Website.Server.Controllers
{
    public abstract class BaseController : Controller
    {
        private IMediator? mediator;

        protected IMediator Mediator => (mediator ??= HttpContext.RequestServices.GetService<IMediator>())!;
    }
}
