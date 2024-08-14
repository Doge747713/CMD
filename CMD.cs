using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace SystemScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear(); // Clear the screen
                DisplayMenu(); // Display the menu options

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayCpuInfo();
                        break;
                    case "2":
                        DisplayMemoryInfo();
                        break;
                    case "3":
                        DisplayDiskInfo();
                        break;
                    case "4":
                        DisplayNetworkInfo();
                        break;
                    case "5":
                        DisplayBatteryInfo();
                        break;
                    case "6":
                        PerformCpuOverloadTest();
                        break;
                    case "7":
                        DisplayGpuInfo();
                        break;
                    case "8":
                        PerformGpuOverloadTest();
                        break;
                    case "9":
                        CheckSystemUptime();
                        break;
                    case "10":
                        GetInstalledSoftwareList();
                        break;
                    case "11":
                        DisplaySystemDateTime();
                        break;
                    case "12":
                        CheckDiskSpaceUsage();
                        break;
                    case "13":
                        ViewRunningProcesses();
                        break;
                    case "14":
                        ControlFanSpeed();
                        break;
                    case "15":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice, please try again.");
                        Console.ResetColor();
                        break;
                }

                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }

        static void DisplayMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("====== SYSTEM TEST MENU ==========");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[1] View CPU Info");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[2] View Memory Info");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[3] View Disk Info");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("[4] View Network Info");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("[5] View Battery Info");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[6] CPU Overload Test");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("[7] View GPU Info");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[8] GPU Overload Test");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[9] Check System Uptime");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[10] Get Installed Software List");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[11] Display System Date and Time");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("[12] Check Disk Space Usage");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("[13] View Running Processes");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[14] Control Fan Speed");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[15] Exit");
            Console.ResetColor();
            Console.Write("\nEnter your choice: ");
        }

        static (string status, double charge, string estimatedTime, string batteryType) GetBatteryInfo()
        {
            using (var searcher = new ManagementObjectSearcher("select * from Win32_Battery"))
            {
                foreach (var battery in searcher.Get())
                {
                    string status = battery["Status"]?.ToString() ?? "Unknown";
                    double charge = double.TryParse(battery["EstimatedChargeRemaining"]?.ToString(), out double result) ? result : 0;
                    string estimatedTime = battery["EstimatedRunTime"] != null ? $"{battery["EstimatedRunTime"]} minutes" : "N/A";
                    string batteryType = battery["BatteryType"]?.ToString() ?? "Unknown";

                    return (status, charge, estimatedTime, batteryType);
                }
            }
            return ("N/A", 0, "N/A", "N/A");
        }

        static void DisplayBatteryInfo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Battery Information:");
            Console.ResetColor();
            var batteryInfo = GetBatteryInfo();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Battery Status: {batteryInfo.status}");
            Console.WriteLine($"Charge Percentage: {batteryInfo.charge}%");
            Console.WriteLine($"Estimated Remaining Time: {batteryInfo.estimatedTime}");
            Console.WriteLine($"Battery Type: {batteryInfo.batteryType}");
            Console.ResetColor();
        }

        static void DisplayCpuInfo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("CPU Information:");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Processor: {GetProcessorInfo()}");
            Console.WriteLine($"Cores: {Environment.ProcessorCount}");
            Console.WriteLine($"CPU Usage: {GetCpuUsage()}%");
            Console.ResetColor();
        }

        static void DisplayMemoryInfo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Memory Information:");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Total Physical Memory: {GetTotalPhysicalMemory()} GB");
            Console.WriteLine($"Available Physical Memory: {GetAvailablePhysicalMemory()} GB");
            Console.ResetColor();
        }

        static void DisplayDiskInfo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Disk Information:");
            Console.ResetColor();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Drive {drive.Name}:");
                    Console.WriteLine($"  Total Size: {drive.TotalSize / (1024 * 1024 * 1024)} GB");
                    Console.WriteLine($"  Free Space: {drive.TotalFreeSpace / (1024 * 1024 * 1024)} GB");
                    Console.WriteLine($"  Used Space: {(drive.TotalSize - drive.TotalFreeSpace) / (1024 * 1024 * 1024)} GB");
                    Console.ResetColor();
                }
            }
        }

        static void DisplayNetworkInfo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Network Information:");
            Console.ResetColor();
            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Adapter: {adapter.Name}");
                    Console.WriteLine($"  Status: {adapter.OperationalStatus}");
                    Console.WriteLine($"  Speed: {adapter.Speed / (1024 * 1024)} Mbps");
                    Console.WriteLine($"  IP Address: {adapter.GetIPProperties().UnicastAddresses.FirstOrDefault()?.Address}");
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
        }

        static void DisplayGpuInfo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("GPU Information:");
            Console.ResetColor();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
            {
                foreach (var gpu in searcher.Get())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Name: {gpu["Name"]}");
                    Console.WriteLine($"Adapter RAM: {Math.Round(Convert.ToDouble(gpu["AdapterRAM"]) / (1024 * 1024 * 1024), 2)} GB");
                    Console.WriteLine($"Driver Version: {gpu["DriverVersion"]}");
                    Console.WriteLine($"Video Processor: {gpu["VideoProcessor"]}");
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
        }

        static void PerformCpuOverloadTest()
        {
            Console.Clear();
            Console.Write("Enter duration (in seconds): ");
            if (int.TryParse(Console.ReadLine(), out int duration))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Performing CPU overload test for {duration} seconds...");
                Console.ResetColor();
                Task.Run(() => CpuOverloadTest(duration)).ContinueWith(t =>
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nCPU Load Test Completed.");
                    Console.ResetColor();
                });
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Please enter a number.");
                Console.ResetColor();
            }
        }

        static async Task CpuOverloadTest(int durationInSeconds)
        {
            var tasks = Enumerable.Range(0, Environment.ProcessorCount).Select(_ => Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                while (sw.Elapsed.TotalSeconds < durationInSeconds)
                {
                    // Perform a CPU-intensive operation
                    Math.Sqrt(12345);
                }
            })).ToArray();

            await Task.WhenAll(tasks);
        }

        static void CheckSystemUptime()
        {
            Console.Clear();
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"System Uptime: {uptime.Days} days, {uptime.Hours} hours, {uptime.Minutes} minutes.");
            Console.ResetColor();
        }

        static void GetInstalledSoftwareList()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Installed Software List:");
            Console.ResetColor();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Product"))
            {
                foreach (var item in searcher.Get())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(item["Name"]);
                }
            }
        }

        static void DisplaySystemDateTime()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Current Date and Time: {DateTime.Now}");
            Console.ResetColor();
        }

        static void CheckDiskSpaceUsage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Disk Space Usage:");
            Console.ResetColor();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    Console.WriteLine($"Drive {drive.Name}:");
                    Console.WriteLine($"  Total Size: {drive.TotalSize / (1024 * 1024 * 1024)} GB");
                    Console.WriteLine($"  Free Space: {drive.TotalFreeSpace / (1024 * 1024 * 1024)} GB");
                    Console.WriteLine($"  Used Space: {(drive.TotalSize - drive.TotalFreeSpace) / (1024 * 1024 * 1024)} GB");
                }
            }
        }

        static void ViewRunningProcesses()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Running Processes:");
            Console.ResetColor();
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Name: {process.ProcessName}, ID: {process.Id}");
            }
        }

        static void ControlFanSpeed()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Fan speed control is not implemented yet.");
            Console.ResetColor();
        }

        static void PerformGpuOverloadTest()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("GPU overload test is not implemented yet.");
            Console.ResetColor();
        }

        static string GetProcessorInfo()
        {
            string info = "";
            using (var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor"))
            {
                foreach (var item in searcher.Get())
                {
                    info = item["Name"]?.ToString();
                }
            }
            return info;
        }

        static double GetCpuUsage()
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            return Math.Round(cpuCounter.NextValue(), 2);
        }

        static double GetTotalPhysicalMemory()
        {
            double totalMemory = 0;
            using (var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory"))
            {
                foreach (var item in searcher.Get())
                {
                    totalMemory += Convert.ToDouble(item["Capacity"]);
                }
            }
            return Math.Round(totalMemory / (1024 * 1024 * 1024), 2); // Convert bytes to GB
        }

        static double GetAvailablePhysicalMemory()
        {
            var searcher = new ManagementObjectSearcher("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
            foreach (var item in searcher.Get())
            {
                return Math.Round(Convert.ToDouble(item["FreePhysicalMemory"]) / 1024, 2); // Convert KB to GB
            }
            return 0;
        }
    }
}
