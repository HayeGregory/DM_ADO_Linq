using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Consommation
{
    class Program
    {
        static string _connectionString = @"Data Source=DESKTOP-RQPUUKM;Initial Catalog=ADO;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static DataSet myDS = new DataSet();

        static void Main(string[] args)
        {

            using (SqlConnection connection = new SqlConnection())
            {
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
                    // 3 ) EXECUTENONQUERY --> sert a envoye les ordres delete insert update et retourne le nombre de ligne affectee
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

                    // requete parametree ( pour securisé la commande sql )
                    // cmd.CommandText = $"insert into student(nom) values (@nom)";
                    // ....
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
                Student Greg = new Student("ENOR", "ENOR", new DateTime(1979, 06, 14), 18, 1010, true);
                int idGreg;
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "insert into Student (FirstName, LastName, BirthDate, YearResult, SectionID, Active) " 
                        + "output inserted.Id values ('" 
                                        + Greg.FirstName + "','"
                                        + Greg.LastName + "','"
                                        + Greg.BirthDate.ToString("yyyy-MM-dd") + "',"
                                        + Greg.YearResult + ","
                                        + Greg.SectionID + ","
                                        + (Greg.Active ? 1 : 0) + ")";
                    idGreg =(int) cmd.ExecuteScalar();
                    Console.WriteLine($"id de l'ajout du student :{idGreg}");
                }
                #endregion
                #region Insertion via requete paramaetree ( securisee )
                Student voisine = new Student("jiji", "jiji", new DateTime(1979, 1, 1), 15, 1010, true);
                int idVoisine;
                using (SqlCommand cmd = connection.CreateCommand()) {
                    cmd.CommandText = "insert into Student (FirstName, LastName, BirthDate, YearResult, SectionID) "
                                        + "output inserted.Id values (@FirstName, @LastName, @BirthDate, @YearResult, @SectionID)";
                    
                    // version complete ( a connaitre egalement ) 
                    //SqlParameter PFirstName = new SqlParameter() {
                    //    ParameterName = "FirstName", Value = voisine.FirstName
                    //};
                    //SqlParameter PLastName = new SqlParameter()
                    //{
                    //    ParameterName = "LastName",
                    //    Value = voisine.LastName
                    //};
                    //SqlParameter PBirthDate = new SqlParameter()
                    //{
                    //    ParameterName = "BirthDate",
                    //    Value = voisine.BirthDate
                    //};
                    //SqlParameter PYearResult = new SqlParameter()
                    //{
                    //    ParameterName = "YearResult",
                    //    Value = voisine.YearResult
                    //};
                    //SqlParameter PSectionID = new SqlParameter()
                    //{
                    //    ParameterName = "SectionID",
                    //    Value = voisine.SectionID
                    //};
                    //cmd.Parameters.Add(PFirstName);
                    //cmd.Parameters.Add(PLastName);
                    //cmd.Parameters.Add(PBirthDate);
                    //cmd.Parameters.Add(PYearResult);
                    //cmd.Parameters.Add(PSectionID);
                    // version courte 
                    cmd.Parameters.AddWithValue("FirstName", voisine.FirstName);
                    cmd.Parameters.AddWithValue("LastName", voisine.LastName);
                    cmd.Parameters.AddWithValue("BirthDate", voisine.BirthDate);
                    cmd.Parameters.AddWithValue("YearResult", voisine.YearResult);
                    cmd.Parameters.AddWithValue("SectionID", voisine.SectionID);

                    idVoisine =(int) cmd.ExecuteScalar();
                    Console.WriteLine("id de la voisine : "+ idVoisine);
                }


                #endregion

                #region exec procedure stockee (updateStudent - deleteStudent)

                // changement de section
                using (SqlCommand cmd = connection.CreateCommand()) {
                    cmd.CommandText = "UpdateStudent";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // @Id INT, @SectionId INT, @YearResult INT
                    cmd.Parameters.AddWithValue("Id", idGreg);
                    cmd.Parameters.AddWithValue("SectionId", 1320);
                    cmd.Parameters.AddWithValue("YearResult", 0);
                    
                    cmd.ExecuteScalar(); // --> plutot utiliser : cmd.ExecuteNonQuery();
                }
                // suppresion de la voisine 
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DeleteStudent";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // @Id INT, @SectionId INT, @YearResult INT
                    cmd.Parameters.AddWithValue("Id", idVoisine);

                    cmd.ExecuteScalar(); // --> plutot utiliser : cmd.ExecuteNonQuery();
                }
                #endregion

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
}
