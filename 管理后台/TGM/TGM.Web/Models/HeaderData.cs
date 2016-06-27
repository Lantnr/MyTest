using System.Collections.Generic;
using TGM.API.Entity;
using TGM.API.Entity.Model;

namespace TGM.Web.Models
{
    public class HeaderData
    {
        public User User { get; set; }

        public List<Server> Server { get; set; }
    }
}