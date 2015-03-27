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
    public class SearchResult
    {
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://a9.com/-/spec/opensearch/1.1/")]
        public string startIndex { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "entry")]
        public Entry Entry { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false, ElementName = "entry")]
    public class Entry
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

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.zune.net/catalog/apps/2008/02", ElementName = "averageUserRating")]
        public string AverageUserRating { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.zune.net/catalog/apps/2008/02", ElementName = "version")]
        public string Version { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "updated")]
        public DateTime Updated { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "title")]
        public string Title { get; set; }
    }
}
