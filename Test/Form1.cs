using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            using(var trkDest = new TruckMileage.Destination("AIzaSyBS5z9Jam1Tu6iiwlQNUFNZKHoujJdpJHo"))
            {
                richTextBox1.Text = trkDest.FindMileage("8820 South Miller Blvd., Oklahoma City, Oklahoma").ToString();
            }

            using (var mpNPlc = new TruckMileage.Address("AIzaSyBS5z9Jam1Tu6iiwlQNUFNZKHoujJdpJHo"))
            {
                mpNPlc.FindPlaceValues("dz comics okc");
            }
        }
    }
}
