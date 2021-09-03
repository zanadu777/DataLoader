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
using DataLoader.Connectors.Teradata;
using DataLoaderWpf.Annotations;
using DataLoaderWpf.Misc;


namespace DataLoaderWpf
{
  public  class MainWindowVm:INotifyPropertyChanged
    {
        public ICommand RunQueryCommand { get; set; }


        public MainWindowVm()
        {
            RunQueryCommand = new DelegateCommand(RunQuery, x=> true);
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

        private DataTable queryResult;

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
