using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;
namespace Modbus_TCP_Server.Model
{
    static class Data
    {
        public static List<Tag> tags = new List<Tag>();
        public static List<Tag_write> tags_write = new List<Tag_write>();
        public static Dictionary<string,Connect> connects= new Dictionary<string, Connect>();
        public static Dictionary<string, Connect> write_connects = new Dictionary<string, Connect>();

        public static string ip;
        public static int port;
        public static int time;
        public static ModbusClient modbusClient;
        public static ModbusClient modbusClient1;
        public static string database_name;
        public static class Base
        {
            public static string server;
            public static string user;
            public static string database;
            public static int port;
            public static string password;
        }
        public class Connect
        {
            public Connect(string ip, int port)
            {
                this.ip = ip;
                this.port = port;
                try
                {
                   modbusClient = new ModbusClient(ip, port);
                   modbusClient.Connect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            public ModbusClient modbusClient;
            public string ip;
            public int port;
        }
    }
}
