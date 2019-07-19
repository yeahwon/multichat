using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/*UDP 클라이언트 통신 기본 예제
 * 기능
 * UDP 서버에 문자열을 송신
 * ->MemoryStream, BinaryFormatter를 활용한 변수/객체 전달
 */
namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            //UdpClient객체생성 - 데이터를 먼저 보내는 송신자측은 포트번호 임시 발급
            UdpClient client = new UdpClient();
            //서버의 IP/PORT 저장
            IPEndPoint des_ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000),
                recv_ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"),0);
            for(; ; )
            {
                //데이터 없이 UDP 소켓에 송신 - 데이터를 수신하는 UDP소켓은 byte[0]과 송신자의 ip port를 받게 됨
                client.Send(new byte[0], 0, des_ip);
                //서버로부터 시간데이터 수신
                byte[] recv_data = client.Receive(ref recv_ip);
                BinaryFormatter formatter = new BinaryFormatter();
                //MemoryStream 객체 생성하면서 byte데이터를 삽입
                MemoryStream stream = new MemoryStream(recv_data);
                //Deserialize를 사용하기 위해 커서위치를 데이터 처음으로 이동
                stream.Seek(0, SeekOrigin.Begin);

                DateTime server_time = (DateTime)formatter.Deserialize(stream);

                Console.WriteLine("서버의 현재시간 : {0}", server_time.ToString());

                
                stream.Close();
                Thread.Sleep(2000); //2초동안 코드 멈춤
            }
            client.Close();
        }
    }
}
