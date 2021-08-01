using System.Xml.Serialization;

namespace Bukimedia.PrestaSharp.Entities
{
    [XmlType(Namespace = "Bukimedia/PrestaSharp/Entities")]
    public class unit : PrestaShopEntity, IPrestaShopFactoryEntity
    {
        public long? id { get; set; }
        public int is_default { get; set; }
        public int multiplier { get; set; }
        public string name { get; set; }
    }
}
