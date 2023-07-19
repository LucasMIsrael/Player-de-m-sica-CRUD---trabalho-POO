using Microsoft.Win32;
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

namespace ListaDeMusicas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Musica> listaMusica = new List<Musica>();

        public MainWindow()
        {
            InitializeComponent();

            listaMusica.Add(new Musica() { codigo = "1", titulo = "Move to the city", banda = "Guns N' Roses", album = "GNR Lies" });         //musicas pré-definidas
            listaMusica.Add(new Musica() { codigo = "2", titulo = "N.I.B", banda = "Black Sabbath", album = "Black Sabbath" });
            listaMusica.Add(new Musica() { codigo = "3", titulo = "Best of You", banda = "Foo Fighters", album = "In Your Honor" });
            listaMusica.Add(new Musica() { codigo = "4", titulo = "Nightrain", banda = "Guns N' Roses", album = "Appetite For Destruction" });
            listaMusica.Add(new Musica() { codigo = "5", titulo = "Bohemian Rhapsody", banda = "Queen", album = "A Night At The Opera" });
            listaMusica.Add(new Musica() { codigo = "6", titulo = "Scary Little Green Men", banda = "Ozzy Osbourne", album = "Ordinary Man" });

            DTGmusicas.ItemsSource = listaMusica;
            DTGmusicas.Items.Refresh();
            navegar();
        }

        MediaPlayer mediaPlayer = new MediaPlayer();

        #region Navegar/Editar
        public void navegar()
        {
            DTGmusicas.IsEnabled = true;
            BTNalterar.IsEnabled = true;
            BTNcadastrar.IsEnabled = true;
            BTNdeletar.IsEnabled = true;

            BTNgravar.IsEnabled = false;
            BTNcancelar.IsEnabled = false;
            TBXcodigo.IsEnabled = false;
            TBXnome.IsEnabled = false;
            TBXbanda.IsEnabled = false;
            TBXalbum.IsEnabled = false;
        }

        public void editar(bool alterar = false)
        {
            DTGmusicas.IsEnabled = false;
            BTNalterar.IsEnabled = false;
            BTNcadastrar.IsEnabled = false;
            BTNdeletar.IsEnabled = false;

            BTNgravar.IsEnabled = true;
            BTNcancelar.IsEnabled = true;
            if (!alterar) TBXcodigo.IsEnabled = true;
            TBXnome.IsEnabled = true;
            TBXbanda.IsEnabled = true;
            TBXalbum.IsEnabled = true;
        }
        #endregion

        #region Botões CRUD
        private void BTNalterar_Click(object sender, RoutedEventArgs e)
        {
            if (listaMusica.Count > 0)
            {
                editar(true);
                TBXnome.Focus();
            }
        }

        private void BTNcadastrar_Click(object sender, RoutedEventArgs e)
        {
            editar();

            TBXcodigo.Text = string.Empty;                  //string.Empty; == ------ = "";  (atribuir valor "limpo")
            TBXnome.Text = string.Empty;
            TBXbanda.Text = string.Empty;
            TBXalbum.Text = string.Empty;

            TBXcodigo.Focus();
        }

        private void BTNgravar_Click(object sender, RoutedEventArgs e)
        {
            if (TBXcodigo.IsEnabled)
            {
                if (TBXcodigo.Text != "" && TBXnome.Text != "" && TBXbanda.Text != "" && TBXalbum.Text != "")               // validando se os campos.text possuem valor (diff de vazio)
                {
                    listaMusica.Add(new Musica() { codigo = TBXcodigo.Text, titulo = TBXnome.Text, banda = TBXbanda.Text, album = TBXalbum.Text });
                }
            }
            else
            {
                for (int x = 0; x < listaMusica.Count; x++)
                {
                    if (listaMusica[x].codigo == TBXcodigo.Text)
                    {
                        listaMusica[x].titulo = TBXnome.Text;
                        listaMusica[x].banda = TBXbanda.Text;
                        listaMusica[x].album = TBXalbum.Text;
                        break;
                    }
                }
            }

            navegar();

            DTGmusicas.Items.Refresh();
            DTGmusicas.Focus();
        }

        private void BTNcancelar_Click(object sender, RoutedEventArgs e)
        {
            navegar();
            DTGmusicas.Focus();
        }

        private void BTNdeletar_Click(object sender, RoutedEventArgs e)
        {
            if (listaMusica.Count > 0)
            {
                string confirme = "Tem certeza que deseja excluir essa música?";
                string boxTitle = "Excluir da Lista";

                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage image = MessageBoxImage.Warning;
                MessageBoxResult resposta;

                resposta = MessageBox.Show(confirme, boxTitle, buttons, image, MessageBoxResult.Yes);

                if (resposta == MessageBoxResult.Yes)
                {
                    for (int x = 0; x < listaMusica.Count; x++)      //procura a linha selecionada
                    {
                        if (listaMusica[x].codigo == TBXcodigo.Text)
                        {
                            listaMusica.RemoveAt(x);                 //apaga e sai do for
                            break;
                        }
                    }
                    DTGmusicas.Items.Refresh();

                    if (listaMusica.Count > 0)                       //permite a navegação caso exista item na lista
                    {
                        DTGmusicas.SelectedIndex = 0;
                    }
                    navegar();
                }
            }
        }
        #endregion

        #region Carregar/Atualizar Tela
        private void JanelaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            if (listaMusica.Count > 0)
            {
                DTGmusicas.SelectedIndex = 0;
            }
            DTGmusicas.Focus();
        }

        private void DTGmusicas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateFields();
        }

        private void DTGmusicas_GotFocus(object sender, RoutedEventArgs e)
        {
            updateFields();
        }

        private void updateFields()
        {
            if (DTGmusicas.SelectedIndex > -1)
            {
                Musica mus = (Musica)DTGmusicas.Items[DTGmusicas.SelectedIndex];
                TBXcodigo.Text = mus.codigo;
                TBXnome.Text = mus.titulo;
                TBXbanda.Text = mus.banda;
                TBXalbum.Text = mus.album;
            }

            if (listaMusica.Count == 0)           //caso n tenha nenhum item na lista, n irá aparecer nada nos campos
            {
                TBXcodigo.Text = "";
                TBXnome.Text = "";
                TBXbanda.Text = "";
                TBXalbum.Text = "";
            }
        }
        #endregion

        #region Botões MediaPlayer
        private void BTNabrir_Click(object sender, RoutedEventArgs e)
        {
            string fileName;

            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                DefaultExt = ".mp3"
            };
            bool? dialogOk = fileDialog.ShowDialog();
            if (dialogOk == true)
            {
                fileName = fileDialog.FileName;
                TBXaudio.Text = fileDialog.SafeFileName;
                mediaPlayer.Open(new Uri(fileName));
            }
        }

        private void BTNplay_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }

        private void BTNpause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void BTNstop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }
        #endregion
    }
}
