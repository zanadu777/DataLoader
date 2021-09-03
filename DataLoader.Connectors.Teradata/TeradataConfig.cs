using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader.Connectors.Teradata
{
    public class TeradataConfig
    {
        public string DataSource { get; set; }= "192.168.50.21";
        public string Database { get; set; } = "";
        public string  UserId { get; set; }= "dbc";
        public string Password { get; set; } = "dbc";
        public string  AuthenticationMechanism { get; set; } = "";
    }
}
