using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDB.Controllers;
using LibraryDB.DB;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using LibraryDB.Exceptions;

namespace LibraryDB.Services
{
    public class PostService
    {
        ApplicationContext _context;
        private readonly ILogger<PostController> _logger;

        DbSet<Post> postSet
        {
            get
            {
                return _context?.Post;
            }
        }
        public PostService(ILogger<PostController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addPost(Post newPost)
        {
            EntityEntry<Post> entry = await postSet.AddAsync(newPost);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public Task<List<Post>> getPost()
        {
            return postSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Post> getPost(int id)
        {
            Post result = await postSet
                .Where(o => o.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("Post", id);
            }

            return result;
        }
        public async Task updatePost(Post post)
        {
            if (post.Name == null)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Post tmp = postSet.Find(post.Id);
            tmp.Name = post.Name;

            await _context.SaveChangesAsync();
        }
        public async Task removePost(int? id)
        {
            Post post = postSet.Where(o => o.Id == id).FirstOrDefault();
            if (post == null)
            {
                throw new NotFoundException("Post", id);
            }
            EntityEntry<Post> entry = postSet.Remove(post);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
