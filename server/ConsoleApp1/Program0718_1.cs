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
 * UDP : 비연결형 통신방식. 서버와 클라이언트가 연결과정을 수행하지 않고 통신하는 방식 따라서 네트워크 상에서 데이터가 손실되더라도 재전송 요청을 할 수 없음
 * UDP서버 통신 기본 예제
 * 기능
 * UDP 클라이언트가 보낸 바이트 배열을 문자열로 변환 및 출력
 * -> 바이트배열을 메모리버퍼에 쌓아놓은 다음 BinaryFormatter 객체를 활용
 */
namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            //UDP클라이언트의 IP,PORT를 얻기 위한 소켓
            UdpClient udp = new UdpClient(11000);
            //UDP클라이언트에게 데이터를 송신할 소켓
            UdpClient sender = new UdpClient();
            IPEndPoint src_ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);

            for (; ; )
            {
                byte[] recv_data = udp.Receive(ref src_ip);
                Console.WriteLine("{0} byte 수신 됨 ", recv_data.Length);
                Console.WriteLine("송신자의 IP : {0} PORT : {1}", src_ip.Address.ToString(), src_ip.Port);
                //DateTime : c#에서 시간데이터를 처리할 때 사용하는 클래스
                //DateTime.Now 정적변수 : 컴퓨터의 현재시간을 저장하는 Datetime 객체
                DateTime datatime = DateTime.Now;
                //객체나 변수값을 전송하기 위해 MemoryStream, BinaryFormatter 사용
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                //메모리 공간에 객체데이터를 누적
                formatter.Serialize(stream, datatime);
                //메모리 공간에 데이터를 byte[]로 추출
                byte[] send_data = stream.ToArray();
                //UDP통신으로 송신 - UDP 클라이언트의 IP/PORT로 전송
                sender.Send(send_data, send_data.Length, src_ip);
            }
            sender.Close();
            udp.Close();
        }
    }
}
