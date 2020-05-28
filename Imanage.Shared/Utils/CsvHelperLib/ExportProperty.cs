
using Imanage.Shared.ViewModels.Export;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Imanage.Shared.Utils.CsvHelper
{
    public class ExportProperty
    {
        public PropertyInfo PropertyInfo { get; set; }

        public ExportAttributes ExportAttributes { get; set; }
    }
}
