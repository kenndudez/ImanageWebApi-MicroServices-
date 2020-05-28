using System;

namespace ExcelManager {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ExcelReaderCellAttribute : Attribute
    {
        public string Name
        {
            get;
            set;
        }
        public int? Index
        {
            get;
            set;
        }

        public ExcelReaderCellAttribute()
        {
        }

        public ExcelReaderCellAttribute(int index)
        {
            Index = index;
        }

        public ExcelReaderCellAttribute(string name)
        {
            Name = name;
        }
        public ExcelReaderCellAttribute(int index, string name)
        {
            Index = index;
            Name = name;
        }
    }
}