using AgentDeskWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentDeskWebService.Repository
{
    public class RoleRepository : IRoleRepository
    {
        public List<Domain.Role> GetAll()
        {
            var mRoles = new List<Domain.Role>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efRoles = (from role in context.Roles
                                   select new
                                   {
                                       Id = role.Id,
                                       Name = role.Name
                                   }).AsEnumerable();

                    mRoles = JsonSerializer.Deserialize<List<Domain.Role>>(JsonSerializer.Serialize(efRoles.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mRoles;
        }
    }
}
