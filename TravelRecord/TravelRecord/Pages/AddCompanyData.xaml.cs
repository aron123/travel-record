using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite; //DEBUG

namespace TravelRecord
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCompanyData : ContentPage
	{
		public AddCompanyData()
		{
            Title = "Cégadatok";
			InitializeComponent();
		}

        private void Button_SaveCompanyData()
        {
            Application.Current.Properties["CompanyName"] = CompanyName.Text;
            Application.Current.Properties["CompanyAddress"] = CompanyAddress.Text;
            Application.Current.Properties["CompanyVAT"] = CompanyVAT.Text;
        }
    }
}