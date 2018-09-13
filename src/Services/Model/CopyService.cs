using System.Reflection;
using Newtonsoft.Json;
using System;

namespace Services
{
    public class CopyService<TObject>
    {
        public static bool Copy(TObject Source, TObject Derivative)
        {
            PropertyInfo[] DerivativeProperties = Derivative.GetType().GetProperties();
            PropertyInfo[] SourceProperties = Source.GetType().GetProperties();

            for (int Property = 0; Property <= SourceProperties.Length; Property++)
            {
                try
                {
                    DerivativeProperties[Property].SetValue(Derivative, SourceProperties[Property].GetValue(Source));
                }
                catch (Exception error) { ExceptionService.WriteLine(error); return false; }
            }
            return true;
        }

        private static bool CopySerialize(TObject Source, TObject Derivative)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

                string SourseString = JsonConvert.SerializeObject(Source, Formatting.Indented, settings);
                Derivative = JsonConvert.DeserializeObject<TObject>(SourseString, settings);
            }
            catch (Exception error) { ExceptionService.WriteLine(error); return false; }
            return true;
        }
    }
}