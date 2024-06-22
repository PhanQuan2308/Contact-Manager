using System;
using MySql.Data.MySqlClient;

namespace ContactManager
{
    class Contact
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public Contact(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }
    }

    class Program
    {
        public static string connString = "Server=127.0.0.1;Database=contacts_db;User=root;Password=";

        static void Main(string[] args)
        {
            CreateDatabaseAndTable();

            int choice = 0;
            while (choice != 4)
            {
                Console.WriteLine("1. Add new contact");
                Console.WriteLine("2. Find a contact by name");
                Console.WriteLine("3. Display contacts");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddContact();
                        break;
                    case 2:
                        FindContact();
                        break;
                    case 3:
                        DisplayContacts();
                        break;
                    case 4:
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void CreateDatabaseAndTable()
        {
            using (var connection = new MySqlConnection(connString))
            {
                connection.Open();
                string sql = "CREATE TABLE IF NOT EXISTS Contacts (Id INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(255) NOT NULL, Phone VARCHAR(255) NOT NULL)";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        static void AddContact()
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            Console.Write("Enter phone number: ");
            string phone = Console.ReadLine();

            using (var connection = new MySqlConnection(connString))
            {
                connection.Open();
                string sql = "INSERT INTO Contacts (Name, Phone) VALUES (@Name, @Phone)";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Phone", phone);
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Contact added successfully.");
        }

        static void FindContact()
        {
            Console.Write("Enter name to find: ");
            string name = Console.ReadLine();

            using (var connection = new MySqlConnection(connString))
            {
                connection.Open();
                string sql = "SELECT Name, Phone FROM Contacts WHERE Name LIKE @Name";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", "%" + name + "%");
                MySqlDataReader reader = command.ExecuteReader();

                bool found = false;
                while (reader.Read())
                {
                    Console.WriteLine($"Name: {reader["Name"]}, Phone: {reader["Phone"]}");
                    found = true;
                }

                if (!found)
                {
                    Console.WriteLine("Not found");
                }
            }
        }

        static void DisplayContacts()
        {
            using (var connection = new MySqlConnection(connString))
            {
                connection.Open();
                string sql = "SELECT Name, Phone FROM Contacts";
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("Address Book");
                Console.WriteLine("Contact Name\tPhone number");

                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Name"]}\t{reader["Phone"]}");
                }
            }
        }
    }
}
