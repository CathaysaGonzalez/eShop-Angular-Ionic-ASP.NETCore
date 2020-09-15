using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BE.Dal.EfStructures;
using BE.Dal.Repos.Base;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;

namespace BE.Dal.Repos
{
    public class SupplierRepo : RepoBase<Supplier>,ISupplierRepo
    {
        public SupplierRepo(BEIdentityContext context) : base(context)
        {
        }

        internal SupplierRepo(DbContextOptions<BEIdentityContext> options) : base(options)
        {
        }

        public override void Dispose()
        {
          base.Dispose();
        }
    }
}
