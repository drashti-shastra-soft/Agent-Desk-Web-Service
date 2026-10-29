using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IReceiptRepository
    {
        Domain.ReceiptLister GetAll(Domain.ReceiptLister mLister);
        Domain.Receipt Upsert(Domain.Receipt mReceipt);
        Domain.Receipt Get(int id);
        Domain.Receipt GetPdf(int id);
        void Delete(int id, int loginUserId);
    }
}
