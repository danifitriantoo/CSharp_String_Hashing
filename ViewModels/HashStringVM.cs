using String_Hashing.Models;
using String_Hashing.Setup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace String_Hashing.ViewModels
{
    public class HashStringVM : INotifyPropertyChanged
    {
        public HashStringVM()
        {
            model = new HashString();
            collection = new ObservableCollection<HashString>();

            // Async Task
            ConvertCommand = new RelayCommand(async () => await ConvertStringAsync());
            DecryptCommand = new RelayCommand(async () => await DecryptStringAsync());
        }

        // Command
        public RelayCommand ConvertCommand { get; set; }
        public RelayCommand DecryptCommand { get; set; }

        // Models
        public HashString Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        // Collection
        public ObservableCollection<HashString> Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        //ADD-ON
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action OnCallBack;
        public string hash = "foxle@rn";

        //PRIVATE PRORS
        private HashString model;
        private ObservableCollection<HashString> collection;


        private async Task ConvertStringAsync()
        {
            await Task.Delay(0);
            // Get String
            byte[] data = UTF8Encoding.UTF8.GetBytes($"{model.String}");
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {

                    // Start Encryption
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

                    // Passing Result To Textbox
                    string a = Convert.ToBase64String(result, 0, result.Length);
         
                    collection.Add(new HashString
                    {
                        Hash = a.ToString()
                    });
                }
            }
        }

        private async Task DecryptStringAsync()
        {
            await Task.Delay(0);
            // Get String
            byte[] data = Convert.FromBase64String($"{model.String}");
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {

                    // Start Decryption
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

                    // Passing Result To Textbox
                    string a = UTF8Encoding.UTF8.GetString(result);

                    collection.Add(new HashString
                    {
                        Hash = a.ToString()
                    });
                }
            }
        }


    }
}
