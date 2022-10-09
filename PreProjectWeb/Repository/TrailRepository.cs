using PreProjectWeb.Models;
using PreProjectWeb.Repository.IRepository;
using System.Net.Http;

namespace PreProjectWeb.Repository
{
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
