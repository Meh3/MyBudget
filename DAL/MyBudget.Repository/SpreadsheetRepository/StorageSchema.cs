using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyBudget.Repository
{
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class StorageSchema
    {
        private Table[] tableField;

        [XmlElement("Table")]
        public Table[] Table
        {
            get => this.tableField;
            set => this.tableField = value;
        }

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
        private string spreadsheetField;
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
        public string Spreadsheet
        {
            get => this.spreadsheetField;
            set => this.spreadsheetField = value;
        }

        [XmlAttribute()]
        public string Orientation
        {
            get => this.orientationField;
            set => this.orientationField = value;
        }
    }

    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Header
    {
        private string nameField;
        private string cellField;

        [XmlAttribute()]
        public string Name
        {
            get => this.nameField;
            set => this.nameField = value;
        }

        [XmlAttribute()]
        public string Cell
        {
            get => this.cellField;
            set => this.cellField = value;
        }
    }
}
