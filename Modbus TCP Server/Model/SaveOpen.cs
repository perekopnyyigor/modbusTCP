using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Modbus_TCP_Server.Model
{
    class SaveOpen
    {
        public void create_file()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "db file (*.db)|*.db";
            saveFileDialog.ShowDialog();
            if(saveFileDialog.FileName != "" && saveFileDialog.FileName != null)
                SQLite.create_db(saveFileDialog.FileName);
           
        }

        public void save_parametr()
        {
            string str = "DROP TABLE IF EXISTS Parametrs; ";
            SQLite.create_table(str);
            str = "CREATE TABLE  IF NOT EXISTS  Parametrs (parametr TEXT, value TEXT);";
            SQLite.create_table(str);
            SQLite.insert_parametrs("ip", Data.ip);
            SQLite.insert_parametrs("port", Data.port.ToString());
        }
        public void save_connections()
        {
            string str = "DROP TABLE IF EXISTS Connects; ";
            SQLite.create_table(str);
            str = "CREATE TABLE  IF NOT EXISTS  Connects (name TEXT, ip TEXT, port INT);";
            SQLite.create_table(str);
            foreach (string name in Data.connects.Keys)
            {
                string ip = Data.connects[name].ip;
                int port = Data.connects[name].port;

                SQLite.insert_connects(name, ip, port);
            }
        }
        public void save_tags()
        {
            string str = "DROP TABLE IF EXISTS Tags; ";
            SQLite.create_table(str);
            str = "CREATE TABLE  IF NOT EXISTS  Tags (name TEXT, registr_number INT, tag_number INT, type TEXT, connect TEXT);";
            SQLite.create_table(str);
            for (int i = 0; i < Data.tags.Count; i++)
            {
                string name = Data.tags[i].name;
                int registr_number = Data.tags[i].registr_number;
                int tag_number = Data.tags[i].tag_number;
                string type = Data.tags[i].type;
                string connect = Data.tags[i].connect;
                SQLite.insert_tags(name, registr_number, tag_number, type, connect);
            }
        }
        public void save_write_tags()
        {
            string str = "DROP TABLE IF EXISTS Tags_write; ";
            SQLite.create_table(str);
            str = "CREATE TABLE  IF NOT EXISTS  Tags_write (name TEXT, registr_number INT, tag_number INT, type TEXT, connect TEXT);";
            SQLite.create_table(str);
            for (int i = 0; i < Data.tags_write.Count; i++)
            {
                string name = Data.tags_write[i].name;
                int registr_number = Data.tags_write[i].registr_number;
                int tag_number = Data.tags_write[i].tag_number;
                string type = Data.tags_write[i].type;
                string connect = Data.tags_write[i].connect;
                SQLite.insert_write_tags(name, registr_number, tag_number, type, connect);
            }
        }
        public void open_file()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            Data.database_name=openFileDialog.FileName;
            if(Data.database_name!="")
            {
                SQLite.sel_query_connects();
                SQLite.sel_query_tags();
                SQLite.sel_query_tags_write();
                SQLite.sel_query_parametr();
            }
            
        }

    }
}
