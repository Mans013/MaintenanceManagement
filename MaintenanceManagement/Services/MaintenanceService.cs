using MaintenanceManagement.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using RabbitMQManagement;
using System;

namespace MaintenanceManagement.Services
{
    public class MaintenanceService
    {
        private readonly IMongoCollection<MaintenanceModel> maintenances;
        RabbitMQMessagePublisher _messagePublisher;
        public MaintenanceService(IConfiguration config, RabbitMQMessagePublisher messagePublisher)
        {
            var client = new MongoClient(config.GetConnectionString("CarChampDb"));
            var database = client.GetDatabase("CarChampDb");
            maintenances = database.GetCollection<MaintenanceModel>("maintenances");
            this._messagePublisher = messagePublisher;
        }

        public List<MaintenanceModel> Get()
        {
            return maintenances.Find(maintenance => true).ToList();
        }

        public MaintenanceModel Create(MaintenanceModel maintenance)
        {
            maintenances.InsertOne(maintenance);
            _messagePublisher.PublishMessageAsync("Maintenance", maintenance, "maintenance.log");
            return maintenance;
        }
    }
}
