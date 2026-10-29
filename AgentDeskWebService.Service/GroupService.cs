using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using AgentDeskWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace AgentDeskWebService.Service
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repo;

        public GroupService(IGroupRepository repo)
        {
            _repo = repo;
        }

        public Domain.GroupLister GetAll(Domain.GroupLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Group Upsert(Domain.Group mGroup)
        {
            if (mGroup.Id <= 0 && string.IsNullOrWhiteSpace(mGroup.Code))
                mGroup.Code = GenerateGroupCode(mGroup);

            return _repo.Upsert(mGroup);
        }

        public Domain.Group Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.Group> GetActiveGroup()
        {
            return _repo.GetActiveGroup();
        }

        #region Private Method
        private string GenerateGroupCode(Domain.Group mGroup)
        {
            if (string.IsNullOrWhiteSpace(mGroup.HeadName))
                throw new Exception("Head name is required to generate the group code.");

            // Normalize head name: remove non-letters and split
            var words = System.Text.RegularExpressions.Regex
                .Replace(mGroup.HeadName.Trim(), @"[^A-Za-z]", "")
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
            string first = words.Length > 0 ? textInfo.ToTitleCase(words[0].ToLower()) : "";
            string last = words.Length > 1 ? textInfo.ToTitleCase(words[words.Length - 1].ToLower()) : "";

            string basePart = (first + last).ToUpper();

            string code;
            do
            {
                // Take first letters from basePart, then pad with randoms until length = 5
                string prefix = basePart.Length >= 2 ? basePart.Substring(0, 2) : basePart;
                string randomSuffix = StaticMethods.GeneratePassword(5 - prefix.Length, true, false, true, false);
                code = (prefix + randomSuffix).Substring(0, 5); // ensure exactly 5 chars
            }
            while (_repo.CodeExists(mGroup.CompanyId, code));

            return code;
        }

        #endregion
    }
}
