using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.IO;

using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("DICE")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("ZED1200")]
[assembly: AssemblyProduct("Dice")]
[assembly: AssemblyCopyright("Free")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("0.2.*")]

namespace Kostka_do_gry
{
	//Klasy
	
	public class Liczba_Losowa
	{
		Random zmienna = new Random();
		int ilosc_sicanek_kostki;
		int poczatek_przedzialu;
		
		int wynik_losowan;
		
		public Liczba_Losowa (int ilosc_sicanek_kostki_z_glownej_pentli)
		{
			ilosc_sicanek_kostki = ilosc_sicanek_kostki_z_glownej_pentli;
		}
		public int Losowa()
		{
			poczatek_przedzialu = 1;
			
				wynik_losowan = zmienna.Next(poczatek_przedzialu,ilosc_sicanek_kostki+1);
				return wynik_losowan;
		}
	}
	
	//Sprawdzanie czy dany ciąg znaków to liczba
	public class Czy_To_Liczba
	{
		int ile_liczb;
		char liczba;
		string ciag_kontorlny;
		
		public Czy_To_Liczba (string ciag_znaki)
		{
			ciag_kontorlny = ciag_znaki;
		}
		   
		public char Czy_To_Jest_Liczba ()
		{
		ile_liczb = ciag_kontorlny.Length;
		liczba = 'n';
		// kod ascii liczb to 0 = 48 a 9 = 57, ',' = 44, '-' = 45
		for(int a = 0;a < ile_liczb;a++)
		{
		   if ((int)ciag_kontorlny[a] >= 48 & (int)ciag_kontorlny[a] <= 57)
		   {
			   liczba = 't';
		   }
		   else
		   {
			   liczba = 'n';
			   a = ile_liczb;
		   }
		}
			   return liczba;
		}
	
	}
	
	//Zapisywanie historii losowań do pliku txt.
	public class Zapisywanie_Historii_Losowan
	{
		string FILE_NAME_IN_CLASS;
		string [] ciag_znakow_do_zapisania_classa;
		
		public Zapisywanie_Historii_Losowan (string nazwa_pliku_z_glowenj_pentli, string ciag_znakow_do_zapisania_z_glownej_pentli)
		{
			FILE_NAME_IN_CLASS = nazwa_pliku_z_glowenj_pentli +".txt";
			
			string b = "";
			for (int a = 0;a < FILE_NAME_IN_CLASS.Length;a++)
			{
				if (FILE_NAME_IN_CLASS[a] == ':')
					b = b + '-';
				else
					b = b + FILE_NAME_IN_CLASS[a];
			}
			FILE_NAME_IN_CLASS = b;
			
			ciag_znakow_do_zapisania_classa = new string [2];
			ciag_znakow_do_zapisania_classa[1] = ciag_znakow_do_zapisania_z_glownej_pentli;
			
		}
		
		public void Polecenie_Zapisania_Historii()
		{

			if (File.Exists(FILE_NAME_IN_CLASS))
			{
				ciag_znakow_do_zapisania_classa[0] = System.IO.File.ReadAllText(FILE_NAME_IN_CLASS);
				System.IO.File.WriteAllLines(FILE_NAME_IN_CLASS, ciag_znakow_do_zapisania_classa);
			}
			else
			{
				System.IO.File.WriteAllLines(FILE_NAME_IN_CLASS, ciag_znakow_do_zapisania_classa);
			}
			ciag_znakow_do_zapisania_classa[1] = "";
		}
		
	}
	
	// Ustawienie odczyt zapis

	public class Ustawienia_odczyt_zapis
	{
		string FILE_NAME_PREFERENCES;
		int [] iDane = new int [4];
		public int [] iWartosc = new int [4];
		
		public Ustawienia_odczyt_zapis(int [] iParemetry)
		{
			for(int a = 0;a < iParemetry.Length;a++)
			{
				iDane[a] = iParemetry[a];
			}
			FILE_NAME_PREFERENCES = "data.config";
		}
		
		public Ustawienia_odczyt_zapis()
		{
			
		}
		
