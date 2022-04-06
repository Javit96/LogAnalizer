using LogAudit.Models;
using System.Linq;

namespace LogAudit
{
    public partial class Form1 : Form
    {
        
        public List<Syslogd> logs= new List<Syslogd>();
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

        /*when the user, push the button "load logs", the program creates another thread 
        with the function of querying to the database the logs of an specific date*/
        private void button1_Click(object sender, EventArgs e)
        {
            
            selDay = dateTimePicker1.Value;
            ThreadRead databaseRead = new ThreadRead(_context: kiwiSyslogContext, _selDay: selDay);
            databaseRead.Callback += CallbackChangeMessage;
            try
            {
                //Start thread
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
                //Stop Thread
                databaseRead.Stop();
            }

        }
        
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //When this button is pushed, another thread is created to filter the logs and search for success/failure logon 
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            MessageBox.Show("Empezado a auditar");
            ThreadAudits threadAudits = new ThreadAudits(_logs: logs, _selDay:selDay);
            threadAudits.Callback += CallbackChangeMessage1;

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
        }

        private void CallbackChangeMessage1(object sender, ThreadAuditsResponse response)
        {
            log = response.Message;
            dataGridView1.Rows.Add(log.MsgDate, log.MsgTime, log.MsgPriority, log.MsgHostname, log.MsgText);
        }


    } 
}