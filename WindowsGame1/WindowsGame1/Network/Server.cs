using Microsoft.Xna.Framework;
using Morningstar.Model;
using Morningstar.Model.Entities;
using Morningstar.Views.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WindowsGame1.Views.Assets;

namespace Morningstar
{
    class Server
    {
        //czas po jakim nastepuje rozlaczenie jezeli nie zostana wyslane oczekiwane dane
        public const int timeoutValue = 10; //(ms)
        public bool assetsBusy = false;

        public const int worldUpdatePeriod = 10; //okres czasu (ms) co ktory aktualizuje sie swiat

        IPAddress ip;
        const int PORT = 8080;
        const string SERVER_IP = "127.0.0.1";

        TcpListener serverSocket;
        UdpClient udpServer;

        public World world;

        public Server()
        {
            //inicjalizacja servera
            ip = IPAddress.Parse(SERVER_IP);
            serverSocket = new TcpListener(ip, PORT);                       

            serverSocket.Start();

            world = new World();

            new Thread(beginBroadcast).Start();

            Console.WriteLine("Server launched successfully!");
        }

        public void beginBroadcast()
        {
            var remoteEP = new IPEndPoint(IPAddress.Broadcast, PORT);
            udpServer = new UdpClient();
            udpServer.EnableBroadcast = true;

            while (true)
            {
                if (assetsBusy) continue;
                if (!assetsBusy)
                {
                    assetsBusy = true;

                    world.update();

                    byte[] assetCount = BitConverter.GetBytes(world.assets.Count);
                    udpServer.Send(assetCount, assetCount.Length, remoteEP);


                    foreach (var asset in world.assets)
                    {
                        string data="";
                        switch (asset.header)
                        {
                            case "EVENT":
                                data = $"{asset.header}|{(asset as DrawableAsset).type}|{(asset as DrawableAsset).position.X}|{(asset as DrawableAsset).position.Y}";
                                break;
                            case "ENTITY":
                                data = $"{asset.header}|{(asset as DrawableAsset).type}|{(asset as DrawableAsset).position.X}|{(asset as DrawableAsset).position.Y}";
                                break;
                            case "SCORE":
                                data = $"{asset.header}|{(asset as ScoreAsset).nick}|{(asset as ScoreAsset).points}|{(asset as ScoreAsset).count}|{(asset as ScoreAsset).kills}|{(asset as ScoreAsset).deads}";
                                break;
                            case "SOUNDS":
                                data = $"{asset.header}|{(asset as ListenableAsset).type}";
                                break;
                        }

                        byte[] dataBytes = Encoding.ASCII.GetBytes(data);

                        udpServer.Send(dataBytes, dataBytes.Length, remoteEP);
                    }
                }

                assetsBusy = false;
                Thread.Sleep(worldUpdatePeriod);
            }
        }

        public void waitForPlayers()
        {
            while (true)
            {
                //polaczenie nadchodzacego klienta
                TcpClient client = serverSocket.AcceptTcpClient();
                Player newPlayer = world.newPlayer();
                world.update();              

                handleClient clientConnection = new handleClient();
                clientConnection.startClient(this, newPlayer, client);
            }
        }





    }

    //TODO przy ruchu zamienic asset na playera

    class handleClient
    {
        Server server;
        TcpClient client;
        Player player;

        StreamReader streamReader;


        public void startClient(Server s, Player newPlayer, TcpClient clientSocket)
        {
            server = s;
            client = clientSocket;
            player = newPlayer;

            Thread playerThread = new Thread(doChat);
            playerThread.Start();
        }

        private void doChat()
        {
            //przyjecie wysylanych danych
            NetworkStream networkStream = client.GetStream();
            networkStream.ReadTimeout = Server.timeoutValue;

            streamReader = new StreamReader(networkStream);

            while (true)
            {
                if (networkStream.DataAvailable)
                {
                    if (!player.isAlive)
                    {
                        client.Close();
                        break;
                    }
                    string header;
                    lock (streamReader)
                    {
                        header = Convert.ToString(streamReader.ReadLine());
                        handleHeader(header);
                    }
                }
            }
        }

        private void handleHeader(string header)
        {
            switch (header)
            {
                //klient nacisnal dany klawisz -> nalezy go odpowiednio ruszyc
                case "KEYBOARD":
                    string keys = Convert.ToString(streamReader.ReadLine());

                    /*
                        case "W":
                            player.move(new Vector2(0, -1));
                            break;
                        case "S":
                            player.move(new Vector2(0, 1));
                            break;
                        case "A":
                            player.move(new Vector2(-1, 0));
                            break;
                        case "D":
                            player.move(new Vector2(1, 0));
                            break;
                        case "Tab":
                             Console.WriteLine("wyniki");
                            break;
                            */
                    Vector2 vector = Vector2.Zero;
                    if (keys.Contains("W"))
                        vector.Y -= 1;
                    if (keys.Contains("S"))
                        vector.Y += 1;
                    if (keys.Contains("A"))
                        vector.X -= 1;
                    if (keys.Contains("D"))
                        vector.X += 1;
                    player.move(vector);

                    break;

                case "MOUSE":
                    int x = Convert.ToInt32(streamReader.ReadLine());
                    int y = Convert.ToInt32(streamReader.ReadLine());

                    //server.world.shot(player, new Vector2(x, y));
                    if (server.assetsBusy)
                    {
                        while (true) if (!server.assetsBusy) break;
                    }

                    server.assetsBusy = true;
                    player.shoot(new Vector2(x,y));
                    server.assetsBusy = false;

                    break;

                case "PLAYER":
                    string nickname = Convert.ToString(streamReader.ReadLine());

                    player.setNick(nickname);

                    break;


            }



        }
    }
}