using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using EasyModbus;

using EasyModbus.Exceptions;
using System.Windows.Forms;

namespace Modbus_TCP_Server.Model
{
    
    class Modbus
    {
       
        private Thread thread;
        public ModbusClient modbusClient;
        public List<Tag> tags;
        public DataGridView dataGridView;
        public ProgressBar progressBar;
        public void connect(string ip,int port)
        {
            try
            {
                Data.modbusClient = new ModbusClient(ip, port);

                Data.modbusClient1 = new ModbusClient(ip, port);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                Data.modbusClient.Connect();
                Data.modbusClient1.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void all_tag()
        {

        }
        private void read()
        {
            for (int i = 0; i < Data.tags.Count; i++)
            {
                Data.tags[i].modbusClient = Data.connects[Data.tags[i].connect].modbusClient;
            }
            while (true)
            {
               
                   
                    Action action = () => { progressBar.Value = 0; };
                    progressBar.Invoke(action);
                    for (int i = 0; i < Data.tags.Count; i++)
                    {
                        Data.tags[i].read();
                    }

                    action = () => { progressBar.Value = 100; };
                    progressBar.Invoke(action);
             
                    Thread.Sleep(100);

            }
        }


        public void start(DataGridView dataGridView, ProgressBar progressBar)
        {
            Tag.dataGrid = dataGridView;
            this.progressBar = progressBar;
           
                if (thread == null)
                {
                    thread = new Thread(new ThreadStart(read));
                    thread.Start();
                }
            
         
               
        }
        public void stop()
        {
            if(thread!=null)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                    thread = null;
                }
                    
            }
            
        }
        public void write(DataGridView dataGridView)
        {
           
            for (int i = 0; i < Data.tags_write.Count; i++)
            {
                Data.tags_write[i].modbusClient = Data.write_connects[Data.tags_write[i].connect].modbusClient;
            }

            for (int i = 0; i < Data.tags_write.Count; i++)
            {
                
                Data.tags_write[i].write(dataGridView);
            }
                           
        }
        public void write_tags_connect()
        {

            for (int i = 0; i < Data.tags_write.Count; i++)
            {
               
                Data.tags_write[i].modbusClient = Data.write_connects[Data.tags_write[i].connect].modbusClient;
            }         

        }

    }
    abstract class Tags
    {
        public string name;
        public int registr_number;
        public int tag_number;
        public string type;
        public string connect;

        public ModbusClient modbusClient;
        public static DataGridView dataGrid;
        public static ProgressBar progressBar;

        

    }
    class Tag: Tags
    {

        private int[] reg;
        private bool[] reg_bool;
        public object num;
        public Tag(string name, int registr_number, int tag_number, string type, string connect)
        {
            this.name = name;
            this.registr_number = registr_number;
            this.tag_number = tag_number;
            this.type = type;
            this.connect = connect;
        }
        public void read()
        {
            
            try
            {
                if (type == "WORD HOLDING")
                {
                    reg = modbusClient.ReadHoldingRegisters(registr_number, 1);
                    num = reg[0];
                }
                if (type == "INT HOLDING")
                {
                    reg = modbusClient.ReadHoldingRegisters(registr_number, 2);
                    num = ModbusClient.ConvertRegistersToInt(reg);
                }
                if (type == "REAL HOLDING")
                {
                    reg = modbusClient.ReadHoldingRegisters(registr_number, 2);
                    num = ModbusClient.ConvertRegistersToFloat(reg);
                }

                if (type == "WORD INPUT")
                {
                    reg = modbusClient.ReadInputRegisters(registr_number, 1);
                    num = reg[0];
                }
                if (type == "INT INPUT")
                {
                    reg = modbusClient.ReadInputRegisters(registr_number, 2);
                    num = ModbusClient.ConvertRegistersToInt(reg);
                }
                if (type == "REAL INPUT")
                {
                    reg = modbusClient.ReadInputRegisters(registr_number, 2);
                    num = ModbusClient.ConvertRegistersToFloat(reg);
                }

                if (type == "BOOL COIL")
                {
                    reg_bool = modbusClient.ReadCoils(registr_number, 1);
                    num = reg_bool[0];
                }
                if (type == "BOOL INPUT")
                {
                    reg_bool = modbusClient.ReadDiscreteInputs(registr_number, 1);
                    num = reg_bool[0];
                }
                Action action = () =>
                {
                    dataGrid.Rows[tag_number].Cells["value"].Value = num.ToString();
                    
                };
                dataGrid.Invoke(action);
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

    }
    class Tag_write:Tags
    {
        public Tag_write(string name, int registr_number, int tag_number, string type, string connect)
        {
            this.name = name;
            this.registr_number = registr_number;
            this.tag_number = tag_number;
            this.type = type;
            this.connect = connect;
        }
        public void write(string arg)
        {
            
            try
            {
                if (type == "WORD")
                {
                    
                    modbusClient.WriteSingleRegister(registr_number, Convert.ToInt32(arg));
                }
                    
                if (type == "BOOL")
                {
                    
                    if(arg=="true" || arg=="TRUE" || arg=="1")
                        modbusClient.WriteSingleCoil(registr_number, true);
                    if (arg == "false" || arg == "FALSE" || arg == "0")
                        modbusClient.WriteSingleCoil(registr_number, false);
                }
                                                      
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void write(DataGridView dataGridView)
        {

            try
            {
                if (type == "WORD")
                {
                    string arg = dataGridView.Rows[tag_number].Cells["value_write"].Value.ToString();
                    modbusClient.WriteSingleRegister(registr_number, Convert.ToInt32(arg));
                }

                if (type == "BOOL")
                {
                    string arg = dataGridView.Rows[tag_number].Cells["value_write"].Value.ToString();
                    if (arg == "true" || arg == "TRUE" || arg == "1")
                        modbusClient.WriteSingleCoil(registr_number, true);
                    if (arg == "false" || arg == "FALSE" || arg == "0")
                        modbusClient.WriteSingleCoil(registr_number, false);
                }

            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            string arg1 = dataGridView.Rows[tag_number].Cells["value_write"].Value.ToString();
            MessageBox.Show(arg1);

        }
    }

}

