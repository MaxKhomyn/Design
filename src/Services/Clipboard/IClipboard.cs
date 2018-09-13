namespace Services.ClipBoard
{
    public interface IClipboard<TModel> where TModel : class
    {
        void Copy(TModel model);        
        TModel Paste();
    }

    public interface IClipboard
    {
        void Copy(object model);
        object Paste();
    }
}