		public void Zapis_Konfiguracji ()
		{
			DirectoryInfo dKatalog = new DirectoryInfo("settings");
			if (!dKatalog.Exists)
				dKatalog.Create();
			
			FileStream fs = new FileStream("settings/"+FILE_NAME_PREFERENCES, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			
			BinaryWriter w = new BinaryWriter(fs);
			for (int a = 0;a < iDane.Length;a++)
			{
				w.Write(iDane[a]);
			}
				w.Close();
		}
		
		public void Odczyt_Konfiguracji ()
		{
			FileStream fs = new FileStream("settings/data.config", FileMode.Open, FileAccess.Read);
			
            BinaryReader r = new BinaryReader(fs);
			
			for (int a = 0;a < 4; a++)
			{
				iWartosc[a] = r.ReadInt32();
			}
			r.Close();
		}
	}


	public class Window:Form
	{
		Form Okno_Pomoc;
		
		private GroupBox groupBox1;
		private Label label_Ilosc_Graczy = new Label();
		private Label label_Ilosc_Kostek_Do_Gry = new Label();
		private Label label_Ilosc_Scianek_W_Kostce = new Label();
		private TextBox TextBox_Ilosc_Graczy = new TextBox ();
		private TextBox TextBox_Ilosc_Kostek_Do_Gry;
		private TextBox TextBox_Ilosc_Scianek_W_Kostce;
		private TextBox TextBox_Wyniki_Losowan = new TextBox();
		private Label label_Zapis_Histori;
		private CheckBox CheckBox_Historia;
		private Button button_Rzut_Kostka;
		private Button butto_Nowa_Gra;
		
		private LinkLabel linkLabel_Pomoc_email;
		private LinkLabel linkLabel_Pomoc_www;
		
		int iIloscGraczy = new int ();
		int iNumerTury = new int ();
		int iKtoryToGracz = new int (); // Do wyliczefnia który to gracz i tura
		int iKontrolaIlosciGraczy = new int ();	
		int iKolejnoscGraczy = new int ();
		int iKontrolaKostekDoGry = new int();
		int iKontrolaScianekKostki = new int ();
		int [] liczby = new int [4]; // Dane do class i funkcji oraz do zapisu ostatnich ustawień
		char chNowaGra = new char ();
		
		
		public DateTime AKUTALNY_CZAS = new DateTime();
		
		public string sWynik_losowan;
		public string FILE_NAME = "";
		
		
		private void OnClosing (Object sender, FormClosingEventArgs e)
		{
		if (MessageBox.Show ("Czy mam wyjść z programu?", "Pytanie",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Ustawienia_odczyt_zapis zapis = new  Ustawienia_odczyt_zapis(liczby);
				zapis.Zapis_Konfiguracji();
			}
			else
			{
				e.Cancel = true;
			}
		}		
		private  void  OnWyswietlPomoc(object sender, EventArgs ea)
			{
				string POMOC ="Program służy do \"symulacji rzutów kośćmi do gry\". \r\n\r\nMożliwości programu: to\r\n\r\n- wybór ilości graczy\r\n- wybór ilości kości do gry\r\n- wybór ilości \"ścianek kostki\", czyli zakresu generowanych liczb\r\n"+
				"- zapis do pliku txt historii wyników";
				Okno_Pomoc = new Form();
				Okno_Pomoc.Icon = new Icon("settings/DICE.ico");
				Okno_Pomoc.Text = "Pomoc"; //tytuł belki
				Okno_Pomoc.Width=515;
				Okno_Pomoc.Height = 340;
			
				TextBox TextBox_Pomoc;
				TextBox_Pomoc = new TextBox();
				TextBox_Pomoc.Size = new Size(500,200);
				TextBox_Pomoc.Multiline = true;
				TextBox_Pomoc.ReadOnly = true;
				TextBox_Pomoc.ScrollBars = ScrollBars.Vertical;
				TextBox_Pomoc.AppendText(POMOC);
				
				linkLabel_Pomoc_email = new LinkLabel();
				linkLabel_Pomoc_email.Location = new System.Drawing.Point(34, 215);
				linkLabel_Pomoc_email.AutoSize = true;
				linkLabel_Pomoc_email.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel_Pomoc_email_LinkClicked);
				linkLabel_Pomoc_email.Text = "email: zed1200@interia.pl";
				
				linkLabel_Pomoc_www = new LinkLabel();
				linkLabel_Pomoc_www.Location = new System.Drawing.Point(34, 235);
				linkLabel_Pomoc_www.AutoSize = true;
				linkLabel_Pomoc_www.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel_Pomoc_www_LinkClicked);
				linkLabel_Pomoc_www.Text = "https://github.com/Zed1200";
				
