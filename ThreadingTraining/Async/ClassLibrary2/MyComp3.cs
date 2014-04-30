using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.ComponentModel;


 public class MyComp3
    {
     public Task<int> TaskDivideAsync(int nominator, int denominator)
     {
         Task<int> task = new Task<int>(() =>
         {
             Thread.Sleep(10000);
             return (nominator / denominator);
         });

         task.Start();

         return (task);
     }
    }
