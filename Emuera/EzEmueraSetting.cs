using Newtonsoft.Json;
using System.Text;

namespace MinorShift.Emuera
{
    public sealed class EzEmueraSetting
    {
        public int ReadEncodingCode { get; set; }

        [JsonIgnore]
        public Encoding ReadEncoding => Encoding.GetEncoding(ReadEncodingCode);
        
        public string EzTransPath { get; set; }
    }
}
