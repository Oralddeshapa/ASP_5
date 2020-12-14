using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TeaChair.Services
{
    public class CurrentTimeService
    {
        public string GetTime(int k)
        {   
            DateTime dt = DateTime.Now;
            dt = dt.AddHours((double)k);
            return  dt.ToString("hh:mm:ss");
        }
    }
}
