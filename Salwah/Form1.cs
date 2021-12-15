using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeleSharp.TL;
using TeleSharp.TL.Contacts;
using TeleSharp.TL.Upload;
using TLSharp.Core;

namespace Salwah
{
    public partial class Form1 : Form
    {

        int apiId = 11855965; //API ID
        string apiHash = "45210851de36ff7adb3400cb97beea44"; //TOKEN
        TelegramClient client;
        string myPhoneNumber = "+9647505667916"; //PHONE NUMBER
        string hash;
        TLContacts tLContact;
        string userIDSelected;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Button1_ClickAsync(object sender, EventArgs e)
        {
            File.Delete("session.dat");
            client = new TelegramClient(apiId, apiHash);
            await client.ConnectAsync();
            hash = await client.SendCodeRequestAsync(myPhoneNumber);
        }

        private async void Button2_Click(object sender, EventArgs e)
        {


            await client.MakeAuthAsync(myPhoneNumber, hash, textBox1.Text);
            tLContact = await client.GetContactsAsync();

            foreach (TLUser user in tLContact.Users)
            {
                if (!string.IsNullOrEmpty(user.Phone))
                {
                    var fullName = $"{user.FirstName} {user.LastName}";
                    var phone = user.Phone;
                    string[] row = { fullName, phone, user.Id.ToString() };
                    var listviewItem = new ListViewItem(row);
                    listView1.Items.Add(listviewItem);
                }

            }

        }

        //public Image ReadImageFromUser(TLUser user)
        //{
        //    //if (user.Photo == null) return Properties.Resources.
        //    var photo = ((TLUserProfilePhoto)user.Photo);
        //    var photoLocation = (TLFileLocation)photo.PhotoBig;

        //    TLFile file = client.GetFile(new TLInputFileLocation()
        //    {
        //        LocalId = photoLocation.LocalId,
        //        Secret = photoLocation.Secret,
        //        VolumeId = photoLocation.VolumeId
        //    }, 1024 * 256).Result;

        //    Image image;
        //    using (var m = new MemoryStream(file.Bytes))
        //    {
        //        image = Image.FromStream(m);
        //    }
        //    return image;
        //}

        private async void Button3_Click(object sender, EventArgs e)
        {
            await client.SendMessageAsync(new TLInputPeerUser() { UserId = Convert.ToInt32(userIDSelected) }, textBox2.Text);
        }


        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            ListViewItem item = listView1.SelectedItems[0];
            var phone = item.SubItems[1].Text;
            userIDSelected = item.SubItems[2].Text;

            var user = tLContact.Users
               .Where(x => x.GetType() == typeof(TLUser))
               .Cast<TLUser>()
               .FirstOrDefault(x => x.Phone == phone);

            //var userPhoto = ReadImageFromUser(user);
            //pictureBox1.Image = userPhoto;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
