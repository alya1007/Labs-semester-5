using TMPS.Application;
using TMPS.DataAccess.Employees;
using TMPS.Domain.Models;

namespace TMPS.Client;
class Program
{
    static void Main(string[] args)
    {
        Menu menu = new();
        menu.Show();
    }
}