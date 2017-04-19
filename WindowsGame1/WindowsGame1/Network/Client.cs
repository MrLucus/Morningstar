using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Morningstar.Views.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WindowsGame1.Views.Assets;

namespace Morningstar
{
    class Client
    {
        //czas po jakim nastepuje rozlaczenie jezeli nie zostana wyslane oczekiwane dane
        const int timeoutValue = 20; //(ms)
        const int PORT = 8080;
        const string SERVER_IP = "127.0.0.1";

        Controller controller;

        TcpClient serverSocket;
        NetworkStream networkStream;
        StreamWriter streamWriter;

        UdpClient udpReceiver;


        public Client(Controller cl)
        {
            controller = cl;
            Console.WriteLine("Initiated!");
        }
    /*    public static bool checkServer()
        {
            try
            {
                TcpClient tmp=new TcpClient(SERVER_IP, PORT);
                tmp.Close();
                return true;
            }
            catch(SocketException se)
            {
                return false;
            }
        }*/

        public bool connectToServer()
        {
            //polaczenie klienta z serverrem
            try
            {
                serverSocket = new TcpClient(SERVER_IP, PORT);
                
            }
            catch(SocketException se)
            {
                return false;
            }
            Thread receivingDataThread = new Thread(receiveData);
            receivingDataThread.Start();

            networkStream = serverSocket.GetStream();
            networkStream.ReadTimeout = timeoutValue;

            streamWriter = new StreamWriter(networkStream);
            streamWriter.AutoFlush = true;

            Console.WriteLine("Client has connected to server");
            return true;
        }

        public void receiveData()
        {
            udpReceiver = new UdpClient();

            IPEndPoint remoteEP = new IPEndPoint(0, PORT);
            udpReceiver.ExclusiveAddressUse = false;
            udpReceiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpReceiver.Client.Bind(remoteEP);

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                //odbieranie danych
                try
                {
                    //sprawdzenie czy klient jest polaczony
                    if (!serverSocket.Connected)
                    {
                        serverSocket.Close();
                        controller.backToMenu();
                        break;
                    }

                    Byte[] receiveBytes = udpReceiver.Receive(ref RemoteIpEndPoint);
                    int assetCount = BitConverter.ToInt32(receiveBytes, 0);

                    List<Asset> receivedAssets = new List<Asset>();

                    for (int i = 0; i < assetCount; i++)
                    {
                        receiveBytes = udpReceiver.Receive(ref RemoteIpEndPoint);
                        string data = Encoding.ASCII.GetString(receiveBytes);
                        string[] dataSeparated = data.Split('|');


                        string header = dataSeparated[0];
                        if (header == "SCORE")
                        {
                            string nick = dataSeparated[1];
                            int points = int.Parse(dataSeparated[2]);
                            int id = int.Parse(dataSeparated[3]);
                            int kills = int.Parse(dataSeparated[4]);
                            int deads = int.Parse(dataSeparated[5]);
                            receivedAssets.Add(new ScoreAsset(header, nick, points, id, kills, deads));
                        }
                        else if (header == "SOUNDS")
                        {
                            string type = dataSeparated[1];
                            receivedAssets.Add(new ListenableAsset(header, type));
                        }
                        else
                        {
                            string type = dataSeparated[1];
                            float x = float.Parse(dataSeparated[2]);
                            float y = float.Parse(dataSeparated[3]);


                            receivedAssets.Add(new DrawableAsset(header, type, new Vector2(x, y)));
                        }
                    }

                    controller.updateAssets(receivedAssets);
                }
                //na wypadek uszkodzenia pakietow
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("NO DATA RECEIVED");
                };
            }

        }

        public void sendAction(KeyboardState keyboard)
        {
            try
            {
                if (keyboard.GetPressedKeys().Length > 0)
                {
                    Keys[] keys = keyboard.GetPressedKeys();
                    String pressedKeys = "";

                    for (int i = 0; i < keys.Length; i++) pressedKeys += keys[i];

                    lock (streamWriter)
                    {
                        streamWriter.WriteLine("KEYBOARD");
                        streamWriter.WriteLine(pressedKeys);
                    }
                }
            }
            catch (Exception e) { }


        }

        public void sendAction(MouseState mouse)
        {
            try
            {
                lock (streamWriter)
                {
                    streamWriter.WriteLine("MOUSE");
                    streamWriter.WriteLine(mouse.X);
                    streamWriter.WriteLine(mouse.Y);
                }
            }
            catch (Exception e) { }
        }

        public void sendAction(String nick)
        {
            try
            {
                lock (streamWriter)
                {
                    streamWriter.WriteLine("PLAYER");
                    streamWriter.WriteLine(nick);
                }
            }
            catch (Exception e) { }
        }


    }
}
