using FNO.Common;
using FNO.Domain;
using Microsoft.EntityFrameworkCore.Design;

namespace FNO.ReadModel
{
    public class ReadModelDbContextFactory : IDesignTimeDbContextFactory<ReadModelDbContext>
    {
        public ReadModelDbContext CreateDbContext(string[] args)
        {
            var config = Configuration.GetConfiguration();
            return ReadModelDbContext.CreateContext(config);
        }
    }
}
