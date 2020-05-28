using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imanage.Shared.Extensions
{
    public static class EnvironmentExtensions
    {
        const string StagingEnvironment = "Staging";
        const string TestingEnvironment = "Testing";

        public static bool IsStaging(this IHostingEnvironment env)
        {
            return env.IsEnvironment(StagingEnvironment);
        }

        public static bool IsTesting(this IHostingEnvironment env)
        {
            return env.IsEnvironment(TestingEnvironment);
        }
    }
}
