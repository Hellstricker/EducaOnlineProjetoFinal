using EducaOnline.MessageBus;
using EducaOnline.Core.Utils;
using EducaOnline.Aluno.API.Services;

namespace EducaOnline.Aluno.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<RegistroAlunoIntegrationHandler>();
        }
    }


}
