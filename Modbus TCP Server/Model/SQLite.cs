﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Modbus_TCP_Server.Model
{
    static class SQLite
    {
        public static void create_db(string databaseName)
        {
           
                Data.database_name = databaseName;

            if (!File.Exists(databaseName) && databaseName!="" && databaseName!= null)
            {
                SQLiteConnection.CreateFile(databaseName);
                MessageBox.Show(File.Exists(databaseName) ? "База данных создана" : "Возникла ошибка при создании базы данных");
            }
           

        }
        public static void create_table(string str)
        {


            //string databaseName = @"D:\cyber1.db";

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Data.database_name));
            SQLiteCommand command = new SQLiteCommand(str, connection);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public static void insert_parametrs(string parametr, string value)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Data.database_name));
            connection.Open();


            SQLiteCommand command = new SQLiteCommand("INSERT INTO  Parametrs (parametr, value)VALUES('" + parametr + "', '" + value + "');", connection);

            command.ExecuteNonQuery();
            connection.Close();
        }
        public static void insert_connects(string name, string ip, int port)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Data.database_name));
            connection.Open();


            SQLiteCommand command = new SQLiteCommand("INSERT INTO  Connects (name, ip, port)VALUES('" + name + "', '" + ip + "', '" + port + "');", connection);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void insert_tags(string name, int registr_number, int tag_number, string type, string connect)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Data.database_name));
            connection.Open();


            SQLiteCommand command = new SQLiteCommand("INSERT INTO  Tags (name, registr_number, tag_number, type, connect)VALUES('" + name + "', '" + registr_number + "', '" + tag_number + "', '" + type + "', '" + connect + "');", connection);

            command.ExecuteNonQuery();
            connection.Close();
        }
        public static void insert_write_tags(string name, int registr_number, int tag_number, string type, string connect)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Data.database_name));
            connection.Open();


            SQLiteCommand command = new SQLiteCommand("INSERT INTO  Tags_write (name, registr_number, tag_number, type, connect)VALUES('" + name + "', '" + registr_number + "', '" + tag_number + "', '" + type + "', '" + connect + "');", connection);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void sel_query_tags()
        {

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Data.database_name));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT name, registr_number, tag_number, type, connect FROM  Tags ;", connection);

            var result = command.ExecuteReader();
            Data.tags.Clear();
            while (result.Read())
            {
                string name = result["name"].ToString();
                int registr_number =Convert.ToInt32( result["registr_number"]);
                int tag_number = Convert.ToInt32(result["tag_number"]);
                string type = result["type"].ToString();
                string connect = result["connect"].ToString();
                Data.tags.Add(new Tag(name, registr_number, tag_number, type, connect));

            }
            connection.Close();
        }
        public static void sel_query_tags_write()
        {

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Data.database_name));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT name, registr_number, tag_number, type, connect FROM  Tags_write ;", connection);

            var result = command.ExecuteReader();
            Data.tags_write.Clear();
            while (result.Read())
            {
                string name = result["name"].ToString();
                int registr_number = Convert.ToInt32(result["registr_number"]);
                int tag_number = Convert.ToInt32(result["tag_number"]);
                string type = result["type"].ToString();
                string connect = result["connect"].ToString();
                Data.tags_write.Add(new Tag_write(name, registr_number, tag_number, type, connect));

            }
            connection.Close();
        }
        public static void sel_query_parametr()
        {

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Data.database_name));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT parametr, value  FROM  Parametrs ;", connection);

            var result = command.ExecuteReader();
            result.Read();
            Data.ip = result["value"].ToString();
            result.Read();
            Data.port = Convert.ToInt32(result["value"]);
        
            connection.Close();
        }

        public static void sel_query_connects()
        {

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Data.database_name));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT name, ip, port FROM Connects ;", connection);

            var result = command.ExecuteReader();
            Data.connects.Clear();
            Data.write_connects.Clear();
            while (result.Read())
            {
                string name = result["name"].ToString();
                string ip = result["ip"].ToString();
                int port = Convert.ToInt32(result["port"]);
                Data.connects.Add(name, new Data.Connect(ip,port));
                Data.write_connects.Add(name, new Data.Connect(ip, port));
            }
            connection.Close();
        }

    }
}
