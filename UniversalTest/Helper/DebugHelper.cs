using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTest.Helper
{
    public class DebugHelper
    {
        public static void PrintTimeNow()
        {
            Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }
    }
}
