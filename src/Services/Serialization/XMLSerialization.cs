using System.Xml.Serialization;
using System.IO;
using System;

namespace Services.Serialization
{
    public class XMLSerialization<TModel> : ISerialization<TModel> where TModel : class
    {
        public TModel _Model;
        public XMLSerialization() { }

        public bool GetModel(string FilePath) => GetModel(FilePath, "");
        public bool CreateModel(string FilePath) => CreateModel(FilePath, "");

        public bool GetModel(string Path, string FileName)
        {
            try
            {
                using (FileStream fs = new FileStream(Path + FileName, FileMode.Open))
                {
                    _Model = (TModel)new XmlSerializer(typeof(TModel)).Deserialize(fs);
                    return true;
                }
            }
            catch (Exception error)
            {
                ExceptionService.WriteLine(error); return false;
            }
        }
        public bool CreateModel(string Path, string FileName)
        {
            try
            {
                using (FileStream fs = new FileStream(Path + FileName, FileMode.Create))
                {
                    new XmlSerializer(typeof(TModel)).Serialize(fs, _Model);
                    return true;
                }
            }
            catch (Exception error)
            {
                ExceptionService.WriteLine(error); return false;
            }
        }
        
        public TModel GetModelFromString(string JSON_Content)
        {
            return _Model;
        }

        public string CreateModelToString(TModel Model)
        {
            _Model = Model;
            return CreateModelToString();
        }
        public string CreateModelToString()
        {
            return "";
        }
    }
}