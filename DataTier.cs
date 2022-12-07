using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace FinalProject
{
    class DataTier{
        public string connStr = "server=20.172.0.16;database=crbaca1;port=8080;username=crbaca1;password=crbaca1";

        // perform login check using Stored Procedure "LoginCount" in Database based on given staff' staffid and Password
        public bool LoginCheck(Staff staff){
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {  
                conn.Open();
                string procedure = "LoginCount";
                MySqlCommand cmd = new MySqlCommand(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure; // set the commandType as storedProcedure
                cmd.Parameters.AddWithValue("@staff_username", staff.username);
                cmd.Parameters.AddWithValue("@staff_password", staff.password);
                cmd.Parameters.Add("@userCount", MySqlDbType.Int32).Direction =  ParameterDirection.Output;
                MySqlDataReader rdr = cmd.ExecuteReader();
            
                int returnCount = (int) cmd.Parameters["@userCount"].Value;

                rdr.Close();
                conn.Close();
            

                if (returnCount == 1){
                    return true;
                }
                else{
                    return false;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                conn.Close();
                return false;
            }
        
        }
        public DataTable getResident(string full_name)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string procedure = "getResident";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@full_name", full_name);
            cmd.Parameters["@full_name"].Direction = ParameterDirection.Input;

            MySqlDataReader rdr = cmd.ExecuteReader();

            DataTable tableEnrollment = new DataTable();
            tableEnrollment.Load(rdr);
            rdr.Close();
            conn.Close();
            return tableEnrollment;
        }

    //perform enrollment check using Stored Procedure "Search" based on staff and full_name
        public DataTable SearchResident(){
                Console.WriteLine("Please input Resident full name:");
                string full_name = Console.ReadLine();
                 Console.WriteLine("Please input Agency:");
                string agency = Console.ReadLine();
                 Console.WriteLine("Please input Status:");
                string Status = Console.ReadLine();
            try
            {  
                DataTable tableResidents = new DataTable();
                tableResidents =  getResident(full_name);
                if(tableResidents.Rows.Count > 0){
                    InserRecordtopendingArea(full_name, agency, Status);
                }
                else{
                    InserRecordtoUnknowArea(full_name, agency, Status);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return null;
        } 
        
        //insert records to pending area
        public bool InserRecordtopendingArea(string full_name, string agency, string Status)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string procedure = "insertToPendingArea";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@full_name", full_name);
            cmd.Parameters["@full_name"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@agency", agency);
            cmd.Parameters["@agency"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters["@Status"].Direction = ParameterDirection.Input;

            MySqlDataReader rdr = cmd.ExecuteReader();

            rdr.Close();
            conn.Close();
            return true;
        }

        // insert records to unknown area
        public bool InserRecordtoUnknowArea(string full_name, string agency, string Status)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string procedure = "insertToUnknownArea";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@full_name", full_name);
            cmd.Parameters["@full_name"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@agency", agency);
            cmd.Parameters["@agency"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters["@Status"].Direction = ParameterDirection.Input;

            MySqlDataReader rdr = cmd.ExecuteReader();

            rdr.Close();
            conn.Close();
            return true;
        }
    }
}