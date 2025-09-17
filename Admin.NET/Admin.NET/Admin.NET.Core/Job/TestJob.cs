

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Job;

[JobDetail("job_Test", Description = "测试Job", GroupName = "default", Concurrent = false)]
[PeriodSeconds(1, TriggerId = "trigger_Test", Description = "测试一下", MaxNumberOfRuns = 0, RunOnStart = true)]
public class TestJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    public TestJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        Console.WriteLine(DateTime.Now);
        await Task.Delay(5000);
    }
}

