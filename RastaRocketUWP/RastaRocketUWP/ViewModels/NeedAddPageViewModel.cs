using RastaRocketUWP.Helpers;
using RastaRocketUWP.Models;
using RastaRocketUWP.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace RastaRocketUWP.ViewModels
{
    public class NeedAddPageViewModel : Observable
    {
        private DateTime _startAtLatest;
        public DateTime StartAtLatest
        {
            get { return _startAtLatest; }
            set { Set(ref _startAtLatest, value);  }
        }

        private List<CustomerModel> _customersSuggest;
        public List<CustomerModel> CustomerSuggest
        {
            get { return _customersSuggest; }
            set { Set(ref _customersSuggest, value); }
        }

        public List<String> WeekFrequencys { get { return new List<string> { "1", "2", "3", "4", "5" }; ; } }

        private string _selectedFrequency;
        public string SelectedFrequency
        {
            get { return _selectedFrequency; }
            set { Set(ref _selectedFrequency, value); }
        }

        private string _title;
        public String Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        private string _monthDuration;
        public String MonthDuration
        {
            get { return _monthDuration; }
            set { Set(ref _monthDuration, value); }
        }

        private string _rate;
        public String Rate
        {
            get { return _rate; }
            set { Set(ref _rate, value); }
        }

        private string _succesKey1;
        public String SuccessKey1
        {
            get { return _succesKey1; }
            set { Set(ref _succesKey1, value); }
        }

        private string _succesKey2;
        public String SuccessKey2
        {
            get { return _succesKey2; }
            set { Set(ref _succesKey2, value); }
        }

        private string _description;
        public String Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        private string _succesKey3;
        public String SuccessKey3
        {
            get { return _succesKey3; }
            set { Set(ref _succesKey3, value); }
        }

        private List<PersonModel> _contactsSuggest;
        public List<PersonModel> ContactsSuggest
        {
            get { return _contactsSuggest; }
            set { Set(ref _contactsSuggest, value); }
        }

        private CustomerModel _selectedCustomer;
        public CustomerModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { Set(ref _selectedCustomer, value); }
        }

        private PersonModel _selectedContact;
        public PersonModel SelectedContact
        {
            get { return _selectedContact; }
            set { Set(ref _selectedContact, value); }
        }

        private PersonModel _selectedConsultant1;
        public PersonModel SelectedConsultant1
        {
            get { return _selectedConsultant1; }
            set { Set(ref _selectedConsultant1, value); }
        }

        private PersonModel _selectedConsultant2;
        public PersonModel SelectedConsultant2
        {
            get { return _selectedConsultant2; }
            set { Set(ref _selectedConsultant2, value); }
        }

        private PersonModel _selectedConsultant3;
        public PersonModel SelectedConsultant3
        {
            get { return _selectedConsultant3; }
            set { Set(ref _selectedConsultant3, value); }
        }

        private PersonModel _selectedConsultant4;
        public PersonModel SelectedConsultant4
        {
            get { return _selectedConsultant4; }
            set { Set(ref _selectedConsultant4, value); }
        }

        private PersonModel _selectedConsultant5;
        public PersonModel SelectedConsultant5
        {
            get { return _selectedConsultant5; }
            set { Set(ref _selectedConsultant5, value); }
        }

        public ICommand WeekFrequencySelectionChangedCommand { get; private set; }
        public ICommand AddClickCommand { get; private set; }

        public ICommand CancelClickCommand { get; private set; }

        private APIService _api;

        public NeedAddPageViewModel()
        {
            StartAtLatest = DateTime.Now;
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
        }

        private void OnWeekFrequencySelectionChanged(SelectionChangedEventArgs args)
        {
            var selectedFreq = args.AddedItems[0] as String;
            SelectedFrequency = selectedFreq;
        }

        public async void CustomerSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var search_term = args.QueryText.ToLower();
            if (SelectedCustomer == null || search_term != SelectedCustomer.Name.ToLower())
            {
                List<CustomerModel> results = await _api.AutoSuggestCustomerWithRetryAsync(search_term);
                CustomerSuggest = results;
                sender.IsSuggestionListOpen = true;
            }
            
        }

        public async void CustomerSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.CheckCurrent())
            {
                var search_term = sender.Text.ToLower();
                List<CustomerModel> results = await _api.AutoSuggestCustomerWithRetryAsync(search_term);
                CustomerSuggest = results;
            }
        }

        public void CustomerSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            CustomerModel customer = args.SelectedItem as CustomerModel;
            sender.Text = customer.Name;
            SelectedCustomer = customer;
        }

        public async void ContactSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var search_term = args.QueryText.ToLower();
            if (SelectedContact == null || search_term != SelectedContact.Name.ToLower())
            {
                List<PersonModel> results = await _api.AutoSuggestContactWithRetryAsync(search_term);
                ContactsSuggest = results;
                sender.IsSuggestionListOpen = true;
            }
        }

        public async void ContactSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.CheckCurrent())
            {
                var search_term = sender.Text.ToLower();

                if (SelectedContact == null || search_term != SelectedContact.Name.ToLower())
                {
                    List<PersonModel> results = await _api.AutoSuggestContactWithRetryAsync(search_term);
                    ContactsSuggest = results;
                }                
            }
        }

        public void ContactSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            PersonModel contact = args.SelectedItem as PersonModel;
            sender.Text = contact.Name;
            SelectedContact = contact;
        }



        public async void ConsultantSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            PersonModel currentConsultant = null;
            switch (sender.Name)
            {
                case "ConsultantAutoSuggest1":
                    currentConsultant = SelectedConsultant1;
                    break;

                case "ConsultantAutoSuggest2":
                    currentConsultant = SelectedConsultant2;
                    break;

                case "ConsultantAutoSuggest3":
                    currentConsultant = SelectedConsultant3;
                    break;

                case "ConsultantAutoSuggest4":
                    currentConsultant = SelectedConsultant4;
                    break;

                case "ConsultantAutoSuggest5":
                    currentConsultant = SelectedConsultant5;
                    break;
            }

            var search_term = args.QueryText.ToLower();
            if (currentConsultant == null || search_term != currentConsultant.Name.ToLower())
            {
                List<PersonModel> results = await _api.AutoSuggestConsultantWithRetryAsync(search_term);
                sender.ItemsSource = results;
                sender.IsSuggestionListOpen = true;
            }
        }

        public async void ConsultantSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.CheckCurrent())
            {
                var search_term = sender.Text.ToLower();

                PersonModel currentConsultant = null;
                Debug.WriteLine(sender.Name);
                switch (sender.Name)
                {
                    case "ConsultantAutoSuggest1":
                        currentConsultant = SelectedConsultant1;
                        break;

                    case "ConsultantAutoSuggest2":
                        currentConsultant = SelectedConsultant2;
                        break;

                    case "ConsultantAutoSuggest3":
                        currentConsultant = SelectedConsultant3;
                        break;

                    case "ConsultantAutoSuggest4":
                        currentConsultant = SelectedConsultant4;
                        break;

                    case "ConsultantAutoSuggest5":
                        currentConsultant = SelectedConsultant5;
                        break;
                }

                if (currentConsultant == null || search_term != currentConsultant.Name.ToLower())
                {
                    List<PersonModel> results = await _api.AutoSuggestConsultantWithRetryAsync(search_term);
                    sender.ItemsSource = results;
                }
            }
        }

        public void ConsultantSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            PersonModel consultant = args.SelectedItem as PersonModel;
            sender.Text = consultant.Name;
            switch (sender.Name)
            {
                case "ConsultantAutoSuggest1":
                    SelectedConsultant1 = consultant;
                    break;

                case "ConsultantAutoSuggest2":
                    SelectedConsultant2 = consultant;
                    break;

                case "ConsultantAutoSuggest3":
                    SelectedConsultant3 = consultant;
                    break;

                case "ConsultantAutoSuggest4":
                    SelectedConsultant4 = consultant;
                    break;

                case "ConsultantAutoSuggest5":
                    SelectedConsultant5 = consultant;
                    break;
            }
        }
    }
}
