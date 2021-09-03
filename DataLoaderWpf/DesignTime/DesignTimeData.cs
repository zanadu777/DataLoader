using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoaderWpf.DesignTime
{
   public  class DesignTimeData
    {
        public static MainWindowVm MainWindowVm
        {
            get { return new MainWindowVm
            {
                Query = "sel * from ktest.Deming"
            }  ; }
        }
    }
}
