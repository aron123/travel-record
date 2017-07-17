using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecord
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCompanyData : ContentPage
	{
		public AddCompanyData ()
		{
            Title = "Cégadatok";
			InitializeComponent ();
		}

        private void Button_AddCompanyDataToDB(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}