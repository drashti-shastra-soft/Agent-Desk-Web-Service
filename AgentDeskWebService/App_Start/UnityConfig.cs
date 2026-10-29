using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using AgentDeskWebService.Repository;
using AgentDeskWebService.Service;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace AgentDeskWebService
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            //Services
            container.RegisterType<IAddressService, AddressService>();
            container.RegisterType<IAgencyService, AgencyService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IChangePasswordService, ChangePasswordService>();
            container.RegisterType<IClientDocumentService, ClientDocumentService>();
            container.RegisterType<IClientFinancialInfoService, ClientFinancialInfoService>();
            container.RegisterType<IClientHealthService, ClientHealthService>();
            container.RegisterType<IClientIdentificationService, ClientIdentificationService>();
            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<IDashboardService, DashboardService>();
            container.RegisterType<ICompanyService, CompanyService>();
            container.RegisterType<IEmailSenderService, EmailSenderService>();
            container.RegisterType<IFileReaderService, FileReaderService>();
            container.RegisterType<IForgotPasswordService, ForgotPasswordService>();
            container.RegisterType<IGroupService, GroupService>();
            container.RegisterType<IJWTokenService, JWTokenService>();
            container.RegisterType<IPolicyDocumentService, PolicyDocumentService>();
            container.RegisterType<IPolicyNomineeService, PolicyNomineeService>();
            container.RegisterType<IPolicyPremiumService, PolicyPremiumService>();
            container.RegisterType<IPolicyService, PolicyService>();
            container.RegisterType<IPolicyStatusService, PolicyStatusService>();
            container.RegisterType<IPolicyTypeService, PolicyTypeService>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IUserService, UserService>();

            //Repositories
            container.RegisterType<IAddressRepository, AddressRepository>();
            container.RegisterType<IAgencyRepository, AgencyRepository>();
            container.RegisterType<IClientDocumentRepository, ClientDocumentRepository>();
            container.RegisterType<IClientFinancialInfoRepository, ClientFinancialInfoRepository>();
            container.RegisterType<IClientHealthRepository, ClientHealthRepository>();
            container.RegisterType<IClientIdentificationRepository, ClientIdentificationRepository>();
            container.RegisterType<IClientRepository, ClientRepository>();
            container.RegisterType<ICompanyRepository, CompanyRepository>();
            container.RegisterType<IDashboardRepository, DashboardRepository>();
            container.RegisterType<IGroupRepository, GroupRepository>();
            container.RegisterType<IPolicyDocumentRepository, PolicyDocumentRepository>();  
            container.RegisterType<IPolicyNomineeRepository, PolicyNomineeRepository>();  
            container.RegisterType<IPolicyPremiumRepository, PolicyPremiumRepository>();  
            container.RegisterType<IPolicyRepository, PolicyRepository>();
            container.RegisterType<IPolicyStatusRepository, PolicyStatusRepository>();
            container.RegisterType<IPolicyTypeRepository, PolicyTypeRepository>();
            container.RegisterType<IReportRepository, ReportRepository>();
            container.RegisterType<IRoleRepository, RoleRepository>();
            container.RegisterType<IUserRepository, UserRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}