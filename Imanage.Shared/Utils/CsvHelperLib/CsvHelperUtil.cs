using Imanage.Shared.ViewModels.Export;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Imanage.Shared.Utils.CsvHelper
{
    public static class CsvHelperUtil
    {
        private const string CsvDelimeter = ",";

        public static string Export<TAttribute, T>(IEnumerable<T> batchDto) where T : class
            where TAttribute : ExportAttributes
        {
            using (var exportStream = (MemoryStream)GetStream<TAttribute, T>(batchDto))
            {
                var encoding = new UTF8Encoding(false);
                return encoding.GetString(exportStream.ToArray());
            }
        }

        private static Stream GetStream<TAttribute, T>(IEnumerable<T> objectList)
            where TAttribute : ExportAttributes
        {
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream, new UTF8Encoding(false));

            var columns = GetColumns<TAttribute, T>()
                .Where(o => !o.ExportAttributes.Skip)
                .OrderBy(o => o.ExportAttributes.Order);
            var columnNames = columns.Select(c => c.ExportAttributes.ExportName != null
                ? c.ExportAttributes.ExportName
                : c.PropertyInfo.Name);
            streamWriter.WriteLine(string.Join(CsvDelimeter, columnNames));

            foreach (var item in objectList)
            {
                var values = GetValues<TAttribute>(item, columns);
                var newValues = new List<string>();
                newValues.AddRange(values.Select(x =>
                {
                    if (x.Contains(CsvDelimeter))
                    {
                        return ReplaceDelimeterString(x);
                    }
                    return x;
                }));

                var outputValues = string.Join(CsvDelimeter, newValues);

                streamWriter.WriteLine(string.Join(CsvDelimeter, outputValues));
            }

            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private static string ReplaceDelimeterString(string outputValues)
        {
            return outputValues.Replace(CsvDelimeter, " ");
        }

        private static IEnumerable<ExportProperty> GetColumns<TAttribute, T>()
            where TAttribute : ExportAttributes
        {
            return typeof(T).GetProperties().Select(
                property =>
                {
                    var exportAttribute = ((TAttribute)property.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault());
                    return exportAttribute == null
                        ? null
                        : new ExportProperty { PropertyInfo = property, ExportAttributes = exportAttribute };
                }).Where(p => p != null);
        }

        private static List<string> GetValues<TAttribute>(object objVM, IEnumerable<ExportProperty> columns)
            where TAttribute : ExportAttributes
        {
            var propertyValues = new List<string>();
            foreach (var column in columns)
            {
                propertyValues.Add(GetAttributeValue(objVM, column.PropertyInfo, column.ExportAttributes));
            }
            return propertyValues;
        }

        private static string GetAttributeValue<TAttribute>(object objVM, PropertyInfo propertyInfo, TAttribute attribute)
            where TAttribute : ExportAttributes
        {
            object value = propertyInfo.GetValue(objVM);

            if (value == null || attribute == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(attribute.Format) && value is IFormattable)
            {
                return (value as IFormattable).ToString(attribute.Format, CultureInfo.CurrentCulture);
            }

            if (!string.IsNullOrWhiteSpace(attribute.Format))
            {
                return string.Format(attribute.Format, value);
            }

            return propertyInfo.GetValue(objVM).ToString();
        }
    }
}
