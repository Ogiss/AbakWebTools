using System;

namespace AbakTools.Core.Infrastructure.RecurringTasks
{
    class RecurringTaskConfiguration
    {
        public string TypeName { get; set; }
        public bool Enabled { get; set; }
        public TimeSpan Interval { get; set; }
        public int TaskHourOccurence { get; set; }
    }
}
