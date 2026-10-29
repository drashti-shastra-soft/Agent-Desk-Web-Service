using AgentDeskWebService.Infrastructure.CustomException;
using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using AgentDeskWebService.Infrastructure.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using System;

namespace AgentDeskWebService.Service
{
    public class UserService : IUserService
    {
        #region Declaration
        private readonly IUserRepository _repo;
        private readonly IFileReaderService _fileReaderService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ICompanyRepository _companyRepository;

        public UserService(IUserRepository repo, IFileReaderService fileReaderService, IEmailSenderService emailSenderService, ICompanyRepository companyRepository)
        {
            _repo = repo;
            _fileReaderService = fileReaderService;
            _emailSenderService = emailSenderService;
            _companyRepository = companyRepository;
        }

        #endregion

        public Domain.UserLister GetAll(Domain.UserLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.User Upsert(Domain.User mUser)
        {
            if (mUser == null)
                throw new APIRequestFailedException("User object must not be null.");

            SetUserPassword(mUser);

            var mCreatedUser = _repo.Upsert(mUser);
            if (IsValidUser(mCreatedUser))
            {
                var mCompany = _companyRepository.Get(mCreatedUser.CompanyId);
                SendEmailOnCreateUser(mCreatedUser, mCompany);
            }
            return mCreatedUser;

            //return _repo.Upsert(mUser);
        }

        public Domain.User Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public bool CheckEmailAddressExist(Domain.User mUser)
        {
            return _repo.CheckEmailAddressExist(mUser);
        }

        #region Private Methods

        private void SendEmailOnCreateUser(Domain.User mUser, Domain.Company mCompany)
        {
            string content = _fileReaderService.ReadFileContent("~/EmailTemplates/SendCredentialsOnCreateUser.html");
            string finalContent = PopulateEmailTemplate(content, mUser, mCompany);

            Task.Run(async () =>
            {
                try
                {
                    await _emailSenderService.SendEmailAsync($"Welcome to {mCompany.Name} – Your Account Details Inside", finalContent, mUser.EmailAddress);
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText("ErrorLog.txt", $"{DateTime.Now}: Background send failed - {ex.Message}{Environment.NewLine}");
                }
            });
            //_emailSenderService.SendEmailAsync($"Welcome to {mCompany.Name} – Your Account Details Inside", finalContent, mUser.EmailAddress);
        }

        private string PopulateEmailTemplate(string template, Domain.User mUser, Domain.Company mCompany)
        {
            var replacements = new Dictionary<string, string>
            {
                { "{{CompanyName}}", mCompany.Name },
                { "{{FirstName}}", mUser.FirstName },
                { "{{Username}}", mUser.EmailAddress },
                { "{{Password}}", mUser.PlainTextPassword},
                { "{{Year}}", DateTime.Now.Year.ToString() }
            };

            foreach (var pair in replacements)
            {
                template = template.Replace(pair.Key, pair.Value);
            }

            return template;
        }

        private void SetUserPassword(Domain.User user)
        {
            user.PlainTextPassword = StaticMethods.GeneratePassword(8);
            var salt = ConfigurationManager.AppSettings.Get("Salt");
            user.Password = PasswordEncryptor.Encrypt(user.PlainTextPassword, salt);
        }

        private bool IsValidUser(Domain.User user)
        {
            return user != null && user.Id > 0;
        }

        #endregion
    }
}
