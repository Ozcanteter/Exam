//using Exam.Etos;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;
//using Volo.Abp.DependencyInjection;
//using Volo.Abp.EventBus.Distributed;

//namespace Exam.Handlers
//{
//    public class ProgressEntryCreateHandler : IDistributedEventHandler<ProgressEntryCreateEto>, ITransientDependency
//    {
//        protected ILogger<ProgressEntryCreateHandler> _logger;

//        public ProgressEntryCreateHandler(ILogger<ProgressEntryCreateHandler> logger)
//        {
//            _logger = logger;
//        }

//        public Task HandleEventAsync(ProgressEntryCreateEto eventData)
//        {
//            _logger.LogInformation("ProgressEntryCreateEto event handled at {Time}", DateTimeOffset.Now);

//            return Task.CompletedTask;
//        }
//    }
//}
