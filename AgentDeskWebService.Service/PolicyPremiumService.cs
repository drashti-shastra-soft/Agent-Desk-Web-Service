using AgentDeskWebService.Infrastructure.CustomException;
using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;

namespace AgentDeskWebService.Service
{
    public class PolicyPremiumService : IPolicyPremiumService
    {
        private readonly IPolicyPremiumRepository _repo;

        public PolicyPremiumService(IPolicyPremiumRepository repo)
        {
            _repo = repo;
        }


        public Domain.PolicyPremium Upsert(Domain.PolicyPremium mPolicyPremium)
        {

            mPolicyPremium.PolicyNumber = (mPolicyPremium.PolicyNumber ?? "").Trim();
            if (mPolicyPremium.PolicyNumber == null)
                throw new APIRequestFailedException("Policy number is required!");

            if (mPolicyPremium.AdjDate.Year < 1900)
                throw new APIRequestFailedException("Adj date is required!");

            mPolicyPremium.AdjDate = mPolicyPremium.AdjDate.Date;
            mPolicyPremium.DueDate = mPolicyPremium.DueDate.Date;

            return _repo.UpsertAndUpdateFup(mPolicyPremium);
        }


    }
}
