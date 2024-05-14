using Application.Dtos;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductFeatures.Queries
{
    public class GetAllProductsQuery : IRequest<List<ProductDto>>
    {
        public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllProductsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<List<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
            {
                var productList = await _context.Products
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .ToListAsync();

                if (productList == null || !productList.Any())
                {
                    return new List<ProductDto>();
                }
                var productDtoList = productList.Select(product => new ProductDto
                {
                    Barcode = product.Barcode,
                    Description = product.Description,
                    Rate = product.Rate,
                    Id = product.Id,
                    Name = product.Name,
                    CategoryId = product.Category.Id,
                }).ToList();


                return productDtoList;
                
            }
           
        }
    }
}
