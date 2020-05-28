using DinkToPdf;
using DinkToPdf.Contracts;
using Imanage.Shared.Timing;
using Imanage.Shared.Utils.DinkToPdf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ValueGetter;

namespace Imanage.Shared.Utils
{
    public static class HtmlToPdfHelper
    {
        #region gotten from SmartStore/NopCommerce

        /// <summary>
        /// Replace all of the token key occurences inside the specified template text with corresponded token values
        /// </summary>
        /// <param name="template">The template with token keys inside</param>
        /// <param name="tokens">The sequence of tokens to use</param>
        /// <param name="htmlEncode">The value indicating whether tokens should be HTML encoded</param>
        /// <returns>Text with all token keys replaces by token value</returns>
        public static string Replace(string template, StringDictionary tokens, bool htmlEncode)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentNullException("template");

            if (tokens == null)
                throw new ArgumentNullException("tokens");

            foreach (string key in tokens.Keys)
            {
                string tokenValue = tokens[key];
                //do not encode URLs
                if (htmlEncode)
                    tokenValue = HtmlEncoder.Default.Encode(tokenValue);
                var replaceable = "{{" + key + "}}";
                template = Replace(template, replaceable, tokenValue);
            }
            return template;
        }

        private static string Replace(string original, string pattern, string replacement)
        {
            int count, position0, position1;
            count = position0 = position1 = 0;
            int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = original.IndexOf(pattern, position0, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);
        }

        #endregion gotten from SmartStore/NopCommerce

        private static async Task<string> ReadTemplateFileContent(string templateLocation)
        {
            StreamReader sr;
            string body;
            try
            {
                if (templateLocation.ToLower().StartsWith("http"))
                {
                    using (var wc = new WebClient())
                    {
                        sr = new StreamReader(await wc.OpenReadTaskAsync(templateLocation));
                    }
                }
                else
                    sr = new StreamReader(templateLocation, Encoding.Default);

                body = sr.ReadToEnd();

                sr.Close();
            }
            catch (Exception e)
            {
                throw e;
            }

            return body;
        }

