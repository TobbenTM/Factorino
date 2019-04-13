using System.Collections.Generic;
using FNO.Domain.Models;
using FNO.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FNO.WebApp.Controllers.Api
{
    [ApiController]
    [Route("api/items")]
    public class FactorioEntityController
    {
        private readonly IEntityRepository _repo;

        public FactorioEntityController(IEntityRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("search")]
        public IEnumerable<FactorioEntity> Search(string query)
        {
            return _repo.Search(query);
        }
    }
}
