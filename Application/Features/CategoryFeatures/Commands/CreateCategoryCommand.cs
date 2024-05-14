using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeatures.Commands
{
    public class CreateCategoryCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CreateCategoryCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
            {
                var category = new Category();
                category.Name = command.Name;
                category.Description = command.Description;
                await _context.Category.AddAsync(category);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return category.Id;
                }
                return Guid.Empty;
            }
        }
    }
}
