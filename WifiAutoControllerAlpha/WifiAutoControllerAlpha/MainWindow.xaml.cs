using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;

namespace WifiAutoControllerAlpha
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient tcp;
        NetworkStream stream;
        byte[] buffer=new byte[256];
        public MainWindow()
        {
            InitializeComponent();
            tcp = new TcpClient();
            tcp.Connect(IPAddress.Parse("192.168.4.1"), 333);
            stream = tcp.GetStream();
        }

        private void LeftValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            leftLabel.Content = leftSlider.Value.ToString();
            if (leftSlider.Value >=0)
            {
                buffer[0] = 4;
                buffer[1] = (byte)leftSlider.Value;
                stream.Write(buffer, 0, 2);
            } 
            else
            {
                buffer[0] = 7;
                buffer[1] = (byte)(-leftSlider.Value);
                stream.Write(buffer, 0, 2);
            }
        }

        private void MiddleValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            middleLabel.Content = middleSlider.Value.ToString();
            if (middleSlider.Value >= 0)
            {
                buffer[0] = 6;
                buffer[1] = (byte)middleSlider.Value;
                stream.Write(buffer, 0, 2);
            }
            else
            {
                buffer[0] = 9;
                buffer[1] = (byte)(-middleSlider.Value);
                stream.Write(buffer, 0, 2);
            }
        }

        private void RightValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rightLabel.Content = rightSlider.Value.ToString();
            if (rightSlider.Value >= 0)
            {
                buffer[0] = 5;
                buffer[1] = (byte)rightSlider.Value;
                stream.Write(buffer, 0, 2);
            }
            else
            {
                buffer[0] = 8;
                buffer[1] = (byte)(-rightSlider.Value);
                stream.Write(buffer, 0, 2);
            }
        }

        private void LButtonClick(object sender, RoutedEventArgs e)
        {
            buffer[0] = 1;
            stream.Write(buffer, 0, 1);
        }

        private void MButtonClick(object sender, RoutedEventArgs e)
        {
            buffer[0] = 3;
            stream.Write(buffer, 0, 1);
        }

        private void RButtonClick(object sender, RoutedEventArgs e)
        {
            buffer[0] = 2;
            stream.Write(buffer, 0, 1);
        }

        private void PWMValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PWM.Content = PWMSlider.Value.ToString();
        }

        bool wDown, sDown, aDown, dDown;

        void WDown()
        {
            if (wDown) return;
            wDown = true;
            if(aDown)
            {

            }
            else if(dDown)
            {

            }
            else
            {
                buffer[0] = 6;
                buffer[1] = (byte)PWMSlider.Value;
                stream.Write(buffer, 0, 2);
            }
        }

        void WUp()
        {
            wDown = false;
            if (aDown)
            {

            }
            else if (dDown)
            {

            }
            else
            {
                buffer[0] = 6;
                buffer[1] = 0;
                stream.Write(buffer, 0, 2);
            }
        }

        void SDown()
        {
            if (sDown) return;
            sDown = true;
            if (aDown)
            {

            }
            else if (dDown)
            {

            }
            else
            {
                buffer[0] = 9;
                buffer[1] = (byte)PWMSlider.Value;
                stream.Write(buffer, 0, 2);
            }
        }

        void SUp()
        {
            sDown = false;
            if (aDown)
            {

            }
            else if (dDown)
            {

            }
            else
            {
                buffer[0] = 9;
                buffer[1] = 0;
                stream.Write(buffer, 0, 2);
            }
        }

        void ADown()
        {
            if (aDown) return;
            aDown = true;
            if (wDown || sDown)
            {
                buffer[0] = 4;
                buffer[1] = 0;
                stream.Write(buffer, 0, 2);
            }
        }

        void AUp()
        {
            aDown = false;
            if (wDown)
            {
                buffer[0] = 4;
                buffer[1] = (byte)PWMSlider.Value;
                stream.Write(buffer, 0, 2);
            } else if (sDown)
            {
                buffer[0] = 7;
                buffer[1] = (byte)PWMSlider.Value;
                stream.Write(buffer, 0, 2);
            }
        }

        void DDown()
        {
            if (dDown) return;
            dDown = true;
            if (wDown || sDown)
            {
                buffer[0] = 5;
                buffer[1] = 0;
                stream.Write(buffer, 0, 2);
            }
        }

        void DUp()
        {
            dDown = false;
            if (wDown)
            {
                buffer[0] = 5;
                buffer[1] = (byte)PWMSlider.Value;
                stream.Write(buffer, 0, 2);
            }
            else if (sDown)
            {
                buffer[0] = 8;
                buffer[1] = (byte)PWMSlider.Value;
                stream.Write(buffer, 0, 2);
            }
        }

        private void WinKeyDown(object sender, KeyEventArgs e)
        {
            PWMSlider.IsEnabled = false;
            switch(e.Key)
            {
                case Key.W:
                    WDown();
                    break;
                case Key.S:
                    SDown();
                    break;
                case Key.A:
                    ADown();
                    break;
                case Key.D:
                    DDown();
                    break;
            }
        }

        private void WinKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    WUp();
                    break;
                case Key.S:
                    SUp();
                    break;
                case Key.A:
                    AUp();
                    break;
                case Key.D:
                    DUp();
                    break;
            }
            if (!(wDown || sDown || aDown || dDown))
                PWMSlider.IsEnabled = true;
        }

        private void WMouseDown(object sender, MouseButtonEventArgs e)
        {
            WDown();
        }

        private void WMouseUp(object sender, MouseButtonEventArgs e)
        {
            WUp();
        }

        private void SMouseDown(object sender, MouseButtonEventArgs e)
        {
            SDown();
        }

        private void SMouseUp(object sender, MouseButtonEventArgs e)
        {
            SUp();
        }

        private void AMouseDown(object sender, MouseButtonEventArgs e)
        {
            ADown();
        }

        private void AMouseUp(object sender, MouseButtonEventArgs e)
        {
            AUp();
        }

        private void DMouseDown(object sender, MouseButtonEventArgs e)
        {
            DDown();
        }

        private void DmouseUp(object sender, MouseButtonEventArgs e)
        {
            DUp();
        }
    }
}
