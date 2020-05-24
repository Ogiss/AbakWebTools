using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace AbakTools.Web.Components.ModalDialog
{
    public partial class ModalDialog : IDisposable
    {
        [Inject]
        private IModalDialogService ModalService { get; set; }

        private bool disposed;
        private bool IsVisible { get; set; }
        private string Title { get; set; }
        private RenderFragment Content { get; set; }
        private ModalDialogParameters Parameters { get; set; }

        protected override void OnInitialized()
        {
            ((ModalDialogService)ModalService).OnShow += ModalDialog_OnShow;
            ((ModalDialogService)ModalService).OnClose += ModalDialog_OnClose;
        }

        private async void ModalDialog_OnShow(string title, RenderFragment content, ModalDialogParameters parameters)
        {
            Title = title;
            Content = content;
            Parameters = parameters;
            IsVisible = true;

            await InvokeAsync(StateHasChanged);
        }

        private async void ModalDialog_OnClose()
        {
            IsVisible = false;
            Title = "";
            Content = null;

            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool userCall)
        {
            if (!disposed)
            {
                disposed = true;
                ((ModalDialogService)ModalService).OnShow -= ModalDialog_OnShow;
                ((ModalDialogService)ModalService).OnClose -= ModalDialog_OnClose;
            }
        }

    }
}
