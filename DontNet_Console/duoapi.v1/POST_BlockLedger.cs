﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duoapi.v1
{
    class POST_BlockLedger
    {
        public string gulcoid { get; set; }
        public string guaccountid { get; set; }
        public decimal amount { get; set; }
    }
}