        public static byte[] ConvertToPDFBytes<T>(this IConverter converter, IEnumerable<T> data, string htmlTemplatePath)
        {

            var htmlTemplate = ReadTemplateFileContent(htmlTemplatePath).Result;

            var tableKey = new StringDictionary()
            {
                ["Table"] = data.ToHtmlTable(
                    tableAttributes: new { @class = "table text-center table-hover table-borderless border" },
                    tbAttributes: new { @class = "mt-4" },
                    thAttributes: new { scope = "col" },
                    HTMLTableSetting: new HtmlTableSetting { IsHtmlEncodeMode = false }
                    ),
                ["Time"] = Clock.Now.ToString(CoreConstants.DateFormat)
            };

            var completeTemplate = Replace(htmlTemplate, tableKey, false);


            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                           //ImageQuality=70,
                           PaperSize = PaperKind.A4,
                           Orientation = Orientation.Landscape,
                       },
                Objects = {
                           new ObjectSettings()
                           {
                               FooterSettings = {Right = "Page [page] of [toPage]"},
                               WebSettings = { DefaultEncoding = "utf-8" },
                               //PagesCount = true,
                               HtmlContent = completeTemplate
                           }
                       }
            };

            return converter.Convert(doc);
        }

        public static void RegisterDinkToPdfTool(this IServiceCollection services)
        {
            var context = new LoadUnmanagedAssembly();
            context.LoadUnmanagedLibrary(Path.Combine(
                AppContext.BaseDirectory
                , @"utils\\DinkToPdf\\64 bit\\libwkhtmltox.dll"));

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }
    }

    public static partial class HtmlTableHelper
    {
        #region Object to HTML Table string converter
        private class HtmlTableGeneraterFactory
        {
            internal static readonly HtmlTableSetting _DefualtHTMLTableSetting = new HtmlTableSetting()
            {
                IsHtmlEncodeMode = true
            };

            public static HtmlTableGenerater CreateInstance<T1, T2, T3, T4, T5>(T1 tableAttributes, T2 trAttributes, T3 tdAttributes, T4 thAttributes, T5 tbAttributes, HtmlTableSetting htmlTableSetting)
            {
                var htmltablegenerater = new HtmlTableGenerater
                {
                    _HtmlTableSetting = htmlTableSetting ?? _DefualtHTMLTableSetting,
                    _TableAttributes = AttributeToHtml(tableAttributes),
                    _TrAttributes = AttributeToHtml(trAttributes),
                    _TdAttributes = AttributeToHtml(tdAttributes),
                    _ThAttributes = AttributeToHtml(thAttributes),
                    _TbAttributes = AttributeToHtml(tbAttributes),
                };

                htmltablegenerater.RenderTableTrTdAttributehtml();
                return htmltablegenerater;
            }

            public static Dictionary<string, string> AttributeToHtml<T>(T tableAttributes)
            {
                if (tableAttributes == null)
                    return null;
                var dic = tableAttributes.GetToStringValues();
                return dic;
            }
        }

        private class HtmlTableGenerater
        {

            #region Prop

            internal HtmlTableSetting _HtmlTableSetting { get; set; }
            internal Dictionary<string, string> _TableAttributes { get; set; }
            internal Dictionary<string, string> _TrAttributes { get; set; }
            internal Dictionary<string, string> _TbAttributes { get; set; }
            internal Dictionary<string, string> _TdAttributes { get; set; }
            internal Dictionary<string, string> _ThAttributes { get; set; }
            internal string _TableAttHtml { get; set; }
            internal string _TbAttHtml { get; set; }
            internal string _TrAttHtml { get; set; }
            internal string _TdAttHtml { get; set; }
            internal string _ThAttHtml { get; set; }
            internal IEnumerable<HtmlTableColumnAttribute> _customAttributes { get; set; }
            internal HtmlTableHelperBuilder _HtmlTableHelperBuilder { get; set; }
            #endregion

            internal HtmlTableGenerater() { }

            internal void RenderTableTrTdAttributehtml()
            {
                this._TableAttHtml = _TableAttributes != null
                    ? string.Join("", _TableAttributes.Select(s => $" {s.Key}=\"{Encode(s.Value)}\" "))
                    : "";
                this._TbAttHtml = _TbAttributes != null
                  ? string.Join("", _TbAttributes.Select(s => $" {s.Key}=\"{Encode(s.Value)}\" "))
                  : "";
                this._TrAttHtml = _TrAttributes != null
                    ? string.Join("", _TrAttributes.Select(s => $" {s.Key}=\"{Encode(s.Value)}\" "))
                    : "";
                this._TdAttHtml = _TdAttributes != null
                    ? string.Join("", _TdAttributes.Select(s => $" {s.Key}=\"{Encode(s.Value)}\" "))
                    : "";

                this._ThAttHtml = _ThAttributes != null
                    ? string.Join("", _ThAttributes.Select(s => $" {s.Key}=\"{Encode(s.Value)}\" "))
                    : "";
            }

            private StringBuilder RenderHtmlTable(StringBuilder thead, StringBuilder tbody)
            {
                var html = new StringBuilder($"<table{_TableAttHtml}>");
                if (this._HtmlTableHelperBuilder != null)
                {
                    var attrs = this._HtmlTableHelperBuilder.Caption.Attributes;
                    var htmlAtt = attrs != null
                    ? string.Join("", HtmlTableGeneraterFactory.AttributeToHtml(attrs).Select(s => $" {s.Key}=\"{Encode(s.Value)}\" "))
                    : "";
                    html.Append($"<caption{htmlAtt}>{_HtmlTableHelperBuilder.Caption.Content}</caption>");
                }

                html.Append($"<thead><tr{_TrAttHtml}>{thead}</tr></thead>");
                html.Append($"<tbody{_TbAttHtml}>{tbody.ToString()}</tbody>");
                html.Append("</table>");
                return html;
            }

            public string ToHtmlTableByDataTable(System.Data.DataTable dt) //Not Support Annotation
            {
                //Head
                var thead = new StringBuilder();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string thInnerHTML = Encode(dt.Columns[i].ColumnName);
                    thead.Append($"<th{_ThAttHtml}>{thInnerHTML}</th>");
                }

                //Body
                var tbody = new StringBuilder();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tbody.Append($"<tr{_TrAttHtml}>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        var value = dt.Rows[i][j];
                        string tdInnerHTML = Encode(value);
                        tbody.Append($"<td{_TdAttHtml}>{tdInnerHTML}</td>");
                    }
                    tbody.Append("</tr>");
                }

                //Table html
                var html = RenderHtmlTable(thead, tbody);

                return html.ToString();
            }

            public string ToHtmlTableByProperties<T>(IEnumerable<T> enums)
            {
                var firstData = enums.FirstOrDefault();
                //done: First use type with GetType func then type with typeof 
                var type = firstData != null
                        ? firstData.GetType()
                        : typeof(T);
                var props = GetPropertiesByAttrSkipFiliter(type);

                #region Check
                if (props.Count == 0)
                {
                    throw new Exception("At least one Property");
                }
                #endregion

                //Head
                var thead = new StringBuilder();
                foreach (var p in props)
                {
                    var costomAtt = CustomAttributeHelper.GetCustomAttributeByProperty(_customAttributes, p);
                    string thInnerHTML = costomAtt != null ? costomAtt.DisplayName : Encode(p.Name);
                    thead.Append($"<th{_ThAttHtml}>{thInnerHTML}</th>");
                }

                //Body
                var tbody = new StringBuilder();
                foreach (var e in enums)
                {
                    tbody.Append($"<tr{_TrAttHtml}>");
                    foreach (var prop in props)
                    {
                        var value = prop.GetToStringValue(e);
                        string tdInnerHTML = Encode(value);
                        tbody.Append($"<td{_TdAttHtml}>{tdInnerHTML}</td>");
                    }
                    tbody.Append("</tr>");
                }

                //Table html
                var html = RenderHtmlTable(thead, tbody);

                return html.ToString();
            }

            private IList<System.Reflection.PropertyInfo> GetPropertiesByAttrSkipFiliter(Type type)
            {
                var props = type.GetPropertiesFromCache();
                _customAttributes = CustomAttributeHelper.GetCustomAttributes(type);
                if (_customAttributes.FirstOrDefault() != null)
                {
                    _customAttributes = _customAttributes.Where(attr => attr.Skip == false);
                    var notSkipAttrsName = _customAttributes.Select(attr => attr.memberInfo.Name);
                    props = props.Where(prop => notSkipAttrsName.Contains(prop.Name)).ToArray();
                }
                return props;
            }

            //Q:    Why use two overload ToHtmlTableByKeyValue , it looks like same logic?
            //A:    Because IDictionary<TKey, TValue> and IDictionary they are not same type.
            public string ToHtmlTableByKeyValue<TKey, TValue>(IEnumerable<IDictionary<TKey, TValue>> enums)
            {
                //Head
                var thead = new StringBuilder();
                foreach (var p in enums.First().Keys)
                {
                    string thInnerHTML = Encode(p);
                    thead.Append($"<th>{thInnerHTML}</th>");
                }

                //Body
                var tbody = new StringBuilder();
                foreach (var values in enums)
                {
                    tbody.Append($"<tr{_TrAttHtml}>");
                    foreach (var v in values)
                    {
                        string tdInnerHTML = Encode(v.Value);
                        tbody.Append($"<td{_TdAttHtml}>{tdInnerHTML}</td>");
                    }
                    tbody.Append("</tr>");
                }

                //Table html
                var html = RenderHtmlTable(thead, tbody);
                return html.ToString();
            }

            public string ToHtmlTableByKeyValue(IEnumerable<IDictionary> enums)
            {
                //Head
                var thead = new StringBuilder();
                foreach (var p in enums.First().Keys)
                {
                    string thInnerHTML = Encode(p);
                    thead.Append($"<th>{thInnerHTML}</th>");
                }

                //Body
                var tbody = new StringBuilder();
                foreach (var values in enums)
                {
                    tbody.Append($"<tr{_TrAttHtml}>");
                    foreach (var v in values.Values)
                    {
                        string tdInnerHTML = Encode(v);
                        tbody.Append($"<td{_TdAttHtml}>{tdInnerHTML}</td>");
                    }
                    tbody.Append("</tr>");
                }

                //Table html
                var html = RenderHtmlTable(thead, tbody);
                return html.ToString();
            }

            private string Encode(object obj)
            {
                if (obj != null)
                    return _HtmlTableSetting.IsHtmlEncodeMode ? HtmlUtils.HtmlEncode(obj.ToString()) : obj.ToString();
                else
                    return "";
            }
        }
        #endregion
    }


    public class HtmlTableHelperBuilder
    {
        public IEnumerable<object> Enums { get; set; }
        public HtmlCaption Caption { get; set; }
        public HtmlTableHelperBuilder(IEnumerable<object> enums) => Enums = enums;
        public static HtmlTableHelperBuilder Create(IEnumerable<object> enums) => new HtmlTableHelperBuilder(enums);
        public HtmlTableHelperBuilder SetCaption(string captionContent, object captionAttributes = null)
        {
            this.Caption = new HtmlCaption() { Content = captionContent, Attributes = captionAttributes };
            return this;
        }
    }

    public class HtmlCaption
    {
        public string Content { get; set; }
        public object Attributes { get; set; }
    }

    public static class HtmlTableHelperBuilderExtension
    {
        public static HtmlTableHelperBuilder CreateBuilder(this IEnumerable<object> enums) => new HtmlTableHelperBuilder(enums);
    }

    public class HtmlTableSetting
    {
        public bool IsHtmlEncodeMode { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class HtmlTableColumnAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public bool Skip { get; set; }
        public MemberInfo memberInfo { get; set; }
    }

    internal static class HtmlUtils
    {
        /// <summary>
        /// From [Westwind.Utilities/HtmlUtils.cs at master · RickStrahl/Westwind.Utilities]
        /// (https://github.com/RickStrahl/Westwind.Utilities/blob/master/Westwind.Utilities/Utilities/HtmlUtils.cs)
        /// HTML-encodes a string and returns the encoded string.
        /// </summary>
        /// <param name="text">The text string to encode. </param>
        /// <returns>The HTML-encoded text.</returns>
        public static string HtmlEncode(string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(text.Length);

            int len = text.Length;
            for (int i = 0; i < len; i++)
            {
                switch (text[i])
                {

                    case '<':
                        sb.Append("&lt;");
                        break;
                    case '>':
                        sb.Append("&gt;");
                        break;
                    case '"':
                        sb.Append("&quot;");
                        break;
                    case '&':
                        sb.Append("&amp;");
                        break;
                    case '\'':
                        sb.Append("&#39;");
                        break;
                    default:
                        if (text[i] > 159)
                        {
                            // decimal numeric entity
                            sb.Append("&#");
                            sb.Append(((int)text[i]).ToString(CultureInfo.InvariantCulture));
                            sb.Append(";");
                        }
                        else
                        {
                            sb.Append(text[i]);
                        }

                        break;
                }
            }
            return sb.ToString();
        }
    }

    public static partial class HtmlTableHelper
    {
        internal static class CustomAttributeHelper
        {
            public static IEnumerable<HtmlTableColumnAttribute> GetCustomAttributes(System.Type type)
            {
                var props = type.GetPropertiesFromCache();
                foreach (var prop in props)
                {
                    var data = GetCustomAttribute(prop);
                    if (data != null)
                        data.memberInfo = prop;
                    yield return data;
                }
            }

            public static HtmlTableColumnAttribute GetCustomAttribute(MemberInfo memberInfo)
            {
                return Attribute.GetCustomAttribute(memberInfo, typeof(HtmlTableColumnAttribute)) as HtmlTableColumnAttribute;
            }

            public static HtmlTableColumnAttribute GetCustomAttributeByProperty(
                IEnumerable<HtmlTableColumnAttribute> customAttributes
                , MemberInfo memberInfo)
            {
                #region Check
                if (customAttributes == null)
                {
                    throw new ArgumentNullException(nameof(customAttributes));
                }

                if (memberInfo == null)
                {
                    throw new ArgumentNullException(nameof(memberInfo));
                }
                #endregion

                //var firstData = customAttributes.FirstOrDefault();
                return  customAttributes.Where(w => w?.memberInfo?.Name == memberInfo.Name).FirstOrDefault();
            }
        }

        public static string ToHtmlTable(this HtmlTableHelperBuilder builder, object tableAttributes = null, object tbAttributes = null, object trAttributes = null, object tdAttributes = null, object thAttributes = null, HtmlTableSetting HTMLTableSetting = null)
        {
            return ToHtmlTableByIEnumrable(builder.Enums, tableAttributes, tbAttributes, trAttributes, tdAttributes, thAttributes, HTMLTableSetting, builder);
        }

        public static string ToHtmlTable<T>(this IEnumerable<T> enums, object tableAttributes = null, object tbAttributes = null, object trAttributes = null, object tdAttributes = null, object thAttributes = null, HtmlTableSetting HTMLTableSetting = null)
        {
            return ToHtmlTableByIEnumrable(enums, tableAttributes, tbAttributes, trAttributes, tdAttributes, thAttributes, HTMLTableSetting);
        }

        public static string ToHtmlTable(this System.Data.DataTable datatable, object tableAttributes = null, object tbAttributes = null, object trAttributes = null, object tdAttributes = null, object thAttributes = null, HtmlTableSetting HTMLTableSetting = null)
        {
            var htmltablegenerater = HtmlTableGeneraterFactory.CreateInstance(tableAttributes, trAttributes, tdAttributes, thAttributes, tbAttributes, HTMLTableSetting);
            return htmltablegenerater.ToHtmlTableByDataTable(datatable);
        }

        private static string ToHtmlTableByIEnumrable<T>(IEnumerable<T> enums, object tableAttributes = null, object tbAttributes = null, object trAttributes = null, object tdAttributes = null, object thAttributes = null, HtmlTableSetting HTMLTableSetting = null, HtmlTableHelperBuilder builder = null)
        {
            var htmltablegenerater = HtmlTableGeneraterFactory.CreateInstance(tableAttributes, trAttributes, tdAttributes, thAttributes, tbAttributes, HTMLTableSetting);
            htmltablegenerater._HtmlTableHelperBuilder = builder;
            // Q:   Why not only IEnumerable<IDictionary> ?
            // A:   Example Dapper Dynamic Query Only implement IDictionary<string,object> without IDictionary
            // Q:   Why not use overload ToHtmlTable<TKey,TValue>(this IEnumerable<Dictionary<Tkey,TValue>> enums)?
            // A:   Because ToHtmlTable<T>(this IEnumerable<T> enums) and ToHtmlTable<TKey,TValue>(this IEnumerable<Dictionary<Tkey,TValue>> enums)
            //      System prefer use the former
            //      ps. https://stackoverflow.com/questions/54251262/c-sharp-overload-key-value-and-non-key-value-type-using-var-without-specifying
            if (enums is IEnumerable<IDictionary<string, object>>) //Special for Dapper Dynamic Query
            {
                return htmltablegenerater.ToHtmlTableByKeyValue(enums as IEnumerable<IDictionary<string, object>>);
            }
            else if (enums is IEnumerable<IDictionary>)
            {
                return htmltablegenerater.ToHtmlTableByKeyValue(enums as IEnumerable<IDictionary>);
            }
            else
            {
                return htmltablegenerater.ToHtmlTableByProperties(enums);
            }
        }
    }
}
