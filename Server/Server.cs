using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Server;

public class ServerSocket
{
    private static int _MAX_NUMBER_OF_CONNECTIONS = 10;
    private static int _PORT = 5000;
    private static IPAddress _LOCAL_IP = IPAddress.Parse("127.0.0.1");
    private IPEndPoint _ENDPOINT;
    
    private Socket _listenerSocket;
    private List<Socket> _clientSockets;
    
    public ServerSocket()
    {
        _clientSockets = new List<Socket>();
        _ENDPOINT = new IPEndPoint(_LOCAL_IP, _PORT);

        // AddressFamily.InterNetwork means ipv4
        _listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _listenerSocket.Bind(_ENDPOINT);
        _listenerSocket.Listen(_MAX_NUMBER_OF_CONNECTIONS);
        Console.WriteLine("Server started on {0}", _ENDPOINT);
    }

    public async Task Run()
    {
        while (true)
        {
            Socket clientSocket = await _listenerSocket.AcceptAsync();
                
            _clientSockets.Add(clientSocket);
            Console.WriteLine("Client connected from {0}", clientSocket.RemoteEndPoint);
            break;
        }
    }
    
    public void Close()
    {
        for (int i = 0; i < _clientSockets.Count; i++)
        {
            _clientSockets[i].Shutdown(SocketShutdown.Both);
            _clientSockets[i].Close();
        }
        
        _listenerSocket.Close();
    }
}

