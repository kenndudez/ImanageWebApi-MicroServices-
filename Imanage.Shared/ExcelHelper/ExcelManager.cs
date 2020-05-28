using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NPOI.SS.UserModel;

namespace ExcelManager
{
    public class ExcelReader
    {
        public event EventHandler<ExcelReaderEventArgs> OnReadComplete;

        public event EventHandler<string> OnSheetChanged;

        /// <summary>
        /// Get or Set Filepath
        /// </summary>
        public string FilePath { get; set; }
        // private String _filePath { get; set; }
        private Stream _stream { get; set; }
        private List<ISheet> _readableSheets { get; set; }
        private IWorkbook _workbook { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExcelReader()
        {
        }

        public ExcelReader(Stream stream)
        {
            _stream = stream;
        }

        public ExcelReader(string filePath)
        {
            FilePath = filePath;
        }

        private void InitializeWorkBook()
        {
            var _fullPath = string.Empty;
            if (_stream == null)
            {
                if (!string.IsNullOrEmpty(FilePath))
                    _fullPath = Path.GetFullPath(FilePath);

                else { throw new Exception("FilePath or Stream cannot be empty."); }
                _stream = new FileStream(_fullPath, FileMode.Open, FileAccess.Read);
            }
            _workbook = WorkbookFactory.Create(_stream);
        }
        protected virtual void ExecuteOnSheetReadComplete(object sender, ExcelReaderEventArgs e)
        {
            OnReadComplete?.Invoke(sender, e);
        }

        protected virtual void ExecuteSheetChange(object sender, string e)
        {
            OnSheetChanged?.Invoke(sender, e);
        }

        public IWorkbook WorkBookProperties()
        {
            return _workbook;
        }

        /// <summary>
        /// Read all sheet in an excel file to the class specified
        /// </summary>
        /// <param name="predicate">A function returning a value for an unspecified column property</param>
        /// <returns></returns>
        public IEnumerable<T> ReadAllSheets<T>(bool includeHeaders = false, Func<List<ICell>, string, object> predicate = null)
        {

            InitializeWorkBook();

            var result = new List<T> { };

            _readableSheets = new List<ISheet> { };

            try
            {

                for (int i = 0; i < _workbook.NumberOfSheets; i++)
                    _readableSheets.Add(_workbook.GetSheetAt(i));

                result = ReadExcelSheet<T>(_readableSheets, includeHeaders, predicate);
                return result;
            }
            catch (Exception e) when (e is ArgumentOutOfRangeException)
            {
                throw new ColumnsDontMatchException("Columns and cells do not match.");
            }
        }


        /// <summary>
        /// Read excel by work book sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeHeaders"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> ReadWorkBookSheets<T>(int sheetIndex, bool includeHeaders = false, Func<List<ICell>, string, object> predicate = null, params Dictionary<string, object>[] additionalParams)
        {

            InitializeWorkBook();

            var result = new List<T> { };

            _readableSheets = new List<ISheet> { };

            try
            {

                _readableSheets.Add(_workbook.GetSheetAt(sheetIndex));

                result = ReadExcelSheet<T>(_readableSheets, includeHeaders, predicate, additionalParams);

                return result;
            }
            catch (Exception e) when (e is ArgumentOutOfRangeException)
            {
                throw new ColumnsDontMatchException("Columns and cells do not match.");
            }
        }


        /// <summary>
        /// Process excel sheets and bind to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_readableSheets"></param>
        /// <param name="includeHeaders"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>

        private List<T> ReadExcelSheet<T>(List<ISheet> _readableSheets, bool includeHeaders = false, Func<List<ICell>, string, object> predicate = null,
            params Dictionary<string, object>[] additionalParams)
        {
            var result = new List<T> { };

            _readableSheets.ForEach(sheet =>
            {
                var rows = sheet.GetRowEnumerator();

                while (rows.MoveNext())
                {
                    IRow row = (IRow)rows.Current;

                    //firstrow as header
                    if ((row.RowNum == 0 && !includeHeaders) || (row == null))
                        continue;

                    if (row.Cells.All(d => d.CellType == CellType.Blank))
                        continue;

                    var instance = Activator.CreateInstance<T>();

                    object PredicateExecutionResult;

                    var instanceProperties = instance.GetType().GetProperties()
              .Where(p => p.GetCustomAttributes(typeof(ExcelReaderCellAttribute), true).Length > 0)
              .Select(p => p).ToList();

                    if (instanceProperties == null || !instanceProperties.Any())
                        throw new ExcelCellReaderAttributeException($"No property was decorated with {nameof(ExcelReaderCellAttribute)} in type {instance.GetType().Name}");

                    var currentCells = row.Cells;

                    if (currentCells.Count > 0)
                    {
                        foreach (var property in instanceProperties)
                        {
                            var attribute = property.GetCustomAttributes<ExcelReaderCellAttribute>().FirstOrDefault();

                            ICell matchedCell;
                            if (attribute.Index != null)
                                matchedCell = currentCells[attribute.Index.Value];

                            else
                            {
                                matchedCell = currentCells.FirstOrDefault((cell) =>
                                {
                                    if (cell == null)
                                        return false;

                                    else
                                    {
                                        var header = sheet.GetRow(sheet.FirstRowNum).Cells[cell.ColumnIndex];

                                        return header.StringCellValue
                                                .Equals(attribute.Name ?? property.Name, StringComparison.InvariantCultureIgnoreCase);
                                    }
                                });
                            }


                            if (matchedCell != null)
                            {
                                try
                                {
                                    var cellType = matchedCell.CellType;

                                    var type = IsNullable(property.PropertyType) ? Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType;

                                    if (cellType == CellType.Numeric)
                                    {

                                        var isDateOrData = DateUtil.IsCellDateFormatted(matchedCell) ? matchedCell.DateCellValue.ToString() : matchedCell.NumericCellValue.ToString();
                                        var data = Convert.ChangeType(isDateOrData, type);
                                        property.SetValue(instance, data);
                                    }

                                    else if (cellType == CellType.Boolean)
                                    {
                                        property.SetValue(instance, matchedCell.BooleanCellValue);
                                    }

                                    else if (cellType == CellType.Error)
                                    {
                                        byte val = matchedCell.ErrorCellValue;
                                        property.SetValue(instance, val);
                                    }

                                    else if (cellType == CellType.Formula)

                                        if (matchedCell.CachedFormulaResultType == CellType.Numeric)
                                        {
                                            property.SetValue(instance, (int)matchedCell.NumericCellValue);
                                        }
                                        else if (matchedCell.CachedFormulaResultType == CellType.String)
                                        {
                                            property.SetValue(instance, matchedCell.StringCellValue);
                                        }
                                        else
                                        {
                                            throw new ArgumentException();
                                        }


                                    else if (cellType == CellType.String)
                                        property.SetValue(instance, string.IsNullOrEmpty(matchedCell.StringCellValue) ? string.Empty : matchedCell.StringCellValue.Trim());

                                    else
                                        property.SetValue(instance, string.Empty);
                                }

                                catch (Exception e) when (e is ArgumentException)
                                {//usually type conversion

                                    var msg = $"Invalid conversion in Cell [{matchedCell.RowIndex}, {matchedCell.ColumnIndex}]";
                                    throw new CellValueConvertionException(msg);
                                }

                                catch (Exception e)
                                {
                                    throw e;
                                }
                            }
                            else
                            {
                                if (predicate != null)
                                    if ((PredicateExecutionResult = predicate(currentCells, property.Name)) != null)
                                        property.SetValue(instance, PredicateExecutionResult);
                            }

                        }

                        foreach (var param in additionalParams)
                        {
                            instance.GetType().GetProperties().Where(x => x.Name == param["Name"].ToString()).First().SetValue(instance, param["Value"]);
                        }

                        result.Add(instance);
                    }


                }
            });
            return result;
        }


        private static bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}