using System;
using BE.Dal.EfStructures;
using BE.Dal.Initialization;

namespace BE.Service.Tests.TestClasses.Base
{
    public abstract class BaseTestClass:IDisposable
    {
        protected string ServiceAddress = "https://localhost:5001/";
        protected string RootAddress = String.Empty;


        public virtual void Dispose()
        {
        }

        protected void ResetTheDatabase()
        {
            SampleDataInitializer.InitializeData(new BEIdentityContextFactory().CreateDbContext(null));
        }
    }
}