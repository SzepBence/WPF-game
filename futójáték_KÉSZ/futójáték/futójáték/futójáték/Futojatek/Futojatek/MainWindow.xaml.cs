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
using System.Windows.Threading;

namespace Futojatek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer idomero = new DispatcherTimer();

        Rect jatekosHitBox;
        Rect foldHitBox;
        Rect akadalyhitBox;

        bool ugras;
        int ero = 20;
        int sebesseg = 5;
        Random r = new Random();
        bool jatekvege = false;
        double spriteInt = 0;

        ImageBrush jatekosKep = new ImageBrush();
        ImageBrush hatterKep = new ImageBrush();
        ImageBrush akadalyKep = new ImageBrush();

        //int[] akadalyHelyezes = { 320, 310, 300, 305, 315 };
        int[] akadalyHelyezes = { 300, 300, 300, 300, 300 };

        int eredmeny = 0;
        public MainWindow()
        {
            InitializeComponent();
            MyCanvas.Focus();
            idomero.Tick += Engine;
            idomero.Interval = TimeSpan.FromMilliseconds(20);
            hatterKep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.gif"));
            background.Fill = hatterKep;
            background2.Fill = hatterKep;
            StartJatek();
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space && jatekvege)
            {
                StartJatek();
            }
            if(e.Key == Key.Escape)
            {
                if (MessageBox.Show("Valóban ki szeretne lépni?", "Kilépés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Environment.Exit(0);
                }
            }
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space && !ugras && Canvas.GetTop(player) > 260)
            {
                ugras = true;
                ero = 15;
                sebesseg = -12;
                jatekosKep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif"));
            }
        }

        private void StartJatek()
        {
       

            Canvas.SetLeft(background, 0); 
            Canvas.SetLeft(background2, 1262); 

            Canvas.SetLeft(player, 110);
            Canvas.SetTop(player, 140);

            Canvas.SetLeft(obstacle, 950);
            Canvas.SetTop(obstacle, 310);

            futKarakter(1);


            akadalyKep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/obstacle.png"));
            obstacle.Fill = akadalyKep; 

            ugras = false;

            jatekvege = false;

            eredmeny = 0;

            scoreText.Content = "Eredmény: " + eredmeny;

            idomero.Start();
        }

        private void futKarakter(double i)
        {
            switch (i)
            {

                case 1:
                    jatekosKep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_01.gif"));
                    break;
                case 2:
                    jatekosKep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif"));
                    break;
                case 3:
                    jatekosKep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_03.gif"));
                    break;
                case 4:
                    jatekosKep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_04.gif"));
                    break;
                case 5:
                    jatekosKep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_05.gif"));
                    break;
                case 6:
                    jatekosKep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_06.gif"));
                    break;
                case 7:
                    jatekosKep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_07.gif"));
                    break;
                case 8:
                    jatekosKep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_08.gif"));
                    break;

            }

            player.Fill = jatekosKep;
        }

        private void Engine(object sender, EventArgs e)
        {
            Canvas.SetTop(player, Canvas.GetTop(player) + sebesseg);
            Canvas.SetLeft(background, Canvas.GetLeft(background) - 3);
            Canvas.SetLeft(background2, Canvas.GetLeft(background2) - 3);
            Canvas.SetLeft(obstacle, Canvas.GetLeft(obstacle) - 12);
            scoreText.Content = "Eredmény: " + eredmeny;

            jatekosHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            foldHitBox = new Rect(Canvas.GetLeft(ground), Canvas.GetTop(ground), ground.Width, ground.Height);
            akadalyhitBox = new Rect(Canvas.GetLeft(obstacle), Canvas.GetTop(obstacle), obstacle.Width, obstacle.Height);

            if (jatekosHitBox.IntersectsWith(foldHitBox))
            {
                sebesseg = 0;
                Canvas.SetTop(player, Canvas.GetTop(ground) - player.Height);
                ugras = false;
                spriteInt += .5;
                if (spriteInt > 8)
                {
                    spriteInt = 1;
                }
                futKarakter(spriteInt);
            }

            if (jatekosHitBox.IntersectsWith(akadalyhitBox))
            {
                jatekvege = true;
                idomero.Stop();

            }

            if (ugras)
            {
                sebesseg = -9;
                ero--;
            }
            else
            {
                sebesseg = 12;
            }

            if (ero < 0)
            {
                ugras = false;
            }


            if (Canvas.GetLeft(background) < -1262)
            {

                Canvas.SetLeft(background, Canvas.GetLeft(background2) + background2.Width);
            }

            if (Canvas.GetLeft(background2) < -1262)
            {

                Canvas.SetLeft(background2, Canvas.GetLeft(background) + background.Width);
            }

            if (Canvas.GetLeft(obstacle) < -50)
            {

                Canvas.SetLeft(obstacle, 950);
                Canvas.SetTop(obstacle, akadalyHelyezes[r.Next(0, akadalyHelyezes.Length)]);
                eredmeny += 1;
            }

            if (jatekvege)
            {
                obstacle.Stroke = Brushes.Black;
                obstacle.StrokeThickness = 1;


                player.Stroke = Brushes.Red;
                player.StrokeThickness = 1;

                SpaceUjra.Content = "Nyomjon Space-t az újrakezdéshez!";
            }
            else
            {
                player.StrokeThickness = 0;
                obstacle.StrokeThickness = 0;
                SpaceUjra.Content = "";
            }
        }
    }
}
