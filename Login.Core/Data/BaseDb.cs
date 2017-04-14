using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Login.Common.PetaPoco;
using PetaPoco;

namespace Login.Core.Data
{
    public class BaseDB : Database
    {
        protected static ContextDB NewDB(string databaseKey= "")
        {
            if (string.IsNullOrEmpty(databaseKey))
            {
                databaseKey = DBConnections.DefaultKey;
            }
            return new ContextDB(databaseKey);
        }


        protected ILog Log { get { return LogManager.GetLogger("systemlog"); } }
        
    }
}
