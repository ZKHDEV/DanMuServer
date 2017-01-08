using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DanMuServer
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("欢迎使用弹幕服务器，请输入监听端口号：");
            int port = 0;

            while (true)
            {
                string input = Console.ReadLine().Trim();
                if (Regex.IsMatch(input, "[0-9]{1,5}"))
                {
                    port = Int32.Parse(input);
                    if (port < 65536 && port > 0)
                    {
                        break;
                    }
                    System.Console.WriteLine("端口号不合法");
                }
                else
                {
                    System.Console.WriteLine("端口号不合法");
                }
            }

            Server server = new Server(port);
            try
            {
                server.Start();
                Console.WriteLine("- - - 服务器已启用 - - -");
                Console.WriteLine("- - - 输入\"off\"关闭服务器 - - -");
            }
            catch (Exception e)
            {
                Console.WriteLine("- - - 服务器断开 - - -");
                throw e;
            }

            while (Console.ReadLine() != "off")
            {
                
            }
        } 
    }
}
