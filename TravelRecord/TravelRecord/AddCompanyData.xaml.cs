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

        //FOR DEBUG
        private void DEBUGButton_CheckData(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("CompanyName")
                && Application.Current.Properties.ContainsKey("CompanyAddress")
                && Application.Current.Properties.ContainsKey("CompanyVAT"))
            {
                var name = Application.Current.Properties["CompanyName"] as string;
                var address = Application.Current.Properties["CompanyAddress"] as string;
                var vat = Application.Current.Properties["CompanyVAT"] as string;

                DisplayAlert("Data is", string.Format("Name: {0}, \nAddress: {1}, \nVAT: {2}", name, address, vat), "OK");
            }
            else
                DisplayAlert("NINCS TÁROLT ADAT", "", "ok");
        }
        //
    }
}