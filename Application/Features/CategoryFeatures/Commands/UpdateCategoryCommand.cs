using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeatures.Commands
{
    public class UpdateCategoryCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public UpdateCategoryCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
            {
                var category = _context.Category.Where(a => a.Id == command.Id).FirstOrDefault();

                if (category == null)
                {
                    return default;
                }
                else
                {
                    category.Name = command.Name;
                    category.Description = command.Description;
                    await _context.SaveChangesAsync();
                    return category.Id;
                }
            }
        }
    }
}
