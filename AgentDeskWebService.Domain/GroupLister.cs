using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Domain
{
    public class GroupLister
    {
        public List<Group> List { get; set; } = new List<Group>();

        public Group SearchCriteria { get; set; } = new Group();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
