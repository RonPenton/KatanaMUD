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
using System.Text.RegularExpressions;
using KatanaMUD.Importer.Structures;

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
            var context = new GameEntities("Server=KATANAMUD\\SQLEXPRESS;Database=KatanaMUD;integrated security=True;");
            context.LoadFromDatabase();

            //var races = Btrieve.GetAllRaces(new FileInfo(@"C:\Users\spsadmin\Documents\MMUDDats\wccrace2.dat").FullName, context.RaceTemplates);
            //var classes = Btrieve.GetAllClasses(new FileInfo(@"C:\Users\spsadmin\Documents\MMUDDats\wccclas2.dat").FullName, context.ClassTemplates);


            //var rooms = Btrieve.GetAllRooms(new FileInfo(@"C:\CleanP\wccmp002.dat").FullName);

            //Regex r = new Regex("\\s+");
            //var descriptions = rooms.GroupBy(x => x.Description).ToList();//.OrderBy(x => x.Key).ToList();

            //foreach (var group in descriptions)
            //{
            //    var textBlock = context.TextBlocks.New();
            //    textBlock.Text = group.Key;

            //    foreach (var room in group)
            //    {
            //        var dbRoom = room.ToRoom(null);
            //        dbRoom.TextBlock = textBlock;
            //        context.Rooms.Add(dbRoom, false);
            //    }
            //}

            //foreach (var room in rooms)
            //{
            //	var dbRoom = room.ToRoom(context.Rooms.SingleOrDefault(x => x.Id == RoomBuffer.GetRoomNumber(room.MapNumber, room.RoomNumber)));
            //}


            var items = Btrieve.GetAllItems(new FileInfo(@"C:\CleanP\wccitem2.dat").FullName);

            var notdrop = items.Where(x => x.NotDroppable != 0).ToList();
            var retain = items.Where(x => x.RetainAfterUses != 0).ToList();
            var destroy = items.Where(x => x.DestroyOnDeath != 0).ToList();



            //context.RaceTemplates.AddRange(races, true);
            //context.ClassTemplates.AddRange(classes, true);
            context.SaveChanges();
        }
    }
}