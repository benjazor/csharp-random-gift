using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace RandomGift
{
    public class Client
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public virtual List<Item> Wishlist { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Client Client { get; set; }

    }

    public class DataContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Item> Items { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var database = new DataContext())
            {

                void MainMenu()
                {
                    Console.Clear();
                    Console.Write("\tMain Menu\n\n  1:\tClients Manager\n  2:\tAdd item to Wishlist\n  3:\tRandom Gift\n  4:\tExit\n\n What to do? ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            ClientsManager();
                            break;
                        case "2":
                            AddToWishlist();
                            break;
                        case "3":
                            RandomGift();
                            break;
                        case "4":
                            break;
                        default:
                            MainMenu();
                            break;
                    }
                }

                bool CheckClient(string id)
                {
                    if (Int32.TryParse(id, out var tmp))
                    {
                        return database.Clients.ToList().Exists(client => client.Id == tmp);
                    } else
                    {
                        return false;
                    }
                }

                void ClientsManager()
                {
                    Console.Clear();
                    Console.Write("\tClients Manager\n\n  1:\tList\n  2:\tSearch\n  3:\tAdd\n  4:\tEdit\n  5:\tRemove\n  6:\tMain menu\n\n What to do? ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            ListClient();
                            break;
                        case "2":
                            SearchClient();
                            break;
                        case "3":
                            AddClient();
                            break;
                        case "4":
                            EditClient();
                            break;
                        case "5":
                            RemoveClient();
                            break;
                        case "6":
                            MainMenu();
                            break;
                        default:
                            ClientsManager();
                            break;
                    }
                }

                void ListClient()
                {
                    Console.Clear();
                    Console.Write("\tClients List\n\n  ID : FULLNAME : PHONE : EMAIL\n");
                    var listclient = from client in database.Clients
                                orderby client.Id
                                select client;
                    foreach (var client in listclient)
                    {
                        Console.Write(" " + client.Id.ToString() + " : " + client.Fullname + " : " + client.Phone + " : " + client.Email +"\n");
                    }

                    Console.Write("\n  Action:\n  1:\tEdit\n  2:\tRemove\n  3:\tExit\n\n What to do? ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            EditClient();
                            break;
                        case "2":
                            RemoveClient();
                            break;
                        case "3":
                            ClientsManager();
                            break;
                        default:
                            ListClient();
                            break;
                    }
                }
                   
                void SearchClient()
                {
                    Console.Clear();
                    Console.Write("\tSearch Client\n\nEnter client's Id: ");
                    string tmp = Console.ReadLine();
                    if (CheckClient(tmp))
                    {
                        Console.Clear();
                        var tmp2 = database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp)));
                        Console.Write("\tSearch Client\n\n ID :\t\t" + tmp2.Id + "\n Name :\t\t" + tmp2.Fullname + "\n Phone :\t" + tmp2.Phone + "\n Email :\t" + tmp2.Email + "\n\n Wishlist :\n");
                        foreach(var item in tmp2.Wishlist)
                        {
                            Console.WriteLine("\t-  " + item.Name + " (" + item.Description + ")");
                        }
                        Console.WriteLine("\n\nPress any key to continue...");
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write("\tSearch Client\n\nClient not found, try with another Id!\n\nPress any key to continue...");
                    }
                    Console.ReadKey();
                    ClientsManager();
                }

                void AddClient()
                {
                    Console.Clear();
                    Console.Write("\tAdd New Client\n\nEnter client's fullname: ");
                    string fullname = Console.ReadLine();
                    Console.Clear();
                    Console.Write("\tAdd New Client\n\nEnter client's phone number: ");
                    string phone = Console.ReadLine();
                    Console.Clear();
                    Console.Write("\tAdd New Client\n\nEnter client's email adress: ");
                    string email = Console.ReadLine();
                    Console.Clear();
                    Console.Write("\tAdd New Client\n\nA new Client has been added (" + fullname + " " + phone + " " + email + ")\n\nPress any key to continue...");
                    // AJOUTER LE CLIENT DANS LA BDD
                    database.Clients.Add(new Client()
                    {
                        Fullname = fullname,
                        Phone = phone,
                        Email = email,
                        Wishlist = new List<Item>()
                    });
                    database.SaveChanges();
                    Console.ReadKey();
                    ClientsManager();
                }

                void RemoveClient()
                {
                    Console.Clear();
                    Console.Write("\tRemove Client\n\nEnter client's Id: ");
                    string tmp = Console.ReadLine();
                    if (CheckClient(tmp))
                    {
                        Console.Clear();
                        Console.Write("\tRemove Client\n\nConfirmation to remove " + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Id + " : " + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Fullname + " : " + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Phone + " : " + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Email + ": [Y/N]");
                        while (true)
                        {
                            string tmp2 = Console.ReadLine();
                            if (tmp2 == "Y")
                            {
                                Console.Clear();
                                var clientid = Int32.Parse(tmp);
                                var clientToRemove =
                                    from client in database.Clients
                                    where client.Id == clientid
                                    select client;

                                foreach (var client in clientToRemove)
                                {
                                    database.Clients.Remove(client);
                                }
                                Console.Write("\tRemove Client\n\nClient (" + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Id + " : " + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Fullname + " : " + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Phone + " : " + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Email + ") Removed!\n\nPress any key to continue...");
                                break;
                            }
                            else if (tmp2 == "N")
                            {
                                Console.Clear();
                                Console.Write("\tRemove Client\n\nAction canceled\n\nPress any key to continue...");
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write("\tRemove Client\n\nClient not found, try with another Id!\n\nPress any key to continue...");

                    }
                    Console.ReadKey();
                    database.SaveChanges();
                    ClientsManager();
                }

                void EditClient()
                {
                    Console.Clear();
                    Console.Write("\tEdit Client\n\nEnter client's Id: ");
                    var tmp = Console.ReadLine();
                    if (CheckClient(tmp)) // GET ID TO INT
                    {
                        Console.Clear();
                        var tmp2 = database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp)));
                        Console.Write("\tEdit Client\n\n  " + tmp2.Id + " : " + tmp2.Fullname + " : " + tmp2.Phone + " : " + tmp2.Email + "\n\n  1:\tEdit Name\n  2:\tEdit Phone\n  3:\tEdit Email\n  4:\tExit\n\n What to do? ");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                Console.Clear();
                                Console.Write("\tEdit Client\n\nEnter new name: ");
                                database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Fullname = Console.ReadLine();
                                Console.Clear();
                                Console.Write("\tEdit Client\n\nNew name set as " + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Fullname + "\n\nPress any key to continue...");
                                break;
                            case "2":
                                Console.Clear();
                                Console.Write("\tEdit Client\n\nEnter new phone number: ");
                                database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Phone = Console.ReadLine();
                                Console.Clear();
                                Console.Write("\tEdit Client\n\nNew phone number set as " + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Phone + "\n\nPress any key to continue...");
                                break;
                            case "3":
                                Console.Clear();
                                Console.Write("\tEdit Client\n\nEnter new email: ");
                                database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Email = Console.ReadLine();
                                Console.Clear();
                                Console.Write("\tEdit Client\n\nNew email set as " + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Email + "\n\nPress any key to continue...");
                                break;
                            case "4":
                                ClientsManager();
                                break;
                            default:
                                Console.Clear();
                                Console.Write("\tEdit Client\n\nOperation canceled!\n\nPress any key to continue...");
                                break;
                        }
                        database.SaveChanges();
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write("\tEdit Client\n\nClient not found, try with another Id!\n\nPress any key to continue...");
                    }
                    Console.ReadKey();
                    ClientsManager();
                }

                void ItemsManager()
                {
                    Console.Clear();
                    Console.Write("\tClients Manager\n\n  1:\tItems List\n  2:\tAdd new Item\n  3:\tEdit Item\n  4:\tRemove Item\n\n  5:\tMain menu\n\nWhat to do? ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            ItemsManager();
                            break;
                        case "2":
                            ItemsManager();
                            break;
                        case "3":
                            ItemsManager();
                            break;
                        case "4":
                            ItemsManager();
                            break;
                        case "5":
                            MainMenu();
                            break;
                        default:
                            ItemsManager();
                            break;
                    }
                }

                void RandomGift()
                {
                    Random rand = new Random();
                    var tmp = database.Clients.ToList();
                    var winner = rand.Next(0, tmp.Count);
                    var prize = rand.Next(0, tmp[winner].Wishlist.Count);

                    Console.Clear();
                    Console.Write("\tRandom Gift\n\nClient (" + tmp[winner].Id + " : " + tmp[winner].Fullname + " : " + tmp[winner].Phone + " : " + tmp[winner].Email + ") won a beautiful prize : " + tmp[winner].Wishlist[prize].Name + " (" + tmp[winner].Wishlist[prize].Description + ")" + "!!!\n\nPress any key to continue...");
                    Console.ReadKey();
                    MainMenu();
                }

                void AddToWishlist()
                {
                    Console.Clear();
                    Console.Write("\tAdd to Wishlist \n\nEnter client's Id: ");
                    var tmp = Console.ReadLine();
                    if (CheckClient(tmp))
                    {
                        Console.Clear();
                        Console.Write("\tAdd to Wishlist \n\nEnter item's Name: ");
                        var name = Console.ReadLine();
                        Console.Clear();
                        Console.Write("\tAdd to Wishlist \n\nEnter item's Description: ");
                        var description = Console.ReadLine();
                        database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Wishlist.Add(new Item() { Name = name, Description = description, Client = database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))) });
                        Console.Clear();
                        Console.Write("\tAdd to Wishlist \n\n" + name + " (" + description + ") has been added to " + database.Clients.ToList().Find(client => client.Id.Equals(Int32.Parse(tmp))).Fullname + "'s wishlist !");
                        Console.WriteLine("\n\nPress any key to continue...");
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write("\tAdd to Wishlist\n\nClient not found, try with another Id!\n\nPress any key to continue...");
                    }
                    Console.ReadKey();
                    MainMenu();
                }

                MainMenu();
                database.SaveChanges();
            }
        }
    }
}
