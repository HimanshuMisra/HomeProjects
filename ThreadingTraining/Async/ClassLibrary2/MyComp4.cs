using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.ComponentModel;


public class MyComp4
{
    public async Task<int> TaskDivideAsync(int nominator, int denominator)
    {
        await Task.Run(() => Thread.Sleep(10000));
        return (nominator / denominator);
    }  
}
