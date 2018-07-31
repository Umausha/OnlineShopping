using Shoppingsmart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoppingsmart.Models.ViewModels.Pages
{
    public class SidebarVM
    {
        public SidebarVM()
        {

        }
        public SidebarVM(SideDto row)
        {
            Id = row.Id;
            Body = row.Body;

        }
        public int Id { get; set; }
        public string Body { get; set; }
    }
}