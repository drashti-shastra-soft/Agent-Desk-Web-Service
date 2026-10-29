namespace AgentDeskWebService.Domain
{
    public class Agency
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string AgencyType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string CompanyName { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
    }
}
