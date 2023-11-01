using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using openai_api.Services;
using UAParser;

namespace openai_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class csvController : ControllerBase
    {
        private readonly IConfiguration? config;

        // Access the Request property here
        

        [HttpPost("NewQuestion")]
        public Task<string> PostQuestion(IConfiguration config, string NewQuestion, string clientIP, [FromHeader(Name = "userEmail")] string userEmail)
        {
            Uri callUrl = Request.GetTypedHeaders().Referer;

            return csvService.PostOpenAIquestion(callUrl, config, NewQuestion, clientIP, userEmail);
        }
    }
}
