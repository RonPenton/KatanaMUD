using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharp;
using KatanaMUD.Models;

namespace KatanaMUD.Importer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			var races = Btrieve.GetAllRaces(new FileInfo(@"C:\Users\spsadmin\Documents\MMUDDats\wccrace2.dat").FullName);
			var classes = Btrieve.GetAllClasses(new FileInfo(@"C:\Users\spsadmin\Documents\MMUDDats\wccclas2.dat").FullName);

            var context = new GameContext();

            context.RaceTemplates.AddRange(races);
            context.ClassTemplates.AddRange(classes);
            context.SaveChanges();
		}
	}
}
