using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbakTools.Web.Components.ModalDialog
{
    public class ModalDialogService : IModalDialogService
    {
        internal event Action<string, RenderFragment, ModalDialogParameters> OnShow;
        internal event Action OnClose;

		private Type _modalType;

		public void Show<T>(string title, ModalDialogParameters parameters) where T : ComponentBase
		{
			Show(typeof(T), title, parameters);
		}

		public void Show(Type componentType, string title, ModalDialogParameters parameters)
		{
			if (!typeof(ComponentBase).IsAssignableFrom(componentType))
			{
				throw new ArgumentException($"{componentType.FullName} must be a Blazor Component");
			}

			var content = new RenderFragment(x => { x.OpenComponent(1, componentType); x.CloseComponent(); });
			_modalType = componentType;

			OnShow?.Invoke(title, content, parameters);
		}

		public void Close()
		{
			OnClose?.Invoke();
		}
	}
}
