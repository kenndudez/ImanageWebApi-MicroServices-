using System;

namespace Imanage.Shared.ViewModels.Export
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public abstract class ExportAttributes : Attribute
    {
        public string ExportName { get; set; }

        public string Format { get; set; }
        public bool Skip { get; set; }

        public int Order { get; set; }
    }

    public class AnalyticsAttribute : ExportAttributes
    {
    }
}