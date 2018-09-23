using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MyBudget.Spreadsheet;

namespace MyBudget.Repository
{
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class StorageSchema
    {
        private Table[] tablesField;
        private string spreadsheetField;

        [XmlArrayItem("Table", IsNullable = false)]
        public Table[] Tables
        {
            get => this.tablesField;
            set => this.tablesField = value;
        }

        [XmlAttribute()]
        public string Spreadsheet
        {
            get => this.spreadsheetField;
            set => this.spreadsheetField = value;
        }

        public Table GetTable(string name) =>
            Tables.First(table => table.Name == name);

        public static StorageSchema Load(string filePath)
        {
            StorageSchema schema;
            using (var reader = new StreamReader(filePath))
            {
                var serializer = new XmlSerializer(typeof(StorageSchema));
                schema = (StorageSchema)serializer.Deserialize(reader);
            }
            return schema;
        }
    }

    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Table
    {
        private Header[] headersField;
        private string nameField;
        private string sheetField;
        private string orientationField;

        [XmlArrayItem("Header", IsNullable = false)]
        public Header[] Headers
        {
            get => this.headersField;
            set => this.headersField = value;
        }

        [XmlAttribute()]
        public string Name
        {
            get => this.nameField;
            set => this.nameField = value;
        }

        [XmlAttribute()]
        public string Sheet
        {
            get => this.sheetField;
            set => this.sheetField = value;
        }

        [XmlAttribute()]
        public string Orientation
        {
            get => this.orientationField;
            set => this.orientationField = value;
        }

        public Header GetHeader(string name) =>
            Headers.First(header => header.Name == name);
    }

    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Header
    {
        private string nameField;
        private string columnField;
        private string rowField;

        [XmlAttribute()]
        public string Name
        {
            get => this.nameField;
            set => this.nameField = value;
        }

        [XmlAttribute()]
        public string Column
        {
            get => this.columnField;
            set => this.columnField = value;
        }

        [XmlAttribute()]
        public string Row
        {
            get => this.rowField;
            set => this.rowField = value;
        }
        
        public Cell Cell { get => new Cell(Column, int.Parse(Row)); }
    }
}
