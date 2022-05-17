using System.Collections.Generic;

namespace PhramacyApp.Areas.Admin.Models
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }

        public static class Dashboard
        {
            public const string View = "Permissions.Dashboard.View";
            public const string Create = "Permissions.Dashboard.Create";
            public const string Edit = "Permissions.Dashboard.Edit";
            public const string Delete = "Permissions.Dashboard.Delete";
        }

        public static class Shelfs
        {
            public const string View = "Permissions.Shelfs.View";
            public const string Create = "Permissions.Shelfs.Create";
            public const string Edit = "Permissions.Shelfs.Edit";
            public const string Delete = "Permissions.Shelfs.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Categories
        {
            public const string View = "Permissions.Categories.View";
            public const string Create = "Permissions.Categories.Create";
            public const string Edit = "Permissions.Categories.Edit";
            public const string Delete = "Permissions.Categories.Delete";
        }
        public static class Medicines
        {
            public const string View = "Permissions.Medicines.View";
            public const string Create = "Permissions.Medicines.Create";
            public const string Edit = "Permissions.Medicines.Edit";
            public const string Delete = "Permissions.Medicines.Delete";
        }

        public static class Customers
        {
            public const string View = "Permissions.Customers.View";
            public const string Create = "Permissions.Customers.Create";
            public const string Edit = "Permissions.Customers.Edit";
            public const string Delete = "Permissions.Customers.Delete";
        }
        public static class Suppliers
        {
            public const string View = "Permissions.Suppliers.View";
            public const string Create = "Permissions.Suppliers.Create";
            public const string Edit = "Permissions.Suppliers.Edit";
            public const string Delete = "Permissions.Suppliers.Delete";
        }
        public static class Menus
        {
            public const string View = "Permissions.Menus.View";
            public const string Create = "Permissions.Menus.Create";
            public const string Edit = "Permissions.Menus.Edit";
            public const string Delete = "Permissions.Menus.Delete";
        }
    }
}