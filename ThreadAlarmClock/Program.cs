/*************************************************************************************************************
 * Created by Marque Burgess                                                                                 *
 * 4/13/2018                                                                                                 *
 * Module 5 Programming Assignment                                                                           *
 * **********************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace ThreadAlarmClock
{
    public class ProgramDriver
    {
        public static void Main(string[] args)
        {
            Observer ob = new Observer();
            Observer ab = new Observer();
            CountdownClock cc = new CountdownClock(10);
            CountdownClock dc = new CountdownClock(5);
            CountdownClock bc = new CountdownClock(7);
            CountdownClock dd = new CountdownClock(7);


            //2 events running
            cc.Alarm += new ClockEventHandler(ob.ClockUser);
            cc.Alarm += new ClockEventHandler(ab.ClockUser);
            cc.Start();

            //using pause method for 10 second delay;
            dc.Alarm += new ClockEventHandler(ab.ClockUser);
            dc.Alarm += new ClockEventHandler(ob.ClockUser);
            Console.WriteLine("\nNew events are paused here using sleep(10000) for 10 second pause and will resume");
            dc.PauseCount();
            dc.Start();

            //last method stops thread before it can run
            bc.Alarm += new ClockEventHandler(ob.ClockUser);
            bc.Alarm += new ClockEventHandler(ab.ClockUser);
            Console.WriteLine("\nNew events are stopped before they are ran(infinite timeout) -- close screen");
            bc.StopCount();
            bc.Start();


            Console.ReadKey();
        }
    }

    /////////////////////////////////////Clock Event/////////////////////////////////////////////////
    public class ClockArgs : System.EventArgs
    {
        public int clockCount;

        public ClockArgs(int clockCount)
        {
            this.clockCount = clockCount;
        }

        public int ClockCounted
        {
             get { return clockCount; }
        }

    }

    public delegate void ClockEventHandler(object sender, ClockArgs e);

    public class CountdownClock
    {
        private int clockCount;

        public CountdownClock(int clockCount)
        {
            this.clockCount = clockCount;
        }

        //////////////////////////////clock methods//////////////////////////////////////////////
        public void Start()
        {
            while(this.clockCount > 0)
            {
                Thread.Sleep(1000);
                this.clockCount--;
                ClockArgs e = new ClockArgs(this.clockCount);
                OnAlarm(e);
            }
        }

        public void StopCount()
        {
            Thread.Sleep(Timeout.Infinite); //set permanent pause
            ClockArgs e = new ClockArgs(this.clockCount);
            OnAlarm(e);
        }

        public void PauseCount()
        {
            Thread.Sleep(10000);  //10 second pause
            ClockArgs e = new ClockArgs(this.clockCount);
            OnAlarm(e);
        }


        public event ClockEventHandler Alarm;
        protected virtual void OnAlarm(ClockArgs e)
        {
            ClockEventHandler handler = Alarm;
            if (handler != null)
            {
                // Invokes the delegates.
                handler(this, e);
            }
        }

    }

    public class Observer
    {
        public void ClockUser(object sender, ClockArgs e)
        {
            Console.WriteLine(e.clockCount + " Seconds remaining");
            if (e.clockCount == 0)
            {
                Console.WriteLine("Goodbye");
            }

        }
    }
}

