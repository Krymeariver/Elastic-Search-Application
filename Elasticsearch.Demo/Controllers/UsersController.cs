using CensusApplication;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace CensusApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IElasticClient elasticClient;

        public UsersController(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        /// <summary>
        /// Gets data for a user using Name as a search criteria
        /// </summary>
        /// <returns>A user</returns>
        [HttpGet("{name}")]
        public async Task<User> Get(string name)
        {
            var response = await elasticClient.SearchAsync<User>(s => s
                .Index("users")
                .Query(q => q.Match(m => m.Field(f => f.Name).Query(name))));

           return response?.Documents?.FirstOrDefault();
        }
        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        public async Task<List<User>> GetAllUsers()
        {
            var response = await elasticClient.SearchAsync<User>(s => s
                .Index("users"));
                
            return response?.Documents?.ToList();
        }
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">Request payload</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Post([FromBody] User value)
        {
            var response = await elasticClient.IndexAsync<User>(value, x => x.Index("users"));
            return response.Id;
        }

       /// <summary>
       /// Deletes user using their ES id as a parameter
       /// </summary>
       /// <param name="index"></param>
       /// <returns></returns>
        [HttpDelete]
        public async void Delete(string index)
        {
            //var index = await elasticClient.SearchAsync<User>(s => s
            //.Index("users")
            //.Query(q => q.Match(m => m.Field(f => f.Name).Query(name))));
            await elasticClient.Indices.DeleteAsync(index);

        }

        /// <summary>
        /// Changes a user data
        /// </summary>
        /// <returns></returns>
        //[HttpPut]
        //public async Task<string> Put([FromBody] User value)
        //{
        //    var response = await elasticClient.IndexAsync<User>(value, x => x.Index("users"));
        //    return response.Id;
    }

    }

