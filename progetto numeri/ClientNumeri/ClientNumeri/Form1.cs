using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ClientNumeri
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //timer1.Start();
            txt_max.Text = 100.ToString();
            txt_min.Text = 1.ToString();

        }

        public void StartClient(byte[] msg)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPAddress ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 5000);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    //byte[] msg = Encoding.ASCII.GetBytes(nMin.ToString() + "\n" + nMax.ToString() + "\n" + username + "\n" + password + "\n" + "<EOF>");
                    //byte[] msg = Encoding.ASCII.GetBytes(txt_min.Text + "\n" + txt_max.Text + "\n" + "<EOF>");

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);
                    txt_estratto.Text = Encoding.ASCII.GetString(bytes, 0, bytesRec);



                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void btn_avviaClient_Click(object sender, EventArgs e)
        {
            //byte[] msg = Encoding.ASCII.GetBytes(txt_min.Text + "\n" + txt_max.Text + "\n" + "<EOF>");
            byte[] msg = Encoding.ASCII.GetBytes(txt_min.Text + "\n" + txt_max.Text + "\n" + txt_username.Text + "\n" + txt_password.Text + "\n" + "<EOF>");

            StartClient(msg);
        }

        private void invia_accesso()
        { }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //StartClient();
        }

       
    }

}




