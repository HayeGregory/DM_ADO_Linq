using ADO_DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace UseADO_DB
{
    class Program
    {
        public static Student Converter(SqlDataReader dr)
        {
            return new Student()
            {
                Id = (int)dr["Id"],
                LastName = dr["LastName"].ToString(),
                FirstName = dr["FirstName"].ToString(),
                BirthDate = dr["BirthDate"] == DBNull.Value ? new DateTime() : (DateTime)dr["BirthDate"],
                YearResult = (int)dr["YearResult"],
                SectionId = (int)dr["SectionId"],
                Active = (bool)dr["Active"]
            };
        }

        static void Main(string[] args)
        {
            string _cs = @"Data Source=DESKTOP-RQPUUKM;Initial Catalog=ADO;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            Connection connection = new Connection(_cs);

            #region ExecuteNonQuery
            Command cmdNonQuery = new Command("DELETE FROM Student WHERE Id = 29");

            Console.WriteLine("Execute Non Query {0}", connection.ExecuteNonQuery(cmdNonQuery));
            #endregion

            #region ExecuteReader
            Command cmd = new Command("SELECT * FROM V_Student WHERE YearResult > 10");

            IEnumerable<Student> liste = connection.ExecuteReader<Student>(cmd, Converter);

            foreach (Student item in liste)
            {
                Console.WriteLine("Id : {0} | Nom : {1} | Résultat : {2}", item.Id, item.LastName, item.YearResult);
            }
            #endregion

            #region ExecuteScalar
            Command cmd2 = new Command("SELECT LastName FROM Student WHERE Id = 1");

            Console.WriteLine("Execute Scalar :  {0}", connection.ExecuteScalar(cmd2));
            #endregion
        }

    }
}
