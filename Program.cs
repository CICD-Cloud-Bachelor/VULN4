using System;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using System.Collections.Generic;

namespace YourNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = BuildConfiguration();
            string connectionString = config.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    List<Employee> employees = GetEmployees(conn);
                    DisplayEmployees(employees);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }


        static List<Employee> GetEmployees(MySqlConnection conn)
        {
            string query = "SELECT Id, Name, Department FROM Employees";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                List<Employee> employees = new List<Employee>();
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Department = reader.GetString("Department")
                    });
                }
                return employees;
            }
        }

        static void DisplayEmployees(List<Employee> employees)
        {
            Console.WriteLine("Employees:");
            foreach (Employee emp in employees)
            {
                Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Department: {emp.Department}");
            }
        }
    }

    class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
    }
}
