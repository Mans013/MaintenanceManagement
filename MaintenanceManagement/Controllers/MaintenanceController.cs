using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQManagement;
using MaintenanceManagement.Models;
using MaintenanceManagement.Services;
using Newtonsoft.Json;

namespace MaintenanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        RabbitMQMessagePublisher _messagePublisher;
        private readonly MaintenanceService _maintenanceService;


        public MaintenanceController(RabbitMQMessagePublisher messagePublisher, MaintenanceService maintenanceService)
        {
            _messagePublisher = messagePublisher;
            _maintenanceService = maintenanceService;
        }

        [HttpGet]
        public ActionResult<List<MaintenanceModel>> Get()
        {
            return _maintenanceService.Get();
        }

        [HttpPost]
        public ActionResult<MaintenanceModel> Post([FromBody] MaintenanceModel maintenance)
        {
            var newMaintenance = _maintenanceService.Create(maintenance);
            _maintenanceService.Finish(maintenance);
            return Ok(JsonConvert.SerializeObject(newMaintenance));
        }
    }
}
