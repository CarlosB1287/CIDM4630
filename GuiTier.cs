using System;
using System.Data;
using MySql.Data.MySqlClient;
namespace FinalProject
{
    class GuiTier{
        Staff staff = new Staff();
        DataTier database = new DataTier();

        // print login page
        public Staff Login(){
            Console.WriteLine("------Welcome------");
            Console.WriteLine("Please input staff Name: ");
            staff.username = Console.ReadLine();
            Console.WriteLine("Please input password: ");
            staff.password = Console.ReadLine();
            return staff;
        }
        // print Options after staff logs in successfully
        public int Options(Staff staff){
            DateTime localDate = DateTime.Now;
            Console.WriteLine("---------------Options-------------------");
            Console.WriteLine($"Hello: {staff.username}; Date/Time: {localDate.ToString()}");
            Console.WriteLine("Please select an option to continue:");
            Console.WriteLine("1. Search Resident");
            Console.WriteLine("2. Pending Area");
            Console.WriteLine("3. Unknown Area");
            Console.WriteLine("4. Retrieve Package History");
            int option = Convert.ToInt16(Console.ReadLine());
            return option;
        }

        // // show Resident records returned from database
        public string DisplayResidents(DataTable tableResident){
            if(tableResident.Rows.Count > 0){
            Console.WriteLine("---------------Resident List-------------------");
            foreach(DataRow row in tableResident.Rows){
            Console.WriteLine($" FullName: {row["full_name"]} \t Email:{row["email"]} \t Unit_Number:{row["unit_number"]}");
            }
            string target_email = tableResident.Rows[0]["email"].ToString();
            return target_email;
            }
            return null;
        }
    }
}