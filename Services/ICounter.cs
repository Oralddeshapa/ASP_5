using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaChair.Services
{
    public interface ICounter
    {
       int Value { get; }
    }

    public class Count : ICounter
    {
        private int count;
        //private static Random rand = new Random();
        public Count()
        {
            count = 0;
            //count = rand.Next(0, 100000);
        }
        public int Value { get { return count++; }}      
    }

    public class CounterServ
    { 
        public ICounter Counter { get; set; }

        public CounterServ(ICounter counter)
        {
            Counter = counter;
        }
    }

}
