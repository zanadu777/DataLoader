using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataLoader.Connectors.SqllServer;
using DataLoader.Connectors.Teradata;
using DataLoaderWpf.Annotations;
using DataLoaderWpf.Misc;


namespace DataLoaderWpf
{
  public  class MainWindowVm:INotifyPropertyChanged
    {
        public ICommand RunQueryCommand { get; set; }
        public ICommand GenerateSqlCommand { get; set; }

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
            RunQueryCommand = new DelegateCommand(RunQuery, x=> true);
            GenerateSqlCommand = new DelegateCommand(GenerateSql, x => true  );// x => QueryResult != null && QueryResult.Rows.Count > 0 should work
        }

        private void GenerateSql(object obj)
        {
            SqlConnector = new SqlServerConnector(SqlServerConfig);
            GeneratedSql = SqlConnector.GenerateSql(QueryResult);
        }

        private void RunQuery(object o)
        {
            Connector = new TeradataConnector(Config);
            QueryResult = Connector.SelectData(TableName,Query);
        }

        public string Query { get; set; } = "sel * from ktest.Deming";
        public string TableName { get; set; } = "Deming";
        public TeradataConnector Connector { get; set; }
        public TeradataConfig Config { get; set; } = new();

        SqlServerConfig SqlServerConfig { get; set; } = new();
        SqlServerConnector SqlConnector { get; set; }

        private DataTable queryResult;
        private string generatedSql;

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
