using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ECommerce.Client.Repository
{
    public class RepositoryService : IRepository
    {
        private readonly HttpClient httpClient;

        public RepositoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        //public async Task<ApplicationUser> GetUserInfo(string userId)
        //{
        //    return await httpClient.GetFromJsonAsync<ApplicationUser>($"{ControllerName.AccountController}/{ControllerName.AccountControllerActions.GetUserInfo}");
        //}
    }
}