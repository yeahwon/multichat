using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
/*UDP 클라이언트 통신 기본 예제
 * 기능
 * UDP 서버에 문자열을 송신
 * 
 */
namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            //UdpClient 객체 생성 - 데이터를 먼저 보내는 송신자측은 포트번호 임시 발급
            UdpClient client = new UdpClient();
            //서버의 IP/PORT를 저장
            IPEndPoint des_ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
            //데이터 송신
            string data = "Hello UDP!";
            //입력한 문자열을 byte배열로 변경
            byte[] byte_data = Encoding.UTF8.GetBytes(data);
            client.Send(byte_data, byte_data.Length, des_ip);
            //UdpClient 객체 종료
            client.Close();
        }
    }
}
