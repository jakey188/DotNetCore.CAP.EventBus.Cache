using DotNetCore.CAP.Cap.EventBus.Cache.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Data.Entites
{
    public class UserEntity : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string NickName { get; set; }
    }
}
