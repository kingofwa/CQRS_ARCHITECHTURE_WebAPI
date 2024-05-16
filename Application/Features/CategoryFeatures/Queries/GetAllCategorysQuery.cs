using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductFeatures.Queries
{
    public class GetAllCategorysQuery : IRequest<List<CategoryDto>>
    {
        public class GetAllCategorysQueryHandler : IRequestHandler<GetAllCategorysQuery, List<CategoryDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllCategorysQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<List<CategoryDto>> Handle(GetAllCategorysQuery query, CancellationToken cancellationToken)
            {
                var categoryList = await _context.Category.ToListAsync();

                if (categoryList == null || !categoryList.Any())
                {
                    return new List<CategoryDto>();
                }

                return categoryList.Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                }).ToList();
            }
        }
    }
}
