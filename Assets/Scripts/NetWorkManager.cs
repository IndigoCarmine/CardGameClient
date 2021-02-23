using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class NetWorkManager : MonoBehaviour
{
    
    public string HostIP;
    public int HostPort = 4000;
    Socket socket;

    //受信用Buffer
    const int buffersize = 1024;
    byte[] Buffer = new byte[buffersize];

    //ClientManagerに情報を送る用
    ClientWork client;

    private void Start()
    {
        client = GameObject.FindGameObjectWithTag("ClientManager").GetComponent<ClientWork>();
    }

    //サーバに接続
    public void Connect()
    {
        //ソケット初期化
        IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Parse(HostIP), HostPort);
        socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

        try
        {
            //接続開始
            socket.Connect(ipendpoint);
            Debug.Log("サーバーに接続しました。");

            //非同期で受信開始
            socket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, ReceiveCallBack, socket);
        }
        catch
        {
            Debug.LogError("接続できませんでした。");
        }
    }
    public void Connect(string _HostIP)
    {
        HostIP = _HostIP;
        Connect();
    }
    public void Disconnect()
    {
        socket.Disconnect(true);
    }

    /// <summary>
    /// TCPでサーバーに文字列を送信
    /// </summary>
    /// <param name="Msg">送信内容</param>
    /// <returns>送信成功</returns>
    public bool Request(RequestParameter Parameter, string Content)
    {
        string Msg = Parameter.ToString("G") + "=" + Content;
        byte[] bytes = Encoding.UTF8.GetBytes(Msg + '\n');
        try
        {
            socket.Send(bytes);
        }catch
        {
            Debug.LogError("送信できませんでした。切断された可能性があります。");
            return false;
        }
        return true;
        
    }
    public enum RequestParameter
    {
        PlayerName,
        Select,
        Draw,
        Uno,
        DataRequest
    }
    
    /// <summary>
    ///受信用CallBack関数
    /// </summary>
    /// <param name="asyncResult"></param>
    private void ReceiveCallBack(IAsyncResult asyncResult)
    {
        Socket socket = asyncResult.AsyncState as Socket;

        int byteSize = -1;
        try
        {
            // 受信を待機
            byteSize = socket.EndReceive(asyncResult);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return;
        }

        // 受信したデータがある場合、その内容を表示する
        // 再度非同期での受信を開始する
        if (byteSize > 0)
        {
            client.Work(Encoding.UTF8.GetString(this.Buffer, 0, byteSize));
            ///再度、非同期の受信受付開始
            socket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, ReceiveCallBack, socket);
        }
    }

    
}
