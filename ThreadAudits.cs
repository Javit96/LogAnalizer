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


        bool successLogon, failureLogon, userCreated, userEnabled,
            changePass, userDisabled, userDeleted,userLocked, 
            userUnlocked, userNameChanged, typeLogin;
        public List<Syslogd> logs;
        public List<Syslogd> successLogIn = new List<Syslogd>();
        public List<Syslogd> failureLogIn = new List<Syslogd>();
        public List<Syslogd> usersEnabled = new List<Syslogd>();
        public List<Syslogd> usersDisabled = new List<Syslogd>();
        public List<Syslogd> usersCreated = new List<Syslogd>();
        public List<Syslogd> usersDeleted = new List<Syslogd>();
        public List<Syslogd> usersLocked = new List<Syslogd>();
        public List<Syslogd> usersUnlocked = new List<Syslogd>();
        public List<Syslogd> usersNameChanged = new List<Syslogd>();
        public List<Syslogd> passwordChanged = new List<Syslogd>();
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
                    foreach (var log in logs)
                    {
                        addLogToList(log);
                    }

                    
                    Parallel.ForEach(successLogIn, logs =>
                    {
                        SyncContext.Post(e => triggerCallback1(new ThreadAuditsResponse(logs)), null);
                        Thread.Sleep(10);
                    });
                    Thread.Sleep(10);
                    Parallel.ForEach(failureLogIn, logs =>
                    {
                        SyncContext.Post(e => triggerCallback1(new ThreadAuditsResponse(logs)), null);
                        Thread.Sleep(10);
                    });

                    MessageBox.Show("Termine la auditoria\nHubieron: \n" +
                        successLogIn.Count + " Intentos exitosos de logeo\n" +
                        failureLogIn.Count + " Intentos Fallidos de logeo\n" +
                        usersCreated.Count + " Usuarios creados\n" +
                        usersEnabled.Count + " Usuarios habilitados\n" +
                        passwordChanged.Count + " Contraseñas cambiadas\n" +
                        usersDisabled.Count + " Usuarios deshabilitados\n" +
                        usersDeleted.Count + " Cuentas eliminadas\n"+
                        usersLocked.Count + " Cuentas bloqueadas\n"+
                        + usersUnlocked.Count + " Cuentas desbloqueadas");

                    if (xlApp == null)
                    {
                        MessageBox.Show("Excel no esta instalado correctamente");
                        return;
                    }
                    Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Jmoreno\Documents\Auditoria Semanal.xlsx");

                    var successSheet = (Excel.Worksheet)xlWorkbook.Sheets[1];
                    string cellName;
                    int counter = 5;
                    var range = successSheet.get_Range("A2");
                    range.Value = "Semana del " + selDay.Date.ToString("dd/MM/yy") + " al " + selDay.AddDays(7).Date.ToString("dd/MM/yy");

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
                        counter++;
                    }



                    var failure = (Excel.Worksheet)xlWorkbook.Sheets[2];
                    counter = 5;
                    var frange = failure.get_Range("A2");
                    frange.Value = "Semana del " + selDay.Date.ToString("dd/MM/yy") + " al " + selDay.AddDays(7).Date.ToString("dd/MM/yy");
                    foreach (var item in failureLogIn)
                    {
                        cellName = "A" + counter.ToString();
                        frange = failure.get_Range(cellName, cellName);
                        frange.Value2 = item.MsgDate.ToString();
                        cellName = "B" + counter.ToString();
                        frange = failure.get_Range(cellName, cellName);
                        frange.Value2 = item.MsgTime.ToString();
                        cellName = "C" + counter.ToString();
                        frange = failure.get_Range(cellName, cellName);
                        frange.Value2 = item.MsgHostname.ToString();
                        cellName = "D" + counter.ToString();
                        frange = failure.get_Range(cellName, cellName);
                        frange.Value2 = item.MsgText.ToString();
                        counter++;
                    }

                    var disAccounts = (Excel.Worksheet)xlWorkbook.Sheets[3];
                    counter = 5;
                    var disrange = disAccounts.get_Range("A2");
                    disrange.Value = "Semana del " + selDay.Date.ToString("dd/MM/yy") + " al " + selDay.AddDays(7).Date.ToString("dd/MM/yy");
                    foreach (var item in usersDisabled)
                    {
                        cellName = "A" + counter.ToString();
                        disrange = disAccounts.get_Range(cellName, cellName);
                        disrange.Value2 = item.MsgDate.ToString();
                        cellName = "B" + counter.ToString();
                        disrange = disAccounts.get_Range(cellName, cellName);
                        disrange.Value2 = item.MsgTime.ToString();
                        cellName = "C" + counter.ToString();
                        disrange = disAccounts.get_Range(cellName, cellName);
                        disrange.Value2 = item.MsgHostname.ToString();
                        cellName = "D" + counter.ToString();
                        disrange = disAccounts.get_Range(cellName, cellName);
                        disrange.Value2 = item.MsgText.ToString();
                        counter++;
                    }

                    var enabledAccounts = (Excel.Worksheet)xlWorkbook.Sheets[4];
                    counter = 5;
                    var enarange = enabledAccounts.get_Range("A2");
                    enarange.Value = "Semana del " + selDay.Date.ToString("dd/MM/yy") + " al " + selDay.AddDays(7).Date.ToString("dd/MM/yy");
                    foreach (var item in usersEnabled)
                    {
                        cellName = "A" + counter.ToString();
                        enarange = enabledAccounts.get_Range(cellName, cellName);
                        enarange.Value2 = item.MsgDate.ToString();
                        cellName = "B" + counter.ToString();
                        enarange = enabledAccounts.get_Range(cellName, cellName);
                        enarange.Value2 = item.MsgTime.ToString();
                        cellName = "C" + counter.ToString();
                        enarange = enabledAccounts.get_Range(cellName, cellName);
                        enarange.Value2 = item.MsgHostname.ToString();
                        cellName = "D" + counter.ToString();
                        enarange = enabledAccounts.get_Range(cellName, cellName);
                        enarange.Value2 = item.MsgText.ToString();
                        counter++;
                    }

                    var passChanged = (Excel.Worksheet)xlWorkbook.Sheets[5];
                    counter = 5;
                    var passrange = passChanged.get_Range("A2");
                    passrange.Value = "Semana del " + selDay.Date.ToString("dd/MM/yy") + " al " + selDay.AddDays(7).Date.ToString("dd/MM/yy");
                    foreach (var item in passwordChanged)
                    {
                        cellName = "A" + counter.ToString();
                        passrange = passChanged.get_Range(cellName, cellName);
                        passrange.Value2 = item.MsgDate.ToString();
                        cellName = "B" + counter.ToString();
                        passrange = passChanged.get_Range(cellName, cellName);
                        passrange.Value2 = item.MsgTime.ToString();
                        cellName = "C" + counter.ToString();
                        passrange = passChanged.get_Range(cellName, cellName);
                        passrange.Value2 = item.MsgHostname.ToString();
                        cellName = "D" + counter.ToString();
                        passrange = passChanged.get_Range(cellName, cellName);
                        passrange.Value2 = item.MsgText.ToString();
                        counter++;
                    }

                    var createdUserSheet = (Excel.Worksheet)xlWorkbook.Sheets[6];
                    counter = 5;
                    var createdUserRange = createdUserSheet.get_Range("A2");
                    createdUserRange.Value = "Semana del " + selDay.Date.ToString("dd/MM/yy") + " al " + selDay.AddDays(7).Date.ToString("dd/MM/yy");
                    foreach (var item in passwordChanged)
                    {
                        cellName = "A" + counter.ToString();
                        createdUserRange = createdUserSheet.get_Range(cellName, cellName);
                        createdUserRange.Value2 = item.MsgDate.ToString();
                        cellName = "B" + counter.ToString();
                        createdUserRange = createdUserSheet.get_Range(cellName, cellName);
                        createdUserRange.Value2 = item.MsgTime.ToString();
                        cellName = "C" + counter.ToString();
                        createdUserRange = createdUserSheet.get_Range(cellName, cellName);
                        createdUserRange.Value2 = item.MsgHostname.ToString();
                        cellName = "D" + counter.ToString();
                        createdUserRange = createdUserSheet.get_Range(cellName, cellName);
                        createdUserRange.Value2 = item.MsgText.ToString();
                        counter++;
                    }

                    var deletedUserSheet = (Excel.Worksheet)xlWorkbook.Sheets[7];
                    counter = 5;
                    var deletedUserRange = deletedUserSheet.get_Range("A2");
                    deletedUserRange.Value = "Semana del " + selDay.Date.ToString("dd/MM/yy") + " al " + selDay.AddDays(7).Date.ToString("dd/MM/yy");
                    foreach (var item in passwordChanged)
                    {
                        cellName = "A" + counter.ToString();
                        deletedUserRange = deletedUserSheet.get_Range(cellName, cellName);
                        deletedUserRange.Value2 = item.MsgDate.ToString();
                        cellName = "B" + counter.ToString();
                        deletedUserRange = deletedUserSheet.get_Range(cellName, cellName);
                        deletedUserRange.Value2 = item.MsgTime.ToString();
                        cellName = "C" + counter.ToString();
                        deletedUserRange = deletedUserSheet.get_Range(cellName, cellName);
                        deletedUserRange.Value2 = item.MsgHostname.ToString();
                        cellName = "D" + counter.ToString();
                        deletedUserRange = deletedUserSheet.get_Range(cellName, cellName);
                        deletedUserRange.Value2 = item.MsgText.ToString();
                        counter++;
                    }

                    var lockedUserSheet = (Excel.Worksheet)xlWorkbook.Sheets[8];
                    counter = 5;
                    var lockedUserRange = lockedUserSheet.get_Range("A2");
                    lockedUserRange.Value = "Semana del " + selDay.Date.ToString("dd/MM/yy") + " al " + selDay.AddDays(7).Date.ToString("dd/MM/yy");
                    foreach (var item in passwordChanged)
                    {
                        cellName = "A" + counter.ToString();
                        lockedUserRange = lockedUserSheet.get_Range(cellName, cellName);
                        lockedUserRange.Value2 = item.MsgDate.ToString();
                        cellName = "B" + counter.ToString();
                        lockedUserRange = lockedUserSheet.get_Range(cellName, cellName);
                        lockedUserRange.Value2 = item.MsgTime.ToString();
                        cellName = "C" + counter.ToString();
                        lockedUserRange = lockedUserSheet.get_Range(cellName, cellName);
                        lockedUserRange.Value2 = item.MsgHostname.ToString();
                        cellName = "D" + counter.ToString();
                        lockedUserRange = lockedUserSheet.get_Range(cellName, cellName);
                        lockedUserRange.Value2 = item.MsgText.ToString();
                        counter++;
                    }

                    var unlockedUserSheet = (Excel.Worksheet)xlWorkbook.Sheets[9];
                    counter = 5;
                    var unlockedUserRange = unlockedUserSheet.get_Range("A2");
                    lockedUserRange.Value = "Semana del " + selDay.Date.ToString("dd/MM/yy") + " al " + selDay.AddDays(7).Date.ToString("dd/MM/yy");
                    foreach (var item in passwordChanged)
                    {
                        cellName = "A" + counter.ToString();
                        unlockedUserRange = unlockedUserSheet.get_Range(cellName, cellName);
                        unlockedUserRange.Value2 = item.MsgDate.ToString();
                        cellName = "B" + counter.ToString();
                        unlockedUserRange = unlockedUserSheet.get_Range(cellName, cellName);
                        unlockedUserRange.Value2 = item.MsgTime.ToString();
                        cellName = "C" + counter.ToString();
                        unlockedUserRange = unlockedUserSheet.get_Range(cellName, cellName);
                        unlockedUserRange.Value2 = item.MsgHostname.ToString();
                        cellName = "D" + counter.ToString();
                        unlockedUserRange = unlockedUserSheet.get_Range(cellName, cellName);
                        unlockedUserRange.Value2 = item.MsgText.ToString();
                        counter++;
                    }

                    var changedNameUserSheet = (Excel.Worksheet)xlWorkbook.Sheets[10];
                    counter = 5;
                    var changedNameUserRange = changedNameUserSheet.get_Range("A2");
                    changedNameUserRange.Value = "Semana del " + selDay.Date.ToString("dd/MM/yy") + " al " + selDay.AddDays(7).Date.ToString("dd/MM/yy");
                    foreach (var item in passwordChanged)
                    {
                        cellName = "A" + counter.ToString();
                        changedNameUserRange = changedNameUserSheet.get_Range(cellName, cellName);
                        changedNameUserRange.Value2 = item.MsgDate.ToString();
                        cellName = "B" + counter.ToString();
                        changedNameUserRange = changedNameUserSheet.get_Range(cellName, cellName);
                        changedNameUserRange.Value2 = item.MsgTime.ToString();
                        cellName = "C" + counter.ToString();
                        changedNameUserRange = changedNameUserSheet.get_Range(cellName, cellName);
                        changedNameUserRange.Value2 = item.MsgHostname.ToString();
                        cellName = "D" + counter.ToString();
                        changedNameUserRange = changedNameUserSheet.get_Range(cellName, cellName);
                        changedNameUserRange.Value2 = item.MsgText.ToString();
                        counter++;
                    }
                    string path = @"C:\";
                    xlWorkbook.SaveAs2("Auditoria Semanal " + selDay.Date.ToString("ddMMyy") +
                        ".xlsx");
                    xlApp.Workbooks.Close();
                    xlApp.Quit();
                    MessageBox.Show("Termine");
                }
                catch (Exception e) 
                {
                    MessageBox.Show(e.Message);
                }

                /* // Esperar otros 2 segundos para más tareas pesadas.
                 Thread.Sleep(2000);

                 // Ejecutar segundo callback desde el segundo proceso al primero
                 
                */
                // La tarea heavy task finaliza, así que hay que detenerla.
                Stop();
            }
        }


        private void addLogToList(Syslogd _log)
        {
            successLogon = (Regex.IsMatch(_log.MsgText, "\t4624\t"));
            failureLogon = (Regex.IsMatch(_log.MsgText, "	4625	"));
            userEnabled = (Regex.IsMatch(_log.MsgText, "\t4722\t"));
            userDisabled = (Regex.IsMatch(_log.MsgText, "\t4725\t"));
            userCreated = (Regex.IsMatch(_log.MsgText, "\t4720\t"));
            changePass = (Regex.IsMatch(_log.MsgText, "\t4723\t"));
            userDeleted = (Regex.IsMatch(_log.MsgText, "\t4726\t"));
            userLocked = (Regex.IsMatch(_log.MsgText, "\t4740\t"));
            userUnlocked = (Regex.IsMatch(_log.MsgText, "\t4767\t"));
            userNameChanged = (Regex.IsMatch(_log.MsgText, "\t4781\t"));
            typeLogin = (Regex.IsMatch(_log.MsgText, "		2") ||
                Regex.IsMatch(_log.MsgText, "		7") ||
                Regex.IsMatch(_log.MsgText, "		11"));

            if (userCreated) usersCreated.Add(_log);
            if (userEnabled) usersEnabled.Add(_log);
            if (changePass) passwordChanged.Add(_log);
            if (userDisabled) usersDisabled.Add(_log);
            if (userDeleted) usersDeleted.Add(_log);
            if (userLocked) usersLocked.Add(_log);
            if (userUnlocked) usersUnlocked.Add(_log);
            if (userNameChanged) usersNameChanged.Add(_log);
            if (successLogon)
            {
                if (typeLogin)
                {
                    successLogIn.Add(_log);
                }
            }
            if (failureLogon)
            {
                if (typeLogin)
                {
                    failureLogIn.Add(_log);
                }
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