using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{    /// <summary>
     ///  This class is responsible for interacting with Chore data.
     ///  It inherits from the BaseRepository class so that it can use the BaseRepository's Connection property
     /// </summary>
    public class ChoreRepository : BaseRepository
    {

        ///***REMEMBER to FIRST setup network(connection) first step is to CONSTRUCT that TUNNEL!****
        ///connection string is method in base that takes the connection(address info) and makes new SLConnection

        public ChoreRepository(string connectionString) : base(connectionString) { }

        ///Now that there is a tunnel/connection to the database, we need a place to put the data we want
        ///So we want to make a list of ALL the information there and return it
        ///

        public List<Chore> GetAssignedChore()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = @"SELECT Chore.Id as Id, Name, RoommateId from Chore
	                                    LEFT JOIN RoommateChore
	                                    ON Chore.Id = RoommateChore.ChoreId
                                        WHERE RoommateId is NULL
		                                ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> assignedChores = new List<Chore>();

                    //remember, going to have more than one return, make sure to while loop
                    while (reader.Read())
                    {
                        Chore chore = new Chore
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        //add the new chores to a list of chores 
                        assignedChores.Add(chore);
                    }
                    reader.Close();
                    return assignedChores;

                }
            }
        }

        public List<Chore> GetAll()
        {
            ///Reminder, because the database is shared. we want to close our connection as soon as we
            ///get what we want. 
            ///the "using" block will close our connection to the resource as soon as it is completed
            ///
            using (SqlConnection conn = Connection)
            {
                //although using closes the connection once it is finished, it DOES NOT open it for us
                //Therefore, we must open it manually
                conn.Open();

                //Now we have must send the SQL what it is that we want, this will also be "using"
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //here we write our sql code for what it is and how we want it
                    cmd.CommandText = "SELECT Id, Name From Chore";

                    //next we need to get access or... READ the data. we create a reader
                    SqlDataReader reader = cmd.ExecuteReader();

                    //we now need a list format to read the data received, and store as chores
                    List<Chore> chores = new List<Chore>();

                    //so long as there is data to return (remember, it goes row by row in forward motion
                    //(it can never go back to a row it's alreay visited). chores will return true
                    while (reader.Read())
                    {
                        ///we want to get the index location of each column
                        int idColumnPosition = reader.GetOrdinal("ID");
                        //we then get the value located inside of that column
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");

                        string namevalue = reader.GetString(nameColumnPosition);

                        //Now it's time to take the information received for each row of info, and make an
                        //object out of the information received

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = namevalue
                        };

                        //and lastly we need to send it to the chore list that was made

                        chores.Add(chore);
                        //once the reader ends and returns false, the while loop will end
                    }
                    //so now we need to close the reader connection
                    reader.Close();

                    //and time to return the table information back
                    return chores;
                }
            }
        }

        public void AssignChore(int roommateId, int choreId )
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (RoommateID), (Choreid)
                                        OUTPUT INSERT.Id
                                        VALUES (@roommateId, @choreId)";

                    cmd.Parameters.AddWithValue("@roommateId", roommateId);
                    cmd.Parameters.AddWithValue("@choreId", choreId);

                    int id = (int)cmd.ExecuteScalar();

                    
                    roommateId = id;
                }
            }
        }


        //Now let's make a method that will allow use to get a room by id (will need to take an id parameter)

        public Chore GetChoreByid(int id)
        {
            //again, first step is to get connection address
            using (SqlConnection conn = Connection)
            {
                //remember, using does not open the connection... only closes it
                conn.Open();
                
                //Now that connection is open, we need to let the end know what we want
                //request take up usage so we want to make sure it closes once completed
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore WHERE ID = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    //Now that the DB knows what to give use, we need to create a way to read it
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;

                    //Since we only want one result, an if loop will replace the while loop
                    if(reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            //we still need to get ordinal and the value of that location
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        };
                    }
                    //Now that request was received and the needed object built. we need to close the reader
                    reader.Close();

                    //and return the request chore
                    return chore;
                   
                }
            }
        }

        public void Insert(Chore chore)
        {
            //create connection with using statement
            using (SqlConnection conn = Connection)
            {
                //remember to open as using will not.
                conn.Open();
                
                //then create and sent instructions to SQL Database
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                       OUTPUT INSERTED.Id
                                        VALUES (@name)";

                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
                       }

           
        }

    }
}
