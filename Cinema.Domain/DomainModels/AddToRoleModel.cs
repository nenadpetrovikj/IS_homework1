using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.DomainModels
{
    public class AddToRoleModel
    {
        public string Email { get; set; }
        public List<string> Roles { get; set; }

        public string SelectedRole { get; set; }
    }

}
