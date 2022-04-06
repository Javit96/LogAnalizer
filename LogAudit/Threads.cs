using LogAudit.Models;
using System.ComponentModel;

namespace LogAudit
{
    public partial class ThreadRead
    {
        // Bandera booleana que indica cuando el proceso está siendo ejecutado o ha sido detenido
        private bool HeavyProcessStopped;

        // Expone el contexto de sincronización en la clase entera 
        private readonly SynchronizationContext SyncContext;

        // Crear los 2 contenedores de callbacks
        //public event EventHandler<ThreadReadResponse> Callback1;
        public event EventHandler<ThreadWriteResponse> Callback;


        KiwiSyslogContext kiwiSyslogContext;
        DateTime selDay;
        // Constructor de la clase HeavyTask
        public ThreadRead(KiwiSyslogContext _context, DateTime _selDay)
        {
            kiwiSyslogContext = _context;
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
                    
                        List<Syslogd> SyslogDate = kiwiSyslogContext.Syslogds.Where(x => x.MsgDate.Equals(selDay.Date.ToString("yyyy-MM-dd"))).ToList();
                        SyncContext.Post(e => triggerCallback(new ThreadWriteResponse(SyslogDate)), null);

                        /*Parallel.ForEach(SyslogDate, logs =>
                        {
                            SyncContext.Post(e => triggerCallback1(new ThreadReadResponse(logs)), null);
                            Thread.Sleep(10);
                        });*/

                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }

                /* // Esperar otros 2 segundos para más tareas pesadas.
                 Thread.Sleep(2000);

                 // Ejecutar segundo callback desde el segundo proceso al primero
                 
                */
                // La tarea heavy task finaliza, así que hay que detenerla.
                Stop();
            }
        }


        // Métodos que ejecutan los callback si y solo si fueron declarados durante la instanciación de la clase HeavyTask
        private void triggerCallback(ThreadWriteResponse response)
        {

            // Si el primer callback existe, ejecutarlo con la información dada
            Callback?.Invoke(this, response);
        }

        /* private void triggerCallback2(ThreadReadResponse response)
         {
             // Si el segundo callback existe, ejecutarlo con la información dada
             Callback2?.Invoke(this, response);
         }*/
    }

    /*public class ThreadReadResponse
    {
        private readonly Syslogd message;

        public ThreadReadResponse(Syslogd logs)
        {
            this.message = logs;
        }

        public Syslogd Message { get { return message; } }
    }*/

    public class ThreadWriteResponse
    {
        private List<Syslogd> logs;

        public ThreadWriteResponse(List<Syslogd> list)
        {
            this.logs = list;
        }

        public List<Syslogd> Logs { get { return logs; } }
    }
}



