using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoApplication
{
    class Program
    {
        static string connectionString =
            "Data Source = 10.10.20.67; Database = DemoApplication; User Id = App_Demo ; Password = Gem$ol1234;";

        static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            Read();
            //Create("pravesh 1", "applications", "lead", "abc.xyz");
            //Update(7, "rakesh 1", "dev ops", "lead", "rakesh.gandhi");
            Delete(7);
            //Read();
            ReadFromDataSet();
            Console.ReadLine();
        }

        static void Create(string name, string department, string designation, string emailId)
        {
            var query =
                "INSERT INTO [dbo].[Employee] ([Name], [Department], [Designation], [EmailId]) VALUES (@Name, @Department, @Designation, @EmailId)";
            var command = new SqlCommand(query, connection);
            var sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@Name", name);
            sqlparams[1] = new SqlParameter("@Department", department);
            sqlparams[2] = new SqlParameter("@Designation", designation);
            sqlparams[3] = new SqlParameter("@EmailId", emailId);

            foreach (var param in sqlparams)
            {
                command.Parameters.Add(param);
            }

            /*
            command.Parameters.Add(sqlparams[1]);
            command.Parameters.Add(sqlparams[2]);
            command.Parameters.Add(sqlparams[3]);
            command.Parameters.Add(sqlparams[4]);
            */
            connection.Open();
            var result = command.ExecuteNonQuery();
            Console.WriteLine(result);
            connection.Close();
        }

        static void Update(int id, string name, string department, string designation, string emailId)
        {
            var query = "UPDATE [dbo].[Employee] SET [Name] = @Name, [Department] = @Department, [Designation] = @Designation, [EmailId] = @EmailId WHERE [Id] = @Id";
            var command = new SqlCommand(query, connection);
            var sqlparams = new SqlParameter[5];

            sqlparams[0] = new SqlParameter("@Name", name);
            sqlparams[1] = new SqlParameter("@Department", department);
            sqlparams[2] = new SqlParameter("@Designation", designation);
            sqlparams[3] = new SqlParameter("@EmailId", emailId);
            sqlparams[4] = new SqlParameter("@Id", id);

            foreach (var param in sqlparams)
            {
                command.Parameters.Add(param);
            }
            connection.Open();
            var result = command.ExecuteNonQuery();
            Console.WriteLine(result);
            connection.Close();
        }

        static void Delete(int id)
        {
            var query = "DELETE FROM [dbo].[Employee] WHERE [Id] = @Id";
            var command = new SqlCommand(query, connection);
            var sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@Id", id);
            foreach (var param in sqlparams)
            {
                command.Parameters.Add(param);
            }
            connection.Open();
            var result = command.ExecuteNonQuery();
            Console.WriteLine(result);
            connection.Close();
        }

        static void Read()
        {
            var query = "SELECT * FROM [dbo].[Employee]";
            var command = new SqlCommand(query, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.HasRows == true)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var name = reader.GetString(1);
                    var department = reader.GetString(2);

                    var message = id + " | " + name + " | " + department;
                    Console.WriteLine(message);
                }
            }
            connection.Close();
        }

        static void ReadFromDataSet()
        {
            var query = "SELECT * FROM [dbo].[Department] SELECT * FROM [dbo].[Employee]";
            var command = new SqlCommand(query, connection);
            var adapter = new SqlDataAdapter();
            var dataSet = new DataSet();
            adapter.SelectCommand = command;
            connection.Open();
            adapter.Fill(dataSet);
            var depTable = dataSet.Tables[0];
            var empTable = dataSet.Tables[1];

            foreach (DataRow row in depTable.Rows)
            {
                var id = row[0];
                var name = row["name"];
                var message = id + " | " + name;
                Console.WriteLine(message);
            }

            foreach (DataRow row in empTable.Rows)
            {
                var id = row[0];
                var name = row["name"];
                var department = row["department"];
                var message = id + " | " + name + " | " + department;
                Console.WriteLine(message);
            }

            connection.Close();


        }
    }
}
