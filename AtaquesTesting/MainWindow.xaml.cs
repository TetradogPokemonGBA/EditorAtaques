using Microsoft.Win32;
using PokemonGBAFrameWork;
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
using Gabriel.Cat.Extension;
using Gabriel.Cat;

namespace AtaquesTesting
{
	/// <summary>
	/// Lógica de interacción para MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        RomData romData;
        Ataque[] ataques;
        Ataque ataqueActual;
        public MainWindow()
		{
			ContextMenu menu = new ContextMenu();
			MenuItem item = new MenuItem();
			InitializeComponent();
			item.Header = "Load";
			item.Click += Load;
			menu.Items.Add(item);
			ContextMenu = menu;
            lstAtaques.SelectionChanged += PonAtaque;
            Closing += (s, e) =>
            {
               
                GuardarRom();

            };
            Load();

            

		}

        private void GuardarRom()
        {
            GuardarCambiosHechos();
            if (romData!=null)
            if (MessageBox.Show("Guardar Cambios?", "Atención", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
            	Ataque.SetAtaques(romData,ataques);
                romData.Rom.Save();
            }
        }

        private void Load(object sender = null, RoutedEventArgs e = null)
		{
			
            StringBuilder strPokemon=new StringBuilder();
			OpenFileDialog opnFile = new OpenFileDialog();
			opnFile.Filter = "Pokemon GBA|*.gba";
            GuardarRom();//guardo la antigua rom
		
			if (opnFile.ShowDialog().GetValueOrDefault()) {

				romData = new RomData(opnFile.FileName);
				ataques=Ataque.GetAtaques(romData);
				cmbTipos.ItemsSource = Tipo.GetTipos(romData); 

               // romData.Pokedex[0].OrdenPokedexNacional = 0;//pongo bien a missigno :D
                //romData.Pokedex.Sort();

                lstAtaques.Items.Clear();
                for (int i = 1; i < ataques.Length; i++)
                {
                    lstAtaques.Items.Add(ataques[i]);

                }
                lstAtaques.SelectedIndex = 0;
 
              
			}
            
            
		}
        private void PonAtaque(object sender, SelectionChangedEventArgs e)
        {
            Ataque ataqueSeleecionado = lstAtaques.SelectedItem as Ataque;
            Image imgPokemon;
            int indexAtaque;
            //guardo los datos anteriores
            if (ataqueSeleecionado != null)
            {
                GuardarCambiosHechos();
                ataqueActual = ataqueSeleecionado;
                //pongo los datos
                indexAtaque =ataques.IndexOf(ataqueSeleecionado)-1;
                cmbTipos.SelectedIndex = ataqueSeleecionado.Datos.Type;

                txtPrioridad.Text = ataqueSeleecionado.Datos.Priority + "";
                txtEfecto.Text = ataqueSeleecionado.Datos.Effect + "";
                txtExactitud.Text = ataqueSeleecionado.Datos.Accuracy + "";
                txtPrecisionExactitud.Text = ataqueSeleecionado.Datos.EffectAccuracy + "";
                txtPp.Text = ataqueSeleecionado.Datos.PP + "";
                txtPoderBase.Text = ataqueSeleecionado.Datos.BasePower + "";
                txtTarget.Text = ataqueSeleecionado.Datos.Target + "";

                txtDescripcion.Text = ataqueSeleecionado.Descripcion;
                txtNombre.Text = ataqueSeleecionado.Nombre;

                chkIsAffectedByKingsRock.IsChecked = ataqueSeleecionado.Datos.IsAffectedByKingsRock;
                chkIsAffectedByMagicCoat.IsChecked = ataqueSeleecionado.Datos.IsAffectedByMagicCoat;
                chkIsAffectedByMirrorWave.IsChecked = ataqueSeleecionado.Datos.IsAffectedByMirrorMove;
                chkIsAffectedByProtect.IsChecked = ataqueSeleecionado.Datos.IsAffectedByProtect;
                chkIsAffectedBySnatch.IsChecked = ataqueSeleecionado.Datos.IsAffectedBySnatch;
                chkMakeContact.IsChecked = ataqueSeleecionado.Datos.MakesContact;

                tapDatosAtaque.Header = "Datos ataque: " + ataqueSeleecionado.Nombre;

               /* uniPokemonQueUsanElAtaque.Children.Clear();
                for (int i = 0; i < romData.Pokedex.Count; i++)
                {
                    if (romData.Pokedex[i].AtaquesAprendidos.EstaElAtaque(indexAtaque))
                    {
                        imgPokemon = new Image();
                        imgPokemon.SetImage(romData.Pokedex[i].Sprites.ImagenFrontalNormal);
                        uniPokemonQueUsanElAtaque.Children.Add(imgPokemon);
                    }
                }
                tapPokemonQueLoUsan.Header = "Pokemon que lo aprenden: " + uniPokemonQueUsanElAtaque.Children.Count;*/
            }
        }

        private void GuardarCambiosHechos()
        {
            if(ataqueActual!=null)
            {
                ataqueActual.Datos.Priority=byte.Parse(txtPrioridad.Text);
                ataqueActual.Datos.Type=(byte)cmbTipos.SelectedIndex;

                ataqueActual.Datos.Effect=byte.Parse(txtEfecto.Text);
                ataqueActual.Datos.Accuracy=byte.Parse(txtExactitud.Text);
                ataqueActual.Datos.EffectAccuracy=byte.Parse(txtPrecisionExactitud.Text);
                ataqueActual.Datos.PP=byte.Parse(txtPp.Text);
                ataqueActual.Datos.BasePower=byte.Parse(txtPoderBase.Text);
                ataqueActual.Datos.Target=byte.Parse(txtTarget.Text);

                ataqueActual.Descripcion.Texto= txtDescripcion.Text;
                ataqueActual.Nombre.Texto = txtNombre.Text;

                ataqueActual.Datos.IsAffectedByKingsRock= chkIsAffectedByKingsRock.IsChecked.GetValueOrDefault();
                ataqueActual.Datos.IsAffectedByMagicCoat= chkIsAffectedByMagicCoat.IsChecked.GetValueOrDefault();
                ataqueActual.Datos.IsAffectedByMirrorMove= chkIsAffectedByMirrorWave.IsChecked.GetValueOrDefault();
                ataqueActual.Datos.IsAffectedByProtect= chkIsAffectedByProtect.IsChecked.GetValueOrDefault();
                ataqueActual.Datos.IsAffectedBySnatch= chkIsAffectedBySnatch.IsChecked.GetValueOrDefault();
                ataqueActual.Datos.MakesContact= chkMakeContact.IsChecked.GetValueOrDefault();

                lstAtaques.Items.Refresh();
            }
        }
    }
}
