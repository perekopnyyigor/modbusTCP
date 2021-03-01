using Modbus_TCP_Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modbus_TCP_Server.Present
{
    class Connect_present
    {
        Modbus modbus = new Modbus();
        public void ini(DataGridView dataGridView)
        {
            int i = 0;
            foreach (string name in Data.connects.Keys)
            {
                dataGridView.Rows.Add(name, Data.connects[name].ip, Data.connects[name].port);
             
            }
        }

        public void connect(DataGridView dataGridView )
        {
            for(int i=0; i<dataGridView.Rows.Count-1; i++)
            {
                string name = dataGridView.Rows[i].Cells["name"].Value.ToString();              
                string ip = dataGridView.Rows[i].Cells["ip"].Value.ToString();
                int port = Convert.ToInt32(dataGridView.Rows[i].Cells["port"].Value.ToString());
                Data.connects.Add(name, new Data.Connect(ip,port));
                Data.write_connects.Add(name, new Data.Connect(ip, port));
            }
                
        }
    }
}
