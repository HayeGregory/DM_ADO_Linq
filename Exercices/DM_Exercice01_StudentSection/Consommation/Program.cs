using System;
using System.Data;
using System.Data.SqlClient;

namespace Consommation
{
    class Program
    {
        static string _connectionString = @"Data Source=DESKTOP-RQPUUKM;Initial Catalog=ADO;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static DataSet myDS = new DataSet();

        static void Main(string[] args)
        {

            using (SqlConnection connection = new SqlConnection()) {
                connection.ConnectionString = _connectionString;
                connection.Open();

                #region Theorie explication :
                // premiere facon sql command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;

                }
                // deuxieme facon sql command  <-- better
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    // cmd.CommandText = "";
                    // cmd.CommandText = "select * from Student";

                    // Mode connecté : 
                    // 3 METHODES D'ATTAQUE 
                    //
                    // 1 ) EXECUTE SCALAR --> renvoi une et une seule donnee
                    //cmd.CommandText = "Select FirstName from Student where id = 1";
                    //string resultat = cmd.ExecuteScalar().ToString();
                    //Console.WriteLine($"resultat : {resultat}");

                    // --> oblige de caster car on sait pas ce qui revient
                    //string resultat = (int)cmd.ExecuteScalar();
                    //
                    // 2 ) SQLDATAREADER --> renvoi IDataRecord & IDataReader
                    //
                    //cmd.CommandText = "Select *from student";
                    //using (SqlDataReader reader = cmd.ExecuteReader())
                    //{
                    //    while (reader.Read())
                    //    {
                    //        Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]}");
                    //    }
                    //}
                    //
                    // 3 ) EXECUTENONQUERY --> set a envoye les ordres delete insert update et retourne le nombre de ligne affectee
                    // ---> on fait jamais ca question securite
                    //
                    //cmd.CommandText = @"Insert into Section(Id, SectionName) values (8888, 'section 8888')";
                    //Console.WriteLine($"nbr de ligne affecteee : {cmd.ExecuteNonQuery()}");
                    //
                    // Mode deconnecté : 
                    // SQLDataAdapter --> renvoi un objet ()

                    // insert into (.. , .. ) values (.. , .. )
                    // 
                    // table output apres insert par exemple
                    //cmd.CommandText = @"Insert into Section(Id, SectionName) output inserted.SectionName values (8888, 'section 8888')";
                    //Console.WriteLine($"section rajoutee  : {cmd.ExecuteScalar()}");

                }
                #endregion

                #region Exercices :  

                #region Ex A
                // exercice A : Afficher l’« ID », le « Nom », le « Prenom » de chaque étudiant 
                // depuis la vue « V_Student » en utilisant la méthode connectée
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    Console.WriteLine("exercice A :");
                    cmd.CommandText = "select Id, FirstName, LastName from Student";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Id"]} : {reader["FirstName"]} {reader["LastName"]}");
                        }
                    }
                } 
                #endregion

                #region Ex_B
                // exercice B : Afficher l’« ID », le « Nom » de chaque section en utilisant la méthode déconnectée
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    Console.WriteLine("exercice B :");
                    cmd.CommandText = "select Id, SectionName from Section ";

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    DataSet ds = new DataSet(); // DataTable dt = new DataTable();
                    da.Fill(ds);                // da.Fill(dt);
                    da.Fill(myDS); // --> ici on peut se deco
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Console.WriteLine($"{dr["Id"]} : {dr["SectionName"]} ");
                        }
                    }
                } 
                #endregion

                #region Ex C
                // exercice C :  Afficher la moyenne annuelle des etudiants
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    Console.WriteLine("exercice C :");
                    cmd.CommandText = "select avg(YearResult) as [MoyenneAnnuelle] from Student;";
                    Console.WriteLine($"Moyenne annuelle des etudiants : {cmd.ExecuteScalar()}");
                }
                #endregion

                #endregion


                #region Insertion d'une instance de classe student
                Student Greg = new Student("Gregory", "Haye", new DateTime(1979,06,14), 18, 1010, true);
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "insert into Student (FirstName, LastName, BirthDate, YearResult, SectionID, Active) values ('"
                                        + Greg.FirstName +"','"
                                        + Greg.LastName +"','"
                                        + Greg.BirthDate.ToString("yyyy-MM-dd") + "',"
                                        + Greg.YearResult +","
                                        + Greg.SectionID + "," 
                                        + (Greg.Active ? 1 : 0) +")";
                    Console.WriteLine($"nbr de ligne affecteee : {cmd.ExecuteNonQuery()}");
                }
                #endregion

            }
            #region exercice B : apres deconnection ;)
            // on dispose toujours des informations puisque ramene dans un dataset
            //Console.WriteLine("Exercice B : deconnecte");
            //if (myDS.Tables.Count > 0)
            //{
            //    foreach (DataRow dr in myDS.Tables[0].Rows)
            //    {
            //        Console.WriteLine($"{dr["Id"]} : {dr["SectionName"]} ");
            //    }
            //} 
            #endregion

        }
    }
}
