using System;
using System.Diagnostics;
using System.Management;

namespace RetroLauncher.DesktopClient.Service
{
    public class EmulatorService
    {
        public EmulatorService()
        {

        }

        public void StartRom(string gamepath)
        {
            var process = new Process();
            process.StartInfo.FileName = Storage.Source.PathEmulatorExe;
            process.StartInfo.Arguments = $"\"{gamepath}\"";
            process.Start();

        }

    }

    public class ProcessWatcher : ManagementEventWatcher
    {
        public delegate void ProcessEventHandler(WIN32_Process proc);

        public event ProcessEventHandler ProcessCreated;
        public event ProcessEventHandler ProcessDeleted;
        public event ProcessEventHandler ProcessModified;

        //запросы на поиск объектов
        private readonly string WMI_OPER_EVENT_QUERY = "SELECT * FROM __InstanceOperationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'";
        private readonly string WMI_OPER_EVENT_QUERY_WITH_PROC = "SELECT * FROM __InstanceOperationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'" + " and TargetInstance.Name = '{0}'";

        public ProcessWatcher()
        {
            Init(String.Empty);
        }


        public ProcessWatcher(string processName)
        {
            Init(processName);
        }


        private void Init(string processName)
        {
            this.Query.QueryLanguage = "WQL";

            //либо отслеживаем вообще все процессы
            if (String.IsNullOrEmpty(processName))
                this.Query.QueryString = WMI_OPER_EVENT_QUERY;
            else //либо отслеживаем указаный процесс
                this.Query.QueryString = String.Format(WMI_OPER_EVENT_QUERY_WITH_PROC, processName);

            this.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
        }

        private void watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            //получаем тип события
            string eventType = e.NewEvent.ClassPath.ClassName;
            var proc = new WIN32_Process(e.NewEvent["TargetInstance"] as ManagementBaseObject);

            switch (eventType)
            {
                case "__InstanceCreationEvent":
                    if (ProcessCreated != null) ProcessCreated(proc); break;
                case "__InstanceDeletionEvent":
                    if (ProcessDeleted != null) ProcessDeleted(proc); break;
                case "__InstanceModificationEvent":
                    if (ProcessModified != null) ProcessModified(proc); break;
            }
        }

    }
}