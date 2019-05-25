using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;

namespace DataImporting.Classes.Task
{
    public class DataJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Nestoria.RunTask();
        }
    }
}