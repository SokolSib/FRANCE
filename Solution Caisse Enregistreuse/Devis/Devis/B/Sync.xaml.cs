using System;
using System.Windows;

namespace Devis.B
{

    /// <summary>
    /// Interaction logic for Sync.xaml
    /// </summary>
    public partial class Sync : Window
    {
        private System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
        private System.ComponentModel.BackgroundWorker workerImg = new System.ComponentModel.BackgroundWorker();

        private void worker_DoWork_connect(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Class.ClassSync.connect = new Class.ClassDB(null).connect();

            

        }

        private void worker_RunWorkerCompleted_connect(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

            message.Content = Class.ClassSync.connect ? "Vous êtes connecté" + Environment.NewLine : "Vous êtes n'est pas connecté" + Environment.NewLine;

            message.Content += Class.ClassProducts.shelfProducts.Count != 0 ? "Не заонченное деви вы не" + Environment.NewLine + "можете сделать синх=ию" + Environment.NewLine  : " полка тораров пуста " + Environment.NewLine;

            bOk.IsEnabled = Class.ClassSync.connect;

            bOk.IsEnabled = Class.ClassProducts.shelfProducts.Count == 0;
        }

        private void worker_DoWork_sync(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            Class.ClassSync.ProductDB.syncAll();

          
        }

        private void worker_RunWorkerCompleted_sunc(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

            //  new Class.ClassSync().SyncAll();

            MainWindow mw = (Owner as MainWindow);

            if (mw != null)

            mw.restart();


            message.Content += "Mise à jour terminée" + Environment.NewLine;

            bOk.IsEnabled = true;

            bCancel.IsEnabled = true;

        }

        private void messageModif (object arg)
        {
            message.Content = arg.ToString();  /*RENA   вылазит ошибка на этом месте после синхронизации с базой данных*/
        }


        /*Задание
        
            1 - Список на полке товаров дожен быть как на кассе, то етсь при добавление товара появление надписи на вверху
            2 - Выделение товара жирным, можно сделать негатив обыного вида
            3 - Удаление товара со списка не работает
            4 - При закрытие программы список товаров должен сохраняться
            5 - После отправки Деви на базу нужно очищать послку товаров

            
            */




        private void worker_DoWork_copyImg(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                Class.ClassF.DirectoryCopy(Class.ClassF.path, Class.ClassF.pathTo, true);
            }
            catch(Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        private void worker_RunWorkerCompleted_copyImg(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

            message.Content += "complete copy img" + Environment.NewLine;

            if (e.Result != null)
            message.Content += e.Result.ToString();
           
        }
      

        public Sync()
        {
            InitializeComponent();

            worker.Dispose();

            worker = new System.ComponentModel.BackgroundWorker();

            worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork_connect);

            worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted_connect);

            worker.RunWorkerAsync();
        }


        private void copyImg ()
        {
            workerImg.Dispose();

            workerImg = new System.ComponentModel.BackgroundWorker();

            workerImg.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork_copyImg);

            workerImg.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted_copyImg);

            workerImg.RunWorkerAsync();
        }

        private void bOk_Click(object sender, RoutedEventArgs e)
        {
            message.Content += "patientez s il vous plait... " + Environment.NewLine;

            message.Content += "synchronisation en cours... " + Environment.NewLine; ;

            bOk.IsEnabled = false;

            bCancel.IsEnabled = false;

            worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork_sync);

            worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted_sunc);

            worker.RunWorkerAsync();

            copyImg();

         
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
         
        }
    }
}
