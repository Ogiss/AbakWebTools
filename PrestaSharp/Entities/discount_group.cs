using System.Xml.Serialization;

namespace Bukimedia.PrestaSharp.Entities
{
    [XmlType(Namespace = "Bukimedia/PrestaSharp/Entities")]
    public class discount_group : PrestaShopEntity, IPrestaShopFactoryEntity
    {
        public long? id { get; set; }
        public string category { get; set; }
        public string name { get; set; }
        public int deleted { get; set; }
    }
}
