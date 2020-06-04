using Microsoft.Extensions.Hosting;
using RabbitMQManagement;
using System;
using System.Threading;
using System.Threading.Tasks;
using MaintenanceManagement.Services;
using MaintenanceManagement.Models;

namespace MaintenanceManagement.Manager
{
    public class MaintenanceManager : IHostedService, IMessageHandlerCallback
    {
        private RabbitMQMessageHandler _messageHandler;
        private MaintenanceService _maintenanceService;

        public MaintenanceManager(RabbitMQMessageHandler messageHandler, MaintenanceService maintenanceService)
        {
            _messageHandler = messageHandler;
            _maintenanceService = maintenanceService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }

        public async Task<bool> HandleMessageAsync(string messageType, string message)
        {
            try
            {
                switch (messageType)
                {
                    case "MaintenancePlanned":
                        Console.WriteLine(message);
                        break;
                    case "LimitTest":
                        Console.WriteLine(message);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return true;
        }

    }
}
