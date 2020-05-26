using MaintenanceManagement.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using RabbitMQManagement;
using System;
using Newtonsoft.Json.Linq;
using MongoDB.Bson;

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
            return maintenance;
        }

        public MaintenanceModel Finish(MaintenanceModel maintenance)
        { 
            //Update maintenance
            var filter = Builders<MaintenanceModel>.Filter.Eq("_id", ObjectId.Parse(maintenance.Id));
            var update = Builders<MaintenanceModel>.Update.Set("Status", "Done");
            maintenances.UpdateOne(filter, update);
            //send message
            //Object dataToSend = ("{ 'maintenance_id': '" + maintenance.Id + "',  'car_id': '" + maintenance.Car + "', 'Description': '" + maintenance.Description + "' }");
            _messagePublisher.PublishMessageAsync("MaintenanceDone", maintenance, "maintenance.log");

            return maintenance;
        }
    }
}
