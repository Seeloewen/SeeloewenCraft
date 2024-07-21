using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace SeeloewenCraft
{
    public class NotificationHandler
    {
        //Controls
        System.Windows.Forms.Timer tmrNotification = new System.Windows.Forms.Timer();
        Border bdrNotification = new Border() { Width = 375, Height = 75, BorderBrush = new SolidColorBrush(Colors.Gray), BorderThickness = new Thickness(3), Visibility = Visibility.Hidden };
        Canvas cvsNotification = new Canvas() { Background = new SolidColorBrush(Colors.DarkGray) };
        TextBlock tblNotification = new TextBlock() { FontSize = 18, Width = 270, Height = 50, TextWrapping = TextWrapping.Wrap };
        Canvas cvsImage = new Canvas() { Width = 50, Height = 50 };
        Button btnDismiss = new Button() { Width = 65, Height = 35, Content = "Dismiss", Visibility = Visibility.Hidden };

        //References
        World world;
        public NotificationGui gui;

        //-- Constructor --//

        public NotificationHandler(World world)
        {
            this.world = world;
            gui = new NotificationGui(world, 600, 375, 117, 400, "sc:notifications", this);

            //Setup necessary components for showing the message
            Canvas.SetTop(bdrNotification, 660);
            Canvas.SetLeft(bdrNotification, 800);
            bdrNotification.Child = cvsNotification;

            Canvas.SetTop(tblNotification, 10);
            Canvas.SetLeft(tblNotification, 75);
            cvsNotification.Children.Add(tblNotification);

            Canvas.SetTop(cvsImage, 10);
            Canvas.SetLeft(cvsImage, 10);
            cvsNotification.Children.Add(cvsImage);

            Canvas.SetTop(btnDismiss, 17);
            Canvas.SetLeft(btnDismiss, 295);
            cvsNotification.Children.Add(btnDismiss);

            cvsNotification.MouseEnter += cvsNotification_MouseEnter;
            cvsNotification.MouseLeave += cvsNotification_MouseLeave;
            btnDismiss.Click += btnDismiss_Click;
            world.wndGame.cvsGame.Children.Add(bdrNotification);
        }

        //-- Custom Methods --//

        public void ShowNotification(string message, int timeShown, ImageBrush image)
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

            world.log.Write($"Created new notification: {message}", "Info");
        }

        public void HideNotification()
        {
            //Stop the timer and hide the notification
            bdrNotification.Visibility = Visibility.Hidden;
            tmrNotification.Stop();
        }

        public void ShowGui()
        {
            //Show the gui that lists all the notifications
            gui.Show();
        }

        public void HideGui()
        {
            //Hide the gui that lists all the notifications
            gui.Hide();
        }


        //-- Event Handlers --//

        private void cvsNotification_MouseEnter(object sender, MouseEventArgs e)
        {
            //Show the dismiss button when the mouse is over the notification
            btnDismiss.Visibility = Visibility.Visible;
        }

        private void cvsNotification_MouseLeave(object sender, MouseEventArgs e)
        {
            //Hide the dismiss button when the mouse leaves the notification
            btnDismiss.Visibility = Visibility.Hidden;
        }

        private void tmrNotification_Tick(object sender, EventArgs e)
        {
            //When the timer ticks, hide the notification
            HideNotification();
        }

        private void btnDismiss_Click(object sender, EventArgs e)
        {
            //Also hide the notification when clicking on "Dismiss"
            HideNotification();
        }
    }
}
