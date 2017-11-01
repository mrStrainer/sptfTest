using System;
using System.Collections.Generic;
using sptfApiLib;

namespace sptfTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Sup");
            ApiLib.Authorize();
            Console.WriteLine("after");
        }
    }
}