using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TeaChair.Services
{
    public class CurrentTimeService
    {
        public string GetTime() => DateTime.Now.ToString("hh:mm:ss");
    }
}
