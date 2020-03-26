﻿using System;
using GalaSoft.MvvmLight.Ioc;
using Infonet.CStoreCommander.UI.ViewModel.Reprint;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Infonet.CStoreCommander.UI.View.Reprint
{
    
    public sealed partial class ReceiptPreview : Page
    {
        private ValueSet table = null;
        public ReprintVM ReprintVM { get; set; }
      = SimpleIoc.Default.GetInstance<ReprintVM>();

        public ReceiptPreview()
        {
            this.InitializeComponent();
            
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ReprintVM.PrintReceiptEvent -= RePrintReceipt_Event;
            App.AppServiceConnected -= LastPrint_AppServiceConnected;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ReprintVM.PrintReceiptEvent -= RePrintReceipt_Event;
            App.AppServiceConnected -= LastPrint_AppServiceConnected;
            ReprintVM.PrintReceiptEvent += RePrintReceipt_Event;
            App.AppServiceConnected += LastPrint_AppServiceConnected;
        }
        private async void RePrintReceipt_Event(string text, string ImagePath)
        {
            // create a ValueSet from the datacontext
            table = new ValueSet();
            table.Add("PrintText", text);
            //table.Add("PrintText", "TEST:\r\n");
            if (ImagePath != null)
                table.Add("ImgPath", ImagePath);
            else
                table.Add("ImgPath", null);
            //table.Add("ImgPath", null);
            // launch the fulltrust process and for it to connect to the app service            
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }
            else
            {
                MessageDialog dialog = new MessageDialog("This feature is only available on Windows 10 Desktop SKU");
                await dialog.ShowAsync();
            }
        }

        private async void LastPrint_AppServiceConnected(object sender, EventArgs e)
        {
            // send the ValueSet to the fulltrust process
            AppServiceResponse response = await App.Connection.SendMessageAsync(table);

            // check the result
            MessageDialog dialog;
            object result = null;
            response.Message.TryGetValue("RESPONSE", out result);
            if (result == null)
            {
                dialog = new MessageDialog("Print error.");
                await dialog.ShowAsync();
            }
            else
            {
                if (result.ToString() != "SUCCESS")
                {
                    dialog = new MessageDialog(result.ToString());
                    await dialog.ShowAsync();
                }
            }

            // no longer need the AppService connection
            App.AppServiceDeferral.Complete();
        }
    }
}