				Button button_Zamkniece_Pomocy;
				button_Zamkniece_Pomocy = new Button();
				button_Zamkniece_Pomocy.Parent = this;
				button_Zamkniece_Pomocy.Top = 260;
				button_Zamkniece_Pomocy.Left = 210;
				button_Zamkniece_Pomocy.Text = "OK";
				button_Zamkniece_Pomocy.Click += new System.EventHandler(On_Okno_Przebieg_Pomoc_Zamknij);
				
				Okno_Pomoc.Controls.Add(TextBox_Pomoc);
				Okno_Pomoc.Controls.Add(linkLabel_Pomoc_email);
				Okno_Pomoc.Controls.Add(linkLabel_Pomoc_www);
				Okno_Pomoc.Controls.Add(button_Zamkniece_Pomocy);
				
				Okno_Pomoc.Show();
			}
			
		private  void  On_Okno_Przebieg_Pomoc_Zamknij(object sender, EventArgs ea)
		{
			Okno_Pomoc.Close();
		}
		
		private void linkLabel_Pomoc_email_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			linkLabel_Pomoc_email.LinkVisited = true;
			System.Diagnostics.Process.Start("mailto: zed1200@interia.pl");
		}
		
		private void linkLabel_Pomoc_www_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			linkLabel_Pomoc_www.LinkVisited = true;
			System.Diagnostics.Process.Start("https://github.com/Zed1200");
		}
		
		private void OnWyjdz(object sender, EventArgs ea)
			{
				Ustawienia_odczyt_zapis zapis = new  Ustawienia_odczyt_zapis(liczby);
				zapis.Zapis_Konfiguracji();
				Application.Exit();
			}
		private void OnButtonlClick(object sender, EventArgs ea) 
			{
			Uzycie_klawisza_OK ();
			}
			
		private void OnButtonNowaGra(object sender, EventArgs ea) 
			{
			NowaGra ();
			}
			
		private void UzycieKlawisza(object sender, KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Enter)
					Uzycie_klawisza_OK ();
			}
			
		//Gdy użyjemy zatwierdzenia np. klawisz enter lub button	
		public void Uzycie_klawisza_OK ()
		{
			string ciag_znakow;
			//Wprowadzamy liczby by sprawdzić czy są większe od zera
			ciag_znakow = TextBox_Ilosc_Graczy.Text;
			Czy_To_Liczba to_liczba_1 = new Czy_To_Liczba(ciag_znakow);
			if (to_liczba_1.Czy_To_Jest_Liczba()=='t')
				liczby[0] = int.Parse(TextBox_Ilosc_Graczy.Text);
			
			ciag_znakow = TextBox_Ilosc_Kostek_Do_Gry.Text;
			Czy_To_Liczba to_liczba_2 = new Czy_To_Liczba(ciag_znakow);
			if (to_liczba_2.Czy_To_Jest_Liczba()=='t')
				liczby[1] = int.Parse(TextBox_Ilosc_Kostek_Do_Gry.Text);
			
			ciag_znakow = TextBox_Ilosc_Scianek_W_Kostce.Text;
			Czy_To_Liczba to_liczba_3 = new Czy_To_Liczba(ciag_znakow);
			if (to_liczba_3.Czy_To_Jest_Liczba()=='t')
				liczby[2] = int.Parse(TextBox_Ilosc_Scianek_W_Kostce.Text);
			
			if (to_liczba_1.Czy_To_Jest_Liczba()=='t'& to_liczba_2.Czy_To_Jest_Liczba() == 't'& to_liczba_3.Czy_To_Jest_Liczba() == 't'& liczby[0] > 0 & liczby[1] > 0 & liczby[2] > 0)
			{
				if (chNowaGra == 't')
				{
					iNumerTury = 1;
					iKtoryToGracz = 0;
					iKontrolaIlosciGraczy = int.Parse(TextBox_Ilosc_Graczy.Text);
					iKontrolaKostekDoGry = int.Parse(TextBox_Ilosc_Kostek_Do_Gry.Text);
					iKontrolaScianekKostki = int.Parse(TextBox_Ilosc_Scianek_W_Kostce.Text);
					TextBox_Wyniki_Losowan.Clear();
					chNowaGra = 'n';
				}
				Generowanie_liczb_losowych();
				
				if (CheckBox_Historia.Checked == true & FILE_NAME.Length > 0)
				{
					 Zapisywanie_Historii_Losowan zapisz_historie = new Zapisywanie_Historii_Losowan(FILE_NAME, sWynik_losowan);
					 zapisz_historie.Polecenie_Zapisania_Historii();
				}
			}
			else
				MessageBox.Show("Wprowadzone dane nie są liczbami dodatnimi całkowitywmi !!!",  "UWAGA",MessageBoxButtons.OK,MessageBoxIcon.Error);
		}
		
		public void NowaGra ()
		{
			chNowaGra = 't';
			Uzycie_klawisza_OK ();
		}
		
		
		public Window()
		{
			FormClosing += new FormClosingEventHandler (OnClosing);// do zamknięcia okna
			
			int left_label = new int();
			int top_1 = new int ();
			int top_2 = new int ();
			int top_3 = new int ();
			int top_4 = new int ();
			int textbox_left = new int();
			
				// Parematry dla startu programu
				liczby[0] = 2;
				liczby[1] = 1;
				liczby[2] = 6;
				liczby[3] = 0;
			
			top_1 = 25;
			top_2 = 50;
			top_3 = 75;
			top_4 = 100;
			left_label = 20;
			textbox_left = 170;
			
		iNumerTury = 1;
		iIloscGraczy = 0;
		iKtoryToGracz = 0;
		
		if (File.Exists("settings/data.config"))
		{
			Ustawienia_odczyt_zapis odczyt = new  Ustawienia_odczyt_zapis();
			odczyt.Odczyt_Konfiguracji();
			
			// Odzytanie parametrów do startu programu
			liczby[0] = odczyt.iWartosc[0];
			liczby[1] = odczyt.iWartosc[1];
			liczby[2] = odczyt.iWartosc[2];
			liczby[3] = odczyt.iWartosc[3];
		}
		else
		{
			iKontrolaIlosciGraczy = 2;
			iKontrolaKostekDoGry = 1;
			iKontrolaScianekKostki = 6;
		}	
		
		chNowaGra = 'n';
			
			this.Width=345;
			this.Height=600;
			this.Icon = new Icon("settings/DICE.ico");
			this.Text = "DICE v 0.2"; //tytuł belki 
			
			//Menu
			MainMenu mm = new MainMenu();
			MenuItem miPlik = new MenuItem("Plik");
	
			MenuItem miPomoc = new MenuItem("Pomoc");
			MenuItem miWyjdz = new MenuItem("Wyjdź"); 
	
			miPomoc.Click += new EventHandler(OnWyswietlPomoc);	
			miWyjdz.Click +=new EventHandler(OnWyjdz);
	
			mm.MenuItems.Add(miPlik);
			miPlik.MenuItems.Add(miPomoc);
			miPlik.MenuItems.Add(miWyjdz);
	
			Menu = mm;
			
			//GroupBox
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.Location = new System.Drawing.Point(15, 15);
			this.groupBox1.Size = new System.Drawing.Size(300, 135);
			this.groupBox1.Text = "Parametry kostek do gry";
			this.groupBox1.Font = new Font("Arial",8);
			this.Controls.Add(this.groupBox1);
			
			//Label

			label_Ilosc_Graczy.Text = "Ilość graczy:"; 
			label_Ilosc_Graczy.Top = top_1; 
			label_Ilosc_Graczy.Left = left_label+55; 
			label_Ilosc_Graczy.AutoSize = true;
			label_Ilosc_Graczy.Font = new Font("Arial",10);
			this.groupBox1.Controls.Add(this.label_Ilosc_Graczy);			

			label_Ilosc_Kostek_Do_Gry.Text = "Ilość kostek do gry:"; 
			label_Ilosc_Kostek_Do_Gry.Top = top_2; 
			label_Ilosc_Kostek_Do_Gry.Left = left_label+12; 
			label_Ilosc_Kostek_Do_Gry.AutoSize = true;
			label_Ilosc_Kostek_Do_Gry.Font = new Font("Arial",10);
			this.groupBox1.Controls.Add(this.label_Ilosc_Kostek_Do_Gry);
			
			label_Ilosc_Scianek_W_Kostce.Text = "Ilość ścianek w kostce:"; 
			label_Ilosc_Scianek_W_Kostce.Top = top_3; 
			label_Ilosc_Scianek_W_Kostce.Left = left_label-5; 
			label_Ilosc_Scianek_W_Kostce.AutoSize = true;
			label_Ilosc_Scianek_W_Kostce.Font = new Font("Arial",10);
			this.groupBox1.Controls.Add(this.label_Ilosc_Scianek_W_Kostce);
			
			label_Zapis_Histori = new Label();
			label_Zapis_Histori.Text = "Zapis historii:"; 
			label_Zapis_Histori.Top = top_4; 
			label_Zapis_Histori.Left = left_label+52; 
			label_Zapis_Histori.AutoSize = true;
			label_Zapis_Histori.Font = new Font("Arial",10);
			this.groupBox1.Controls.Add(this.label_Zapis_Histori);
			
			//TextBox
			
			TextBox_Ilosc_Graczy = new TextBox();
			TextBox_Ilosc_Graczy.Top = top_1;
			TextBox_Ilosc_Graczy.Left = textbox_left;
			TextBox_Ilosc_Graczy.TextAlign = HorizontalAlignment.Center;
			TextBox_Ilosc_Graczy.Text = liczby[0].ToString();
			TextBox_Ilosc_Graczy.KeyDown +=UzycieKlawisza;
			this.groupBox1.Controls.Add(TextBox_Ilosc_Graczy);
			
			TextBox_Ilosc_Kostek_Do_Gry = new TextBox();
			TextBox_Ilosc_Kostek_Do_Gry.Top = top_2;
			TextBox_Ilosc_Kostek_Do_Gry.Left = textbox_left;
			TextBox_Ilosc_Kostek_Do_Gry.TextAlign = HorizontalAlignment.Center;
			TextBox_Ilosc_Kostek_Do_Gry.Text = liczby[1].ToString();
			TextBox_Ilosc_Kostek_Do_Gry.KeyDown +=UzycieKlawisza;
			this.groupBox1.Controls.Add(TextBox_Ilosc_Kostek_Do_Gry);
			
			TextBox_Ilosc_Scianek_W_Kostce = new TextBox();
			TextBox_Ilosc_Scianek_W_Kostce.Top = top_3;
			TextBox_Ilosc_Scianek_W_Kostce.Left = textbox_left;
			TextBox_Ilosc_Scianek_W_Kostce.TextAlign = HorizontalAlignment.Center;
			TextBox_Ilosc_Scianek_W_Kostce.Text = liczby[2].ToString();
			TextBox_Ilosc_Scianek_W_Kostce.KeyDown +=UzycieKlawisza;
			this.groupBox1.Controls.Add(TextBox_Ilosc_Scianek_W_Kostce);
			
			//CheckBox
			CheckBox_Historia = new CheckBox();
            CheckBox_Historia.AutoSize = true;
            CheckBox_Historia.Location = new Point(textbox_left, top_4+2);
            CheckBox_Historia.Size = new Size(40, 17);
			if (liczby[3] == 1)
				CheckBox_Historia.Checked = true; // Przycisk zaznaczony
			else
				CheckBox_Historia.Checked = false;
			this.groupBox1.Controls.Add(CheckBox_Historia);
			
			//Button
			button_Rzut_Kostka = new Button();
			button_Rzut_Kostka.Text ="Rzut kostką"; 
			button_Rzut_Kostka.Left = (ClientSize.Width - button_Rzut_Kostka.Width) / 5; 
			button_Rzut_Kostka.Top = 165; 
			button_Rzut_Kostka.Click +=new EventHandler(OnButtonlClick); 
			Controls.Add(button_Rzut_Kostka);
			
			butto_Nowa_Gra = new Button();
			butto_Nowa_Gra.Text ="Nowa gra"; 
			butto_Nowa_Gra.Left = ((ClientSize.Width - butto_Nowa_Gra.Width) / 5)*4;
			butto_Nowa_Gra.Top = 165; 
			butto_Nowa_Gra.Click +=new EventHandler(OnButtonNowaGra); 
			Controls.Add(butto_Nowa_Gra);
			
			TextBox_Wyniki_Losowan.Location = new System.Drawing.Point(0, 205);
			TextBox_Wyniki_Losowan.Size = new Size(330,335);
			TextBox_Wyniki_Losowan.Multiline = true;
			TextBox_Wyniki_Losowan.CausesValidation = false;
			TextBox_Wyniki_Losowan.ReadOnly = true;
			TextBox_Wyniki_Losowan.ScrollBars = ScrollBars.Vertical;
			TextBox_Wyniki_Losowan.WordWrap = false;
			this.Controls.Add(TextBox_Wyniki_Losowan);
		}
		
		// Funkcje
		
		public void Generowanie_liczb_losowych()
		{
			iKtoryToGracz++;
			iIloscGraczy = int.Parse(TextBox_Ilosc_Graczy.Text);
			iKolejnoscGraczy = (iKtoryToGracz%iIloscGraczy);
			
			if (iKontrolaIlosciGraczy != int.Parse(TextBox_Ilosc_Graczy.Text) | (iKontrolaKostekDoGry != int.Parse(TextBox_Ilosc_Kostek_Do_Gry.Text)) | (iKontrolaScianekKostki != int.Parse(TextBox_Ilosc_Scianek_W_Kostce.Text)))
			{
				// uważać by resetować dane przy zmianie danych iKontrola i dane do porównania !!!	
					iNumerTury = 1;
					iKtoryToGracz = 0;
					iKontrolaIlosciGraczy = int.Parse(TextBox_Ilosc_Graczy.Text);
					iKontrolaKostekDoGry = int.Parse(TextBox_Ilosc_Kostek_Do_Gry.Text);
					iKontrolaScianekKostki = int.Parse(TextBox_Ilosc_Scianek_W_Kostce.Text);
					TextBox_Wyniki_Losowan.Clear();
					chNowaGra = 'n';
					Generowanie_liczb_losowych();
					
			}
			else
			{
				Liczba_Losowa wyniki = new Liczba_Losowa(int.Parse(TextBox_Ilosc_Scianek_W_Kostce.Text));
				int [] wynik_losowania= new int[int.Parse(TextBox_Ilosc_Kostek_Do_Gry.Text)];
				sWynik_losowan = "";
			
				if ((iKolejnoscGraczy) == 0)
				{
					sWynik_losowan = sWynik_losowan + "Numer tury gry: "+iNumerTury + "\r\n";
					iNumerTury++;
					iKolejnoscGraczy = iIloscGraczy;
				}
				else
				{
					sWynik_losowan = sWynik_losowan + "Numer tury gry: "+iNumerTury + "\r\n";
				}
				
				sWynik_losowan = sWynik_losowan + "Numer Gracza: "+iKolejnoscGraczy+"\r\n\r\n";			
			
				AKUTALNY_CZAS = DateTime.Now; // Aktualny czas
				
				if (CheckBox_Historia.Checked == true & FILE_NAME.Length == 0)
				{
					FILE_NAME = AKUTALNY_CZAS.ToString("yyyy/MM/dd HH:mm:ss");
					liczby[3] = 1;
				}
					

				if (CheckBox_Historia.Checked == false)
				{
					FILE_NAME = "";
					liczby[3] = 0;
				}
						
				sWynik_losowan = sWynik_losowan + (AKUTALNY_CZAS.ToString("yyyy/MM/dd HH:mm:ss"))+ "\r\n" +"\r\n"; 
			
				for (int aa = 0;aa < int.Parse(TextBox_Ilosc_Kostek_Do_Gry.Text);aa++)
				{
					wynik_losowania[aa]  = wyniki.Losowa(); 
					sWynik_losowan = sWynik_losowan +"Kostka nr "+((aa)+1)+ " wynik: "+wynik_losowania[aa]+"\r\n";
				}
				TextBox_Wyniki_Losowan.AppendText(sWynik_losowan);
				TextBox_Wyniki_Losowan.AppendText(Environment.NewLine);
			}
		}
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.Run (new Window());
		}
	}
}