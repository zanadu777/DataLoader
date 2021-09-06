using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DataLoader.Connectors.SqllServer;
using DataLoader.Connectors.Teradata;
using DataLoaderWpf.Annotations;
using DataLoaderWpf.Misc;
using DataLoader.Core.Extensions;
using Microsoft.Win32;
using Newtonsoft.Json;


namespace DataLoaderWpf
{
    public class MainWindowVm : INotifyPropertyChanged
    {
        public ICommand RunQueryCommand { get; set; }
        public ICommand GenerateSqlCommand { get; set; }

        public ICommand TransferDataCommand { get; set; }

        public ICommand SaveDataTableCommand { get; set; }
        public ICommand LoadDataTableCommand { get; set; }

        public string LastAction
        {
            get => lastAction;
            set
            {
                if (value == lastAction) return;
                lastAction = value;
                OnPropertyChanged();
            }
        }

        public string FormattedLastTime

        {
            get => formattedLastTime;
            set
            {
                if (value == formattedLastTime) return;
                formattedLastTime = value;
                OnPropertyChanged();
            }
        }

        public string GeneratedSql
        {
            get => generatedSql;
            set
            {
                if (value == generatedSql) return;
                generatedSql = value;
                OnPropertyChanged();
            }
        }


        public MainWindowVm()
        {
            RunQueryCommand = new DelegateCommand(RunQuery, x => true);
            GenerateSqlCommand = new DelegateCommand(GenerateSql, x => true);// x => QueryResult != null && QueryResult.Rows.Count > 0 should work
            TransferDataCommand = new DelegateCommand(TransferData, x => true);

            SaveDataTableCommand = new DelegateCommand(SaveDataTable, x => true);
            LoadDataTableCommand = new DelegateCommand(LoadDataTable, x => true);
        }

        private void LoadDataTable(object obj)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "xml files (*.xml)|*.xml|json files(*.json)|*.json";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;


            if (dialog.ShowDialog() == true)
            {
                Stopwatch swatch = new Stopwatch();
                swatch.Start();

                var filename = dialog.FileName;

                if (Path.GetExtension(filename) == ".xml")
                {
                    //string result;
                    //using (StringWriter sw = new StringWriter())
                    //{
                    //    Stopwatch swatch = new Stopwatch();
                    //    swatch.Start();
                    //    DataTable dt = new DataTable();


                    //    dt.ReadXml(filename);

                    //    QueryResult = dt;
                    //    LastAction = "Loaded XML";
                    //    FormattedLastTime = swatch.Elapsed.ToFormatedString();
                    //}

                }

                if (Path.GetExtension(filename) == ".json")
                {
                    var json = File.ReadAllText(filename);
                    QueryResult   = JsonConvert.DeserializeObject<DataTable>(json);
                    LastAction = "Loaded Json";
                    FormattedLastTime = swatch.Elapsed.ToFormatedString();

                }



            }
        }

        private void SaveDataTable(object obj)
        {


            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "xml files (*.xml)|*.xml|json files(*.json)|*.json";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;


            if (dialog.ShowDialog() == true)
            {
                Stopwatch swatch = new Stopwatch();
                swatch.Start();

                var filename = dialog.FileName;


                if (Path.GetExtension(filename) == ".xml")
                {
                    string result;
                    using (StringWriter sw = new StringWriter())
                    {


                        QueryResult.WriteXml(sw);
                        result = sw.ToString();
                        File.WriteAllText(filename, result);

                        LastAction = "Saved XML";
                        FormattedLastTime = swatch.Elapsed.ToFormatedString();
                    }

                }

                if (Path.GetExtension(filename) == ".json")
                {
                    string jsonString = JsonConvert.SerializeObject(QueryResult);
                    File.WriteAllText(filename, jsonString);

                    LastAction = "Saved Json";
                    FormattedLastTime = swatch.Elapsed.ToFormatedString();

                }



            }

        }

            private void TransferData(object obj)
            {
                Stopwatch swatch = new Stopwatch();
                swatch.Start();
                SqlConnector = new SqlServerConnector(SqlServerConfig);
                SqlConnector.TransferData(QueryResult);
                LastAction = "Transfered Data";
                FormattedLastTime = swatch.Elapsed.ToFormatedString();
            }

            private void GenerateSql(object obj)
            {
                Stopwatch swatch = new Stopwatch();
                swatch.Start();
                SqlConnector = new SqlServerConnector(SqlServerConfig);
                GeneratedSql = SqlConnector.GenerateSql(QueryResult);
                LastAction = "Generated SQL";
                FormattedLastTime = swatch.Elapsed.ToFormatedString();
            }

            private void RunQuery(object o)
            {
                Stopwatch swatch = new Stopwatch();
                swatch.Start();
                Connector = new TeradataConnector(Config);
                QueryResult = Connector.SelectData(TableName, Query);
                LastAction = "Selected Data";
                FormattedLastTime = swatch.Elapsed.ToFormatedString();
            }

        public string Query
        {
            get => query;
            set
            {
                if (value == query) return;
                query = value;
                OnPropertyChanged();
            }
        }

        public string TableName


        {
            get => tableName;
            set
            {
                if (value == tableName) return;
                tableName = value;
                if (QueryResult != null)
                    QueryResult.TableName = tableName;
                OnPropertyChanged();
            }
        }

        public TeradataConnector Connector { get; set; }
        public TeradataConfig Config { get; set; } = new();

        SqlServerConfig SqlServerConfig { get; set; } = new();
        SqlServerConnector SqlConnector { get; set; }

        private DataTable queryResult;
        private string generatedSql;
        private string lastAction;
        private string formattedLastTime;
        private string query = "sel * from ktest.Deming";
        private string tableName = "Deming";

        public DataTable QueryResult
        {
            get { return queryResult; }
            set
            {
                if (Equals(value, queryResult)) return;
                queryResult = value;
                OnPropertyChanged();
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {


            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
