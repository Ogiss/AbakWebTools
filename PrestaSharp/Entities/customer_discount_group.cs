using System.Xml.Serialization;

namespace Bukimedia.PrestaSharp.Entities
{
    [XmlType(Namespace = "Bukimedia/PrestaSharp/Entities")]
    public class customer_discount_group : PrestaShopEntity, IPrestaShopFactoryEntity
    {
        public long? id { get; set; }
        public long id_customer { get; set; }
        public long id_discount_group { get; set; }
        public decimal discount { get; set; }
    }
}
