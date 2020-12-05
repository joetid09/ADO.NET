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

        public ChoreRepository(string connectionString) : base(connectionString) {}

        ///Now that there is a tunnel/connection to the database, we need a place to put the data we want
        ///So we want to make a list of ALL the information there and return it
        ///
        List<Chore> GetAll()
        { 
            ///Reminder, because the database is shared. we want to close our connection as soon as we
            ///get what we want. 
            ///the "using" block will close our connection to the resource as soon as it is completed
            ///
            using(SqlConnection conn = Connection)
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
                    List < Chore > chores = new List<Chore>();

                    //so long as there is data to return (remember, it goes row by row in forward motion
                    //(it can never go back to a row it's alreay visited). chores will return true
                    while(reader.Read())
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


    }
}
