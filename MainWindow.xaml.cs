using mshtml;
using System;
using System.IO;
using System.Linq;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace lecteurEpub
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        string page { get; set; }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory+"/../files/");
            Lfile.ItemsSource = dir.GetFiles("*.*").Where(s => s.Extension == ".htm" | s.Extension == ".html" | s.Extension == ".xhtml").ToList();
            wb1.LoadCompleted += Wb1_LoadCompleted;
            synth.SetOutputToDefaultAudioDevice();

        }
        SpeechSynthesizer synth=new SpeechSynthesizer();
        private void choixpage()
        {
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync(tb1.Text);
        }
  
        private void Wb1_LoadCompleted(object sender, NavigationEventArgs e)
        {

             //choixpage();
            HTMLDocument htmlDocument = (HTMLDocument)wb1.Document;
            string title=  htmlDocument.title;

            tb1.Text = htmlDocument.documentElement.innerText;
            tb1.Text = tb1.Text.Replace(title, "");
            choixpage();
        }

        private void Lfile_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
             wb1.Navigate(new Uri(((FileInfo)(Lfile.SelectedItem)).FullName, UriKind.Absolute));
      }
        private void changevoix()
        {


          
            InstalledVoice[] voices = synth.GetInstalledVoices().ToArray();
            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                VoiceInfo info = voice.VoiceInfo;
                string AudioFormats = "";
                foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                {
                    AudioFormats += String.Format("{0}\n",
                    fmt.EncodingFormat.ToString());
                }

                Console.WriteLine(" Name:          " + info.Name);
                Console.WriteLine(" Culture:       " + info.Culture);
                Console.WriteLine(" Age:           " + info.Age);
                Console.WriteLine(" Gender:        " + info.Gender);
                Console.WriteLine(" Description:   " + info.Description);
                Console.WriteLine(" ID:            " + info.Id);
                Console.WriteLine(" Enabled:       " + voice.Enabled);
                if (info.SupportedAudioFormats.Count != 0)
                {
                    Console.WriteLine(" Audio formats: " + AudioFormats);
                }
                else
                {
                    Console.WriteLine(" No supported audio formats found");
                }

                string AdditionalInfo = "";
                foreach (string key in info.AdditionalInfo.Keys)
                {
                    AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                }

                Console.WriteLine(" Additional Info - " + AdditionalInfo);
                Console.WriteLine();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            changevoix();
            synth.SelectVoice("Microsoft Zira Desktop");
        }
    }
}
