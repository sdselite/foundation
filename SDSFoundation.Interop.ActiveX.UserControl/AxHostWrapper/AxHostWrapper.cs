using SDSFoundation.Interfaces.Interop.ActiveX.Forms;
using SDSFoundation.Interfaces.Interop.ActiveX.UserControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Windows.Forms;

namespace SDSFoundation.Interop.ActiveX.UserControl.Wrapper.AxHostWrapper
{

    public class AxHostWrapper<TActiveXControl, TActiveXControlInterface>
        where TActiveXControl : AxHost, IAxControl, new()
        where TActiveXControlInterface : IAxControl
    {

        private string axHostAssemblyFullPath;
        //private static TAxHost axControl;
        //public TAxHost AxControl { get {

        //        if (axControl != null && axControl.InvokeRequired)
        //        {

        //        }
        //        return axControl;

        //    } private set { axControl = value; } }
        public AxHostWrapper()
        {

        }

        /// <summary>
        /// Inject the assembly from an alternate path.  This is used to isolate ActiveX exectuables
        /// </summary>
        /// <param name="axHostAssemblyRelativePath"></param>
        public AxHostWrapper(string axHostAssemblyRelativePath)
        {
            if(string.IsNullOrWhiteSpace(axHostAssemblyRelativePath) == false)
            {
                var formattedPath = AppDomain.CurrentDomain.BaseDirectory + axHostAssemblyRelativePath;
                this.axHostAssemblyFullPath = formattedPath;
            }
            else
            {
                this.axHostAssemblyFullPath = axHostAssemblyRelativePath;
            }
        }


        //ActiveXWrapperHiddenForm<TActiveXControl> : Form, IActiveXHiddenForm<TActiveXControl>
        // where TActiveXControl : AxHost, IActiveXWrappableControl, new()

        // public AxHostForm<TAxHost> Create(int timeout = 30000, AxHostForm<TAxHost> form = null)

        public AxHostForm<TActiveXControl, TActiveXControlInterface> Create(int timeout = 30000, AxHostForm<TActiveXControl, TActiveXControlInterface> form = null)
        {
            bool isFormInjected = true;
            if (form == null)
            {
                isFormInjected = false;
                form = new AxHostForm<TActiveXControl, TActiveXControlInterface>();
            }

            if (form.Created == false)
            {
                form.CreateControl();
            }

            Task.Delay(100).Wait();

            Create(form, timeout, isFormInjected);
            return form;
        }


        private TActiveXControlInterface TAxHostCreator(List<string> dependencies = null)
        {
            if (string.IsNullOrWhiteSpace(this.axHostAssemblyFullPath))
            {
                var result = new TActiveXControl();
                return  (TActiveXControlInterface)(object)result;
            }
            else
            {
                Assembly assembly = Assembly.LoadFrom(this.axHostAssemblyFullPath);

                var fullName = assembly.FullName.Split(',').First();
                Type type = assembly.GetType(typeof(TActiveXControl).FullName);

                
                var instanceOfMyType =  Activator.CreateInstance(type);

                return (TActiveXControlInterface)instanceOfMyType;
            }

        }

            private void Create<TForm>(TForm form, int timeout = 30000, bool isFormInjected = false)
            where TForm : Form, IActiveXHiddenForm<TActiveXControl, TActiveXControlInterface>
        {
   
            var hostTask = Task.Run(() => {
                using (var sta = new StaTaskScheduler(1))
                {
                    var taskResult = Task.Factory.StartNew(() =>
                    {


                        using (var axHost = TAxHostCreator())
                        {
                            bool shouldRunAsApp = true;
                            // important to call this just after constructing ActiveX type
                            if (axHost.Created == false)
                            {
                                try
                                {
                                    axHost.CreateControl();
                                }
                                catch (Exception createControlEx)
                                {
                                    //Do nothing
                                    shouldRunAsApp = false;
                                }

                            }


                            form.ActiveXControl = axHost;
                            //form.Visible = false;

                            if (form.Controls.Contains((Control)(object)axHost) == false)
                            {
                                form.Controls.Add(((Control)(object)axHost));
                            }

                            if (shouldRunAsApp && isFormInjected == false)
                            {
                                axHost.BeginInit();

                                //AxControl.Visible = false;
                                //axHost.Visible = false;
                               
                                axHost.HandleCreated += AxControl_HandleCreated;
                                axHost.HandleDestroyed += (s, e) => Application.ExitThread();

                                axHost.EndInit();



                                // start message pump
                                try
                                {
                                    Application.Run(form);
                                }
                                catch (System.Threading.ThreadAbortException threadAbortEx)
                                {
                                    //Do nothing.  This is expected when shutting down the application in a unit test
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                            }
                            else
                            {
                                //while (1 == 1)
                                //{
                                //    //Just keep the wheels turning.
                                //    Task.Delay(1000).Wait();
                                //}

                            }



                        }
                    }, CancellationToken.None, TaskCreationOptions.None, sta);

                }

            });


            int runningTotal = 0;
            int delay = 100;
            while (runningTotal < timeout)
            {
                if (form == null || form.Created == false || form.ActiveXControl == null || form.ActiveXControl.Created == false)
                {
                    Task.Delay(delay).Wait();
                    runningTotal += delay;
                }
                else
                {
                    break;
                }
            }
            Task.Delay(2500).Wait();
            
        }

  
        private void AxControl_HandleCreated(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
