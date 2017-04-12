using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Login.Common.PetaPoco;

namespace Login.Core.Data
{
    public class BaseDb
    {
        protected ContextDB NewDB(string databaseKey= "Default")
        {
            return new ContextDB(databaseKey);
        }


        protected ILog Log { get { return LogManager.GetLogger("systemlog"); } }
    }
}
