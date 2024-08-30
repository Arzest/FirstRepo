using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgTwo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Xdik");
            ComputerClub computerClub = new ComputerClub(9);
            computerClub.Work();
        }
    }
    class ComputerClub
    {
        private int _money = 0;
        private List<Computer> _computers = new List<Computer>();
        private Queue<Client> _clients = new Queue<Client>();
        public ComputerClub(int computersCount)
        {
            Random rand = new Random();
            for (int i = 0; i < computersCount; i++)
            {
                _computers.Add(new Computer(rand.Next(5, 10)));
                CreateNewClient(18, rand);
            }
        }
        public void CreateNewClient(int count, Random rand)
        {
            for (int i = 0; i < count; i++)
            {
                _clients.Enqueue(new Client(rand.Next(100, 150), rand));
            }
        }
        public void Work()
        {
            while (_clients.Count > 0)
            {
                Client NewClient = _clients.Dequeue();
                Console.WriteLine($"Баланс компьютерного клуба: {_money}. Ждем нового посетителя.");
                Console.WriteLine($"У вас появился новый клиент. Он желает преобрести {NewClient.DesiredMinutes} минут за компьютером.");
                ShowAllComputersState();
                Console.Write("Вы предлагаете ему компьютер под номером: ");
                string UserInput = Console.ReadLine();
                if (int.TryParse(UserInput, out int computerNumber))
                {
                    computerNumber -= 1;
                    if (computerNumber >= 0 && computerNumber < _computers.Count)
                    {
                        if (_computers[computerNumber].IsTaken)
                        {
                            Console.WriteLine("Вы направили клиента к занятому компьютеру. Он разозлился и ушел.");
                        }
                        else
                        {
                            if (NewClient.CheckSolvency(_computers[computerNumber]))
                            {
                                Console.WriteLine($"Клиент пересчетал деньги. Оплатил место и сел за компьютер {computerNumber+1}");
                                _money += NewClient.Pay();
                                _computers[computerNumber].BecameTaken(NewClient);
                            }
                            else
                            {
                                Console.WriteLine("У клиента недостаточно средств. Он развернулся и ушел.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Вы не знаете за какой компьютер посадить клиента. Он подождал и ушел.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Повторите снова.");
                    CreateNewClient(1, new Random());
                }
                Console.ReadKey();
                Console.Clear();
                SpendOneMinute();
            }
        }
        public void ShowAllComputersState()
        {
            Console.WriteLine("\nCписок всех компьютеров:");
            for (int i = 0; i < _computers.Count; i++)
            {
                Console.Write(i + 1 + " - ");
                _computers[i].ShowState();
            }
        }
        private void SpendOneMinute()
        {
            foreach (var xd in _computers)
            {
                xd.SpendOneMinute();
            }
        }
    }
    class Computer 
    {
        private Client _client;
        private int _minuteRemaining;
        public bool IsTaken
        {
            get { return _minuteRemaining > 0; }
        }

        public int PriceForMinute { get; private set; }
        public Computer(int priceForMinute)
        {
            PriceForMinute = priceForMinute;
        }
        public void BecameTaken(Client client) 
        {
            _client = client;
            _minuteRemaining = client.DesiredMinutes;
        }
        public void BecameEmpty()
        {
            _client = null;
        }
        public void SpendOneMinute()
        {
            _minuteRemaining--;
        }
        public void ShowState() 
        {
            if (IsTaken)
            {
                Console.WriteLine($"Компьютер занят. Осталось минут: {_minuteRemaining}");
            }
            else 
            {
                Console.WriteLine($"Компьютер свободен. Цена за минуту: {PriceForMinute}");
            }
        }
    }
    class Client
    {
        private int _money;
        private int _moneyToPay;
        public int DesiredMinutes { get; private set; }
        public Client(int money, Random rand)
        {
            _money = money;
            DesiredMinutes = rand.Next(10, 40);
        }
        public bool CheckSolvency(Computer computer)
        {
            _moneyToPay = DesiredMinutes * computer.PriceForMinute;
            if (_money >= _moneyToPay)
            {
                return true;
            }
            else
            {
                _moneyToPay = 0;
                return false;
            }
        }
        public int Pay()
        {
            _money -= _moneyToPay;
            return _moneyToPay;
        }
    }
}
