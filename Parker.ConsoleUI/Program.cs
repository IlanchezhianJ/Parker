using Parker.ConsoleUI.ParkinService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parker.ConsoleUI
{
    class Program
    {
        static ParkingClient client = new ParkingClient();

        static void Main(string[] args)
        {
            int InputKey = 0;
            do
            {
                try
                {
                    InputKey = GetMainInput();
                    switch (InputKey)
                    {
                        case 1: GetAvailableList(); break;
                        case 2: Checkin(); break;
                        case 3: CheckOut();  break;
                        case 4: Environment.Exit(0); break;
                    }
                }
                catch (Exception E)
                {
                    Console.WriteLine(E.Message);
                }
            } while (true);
        }

        private static void Checkin()
        {
            Console.WriteLine("Please enter floor name: ");
            var floor = Console.ReadLine();

            Console.WriteLine("Please enter your vehicle type: (supported types are car, bike and van)");
            var vehicleType = Console.ReadLine();

            ParkingToken result = client.Checkin(vehicleType, floor, string.Empty);

            if (result.ParkingNumberk__BackingField > 0)
            {
                Console.WriteLine(string.Format("Your vehicle is successfully parked in {0}", result.ParkingNumberk__BackingField.ToString()));
            }
        }

        private static void CheckOut()
        {
            Console.WriteLine("Please enter floor name: ");
            var floor = Console.ReadLine();

            Console.WriteLine("Please enter your vehicle type: (supported types are car, bike and van)");
            var vehicleType = Console.ReadLine();

            if(client.Checkout(vehicleType, floor, string.Empty))
            {
                Console.WriteLine("Your vehicle is checked out successfully");
            }

        }

        private static void GetAvailableList()
        {
            Console.WriteLine("Please enter your vehicle type: (supported types are car, bike and van)");
            var result = client.GetAvailableFloors(Console.ReadLine());
            foreach (var item in result)
            {
                Console.WriteLine(item); 
            }
            
        }

        static int GetMainInput()
        {
            int inputKey = -1;

            Console.WriteLine("*********************************");
            Console.WriteLine("        Parking Service");
            Console.WriteLine("*********************************");
            Console.WriteLine("1. Get available floors");
            Console.WriteLine("2. Checkin vehicle");
            Console.WriteLine("3. Checkout vehicle");
            Console.WriteLine("4. Exit");
            Console.WriteLine("Please enter your input number: ");
            var input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                int key = 0;
                if (Int32.TryParse(input, out key))
                {
                    if (key > 0 && key < 4)
                        return key;
                    else
                    {
                        Console.WriteLine("Invalid input! please try again!");
                        GetMainInput();
                    }
                }
            }

            return inputKey;
        }
    }
}
