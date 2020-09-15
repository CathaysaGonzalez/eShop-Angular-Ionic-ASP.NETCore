using System;
using BE.Dal.EfStructures;
using BE.Dal.Initialization;
using BE.Dal.Tests.Base;

namespace BE.Dal.Tests.RepoTests.Base
{
  public class RepoTestsBase : TestBase
  {
        public RepoTestsBase()
        {
            CleanDatabase();
        }
        public override void Dispose()
        {
            CleanDatabase();
            base.Dispose();
        }
        protected void CleanDatabase()
        {
            SampleDataInitializer.ClearData(Context);
        }

        protected void LoadDatabase()
        {
            SampleDataInitializer.InitializeData(Context);
            
        }
    }
}