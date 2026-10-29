namespace AgentDeskWebService.Domain
{
    public class Group
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string HeadName { get; set; }
        public string MobileNumber { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
