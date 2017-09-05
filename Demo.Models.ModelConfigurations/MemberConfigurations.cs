using MO.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models.ModelConfigurations
{
    public class MemberConfigurations: EntityConfigurationBase<Member,Guid>
    {
        public MemberConfigurations()
        {
            this.HasKey(p => p.Id);
        }
    }
}
