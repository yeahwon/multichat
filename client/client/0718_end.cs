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
/*
 * 멀티캐스트 : 224.0.1.0 - 239.25..255.255 중 특정 네트워크에 가입해 1대다 통신을 사용하는 기법
 * 224.0.0.0 ~ 224.0.0.255 는 네트워크 장치가 사용하는 영역으로 멀티캐스트에 가입할 수 없음
 * 멀티캐스트에 가입할 수 있는 소켓은 UDP소켓만 가입 가능. UDP소켓은 여러개의 멀티캐스트를 가입할 수 있고, 탈퇴가 자유로움. 
 * 가입된 멀티캐스트로 들어오는 데이터를 수신할 순 있으나 포트번호가 다르면 수신받을 수 없음.
 * 멀티캐스트 그룹으로 데이터 송신시 IP설정을 멀티캐스트의 IP로 설정해 데이터 송신 멀티캐스트는 다른 네트워크 영역에 있는 UDP소켓에게 전달할 수 있음.
 * TTL (Time To Live) : 네트워크 장비(라우터)를 거칠 때 마다 TTL값이 1씩 감소해 TTL이 0이 되면 데이터가 소멸됨. TTL을 높일수록 물리적으로 멀리 떨어진 UDP소켓에게 데이터 전송이 가능함.
 * 
 * UDP송신 프로그램 - 멀티캐스트를 통해 데이터를 송신
 */
namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            //UdpClient 객체 생성 - 13000포트
            UdpClient sender = new UdpClient();
            //송신할 멀티캐스트의 IP/PORT저장할 IPEndPoint객체 생성
            //멀티캐스트 ip : 224.0.1.0 port : 13000 
            IPEndPoint multicast_ip = new IPEndPoint(IPAddress.Parse("224.0.1.0"), 13000);
            //문자열, byte[] BinaryFommatter , memorystream 변수 생성
            string data;
            byte[] send_byte;
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            for (; ; )
            {
                data = Console.ReadLine();
                //문자열 데이터를 memoryStream 객체에 serialize
                formatter.Serialize(stream, data);
                //MemorySteream객체에 저장된 데이터를 byte[]로 변환
                send_byte = stream.ToArray();
                //byte[]를 IPEndPoint 객체와 함께 UdpClient 객체로 송신
                sender.Send(send_byte, send_byte.Length, multicast_ip);
                //버퍼 초기화
                stream.SetLength(0);
            } 
            stream.Close();
            //UdpClient 객체 연결 종료
            sender.Close();
        }
    }
}
