using System.Threading.Tasks;
using BoilerApplication.Context;
using BoilerApplication.Controller;
using BoilerApplication.Model;
using BoilerApplication.Repository;
using BoilerApplication.Service;

namespace BoilerApplication;

public class Program
{
    static void Main(string[] args)
    {
        IFileContext<string> CSVContext = new CSVContext("BoilerLog.csv");
        LogRepository LogRepository = new LogRepository(CSVContext);
        BoilerService service = new BoilerService(new Boiler(new Guid()));
        
        NotificationService notificationService = new NotificationService(service);
        notificationService.SubscribeNotification();
        
        LoggingService loggingService = new LoggingService(service, LogRepository);
        loggingService.SubscribeLogging();

        BoilerController boilerController = new BoilerController(service, notificationService, loggingService);
        boilerController.Initialize();

        boilerController.HandleUserMenu();
        //await service.StartBoiler();

        loggingService.UnsubscribeLogging();
        notificationService.UnsubscribeNotification();
    }
}
