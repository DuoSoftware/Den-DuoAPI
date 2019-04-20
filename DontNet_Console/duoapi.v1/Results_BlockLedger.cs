using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duoapi.v1
{
    class Results_BlockLedger
    {
        public bool success { get; set; }
        public BlockTokenTicket result { get; set; }
        public string message { get; set; }
    }
}
