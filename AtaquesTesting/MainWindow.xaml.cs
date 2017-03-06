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
                romData.SetRomData();
                romData.RomGBA.Guardar();
            }
        }

        private void Load(object sender = null, RoutedEventArgs e = null)
		{
			
            StringBuilder strPokemon=new StringBuilder();
			OpenFileDialog opnFile = new OpenFileDialog();
			opnFile.Filter = "Pokemon GBA|*.gba";
            GuardarRom();//guardo la antigua rom
			lstAtaques.Items.Clear();
			if (opnFile.ShowDialog().GetValueOrDefault()) {

				romData = new RomData(opnFile.FileName);

                cmbTipos.ItemsSource = romData.Tipos;

                romData.Pokedex[0].OrdenPokedexNacional = 0;//pongo bien a missigno :D
                romData.Pokedex.Sort();
                for (int i = 1; i < romData.Ataques.Count; i++)
                {
                    lstAtaques.Items.Add(romData.Ataques[i]);

                }
                lstAtaques.SelectedIndex = 0;
 
              
			}
            
            
		}
        private void PonAtaque(object sender, SelectionChangedEventArgs e)
        {
            Ataque ataqueSeleecionado = lstAtaques.SelectedItem as Ataque;
            Image imgPokemon;
            List<Pokemon> pokemonQueUsanElAtaque;
            //guardo los datos anteriores
            if (ataqueSeleecionado != null)
            {
                GuardarCambiosHechos();
                ataqueActual = ataqueSeleecionado;
                //pongo los datos

                cmbTipos.SelectedIndex = ataqueSeleecionado.DatosAtaque.Type;

                txtPrioridad.Text = ataqueSeleecionado.DatosAtaque.Priority + "";
                txtEfecto.Text = ataqueSeleecionado.DatosAtaque.Effect + "";
                txtExactitud.Text = ataqueSeleecionado.DatosAtaque.Accuracy + "";
                txtPrecisionExactitud.Text = ataqueSeleecionado.DatosAtaque.EffectAccuracy + "";
                txtPp.Text = ataqueSeleecionado.DatosAtaque.PP + "";
                txtPoderBase.Text = ataqueSeleecionado.DatosAtaque.BasePower + "";
                txtTarget.Text = ataqueSeleecionado.DatosAtaque.Target + "";

                txtDescripcion.Text = ataqueSeleecionado.Descripcion;
                txtNombre.Text = ataqueSeleecionado.Nombre;

                chkIsAffectedByKingsRock.IsChecked = ataqueSeleecionado.DatosAtaque.IsAffectedByKingsRock;
                chkIsAffectedByMagicCoat.IsChecked = ataqueSeleecionado.DatosAtaque.IsAffectedByMagicCoat;
                chkIsAffectedByMirrorWave.IsChecked = ataqueSeleecionado.DatosAtaque.IsAffectedByMirrorMove;
                chkIsAffectedByProtect.IsChecked = ataqueSeleecionado.DatosAtaque.IsAffectedByProtect;
                chkIsAffectedBySnatch.IsChecked = ataqueSeleecionado.DatosAtaque.IsAffectedBySnatch;
                chkMakeContact.IsChecked = ataqueSeleecionado.DatosAtaque.MakesContact;

                tapDatosAtaque.Header = "Datos ataque: " + ataqueSeleecionado.Nombre;

                uniPokemonQueUsanElAtaque.Children.Clear();
                pokemonQueUsanElAtaque = romData.Pokedex.Filtra((pokemon) => pokemon.AtaquesAprendidos.Ataques.Filtra((ataqueAprendido) => ataqueAprendido.Ataque == romData.Ataques.IndexOf(ataqueSeleecionado)).Count > 0);
                tapPokemonQueLoUsan.Header = "Pokemon que lo aprenden: " + pokemonQueUsanElAtaque.Count;
                for (int i = 0; i < pokemonQueUsanElAtaque.Count; i++)
                {
                    imgPokemon = new Image();
                    imgPokemon.SetImage(pokemonQueUsanElAtaque[i].Sprites.ImagenFrontalNormal);
                    uniPokemonQueUsanElAtaque.Children.Add(imgPokemon);
                }
            }
        }

        private void GuardarCambiosHechos()
        {
            if(ataqueActual!=null)
            {
                ataqueActual.DatosAtaque.Priority=byte.Parse(txtPrioridad.Text);
                ataqueActual.DatosAtaque.Type=(byte)cmbTipos.SelectedIndex;

                ataqueActual.DatosAtaque.Effect=byte.Parse(txtEfecto.Text);
                ataqueActual.DatosAtaque.Accuracy=byte.Parse(txtExactitud.Text);
                ataqueActual.DatosAtaque.EffectAccuracy=byte.Parse(txtPrecisionExactitud.Text);
                ataqueActual.DatosAtaque.PP=byte.Parse(txtPp.Text);
                ataqueActual.DatosAtaque.BasePower=byte.Parse(txtPoderBase.Text);
                ataqueActual.DatosAtaque.Target=byte.Parse(txtTarget.Text);

                ataqueActual.Descripcion.Texto= txtDescripcion.Text;
                ataqueActual.Nombre.Texto = txtNombre.Text;

                ataqueActual.DatosAtaque.IsAffectedByKingsRock= chkIsAffectedByKingsRock.IsChecked.GetValueOrDefault();
                ataqueActual.DatosAtaque.IsAffectedByMagicCoat= chkIsAffectedByMagicCoat.IsChecked.GetValueOrDefault();
                ataqueActual.DatosAtaque.IsAffectedByMirrorMove= chkIsAffectedByMirrorWave.IsChecked.GetValueOrDefault();
                ataqueActual.DatosAtaque.IsAffectedByProtect= chkIsAffectedByProtect.IsChecked.GetValueOrDefault();
                ataqueActual.DatosAtaque.IsAffectedBySnatch= chkIsAffectedBySnatch.IsChecked.GetValueOrDefault();
                ataqueActual.DatosAtaque.MakesContact= chkMakeContact.IsChecked.GetValueOrDefault();

                lstAtaques.Items.Refresh();
            }
        }
    }
}
