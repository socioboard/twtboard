using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace BaseLib
{
    public class Events
    {
        public event EventHandler addToLogger;

        
        public void LogText(EventsArgs e)
        {
            if (addToLogger != null)
            {
                addToLogger(this, e); //Fires the event
            }
        }

        public event EventHandler incrementCount;

        public void IncreaseCounter(EventsArgs e)
        {
            if (incrementCount != null)
            {
                incrementCount(this, e);
            }
        }

        public event EventHandler raiseScheduler;
        public void RaiseScheduler(EventsArgs e)
        {
            if (raiseScheduler != null)
            {
                raiseScheduler(this, e); //Fires the event
            }
        }

        /// <summary>
        /// Fires the event taking "EventsArgs" instance as parameter
        /// Just call this method where you want to fire the event
        /// </summary>
        /// <param name="e"></param>
        public void RaiseProcessCompletedEvent(EventsArgs e)
        {
            //lock (syncLock)
            {
                if (processCompletedEvent != null)
                {
                    processCompletedEvent(this, e); //Fires the event
                }
            }
        }
        public event EventHandler processCompletedEvent;
    }


    public class EventsArgs : EventArgs
    {
        public string log { get; set; }

        public EventsArgs(string Log)
        {
            this.log = Log;
        }

        public Module module { get; set; }

        public EventsArgs(Module module)
        {
            this.module = module;
        }

        public EventsArgs()
        {
        }

        //public bool setTrueFalse { get; set; }

        //public EventsArgs(string Log, bool setTrueFalse)
        //{
        //    this.log = Log;
        //    this.setTrueFalse = setTrueFalse;
        //}

    }
}
