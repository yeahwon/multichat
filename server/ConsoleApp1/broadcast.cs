using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
/*
 *UDP 수신 프로그램 - 브로드캐스트된 데이터를 수신
 */
namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient receiver = new UdpClient(12000);

            IPEndPoint src_ip = new IPEndPoint(0, 0);

            byte[] recv_data;
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            string result;

            for(; ; )
            {
                //UDP소켓으로 데이터 수신 - 소켓으로 직접적으로 오는 데이터 or 브로드캐스트된 데이터가 수신됨
                recv_data = receiver.Receive(ref src_ip);
                stream.Write(recv_data, 0, recv_data.Length);
                stream.Seek(0, SeekOrigin.Begin);
                result = (string)formatter.Deserialize(stream);
                Console.WriteLine("수신된 데이터 : {0}", result);
                //메모리스트림의 데이터 지우기
                stream.SetLength(0);
            }

            stream.Close();
            receiver.Close();
        }
    }
}
