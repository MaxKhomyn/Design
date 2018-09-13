namespace Services.Serialization
{
    public interface ISerialization<TModel> where TModel : class
    {
        bool GetModel(string FilePath);
        bool CreateModel(string FilePath);

        bool GetModel(string Path, string FileName);
        bool CreateModel(string Path, string FileName);

        TModel GetModelFromString(string JSON_Content);

        string CreateModelToString(TModel Model);
        string CreateModelToString();
    }
}