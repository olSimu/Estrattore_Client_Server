using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
namespace ServerNumeri
{
    public partial class Form1 : Form
    {
        // Incoming data from the client.  
        public static string data = null;
        public Form1()
        {
            InitializeComponent();


        }
        public void StartListening()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the   
            // host running the application.  
            IPAddress ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    data = null;
                    string controlString;

                    // An incoming connection needs to be processed.  
                    while (true)
                    {

                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }


                    }




                    // Echo the data back to the client.
                    Random rand = new Random();
                    string[] ss = data.Split('\n');
                    int min = Convert.ToInt32(ss[0]);
                    int max = Convert.ToInt32(ss[1]);
                    bool statoRichiestaBool = false;
                    string statoRichiesta = "Rifiutata";
                    string username = ss[2];
                    string password = ss[3];

                    if (controllo_password(username, password))
                    {
                        statoRichiesta = "Accettata";
                        statoRichiestaBool = true;
                    }

                    string richiesta = (username + "\t\t" + min.ToString() + "\t\t" + max.ToString() + "\t\t" + statoRichiesta);
                    /*txt_min.Text = ss[0];
                    txt_max.Text = ss[1];*/
                    //controlString = ss[2];
                    listBox1.Items.Add(richiesta);
                    listBox1.Refresh();
                    if (statoRichiestaBool)
                    {
                        int estratto = rand.Next(min, max);

                        byte[] msg = Encoding.ASCII.GetBytes(estratto.ToString());

                        // Show the data on the console.  
                        //Console.WriteLine("Text received : {0}", data);
                        Console.WriteLine(min.ToString() + max.ToString() + estratto.ToString());
                        Thread.Sleep(20000);
                        handler.Send(msg);
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }




        }

        private void btn_avviaServer_Click(object sender, EventArgs e)
        {
            StartListening();
        }

        private bool controllo_password(string username, string password)
        {

            string Path = @"..\..\..\log.csv";
            using (StreamReader sr = File.OpenText(Path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] strinSplit = s.Split(';');
                    //MessageBox.Show(username + " " + strinSplit[1] + " " + password + " " + strinSplit[0]);
                    if (strinSplit[0] == username && strinSplit[1] == password)
                        return true;
                }
            }
            return false;
        }


    }
}



