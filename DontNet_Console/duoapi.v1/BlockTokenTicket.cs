using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duoapi.v1
{
    public class BlockTokenTicket
    {
        //"BlockID": "6", "ApplicationID": "LCOPOTAL-001", "GUVaultID": "M000100010000100001000000040", "Amount": "100.00" 
        public Int64 BlockID { get; set; }
        public string ApplicationID { get; set; }
        public string GUVaultID { get; set; }
        public decimal Amount { get; set; }
    }
}
