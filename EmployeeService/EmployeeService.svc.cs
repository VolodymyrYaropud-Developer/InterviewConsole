using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace EmployeeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IEmployeeService
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;User ID=sa;Password=pass@word1;";

        public Response GetEmployeeById(int id)
        {
            var allEmployees = new Dictionary<int, Employee>();
            var response = new Response();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string command = "SELECT ID, Name, ManagerID, Enable FROM Employee";
                SqlCommand cmd = new SqlCommand(command, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var emp = new Employee
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ManagerID = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                            Enable = reader.GetBoolean(3),
                            Employees = new List<Employee>()
                        };

                        allEmployees[emp.ID] = emp;
                    }
                }
            }

            foreach (var emp in allEmployees.Values)
            {
                if (emp.ManagerID.HasValue && allEmployees.ContainsKey(emp.ManagerID.Value))
                {
                    allEmployees[emp.ManagerID.Value].Employees.Add(emp);
                }
            }

            if (allEmployees.ContainsKey(id))
            {
                response.Employee = allEmployees[id];
            }

            return response;
        }


        public void EnableEmployee(int id, int enable)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string command1 = "UPDATE Employee SET Enable = " + (enable == 1 ? 1 : 0) + " WHERE ID = " + id + " ";
                    SqlCommand cmd = new SqlCommand(command1, conn);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}