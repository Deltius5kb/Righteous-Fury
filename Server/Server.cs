using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Server;

public class ServerSocket
{
    private const int _MAX_NUMBER_OF_CONNECTIONS = 10;
    private const int _PORT = 5000;
    private readonly IPAddress _LOCAL_IP = IPAddress.Parse("127.0.0.1");
    
    private readonly Socket _listenerSocket;
    private readonly List<Socket> _clientSockets;
    
    public ServerSocket()
    {
        _clientSockets = new List<Socket>();
        IPEndPoint endpoint = new IPEndPoint(_LOCAL_IP, _PORT);

        // AddressFamily.InterNetwork means ipv4
        _listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _listenerSocket.Bind(endpoint);
        _listenerSocket.Listen(_MAX_NUMBER_OF_CONNECTIONS);
        Console.WriteLine("Server started on {0}", endpoint);
    }

    public async Task Run()
    {
        while (true)
        {
            Socket clientSocket = await _listenerSocket.AcceptAsync();
                
            _clientSockets.Add(clientSocket);
            Console.WriteLine("Client connected from {0}", clientSocket.RemoteEndPoint);
            StartWaitingForMessages(clientSocket);
        }
    }

    private async Task StartWaitingForMessages(Socket socket)
    {
        await ReceiveMessageAsync(socket);
    }

    private async Task ReceiveMessageAsync(Socket socket)
    {
        byte[] buffer = new byte[1024];
        int bytesReceived = socket.Receive(buffer);
        
    }
    
    public void Close()
    {
        foreach (var socket in _clientSockets)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        _listenerSocket.Close();
    }
}

