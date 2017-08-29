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
        /// <summary>
        /// Initialize AddCompanyData with empty entries
        /// </summary>
		public AddCompanyData()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Initialize AddCompanyData page with prefilled entries
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="vatnumber"></param>
        public AddCompanyData(string name, string address, string vatnumber)
        {
            InitializeComponent();

            CompanyName.Text = name;
            CompanyAddress.Text = address;
            CompanyVAT.Text = vatnumber;
        }

        private void Button_SaveCompanyData()
        {
            Application.Current.Properties["CompanyName"] = CompanyName.Text;
            Application.Current.Properties["CompanyAddress"] = CompanyAddress.Text;
            Application.Current.Properties["CompanyVAT"] = CompanyVAT.Text;

            Navigation.PopAsync();
        }
    }
}