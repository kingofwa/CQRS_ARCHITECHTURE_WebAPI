using Application.Dtos;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductFeatures.Queries
{
    public class GetCategoryByIdQuery : IRequest<CategoryDto>
    {
        public Guid Id { get; set; }
        public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
        {
            private readonly IApplicationDbContext _context;
            public GetCategoryByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<CategoryDto> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
            {
                var category =await _context.Category.Where(a => a.Id == query.Id).SingleOrDefaultAsync();
                if (category == null) return null;
                return new CategoryDto()
                {
                    Description = category.Description,
                    Id = category.Id,
                    Name = category.Name,
                };
            }
        }
    }
}
