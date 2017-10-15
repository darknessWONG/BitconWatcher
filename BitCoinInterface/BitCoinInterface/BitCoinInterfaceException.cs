using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitCoinInterface
{
    public class ErrorCounterException : Exception
    {
        public ErrorCounterException()
            :base("counterNum不能小于toleranceNum")
        {
        }
    }
}
