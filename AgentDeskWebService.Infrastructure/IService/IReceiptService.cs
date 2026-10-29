using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IReceiptService
    {
        Domain.ReceiptLister GetAll(Domain.ReceiptLister mLister);

        Domain.Receipt Upsert(Domain.Receipt mReceipt);

        Domain.Receipt Get(int id);

        void Delete(int id, int loginUserId);

        Domain.Receipt GetPdf(int id);
    }
}
