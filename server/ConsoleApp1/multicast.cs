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
 *UDP 수신 프로그램 - 멀티캐스트된 데이터를 수신
 */
namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            //UdpClient객체 생성 - 13000포트
            UdpClient receiver = new UdpClient(13000);
            //IPAddress 객체 생성 - 가입할 멀티캐스트 주소를 저장할 객체 224.0.1.0
            IPAddress multicast_ip = IPAddress.Parse("224.0.1.0");
            //멀티캐스트 가입 
            //JoinMulticastGroup(IPAddress 객체) :  IPAddress객체가 저장한 IP주소(멀티캐스트 주소)로 해당 UDP소켓이 가입하는 기능이 있는 메소드
            receiver.JoinMulticastGroup(multicast_ip);
            //IPEndPoint객체 생성 - 0,0 인자로 사용
            IPEndPoint recv_ip = new IPEndPoint(0, 0);

            for(; ; )
             {
                //데이터 수신 - byte[]
                byte[] recv_byte = receiver.Receive(ref recv_ip);
                //byte[] 를 MemoryStream에 객체 생성시 인자값으로 사용
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream(recv_byte);
                //MemoryStream의 커서 위치를 데이터 맨 앞으로 이동
                stream.Seek(0, SeekOrigin.Begin);
                //BinaryFormatter로 Deserialize 메소드 호출로 string 변환
                string result = (string)formatter.Deserialize(stream);
                stream.Close();
                //결과출력
                Console.WriteLine("{0}", result);
            }

            //가입한 멀티캐스트 그룹을 탈퇴
            receiver.DropMulticastGroup(multicast_ip);
            //UdpClient객체 연결 종료
            receiver.Close();
        }
    }
}
