using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;


namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";
        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        //*Remember roomRepo is the address for the connection
                        List<Room> rooms = roomRepo.GetAll();

                            foreach(Room r in rooms)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;
                    case ("Search for room"):
                        //*Remember roomRepo is the address for the connection
                        Console.WriteLine("What room number would you like to search for?");
                        int roomResponse = int.Parse(Console.ReadLine());
                        Room room = roomRepo.GetById(roomResponse);
                        Console.WriteLine($"{room.Id}.){room.Name} - Max Occupancy({room.MaxOccupancy})");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;
                    case ("Add a room"):
                        Console.WriteLine("Enter Room Name:");
                        string name = Console.ReadLine();

                        Console.WriteLine("Enter Max Occupancy");
                        int maxOccupancy = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = maxOccupancy
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been assigned an id of {roomToAdd.Id}");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        // Do stuff
                        break;
                    case ("Search for roommate"):
                        //*Remember roommateRepo is the address for the connection
                        Console.WriteLine("Which roommate are you looking for?");
                        int roommateResponse = int.Parse(Console.ReadLine());
                        Roommate roommate = roommateRepo.GetRoommateById(roommateResponse);
                        Console.WriteLine($"{roommate.Id}.){roommate.Firstname} lives in the {roommate.Name} and" +
                            $" pays {roommate.RentPortion}% of the rent");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;

                    case ("Show all chores"):
                        //*Remember roomRepo is the address for the connection
                        List<Chore> chores = choreRepo.GetAll();

                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;

                    case ("Search for chores"):
                        Console.WriteLine("What chore would you like to search for?");
                        int choreResponse = int.Parse(Console.ReadLine());
                        Chore chore= choreRepo.GetChoreByid(choreResponse);
                        //
                        Console.WriteLine($"Your search result is: {chore.Name}");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;

                    case ("Create new chore"):
                        Console.WriteLine("What chore would you like to add?");
                            string choreName = Console.ReadLine();
                        Chore newChore = new Chore()
                        {
                            Name = choreName
                        };
                        choreRepo.Insert(newChore);
                        Console.WriteLine($"You have added {newChore.Name} as a new chore");
                        Console.WriteLine("Press any ket to continue:");
                        Console.ReadLine();
                        break;


                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;

                }
            }
        }


        private static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
        {
            "Show all rooms",
            "Search for room",
            "Add a room",
            "Search for roommate",
            "Show all chores",
            "Search for chores",
            "Create new chore",
            "Exit"
        };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while(true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option >");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {
                    continue;
                }
            }


        }
    }
}
    