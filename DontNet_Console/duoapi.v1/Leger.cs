using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duoapi.v1
{
    public enum LedgerType{
        Customer=0,
        LCO=1
    }
    public class Ledger
    {
        private RestAPICall apicall;
        private LedgerType _ledgerType;
        private string _entity;
        private string _applicationToken;


        public Ledger(LedgerType ledgerType,string entity,string ApplicationToken,string UserName)
        {
            _ledgerType = ledgerType;
            _entity = entity;
            _applicationToken = ApplicationToken;
            apicall = new RestAPICall(ApplicationToken, UserName, entity);
        }

        public decimal GetVauletBalance(string LedgerAccountID)
        {
            Results_LedgerBalance ds =apicall.Get<Results_LedgerBalance>("vault/lco/balance/" + LedgerAccountID);
            if (ds.success)
            {
                return Convert.ToDecimal(ds.result);
            }
            else
            {
                throw new Exception(ds.message);
            }
            //return 0;
        }

        public DataTable GetLedger(string LedgerAccountID,int pageno)
        {
            return new DataTable();
        }

        public BlockTokenTicket BlockAmount(string LedgerAccountID, decimal EstimateAmount)
        {
            //{ "gulcoid":"M000100010000100001000000003","guaccountid":"","amount":100}
            POST_BlockLedger blockLedger = new POST_BlockLedger();
            blockLedger.gulcoid = LedgerAccountID;
            blockLedger.guaccountid = "";
            blockLedger.amount = EstimateAmount;
            var result=apicall.POST<Results_BlockLedger>("vault/Block",blockLedger);
            if (result.success)
            {
                return result.result;
            }
            else
            {
                throw new Exception(result.message);
            }
            
        }

        public BlockTokenTicket BlockAmount(string LedgerAccountID, decimal EstimateAmount, List<SubLedgerAccount> SubLedgerAccounts)
        {
            return new BlockTokenTicket();
        }

        public TranActionResponce SaveTransactions(BlockTokenTicket BlockToken, decimal ActualUtilizedAmount, List<LedgerTransactions> Transactions)
        {
            return new TranActionResponce();
        }

        public TranActionResponce SaveTransactions(decimal TotalAmount, List<LedgerTransactions> Tranactions)
        {
            return new TranActionResponce();
        }

        public TranActionResponce SaveTransactions(LedgerTransactions Tranaction)
        {
            return new TranActionResponce();
        }
    }
}
