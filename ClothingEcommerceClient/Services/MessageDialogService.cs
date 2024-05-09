﻿using ClothingEcommerceClient.Pages.OtherPages;

namespace ClothingEcommerceClient.Services
{
    public class MessageDialogService
    {
        public MessageDialog? messageDialog;
        public bool ShowBusyButton { get; set; }
        public bool ShowSaveButton { get; set; }
        public Action? Action { get; set; }
        public async void SetMessageDialog()
        {
            await messageDialog!.ShowMessage();
            ShowBusyButton = false;
            ShowSaveButton = true;
            Action?.Invoke();
        }
    }
}
