using System;

namespace ExcelManager {

    [Serializable]
    public class ColumnsDontMatchException : Exception {

        public ColumnsDontMatchException(string message) : base(message) {

        }

        public ColumnsDontMatchException(string message, Exception exception) : base(message, exception) {
        }
    }

    [Serializable]
    public class ExcelCellReaderAttributeException : Exception {

        public ExcelCellReaderAttributeException(string message) : base(message) {
        }
    }

    [Serializable]
    public class CellValueConvertionException : Exception {
        public CellValueConvertionException(string message) : base(message) {
        }
    }
}