using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Reviews
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false, ElementName = "feed")]
    public class AppDetails
    {
        private string _id;

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "id")]
        public string Id
        {
            get { return _id.Replace("urn:uuid:", ""); }
            set
            {
                _id = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "updated")]
        public DateTime Updated { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "title")]
        public string Title { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "content")]
        public string Content { get; set; }


        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "entry")]
        public EntryDetails Entry { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false,
        ElementName = "entry")]
    public class EntryDetails
    {
        private string _id;
        private string _skuId;

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "id")]
        public string Id
        {
            get { return _id.Replace("urn:uuid:", ""); }
            set { _id = value; }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.zune.net/catalog/apps/2008/02",
            ElementName = "skuId")]
        public string SkuId
        {
            get { return _skuId.Replace("urn:uuid:", ""); }
            set { _skuId = value; }
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.zune.net/catalog/apps/2008/02",
            ElementName = "url")]
        public string Url { get; set; }

    }
}
