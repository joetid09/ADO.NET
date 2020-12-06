using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        //First make first is to construct a tunnel
        public RoommateRepository(string ConnectionString) : base(ConnectionString) { }

        //Second is to create a method for instructions
        public  Roommate GetRoommateById(int id)
        {
            //make a request pathway by using
            using (SqlConnection conn = Connection)
            {
                //Remember to open that tunnel!
                conn.Open();

                //Time to make the instruction packet!
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT	Roommate.ID, FirstName, RentPortion, Room.Name from Roommate
                                       LEFT JOIN Room
	                                    ON Roommate.RoomId = Room.Id
                                        WHERE Roommate.ID = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    //Now that we tell the DB what we want, we need to be able to read it

                    SqlDataReader reader = cmd.ExecuteReader();

                        //want to go ahead and create the new room instance, so it can be instantiated later
                        Roommate roommate = null;

                        //Now that we have the data requested, and a way to reader once receive. LOGIC TIME!
                        //This is just where we save the requested information so that it can be returned
                        if (reader.Read())
                        {
                        roommate = new Roommate()
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };


                        }
                        //return the new object so it can be viewed on console
                        reader.Close();
                        return roommate;
                    }

                }
            }

        }

    }

