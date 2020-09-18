using System;
using System.Collections.Generic;
using System.Linq;
using BE.Dal.EfStructures;
using BE.Dal.Repos.Base;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BE.Dal.Repos
{
    public class CategoryRepo : RepoBase<Category>, ICategoryRepo
    {
        public CategoryRepo(BEIdentityContext context) : base(context)
        {
        }

        internal CategoryRepo(DbContextOptions<BEIdentityContext> options) : base(options)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public IEnumerable<Category> GetCategories()
        {
            IQueryable<Category> query = Context.Categories;
            return query;
        }
    }
}
