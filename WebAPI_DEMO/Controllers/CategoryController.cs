using Application.Enum;
using Application.Features.CategoryFeatures.Commands;
using Application.Features.ProductFeatures.Queries;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Demo.Filters.Authorizations;

namespace WebAPI_Demo.Controllers
{

    [Route("api/[controller]")]
    [IgnoreAntiforgeryToken]
    [ApplicationAuthorized]
    public class CategoryController : BaseApiController
    {

        [HttpPost]
        [ApplicationAuthorized(Roles.Admin, Roles.Office)]
        public async Task<IActionResult> Create(CreateCategoryCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllCategorysQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await Mediator.Send(new GetCategoryByIdQuery { Id = id }));
        }

        [HttpDelete("{id}")]
        [ApplicationAuthorized(Roles.Admin, Roles.Office)]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteCategoryByIdCommand { Id = id }));
        }

        [HttpPut("[action]")]
        [ApplicationAuthorized(Roles.Admin, Roles.Office)]
        public async Task<IActionResult> Update(Guid id, UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }
    }
}
