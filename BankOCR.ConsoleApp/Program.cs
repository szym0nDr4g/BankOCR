using System;
using System.Collections.Generic;
using System.IO;
using static LanguageExt.Prelude;

namespace BankOCR.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Try(fun(() => File.ReadLines(args[0])))
                .Map(enumerable => enumerable.ToArr());
                
                
            
            
        }
    }
}