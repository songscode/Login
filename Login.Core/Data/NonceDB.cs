using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Core.Data
{
    public class NonceDB : BaseDB
    {
        private NonceDB()
        {

        }
        public static NonceDB New()
        {
            return new NonceDB();
        }
    }
}
