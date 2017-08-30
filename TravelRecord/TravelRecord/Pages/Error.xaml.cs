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
    public partial class Error : ContentPage
    {
        /// <summary>
        /// Initialize an Error page with the give title and message.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public Error(string title, string message)
        {
            InitializeComponent();
            ErrorTitle.Text = title;
            ErrorMessage.Text = message;
        }
    }
}