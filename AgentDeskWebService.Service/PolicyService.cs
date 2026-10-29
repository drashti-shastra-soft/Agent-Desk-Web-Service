using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using AgentDeskWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Service
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _repo;
        private readonly IClientRepository _clientRepo;
        private readonly IPolicyNomineeRepository _policyNomineeRepo;
        private readonly IGroupRepository _groupRepo;
        private readonly IAddressRepository _addressRepo;

        public PolicyService(IPolicyRepository repo, IClientRepository clientRepo, IPolicyNomineeRepository policyNomineeRepo, IGroupRepository groupRepo, IAddressRepository addressRepo)
        {
            _repo = repo;
            _clientRepo = clientRepo;
            _policyNomineeRepo = policyNomineeRepo;
            _groupRepo = groupRepo;
            _addressRepo = addressRepo;
        }

        public Domain.PolicyLister GetAll(Domain.PolicyLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Policy Upsert(Domain.Policy mPolicy)
        {
            if (mPolicy.Base64String.IsNotNullOrEmpty())
            {
                string relativeFolder = "~/Policy/";
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(mPolicy.FilePath);

                mPolicy.FilePath = uniqueFileName;

                StaticMethods.SaveBase64Image(mPolicy.Base64String, relativeFolder, uniqueFileName);
            }
            else
            {
                mPolicy.FilePath = string.Empty;
                mPolicy.MIME = string.Empty;
            }

            return _repo.Upsert(mPolicy);
        }

        public Domain.Policy Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public Domain.Policy GetByPolicyNumber(string policyNumber)
        {
            return _repo.GetByPolicyNumber(policyNumber);
        }

        public Domain.Policy GetPdf(int id)
        {
            return _repo.GetPdf(id);
        }

        public bool CheckPolicyNumberExist(Domain.Policy mPolicy)
        {
            return _repo.CheckPolicyNumberExist(mPolicy);
        }

        //public Domain.Policy ImportPolicy(Domain.Policy mPolicy)
        //{
        //    // group
        //    var mGroup = _groupRepo.GetByName(mPolicy.Group);
        //    if (mGroup == null || mGroup.Id <= 0)
        //        mGroup = _groupRepo.Upsert(PrepareGroupModel(mPolicy));
        //    mPolicy.GroupId = mGroup.Id;

        //    //Address
        //    int? addressId = null;
        //    if (!string.IsNullOrWhiteSpace(mPolicy.Address))
        //    {
        //        var mAddress = PrepareAddressModel(mPolicy);               
        //        var efAddress = _addressRepo.GetByAddress(mAddress);

        //        if (efAddress == null || efAddress.Id <= 0)
        //            mAddress = _addressRepo.Upsert(mAddress);
        //        else
        //            mAddress = efAddress;

        //        addressId = mAddress.Id;
        //    }

        //    //client
        //    var mClient = PrepareClientModel(mPolicy);
        //    var efClient = _clientRepo.GetByDetails(mClient.FirstName, mClient.MiddleName, mClient.LastName, mClient.PhoneNumber, mClient.DOB);

        //    if (efClient == null || efClient.Id <= 0)
        //    {
        //        mClient = _clientRepo.Upsert(mClient);
        //    }
        //    else
        //    {
        //        mClient = efClient;
        //        if (mClient.GroupId != mGroup.Id) 
        //        {
        //            mClient.GroupId = mGroup.Id;
        //            mClient.LastModifiedBy = mPolicy.CreatedBy;
        //            mClient = _clientRepo.Upsert(mClient);
        //        }
        //    }

        //    // policy
        //    mPolicy.ClientId = mClient.Id;
        //    mPolicy.PolicyStatusId = 1;
        //    mPolicy.PolicyTypeId = 1;
        //    var mSaved = _repo.ImportPolicy(mPolicy);

        //    // nominee
        //    if (!string.IsNullOrWhiteSpace(mPolicy.NomineeName))
        //        _policyNomineeRepo.Upsert(PrepareNomineeModel(mSaved.Id, mPolicy));

        //    return mSaved;
        //}
        //public Domain.Policy ImportPolicy(Domain.Policy mPolicy)
        //{
        //    if (mPolicy.CompanyId <= 0)
        //        throw new Exception("Company is required.");

        //    // 1. group (Family value = group Name, per company)
        //    var mGroup = _groupRepo.GetByName(mPolicy.Group, mPolicy.CompanyId);
        //    if (mGroup == null || mGroup.Id <= 0)
        //        mGroup = _groupRepo.Upsert(PrepareGroupModel(mPolicy));   // repository directly: no auto-generate
        //    mPolicy.GroupId = mGroup.Id;

        //    // 2. client (first group wins, so an existing client keeps its group)
        //    var mClient = PrepareClientModel(mPolicy);
        //    var efClient = _clientRepo.GetByDetails(mClient.FirstName, mClient.MiddleName, mClient.LastName,
        //                                            mClient.PhoneNumber, mPolicy.CompanyId, mClient.DOB);

        //    mClient = (efClient == null || efClient.Id <= 0) ? _clientRepo.Upsert(mClient) : efClient;

        //    // 3. address (only when the row has one), linked to the client
        //    if (HasAddress(mPolicy.Address))
        //    {
        //        var mAddress = PrepareAddressModel(mPolicy);   // sets CompanyId
        //        mAddress.ClientId = mClient.Id;

        //        var efAddress = _addressRepo.GetByAddress(mAddress);   // filters CompanyId + ClientId + fields
        //        mAddress = (efAddress == null || efAddress.Id <= 0) ? _addressRepo.Upsert(mAddress) : efAddress;

        //        if (mClient.AddressId != mAddress.Id)
        //        {
        //            mClient.AddressId = mAddress.Id;
        //            mClient.LastModifiedBy = mPolicy.CreatedBy;
        //            mClient = _clientRepo.Upsert(mClient);
        //        }
        //    }

        //    // 4. policy
        //    mPolicy.ClientId = mClient.Id;
        //    mPolicy.PolicyStatusId = 1;
        //    mPolicy.PolicyTypeId = 1;
        //    var mSaved = _repo.ImportPolicy(mPolicy);

        //    // 5. nominee
        //    if (!string.IsNullOrWhiteSpace(mPolicy.NomineeName))
        //        _policyNomineeRepo.Upsert(PrepareNomineeModel(mSaved.Id, mPolicy));

        //    return mSaved;
        //}

        public Domain.Policy ImportPolicy(Domain.Policy mPolicy)
        {
            if (mPolicy.CompanyId <= 0)
                throw new Exception("Company is required.");

            // 1. group (Family value = group Name, per company)
            var mGroup = _groupRepo.GetByName(mPolicy.Group, mPolicy.CompanyId);
            if (mGroup == null || mGroup.Id <= 0)
                mGroup = _groupRepo.Upsert(PrepareGroupModel(mPolicy));   // repository directly: no auto-generate
            mPolicy.GroupId = mGroup.Id;

            // 2. client (first group wins, but an old client without a group gets one)
            var mClient = PrepareClientModel(mPolicy);
            var efClient = _clientRepo.GetByDetails(mClient.FirstName, mClient.MiddleName, mClient.LastName,mClient.PhoneNumber, mClient.DOB, mPolicy.CompanyId);

            mClient = (efClient == null || efClient.Id <= 0) ? _clientRepo.Upsert(mClient) : efClient;

            if (mClient.GroupId <= 0)
            {
                mClient.GroupId = mGroup.Id;
                mClient.LastModifiedBy = mPolicy.CreatedBy;
                mClient = _clientRepo.Upsert(mClient);
            }

            // 3. address (only when the row has one), linked to the client
            if (HasAddress(mPolicy.Address))
            {
                var mAddress = PrepareAddressModel(mPolicy);   // sets CompanyId
                mAddress.ClientId = mClient.Id;

                var efAddress = _addressRepo.GetByAddress(mAddress);   // filters CompanyId + ClientId + fields
                mAddress = (efAddress == null || efAddress.Id <= 0) ? _addressRepo.Upsert(mAddress) : efAddress;

                if (mClient.AddressId != mAddress.Id)
                {
                    mClient.AddressId = mAddress.Id;
                    mClient.LastModifiedBy = mPolicy.CreatedBy;
                    mClient = _clientRepo.Upsert(mClient);
                }
            }

            // 4. policy
            mPolicy.ClientId = mClient.Id;
            mPolicy.PolicyStatusId = 1;
            mPolicy.PolicyTypeId = 1;
            SplitPlanTermPpt(mPolicy);
            mPolicy.MaturityDate = CalculateMaturityDate(mPolicy);
            var mSaved = _repo.ImportPolicy(mPolicy);

            // 5. nominee
            if (!string.IsNullOrWhiteSpace(mPolicy.NomineeName))
                _policyNomineeRepo.Upsert(PrepareNomineeModel(mSaved.Id, mPolicy));

            return mSaved;
        }

        #region Private Method

        #region Group Model
        private Domain.Group PrepareGroupModel(Domain.Policy mPolicy)
        {
            var name = (mPolicy.Group ?? "").Trim();
            if (name == "")
                throw new Exception("Group name is empty.");

            var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;

            var clientName = System.Text.RegularExpressions.Regex.Replace(
                (mPolicy.Name ?? "").Trim(), @"/LA\b", "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
            clientName = textInfo.ToTitleCase(clientName.ToLower());

            return new Domain.Group
            {
                Code = name,
                Name = name,
                HeadName = clientName,
                CompanyId = mPolicy.CompanyId,
                MobileNumber = (mPolicy.PhoneNumber ?? "").Trim(),
                IsActive = true,
                CreatedBy = mPolicy.CreatedBy
            };
        }
        #endregion

        #region Address Model
        private bool HasAddress(string address)
        {
            return !string.IsNullOrWhiteSpace(address)
                && System.Text.RegularExpressions.Regex.IsMatch(address, @"[A-Za-z0-9]");
        }
        
        private Domain.Address PrepareAddressModel(Domain.Policy mPolicy)
        {
            var text = (mPolicy.Address ?? "").Trim();

            // pull the 6 digit pincode off the end first
            string zip = "";
            var zm = System.Text.RegularExpressions.Regex.Match(text, @"(\d{6})\s*\W*$");
            if (zm.Success)
            {
                zip = zm.Groups[1].Value;
                text = text.Substring(0, zm.Index).Trim();
            }

            // treat commas, semicolons and line breaks as separators
            var parts = System.Text.RegularExpressions.Regex.Split(text, @"[,;\r\n]+")
                .Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"\s+", " ").Trim(' ', '-', '.'))
                .Where(x => x != "")
                .ToList();

            string line1 = "", line2 = "", area = "", city = "";

            if (parts.Count == 1)
            {
                // no separators: last word is the city, the rest is line 1
                var words = parts[0].Split(' ');
                if (words.Length > 1)
                {
                    city = words[words.Length - 1];
                    line1 = string.Join(" ", words.Take(words.Length - 1));
                }
                else
                {
                    city = parts[0];
                }
            }
            else if (parts.Count > 1)
            {
                city = parts[parts.Count - 1];
                parts.RemoveAt(parts.Count - 1);

                line1 = parts[0];
                if (parts.Count >= 3)
                {
                    area = parts[parts.Count - 1];
                    line2 = string.Join(", ", parts.Skip(1).Take(parts.Count - 2));
                }
                else if (parts.Count == 2)
                {
                    area = parts[1];
                }
            }

            if (line1 == "") line1 = city;     

            return new Domain.Address
            {
                CompanyId = mPolicy.CompanyId,
                Line1 = line1,
                Line2 = line2,
                Area = area,
                City = city,
                State = "Gujarat",
                ZipCode = zip,
                CreatedBy = mPolicy.CreatedBy
            };
        }
        #endregion

        #region Policy Nominee Model
        private Domain.PolicyNominee PrepareNomineeModel(int policyId, Domain.Policy mPolicy)
        {
            var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;

            return new Domain.PolicyNominee
            {
                PolicyId = policyId,
                Name = textInfo.ToTitleCase((mPolicy.NomineeName ?? "").Trim().ToLower()),
                Age = mPolicy.NomineeAge,
                Relation = textInfo.ToTitleCase((mPolicy.NomineeRelation ?? "").Trim().ToLower()),
                NomineePercentage = 100,
                IsActive = true,
                CreatedBy = mPolicy.CreatedBy
            };
        }
        #endregion

        #region Client Model
        private Domain.Client PrepareClientModel(Domain.Policy mPolicy)
        {
            var name = System.Text.RegularExpressions.Regex.Replace(
                (mPolicy.Name ?? "").Trim(), @"/LA\b", "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();

            var splitStrings = StaticMethods.SplitString(name, ' ')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            if (splitStrings.Length == 0)
                throw new Exception("Client name is empty.");

            var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
            Func<string, string> toPascal = s => textInfo.ToTitleCase((s ?? "").ToLower());

            var mClient = new Domain.Client
            {
                FirstName = toPascal(splitStrings[0]),
                MiddleName = splitStrings.Length > 2 ? toPascal(splitStrings[1]) : "",
                LastName = splitStrings.Length > 1 ? toPascal(splitStrings[splitStrings.Length - 1]) : "",
                Group = mPolicy.Group,
                GroupId = mPolicy.GroupId,
                CompanyId = mPolicy.CompanyId,
                PhoneNumber = mPolicy.PhoneNumber ?? "",
                EmailAddress = mPolicy.Email,
                DOB = mPolicy.DOB,
                IsActive = true,
                CreatedBy = mPolicy.CreatedBy
            };

            return mClient;
        }
        #endregion


        private void SplitPlanTermPpt(Domain.Policy mPolicy)
        {
            var parts = (mPolicy.PlanTermPpt ?? "")
                .Split('/')
                .Select(x => x.Trim())
                .ToArray();

            int term = 0, ppt = 0;

            mPolicy.Plan = parts.Length > 0 ? parts[0] : "";
            mPolicy.Term = parts.Length > 1 && int.TryParse(parts[1], out term) ? term : 0;
            mPolicy.PPT = parts.Length > 2 && int.TryParse(parts[2], out ppt) ? ppt : 0;
        }

        // Terms at or above this are treated as whole life (no maturity date). Adjust to your plans.
        private const int WholeLifeTerm = 99;

        // Maturity = Commencement Date + Term years, date only (no time)
        private DateTime? CalculateMaturityDate(Domain.Policy mPolicy)
        {
            if (mPolicy.CommercementDate == null || mPolicy.Term <= 0 || mPolicy.Term >= WholeLifeTerm)
                return null;

            return mPolicy.CommercementDate.Value.Date.AddYears(mPolicy.Term);
        }
        #endregion
    }

}
