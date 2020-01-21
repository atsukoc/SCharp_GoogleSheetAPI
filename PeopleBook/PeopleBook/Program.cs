﻿
using System;
using System.Collections.Generic;
using System.Data.SQLite;


namespace PeopleBook
{
    public class Program
    {
        static void Main(string[] args)
        {
            Boolean on = true;
            List<Person> people = new List<Person>();
            SQLiteConnection sqlite_conn = CreateConnection();

            while (on)
            {
                char input = AskOperation();

                if (input.Equals('r') || input.Equals('R'))
                {
                    Operations.ReadData(ref people);
                }
                else if (input.Equals('x') || input.Equals('X'))
                {
                    on = false;
                    Console.WriteLine("Bye");
                    Environment.Exit(0);
                    sqlite_conn.Close();
                }
                else if (input.Equals('i') || input.Equals('I'))
                {
                    Person person = new Person();
                    Boolean done = false;

                    while (!done)
                    {
                        // Gather information
                        person.AskFirstName();
                        person.AskLastName();
                        person.AskEmail();

                        // Display the entered information
                        person.Display();

                        // Ask if anything needs to be modified before inserting to the google sheet
                        char validate = ConfirmData();

                        if (validate.Equals('Y') || validate.Equals('y'))
                        {
                            done = true;
                            Operations.WriteData(ref person);
                            people.Add(person);
                        }
                        else
                        {
                            bool updated = false;

                            while (!updated)
                            {
                                person.UpdatePerson();
                                person.Display();
                                char confirmation = ConfirmData();
                                if(confirmation.Equals('y') || confirmation.Equals('Y'))
                                {
                                    updated = true;
                                    Operations.WriteData(ref person);
                                    done = true;
                                    people.Add(person);
                                }
                            }
                        }
                    }
                }
            }
            
        }

        static SQLiteConnection CreateConnection()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source = database.db;Version=3;New=True;Compress=True");

            try
            {
                connection.Open();
            }
            catch(SQLiteException e)
            {
                Console.Write(e.Message);
            }


            return connection;
        }

        static private char AskOperation()
        {
            bool done = false;
            char charInput = '\0';

            if (!done)
            {
                Console.WriteLine("\n=========== Menu ==========");
                Console.WriteLine("R | Read the entire sheet");
                Console.WriteLine("I | Add a new person to the sheet");
                Console.WriteLine("X | Exit");
                Console.WriteLine("=======================");
                var input = Console.ReadLine();
                charInput = Convert.ToChar(input);
                if (charInput == 'R' ||
                    charInput == 'r' ||
                    charInput == 'I' ||
                    charInput == 'i' ||
                    charInput == 'X' ||
                    charInput == 'x')
                    done = true;
            }
   
            return charInput;
        }

        static private char ConfirmData()
        {
            Console.WriteLine("Is everything correct? Enter Y for YES, N for NO");
            var r = Console.ReadLine();
            char response = Convert.ToChar(r);
            return response;
        }

    }
    
}
