using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using BE.Dal.EfStructures;
using BE.Dal.Initialization;
using BE.Models.Entities;

namespace BE.Dal.Tests.Base
{
    public class TestBase : IDisposable
    {
        protected BEIdentityContext Context;

        public TestBase()
        {
            ResetContext();
        }
        public virtual void Dispose()
        {
            Context.Dispose();
        }

        protected void ResetContext()
        {
            Context = new BEIdentityContextFactory().CreateDbContext(null);
        }

    }
}