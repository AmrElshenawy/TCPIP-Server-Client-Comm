# TCP/IP Server Client Socket Program
**This is a C# executable socket program with a Server and Client configuration for TCP/IP communication over a network**.

There are two executables for the Server and Client each. Start off by initiating the server first and then having the client to connect to the network. 
You'll know that the server was initiated successfully if it outputs **"Listening..."**. From the Client side, you'll know that a connection was made when it outputs 
**"Connected to Server!"**. From the Server point of view, you'll see a **"Client connected."** output. From there on out, the Server and Client can engnage in a 2-way communication.

This application is coded in Windows Form Applications to provide intuitive executables when running the Server/Client.

## Technicalities
The code is written with two additional threads running in Asynchronous mode. When a client connection is detected, a separate thread runs in an infinite loop to constantly listen
for the client. This goes on as long as the client is connected.

The second thread is used to handle the outgoing data stream sent back to the client. This thread or backgroundworker is disabled as soon as the data is sent.
