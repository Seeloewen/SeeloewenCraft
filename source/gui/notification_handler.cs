using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace SeeloewenCraft
{
    public static class NotificationHandler
    {
        //Controls
        static System.Windows.Forms.Timer tmrNotification = new System.Windows.Forms.Timer();
        static Border bdrNotification = new Border() { Width = 375, Height = 75, BorderBrush = new SolidColorBrush(Colors.Gray), BorderThickness = new Thickness(3), Visibility = Visibility.Hidden };
        static Canvas cvsNotification = new Canvas() { Background = new SolidColorBrush(Colors.DarkGray) };
        static TextBlock tblNotification = new TextBlock() { FontSize = 18, Width = 270, Height = 50, TextWrapping = TextWrapping.Wrap };
        static Canvas cvsImage = new Canvas() { Width = 50, Height = 50 };
        static Button btnDismiss = new Button() { Width = 65, Height = 35, Content = "Dismiss", Visibility = Visibility.Hidden };

        //References       
        static public NotificationGui gui;

        //-- Constructor --//

        public static void Init()
        {         
            gui = new NotificationGui( 550, 375, 112, 445, "sc:notifications");

            //Setup necessary components for showing the message
            Canvas.SetTop(bdrNotification, 585);
            Canvas.SetLeft(bdrNotification, 870);
            bdrNotification.Child = cvsNotification;

            Canvas.SetTop(tblNotification, 10);
            Canvas.SetLeft(tblNotification, 75);
            Game.world.wndGame.RemoveFromParent(tblNotification);
            cvsNotification.Children.Add(tblNotification);

            Canvas.SetTop(cvsImage, 10);
            Canvas.SetLeft(cvsImage, 10);
            Game.world.wndGame.RemoveFromParent(cvsImage);
            cvsNotification.Children.Add(cvsImage);

            Canvas.SetTop(btnDismiss, 17);
            Canvas.SetLeft(btnDismiss, 295);
            Game.world.wndGame.RemoveFromParent(btnDismiss);
            cvsNotification.Children.Add(btnDismiss);

            cvsNotification.MouseEnter += cvsNotification_MouseEnter;
            cvsNotification.MouseLeave += cvsNotification_MouseLeave;
            btnDismiss.Click += btnDismiss_Click;
            Game.world.wndGame.RemoveFromParent(bdrNotification);
            Game.world.wndGame.cvsGame.Children.Add(bdrNotification);
        }

        //-- Custom Methods --//

        public static void ShowNotification(string message, int timeShown, ImageBrush image)
        {
            if (Settings.showNotifications)
            {
                //Show the notification 
                tblNotification.Text = message;
                cvsImage.Background = image;

                //Start the timer that will later hide the notification
                bdrNotification.Visibility = Visibility.Visible;
                tmrNotification.Tick += tmrNotification_Tick;
                tmrNotification.Interval = timeShown;
                tmrNotification.Start();
            }

            gui.AddNotification(message, image);

            Log.Write($"Created new notification: {message}", "Info");
        }

        public static void HideNotification()
        {
            //Stop the timer and hide the notification
            bdrNotification.Visibility = Visibility.Hidden;
            tmrNotification.Stop();
        }

        public static void ShowGui()
        {
            //Show the gui that lists all the notifications
            gui.Show();
        }

        public static void HideGui()
        {
            //Hide the gui that lists all the notifications
            gui.Hide();
        }


        //-- Event Handlers --//

        private static void cvsNotification_MouseEnter(object sender, MouseEventArgs e)
        {
            //Show the dismiss button when the mouse is over the notification
            btnDismiss.Visibility = Visibility.Visible;
        }

        private static void cvsNotification_MouseLeave(object sender, MouseEventArgs e)
        {
            //Hide the dismiss button when the mouse leaves the notification
            btnDismiss.Visibility = Visibility.Hidden;
        }

        private static void tmrNotification_Tick(object sender, EventArgs e)
        {
            //When the timer ticks, hide the notification
            HideNotification();
        }

        private static void btnDismiss_Click(object sender, EventArgs e)
        {
            //Also hide the notification when clicking on "Dismiss"
            HideNotification();
        }
    }
}
