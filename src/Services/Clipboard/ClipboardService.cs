using Newtonsoft.Json;
using System.Windows;
using System;

namespace Services.ClipBoard
{
    public class ClipboardService<TModel> : IClipboard<TModel> where TModel : class
    {
        #region Fields

        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        #endregion

        #region Constructor

        public ClipboardService() { }

        #endregion

        #region Methods

        public void Copy(TModel model)
        {
            Clipboard.SetText(JsonConvert.SerializeObject(model, Formatting.Indented, settings));
        }
        
        public TModel Paste()
        {
            try
            {
                return JsonConvert.DeserializeObject<TModel>(Clipboard.GetText(), settings);
            }
            catch (Exception erroreption) { ExceptionService.WriteLine(erroreption); return null; }
        }

        #endregion
    }

    public class ClipboardService : IClipboard
    {
        #region Fields

        private string format = "ClipboardServiceData";

        #endregion

        #region Constructor

        public ClipboardService() { }

        #endregion

        #region Methods

        public void Copy(object model)
        {
            Clipboard.SetData(format, model);
        }
        
        public object Paste()
        {
            if (Clipboard.ContainsData(format))
                return Clipboard.GetData(format);
            else return null;
        }

        #endregion
    }
}