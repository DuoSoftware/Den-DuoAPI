using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duoapi.v1
{
    public enum EstimateType
    {
        quick,
        basepackage,
        Alacarte
    }
    class Estimate
    {
        public DataSet GetRechargeEstimate(string VcNumber, EstimateType estimateType)
        {
            return new DataSet();
        }


        public DataSet GetRenewEstimate(string VcNumber, EstimateType estimateType)
        {
            return new DataSet();
        }

        public DataSet GetEstimateForLCOAll(string GULCOID,int PageID)
        {
            return new DataSet();
        }

    }
}
