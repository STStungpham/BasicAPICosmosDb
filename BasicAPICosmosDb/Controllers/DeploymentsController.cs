using BasicAPICosmosDb.Services;
using BasicAPICosmosDb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasicAPICosmosDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeploymentsController : ControllerBase
    {
        private readonly IDeploymentServices _deploymentServices;

        public DeploymentsController(IDeploymentServices deploymentServices)
        {
            _deploymentServices = deploymentServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetDeployments()
        {
            var rst = await _deploymentServices.GetDeploymentsAsync();
            return Ok(rst);
        }

        [HttpGet("{entityId}")]
        public async Task<IActionResult> GetDeployment(string entityId)
        {
            var rst = await _deploymentServices.GetDeploymentByEntityIdAsync(
                entityId);
            return Ok(rst);
        }

        [HttpPost]
        public async Task<IActionResult> InsertDeployments(Deployment input)
        {
            var rst = await _deploymentServices.InsertDeploymentAsync(input);
            return Ok(rst);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDeployment(Deployment input)
        {
            var rst = await _deploymentServices.UpdateDeploymentAsync(input);
            return Ok(rst);
        }

    }
}
