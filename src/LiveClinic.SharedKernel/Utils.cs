using System;
using System.Threading;

namespace LiveClinic.SharedKernel
{
    public static class Utils
    {
        public static string GenerateNo(string  prefix="")
        {
            var uniqueNo = DateTime.Now.Ticks;
            Thread.Sleep(10);
            return $"{prefix}{uniqueNo}".Trim();
        }
    }
}
