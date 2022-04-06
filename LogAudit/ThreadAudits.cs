using LogAudit.Models;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;

namespace LogAudit
{
    internal class ThreadAudits
    {
        // Bandera booleana que indica cuando el proceso está siendo ejecutado o ha sido detenido
        private bool HeavyProcessStopped;

        // Expone el contexto de sincronización en la clase entera 
        private readonly SynchronizationContext SyncContext;

        // Crear los 2 contenedores de callbacks
        public event EventHandler<ThreadAuditsResponse> Callback;
        public event EventHandler<ThreadCountResponse> Callback1;

        Excel.Application xlApp = new Excel.Application();



        public List<Syslogd> logs;
        public Syslogd log;
        public List<Syslogd> successLogIn = new List<Syslogd>();
        public List<Syslogd> failureLogIn = new List<Syslogd>();
        KiwiSyslogContext kiwiSyslogContext;
        DateTime selDay;
        // Constructor de la clase HeavyTask
        public ThreadAudits(List<Syslogd> _logs, DateTime _selDay)
        {
            logs = _logs;
            selDay = _selDay;
            // Importante actualizar el valor de SyncContext en el constructor con
            // el valor de SynchronizationContext del AsyncOperationManager
            SyncContext = AsyncOperationManager.SynchronizationContext;
        }

        // Método para iniciar el proceso
        public void Start()
        {
            Thread thread = new Thread(Run1);
            thread.IsBackground = true;
            thread.Start();
        }

        // Método para detener el proceso
        public void Stop()
        {
            HeavyProcessStopped = true;

        }

        // Método donde la lógica principal de tu tarea se ejecuta
        private void Run1()
        {
            while (!HeavyProcessStopped)
            {
                // En nuestro ejemplo solo esperaremos 2 segundos y eso es todo
                // En tu clase obviamente se ejecutará la tarea pesada
                //Thread.Sleep(2000);

                // Ejecuta el primer callback desde el proceso de fondo al hilo principal (el de la interfaz gráfica)
                // El primer callback activa el primer boton !
                try
                {
                    Parallel.ForEach(logs, log =>
                    {
                        if (Regex.IsMatch(log.MsgText, "	4624	") ||
                        Regex.IsMatch(log.MsgText, "	4801	"))
                        {
                            if (Regex.IsMatch(log.MsgText, "		2") ||
                            Regex.IsMatch(log.MsgText, "		7") ||
                            Regex.IsMatch(log.MsgText, "		11"))
                            {
                                successLogIn.Add(log);
                            }
                        }
                        if (Regex.IsMatch(log.MsgText, "	4625	"))
                        {
                            if (Regex.IsMatch(log.MsgText, "		2") ||
                            Regex.IsMatch(log.MsgText, "		7"))
                            {
                                failureLogIn.Add(log);
                            }
                        }
                    });
                    
                   // SyncContext.Post(e => triggerCallback2(new ThreadCountResponse(successLogIn.Count)), null);
                    Parallel.ForEach(successLogIn, logs =>
                    {
                        SyncContext.Post(e => triggerCallback1(new ThreadAuditsResponse(logs)), null);
                        Thread.Sleep(10);
                    });
                    //SyncContext.Post(e => triggerCallback2(new ThreadCountResponse(failureLogIn.Count)), null);
                    Thread.Sleep(10);
                    Parallel.ForEach(failureLogIn, logs =>
                    {
                        SyncContext.Post(e => triggerCallback1(new ThreadAuditsResponse(logs)), null);
                        Thread.Sleep(10);
                    });

                    MessageBox.Show("Termine\nHubieron: \n" +
                        successLogIn.Count + " Intentos exitosos de logeo\n"
                        + failureLogIn.Count + " Intentos fallidos de logeo");
                
                    if(xlApp == null)
                    {
                        MessageBox.Show("Excel no esta instalado correctamente");
                            return;
                    }
                    //var xlWorkBook = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                    Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Jmoreno\Documents\Auditoria Semanal.xlsx");
                    var successSheet = (Excel.Worksheet)xlWorkbook.Sheets[1];
                    string cellName;
                    int counter = 5;

                    var range = successSheet.get_Range("A2");
                    range.Value = "Semana del " + selDay.Date.ToString("dd/MM/yy") +" al "+ selDay.AddDays(7).Date.ToString("dd/MM/yy");  
                    
                    foreach (var item in successLogIn)
                    {
                        cellName = "A" + counter.ToString();
                        range = successSheet.get_Range(cellName, cellName);
                        range.Value2 = item.MsgDate.ToString();
                        cellName = "B" + counter.ToString();
                        range = successSheet.get_Range(cellName, cellName);
                        range.Value2 = item.MsgTime.ToString();
                        cellName = "C" + counter.ToString();
                        range = successSheet.get_Range(cellName, cellName);
                        range.Value2 = item.MsgHostname.ToString();
                        cellName = "D" + counter.ToString();
                        range = successSheet.get_Range(cellName, cellName);
                        range.Value2 = item.MsgText.ToString();
                        counter ++;
                    }



                    var failure = (Excel.Worksheet)xlWorkbook.Sheets[2];
                    counter = 5;

                    foreach (var item in failureLogIn)
                    {
                        cellName = "A" + counter.ToString();
                        range = failure.get_Range(cellName, cellName);
                        range.Value2 = item.MsgDate.ToString();
                        cellName = "B" + counter.ToString();
                        range = failure.get_Range(cellName, cellName);
                        range.Value2 = item.MsgTime.ToString();
                        cellName = "C" + counter.ToString();
                        range = failure.get_Range(cellName, cellName);
                        range.Value2 = item.MsgHostname.ToString();
                        cellName = "D" + counter.ToString();
                        range = failure.get_Range(cellName, cellName);
                        range.Value2 = item.MsgText.ToString();
                        counter++;
                    }

                    string path = @"C:\";
                    xlWorkbook.SaveAs2("Auditoria Semanal "+selDay.Date.ToString("ddMMyy")+ 
                        ".xlsx");
                    xlApp.Workbooks.Close();
                    xlApp.Quit();
                    MessageBox.Show("Termine");
                }
                catch (Exception) { }

                /* // Esperar otros 2 segundos para más tareas pesadas.
                 Thread.Sleep(2000);

                 // Ejecutar segundo callback desde el segundo proceso al primero
                 
                */
                // La tarea heavy task finaliza, así que hay que detenerla.
                Stop();
            }
        }


        // Métodos que ejecutan los callback si y solo si fueron declarados durante la instanciación de la clase HeavyTask
        private void triggerCallback1(ThreadAuditsResponse response)
        {

            // Si el primer callback existe, ejecutarlo con la información dada
            Callback?.Invoke(this, response);
        }
        private void triggerCallback2(ThreadCountResponse response)
        {

            // Si el primer callback existe, ejecutarlo con la información dada
            Callback1?.Invoke(this, response);
        }



    }

    public class ThreadAuditsResponse
    {
        private readonly Syslogd message;

        public ThreadAuditsResponse(Syslogd logs)
        {
            this.message = logs;
        }

        public Syslogd Message { get { return message; } }
    }
    

    public class ThreadCountResponse
    {
        private readonly int cantidad;

        public ThreadCountResponse(int _cantidad)
        {
            this.cantidad = _cantidad;
        }

        public int Cantidad { get { return cantidad; } }
    }


}