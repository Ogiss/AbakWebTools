using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbakTools.Web.Components.ModalDialog
{
    public class ModalDialogParameters
    {
        private Dictionary<string, object> parameters = new Dictionary<string, object>();


        public static ModalDialogParameters Create(string name, object value)
        {
            return new ModalDialogParameters().Add(name, value);
        }

        public ModalDialogParameters Add(string name, object value)
        {
            parameters[name] = value;

            return this;
        }

        public T Get<T>(string name)
        {
            if (parameters.ContainsKey(name))
            {
                return (T)parameters[name];
            }
            return default(T);
        }
    }
}
