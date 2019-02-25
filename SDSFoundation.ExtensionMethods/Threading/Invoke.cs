using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
 
using System.Windows.Threading;

namespace SDSFoundation.ExtensionMethods.Threading
{
    public static class Invoke
    {
        /// <summary>
        /// Simple helper extension method to marshall to correct
        /// thread if its required
        /// </summary>
        /// <param name="""control""">The source control</param>
        /// <param name="""methodcall""">The method to call</param>
        /// <param name="""priorityForCall""">The thread priority</param>
        public static void InvokeIfRequired(
           this DispatcherObject control,
           Action methodcall,
           DispatcherPriority? priorityForCall)
        {
            if(priorityForCall == null)
            {
                priorityForCall = System.Windows.Threading.DispatcherPriority.Normal;
            }

            //see if we need to Invoke call to Dispatcher thread
            if (control.Dispatcher.Thread != Thread.CurrentThread)
                control.Dispatcher.Invoke(priorityForCall.Value, methodcall);
            else
                methodcall();
        }


        /// <summary>
        /// Simple helper extension method to marshall to correct
        /// thread if its required
        /// </summary>
        /// <param name="""control""">The source control</param>
        /// <param name="""methodcall""">The method to call</param>
        /// <param name="""priorityForCall""">The thread priority</param>
        public static void InvokeIfRequired(
           this System.Windows.Forms.Control control,
           Action methodcall)
        {
            ////see if we need to Invoke call to Dispatcher thread
            //if (control.Dispatcher.Thread != Thread.CurrentThread)
            //    control.Dispatcher.Invoke(priorityForCall, methodcall);
            //else
            //    methodcall();

            if (control.InvokeRequired)
            {
                control.Invoke(new System.Windows.Forms.MethodInvoker(delegate { methodcall(); }));
            }
            else
            {
                methodcall();
            }

        }


        

        //    string name = "";
        //if(textbox1.InvokeRequired)
        //{
        //    textbox1.Invoke(new MethodInvoker(delegate { name = textbox1.text; }));
        //}
        //if(name == "MyName")
        //{
        //    // do whatever
        //}
    }
}
