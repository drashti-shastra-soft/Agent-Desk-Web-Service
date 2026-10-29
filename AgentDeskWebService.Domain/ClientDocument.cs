namespace AgentDeskWebService.Domain
{
    public class ClientDocument
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string FilePath { get; set; }
        public string Base64String { get; set; }
        public string MIME { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
