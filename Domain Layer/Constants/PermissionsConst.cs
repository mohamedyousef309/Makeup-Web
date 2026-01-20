using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Constants
{
    public static class PermissionsConst
    {
        public const int CreateId = 1;
        public const string CreateName = "Products.Create";

        public const int EditId = 2;
        public const string EditName = "Products.Edit";

        public const int DeleteId = 3;
        public const string DeleteName = "Products.Delete";

        public const int UpdateId = 4;
        public const string UpdateName = "Products.Update";
    }
}
