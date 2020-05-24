using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace AbakTools.Web.Components.ModalDialog
{
    public interface IModalDialogService
    {
        void Show<T>(string title, ModalDialogParameters parameters) where T : ComponentBase;
        void Close();
    }
}
