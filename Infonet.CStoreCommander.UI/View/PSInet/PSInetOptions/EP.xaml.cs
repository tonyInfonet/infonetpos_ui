﻿using GalaSoft.MvvmLight.Ioc;
using Infonet.CStoreCommander.UI.Utility;
using Infonet.CStoreCommander.UI.ViewModel.PSInet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Infonet.CStoreCommander.UI.View.PSInet.PSInetOptions
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EP : Page
    {
        
        public PaymentSourceVM PaymentSourceVM { get; set; } = SimpleIoc.Default.GetInstance<PaymentSourceVM>();
        
        public EP()
        {
            this.InitializeComponent();
            this.DataContext = PaymentSourceVM;
            
        }

        

        
        
    }
}
