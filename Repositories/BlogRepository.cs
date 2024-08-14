using BlogAppAPI.Data;
using BlogAppAPI.Models.Domain;
using BlogAppAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BlogAppAPI.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public BlogRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<BlogPostGetDto> Get(string urlHandle)
        {
            var blogPost = await _appDbContext.BlogPosts
                .Where(x => x.UrlHandle == urlHandle)
                .Select(x => new BlogPostGetDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,
                    Content = x.Content,
                    FeatureImageUrl = x.FeatureImageUrl,
                    UrlHandle = x.UrlHandle,
                    PublishDate = x.PublishDate,
                    Author = x.Author,
                    IsVisible = x.IsVisible,
                    Categories = x.Categories.Select(c => new CategoryGetDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle,
                    }).ToList()
                }).FirstOrDefaultAsync();

            return blogPost;
        }

        public int Count()
        {
            return _appDbContext.BlogPosts.Count();
        }

        public async Task Create(BlogPostCreateDto blogPost)
        {
            var categories = await _appDbContext.Categories.Where(category => blogPost.Categories.Contains(category.Id)).ToListAsync();

            var newBlogPost = new BlogPost
            {
                Title = blogPost.Title,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeatureImageUrl = blogPost.FeatureImageUrl,
                UrlHandle = blogPost.UrlHandle,
                IsVisible = blogPost.IsVisible,
                Author = blogPost.Author,
                PublishDate = blogPost.PublishDate,
                Categories = categories,
            };

            await _appDbContext.BlogPosts.AddAsync(newBlogPost);
            await _appDbContext.SaveChangesAsync();

        }

        public async Task Delete(Guid id)
        {
            await _appDbContext.BlogPosts.Where(blogPost => blogPost.Id == id).ExecuteDeleteAsync();
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<BlogPostGetDto>> Get(int page = 1)
        {
            if (page < 0)
            {
                page = 1;
            }

            var blogPosts = await _appDbContext.BlogPosts
                .Include(x => x.Categories)
                .Select(x => new BlogPostGetDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,
                    Content = x.Content,
                    FeatureImageUrl = x.FeatureImageUrl,
                    UrlHandle = x.UrlHandle,
                    PublishDate = x.PublishDate,
                    Author = x.Author,
                    IsVisible = x.IsVisible,
                    Categories = x.Categories.Select(c => new CategoryGetDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    }).ToList()
                }).Skip((page - 1) * 6).Take(6).ToListAsync();

            return blogPosts;
        }

        public async Task<BlogPostGetDto> Get(Guid id)
        {
            var blogPost = await _appDbContext.BlogPosts
                .Where(x => x.Id == id)
                .Include(x => x.Categories).Where(blogPost => blogPost.Id == id)
                .Select(x => new BlogPostGetDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,
                    Content = x.Content,
                    FeatureImageUrl = x.FeatureImageUrl,
                    UrlHandle = x.UrlHandle,
                    PublishDate = x.PublishDate,
                    Author = x.Author,
                    IsVisible = x.IsVisible,
                    Categories = x.Categories.Select(c => new CategoryGetDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle,
                    }).ToList()
            }).FirstOrDefaultAsync();

            return blogPost;
        }

        public async Task Update(Guid id, BlogPostCreateDto blogPost)
        {
            var existingBlogPost = await _appDbContext.BlogPosts.Where(blogPost => blogPost.Id == id).FirstOrDefaultAsync();
            var newCategories = await _appDbContext.Categories.Where(category => blogPost.Categories.Contains(category.Id)).ToListAsync();

            existingBlogPost.Title = blogPost.Title;
            existingBlogPost.ShortDescription = blogPost.ShortDescription;
            existingBlogPost.Content = blogPost.Content;
            existingBlogPost.FeatureImageUrl = blogPost.FeatureImageUrl;
            existingBlogPost.UrlHandle = blogPost.UrlHandle;
            existingBlogPost.PublishDate = blogPost.PublishDate;
            existingBlogPost.Author = blogPost.Author;
            existingBlogPost.IsVisible = blogPost.IsVisible;
            existingBlogPost.Categories = newCategories;

            _appDbContext.BlogPosts.Update(existingBlogPost);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
