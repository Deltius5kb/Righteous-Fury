using Server;

// See https://aka.ms/new-console-template for more information
ServerSocket server = new ServerSocket();

await server.Run();
Console.WriteLine("Server Finished");