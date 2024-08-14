using BlogAppAPI.Models.Domain;
using BlogAppAPI.Models.DTO;

namespace BlogAppAPI.Repositories
{
    public interface IBlogRepository
    {
        public Task<List<BlogPostGetDto>> Get(int page = 1);
        public Task<BlogPostGetDto> Get(Guid id);
        public Task Create(BlogPostCreateDto blogPost);
        public Task Update(Guid id, BlogPostCreateDto blogPost);
        public Task Delete(Guid id);
        public Task<BlogPostGetDto> Get(string urlHandle);
        public int Count();
    }
}
