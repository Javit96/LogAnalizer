using LogAudit.Models;
using System.Linq;

namespace LogAudit
{
    public partial class Form1 : Form
    {
        
        public List<Syslogd> logs= new List<Syslogd>();
        public List<Syslogd> totalLogs = new List<Syslogd>();
        public Syslogd log;
        public int cant_log;
        DateTime selDay;

        private readonly KiwiSyslogContext kiwiSyslogContext;
        public Form1(KiwiSyslogContext context)
        {
            kiwiSyslogContext = context;
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            //progressBar1.Value = progressBar1.Minimum;
            selDay = dateTimePicker1.Value;
            ThreadRead databaseRead = new ThreadRead(_context: kiwiSyslogContext, _selDay: selDay);
            databaseRead.Callback += CallbackChangeMessage;
            //databaseRead.Callback2 += CallbackChangeMessage1;
            try
            {
                databaseRead.Start();

            }
            catch (Exception ex)
            {
                //Log technical exception 
                MessageBox.Show(ex.Message);
                //Return exception repsponse here
                throw;

            }
            if (dataGridView1.Rows.Count == cant_log)
            {
                databaseRead.Stop();
            }

        }
        
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            MessageBox.Show("Empezado a auditar");
            ThreadAudits threadAudits = new ThreadAudits(_logs: logs, _selDay:selDay);
            threadAudits.Callback += CallbackChangeMessage1;
           // threadAudits.Callback1 += CallbackChangeMessage2;

            try
            {
                threadAudits.Start();

            }
            catch (Exception ex)
            {
                //Log technical exception 
                MessageBox.Show(ex.Message);
                //Return exception repsponse here
                throw;

            }

            
        }

        private void CallbackChangeMessage(object sender, ThreadWriteResponse response)
        {
           foreach(var log in response.Logs)
            {
                logs.Add(log);
            }
            MessageBox.Show("Termino la carga de logs");
            /*cant_log = response.Logs.Count;
            int pbValue = cant_log / 100;
            progressBar1.Maximum = cant_log;
            progressBar1.Step = pbValue;
            progressBar1.Visible = true;*/

        }

        private void CallbackChangeMessage1(object sender, ThreadAuditsResponse response)
        {
            log = response.Message;
            //richTextBox1.AppendText(response.Message);
            dataGridView1.Rows.Add(log.MsgDate, log.MsgTime, log.MsgPriority, log.MsgHostname, log.MsgText);

            /*if (dataGridView1.Rows.Count % progressBar1.Step == 0)
            {
                progressBar1.PerformStep();
                progressBar1.Refresh();
            }
            if (dataGridView1.Rows.Count == cant_log)
            {
                progressBar1.Value = 0;
                progressBar1.Visible = false;
            }*/




        }
        /*private void CallbackChangeMessage2(object sender, ThreadCountResponse response)
        {
            cant_log = response.Cantidad;
            int pbValue = cant_log / 100;
            progressBar1.Maximum = cant_log;
            progressBar1.Step = pbValue;
            progressBar1.Visible = true;

        }*/


    } 
}