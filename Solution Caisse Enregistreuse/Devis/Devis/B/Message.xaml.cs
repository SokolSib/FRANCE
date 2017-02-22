using System.Windows;

namespace Devis.B
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class Message : Window
    {
        private System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();

        private void worker_DoWork_Total(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Class.ClassSync.connect = new Class.ClassDB(null).connect();
        }



        private void worker_RunWorkerCompleted_Total(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            message.AppendText(  Class.ClassSync.connect ? "Vous êtes connectées" : "Vous n'êtes pas connecté");

            bOk.IsEnabled = Class.ClassSync.connect;
        }
        public Message()
        {
            InitializeComponent();
            if (worker.IsBusy)
            {

                worker.Dispose();

                worker = new System.ComponentModel.BackgroundWorker();
            }
            worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork_Total);

            worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted_Total);

            worker.RunWorkerAsync();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();

            Devis w = new Devis();

            w.ShowDialog();
        }
    }
}
