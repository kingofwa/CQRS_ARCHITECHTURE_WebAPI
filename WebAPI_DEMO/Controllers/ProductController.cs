using Application.Enum;
using Application.Features.ProductFeatures.Commands;
using Application.Features.ProductFeatures.Queries;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Demo;
using WebAPI_Demo.Filters.Authorizations;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [IgnoreAntiforgeryToken]
    [ApplicationAuthorizedAttribute]
    public class ProductController : BaseApiController
    {
       
        [HttpPost]
        [ApplicationAuthorized(Roles.Admin , Roles.Office)]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllProductsQuery()));
        }
      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        }
     
        [HttpDelete("{id}")]
        [ApplicationAuthorized(Roles.Admin, Roles.Office)]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteProductByIdCommand { Id = id }));
        }
      
        [HttpPut("[action]")]
        [ApplicationAuthorized(Roles.Admin, Roles.Office)]
        public async Task<IActionResult> Update(Guid id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }
    }
}
