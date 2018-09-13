using Newtonsoft.Json;
using System.IO;
using System;

namespace Services.Serialization
{
    public class JSONSerialization<TModel> : ISerialization<TModel> where TModel : class
    {
        #region Fields

        public TModel _Model;

        #endregion Fields

        #region Constructor

        public JSONSerialization() { }
        public JSONSerialization(TModel Model) { _Model = Model; }

        #endregion Constructor

        #region Methods

        public bool GetModel(string FilePath) => GetModel(FilePath, "");
        public bool CreateModel(string FilePath) => CreateModel(FilePath, "");

        public bool GetModel(string Path, string FileName)
        {
            GetModelFromString(File.ReadAllText(Path + FileName));
            return _Model is null ? false : true; 
        }
        public bool CreateModel(string Path, string FileName)
        {
            string JSON_Content = CreateModelToString();
            File.WriteAllText(Path + FileName, JSON_Content);

            return String.IsNullOrEmpty(JSON_Content) ? false : true;
        }

        public TModel GetModelFromString(string JSON_Content)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                _Model = JsonConvert.DeserializeObject<TModel>(JSON_Content, settings);

                return _Model;
            }
            catch (Exception error)
            {
                ExceptionService.WriteLine(error);
                return null;
            }
        }

        public string CreateModelToString(TModel Model)
        {
            _Model = Model;
            return CreateModelToString();
        }
        public string CreateModelToString()
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                return JsonConvert.SerializeObject(_Model, Formatting.Indented, settings);
            }
            catch (Exception error)
            {
                ExceptionService.WriteLine(error); return String.Empty;
            }
        }

        #endregion Methods
    }
}