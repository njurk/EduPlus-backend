using Data.Data;
using Data.Data.CMS;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface IPageContentService
    {
        Task<IEnumerable<PageContentDto>> GetByPageIdAsync(int pageId);
        Task<IEnumerable<PageContentDto>> GetByPageLabelAsync(string pageLabel);
        Task<PageContentDto> UpdateAsync(int id, string newValue);
    }

    public class PageContentService : IPageContentService
    {
        private readonly EduPlusDbContext _context;

        public PageContentService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PageContentDto>> GetByPageIdAsync(int pageId)
        {
            return await _context.PageContents.AsNoTracking()
                .Where(pc => pc.PageId == pageId)
                .Select(pc => new PageContentDto
                {
                    Id = pc.Id,
                    PageId = pc.PageId,
                    Key = pc.Key,
                    Value = pc.Value
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<PageContentDto>> GetByPageLabelAsync(string pageLabel)
        {
            return await _context.PageContents.AsNoTracking()
                .Where(pc => pc.Page.Link == pageLabel)
                .Select(pc => new PageContentDto
                {
                    Id = pc.Id,
                    PageId = pc.PageId,
                    Key = pc.Key,
                    Value = pc.Value
                })
                .ToListAsync();
        }

        public async Task<PageContentDto> UpdateAsync(int id, string newValue)
        {
            var content = await _context.PageContents.FindAsync(id);
            if (content == null)
                throw new KeyNotFoundException("Zawartosc strony nie zostala znaleziona");

            content.Value = newValue;
            await _context.SaveChangesAsync();

            return new PageContentDto
            {
                Id = content.Id,
                PageId = content.PageId,
                Key = content.Key,
                Value = content.Value
            };
        }
    }
}