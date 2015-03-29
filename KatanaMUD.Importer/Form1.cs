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
			var races = Btrieve.GetAllRaces(new FileInfo(@"dats\wccrace2.dat").FullName);
			var classes = Btrieve.GetAllClasses(new FileInfo(@"dats\wccclas2.dat").FullName);


			//IntPtr Racedatabuf = IntPtr.Zero;
			//byte[] RacePosBlock = new byte[128];
			//char[] RaceKeyBuffer = @"E:\PROJECTS\DEVELOPMENT\00-Current\KatanaMUD\KatanaMUD.Importer\bin\Debug\dats\wccrace2.dat".ToCharArray();
			//var dataBuffer = new Btrieve.RaceBuffer();
			//int bufferLength = System.Runtime.InteropServices.Marshal.SizeOf(dataBuffer);

			//int status = Btrieve.BTRCALL(Btrieve.BOPEN, RacePosBlock, ref dataBuffer, ref bufferLength, RaceKeyBuffer, (ushort)RaceKeyBuffer.Length, 0);
			////BTRCALL(BOPEN, RacePosBlock, Racedatabuf, Len(Racedatabuf), ByVal RaceKeyBuffer, KEY_BUF_LEN, 0)


			////var positionBlock = new byte[128];
			////var dataBuffer = new byte[126];
			////var bufferLength = 126;
			////var fileNameArray = @"E:\PROJECTS\DEVELOPMENT\00-Current\KatanaMUD\KatanaMUD.Importer\bin\Debug\dats\wccrace2.dat".ToCharArray();
			////var status = Btrieve.BTRCALL(Btrieve.BOPEN, positionBlock, ref dataBuffer, ref bufferLength, fileNameArray, 0, 0);

			//List<Btrieve.RaceBuffer> list = new List<Btrieve.RaceBuffer>();

			//if (status == 0)
			//{
			//	RaceDataBufSize = 126;
			//	Racedatabuf = IntPtr.Zero;

			//	// Get first record
			//	status = Btrieve.BTRCALL(Btrieve.BGETFIRST, RacePosBlock, ref dataBuffer, ref bufferLength, RaceKeyBuffer, (ushort)RaceKeyBuffer.Length, 0);

			//	list.Add(dataBuffer);

			//	// Get subsequent records
			//	while (status == 0)	// BReturnCodes.END_OF_FILE or an error will occur
			//	{
			//		dataBuffer = new Btrieve.RaceBuffer();
			//		status = Btrieve.BTRCALL(Btrieve.BGETNEXT, RacePosBlock, ref dataBuffer, ref bufferLength, RaceKeyBuffer, (ushort)RaceKeyBuffer.Length, 0);
			//		list.Add(dataBuffer);
			//	}
			//}
			//else
			//{
			//	MessageBox.Show("Error occured while opening file: " + status.ToString());
			//}
		}
	}
}
