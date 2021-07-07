using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS.Models
{

    public interface IAspDataBaseSettings
    {
        string UsersCollectionName { get; set; }
        string LostsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class DBSettings: IAspDataBaseSettings
    {

        public string UsersCollectionName { get; set; }
        public string LostsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
