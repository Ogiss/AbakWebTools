using Microsoft.Extensions.Configuration;
using System;

namespace AbakTools.Core.Infrastructure.RecurringTasks
{
    static class ConfigurationExtensions
    {
        public static RecurringTaskConfiguration GetRecurringTask(this IConfiguration configuration, Type type)
        {
            foreach (var config in configuration.GetSection("RecurringTasks").GetChildren())
            {
                var taskConfig = config.Get<RecurringTaskConfiguration>();
                if (taskConfig.TypeName == type.FullName)
                {
                    return taskConfig;
                }
            }

            throw new ArgumentException("Configuration not found for recurring task type: " + type.FullName);
        }
    }
}
