using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeatures.Commands
{
    public class DeleteCategoryByIdCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public class DeleteCategoryByIdCommandHandler : IRequestHandler<DeleteCategoryByIdCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public DeleteCategoryByIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
            {
                var category = await _context.Category.Where(a => a.Id == request.Id).SingleOrDefaultAsync();
                if (category == null) return default;
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
                return category.Id;
            }

        }
    }
}
