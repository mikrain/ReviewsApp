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
    public class ReviewsDetails
    {
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "entry")]
        public List<ReviewEntry> Entry { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false, ElementName = "entry")]
    public class ReviewEntry
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

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.zune.net/catalog/apps/2008/02", ElementName = "productVersion")]
        public string ProductVersion { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.zune.net/catalog/apps/2008/02", ElementName = "userRating")]
        public string UserRating { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "updated")]
        public DateTime Updated { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "content")]
        public string Content { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "author")]
        public Author Author { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false, ElementName = "author")]
    public class Author
    {
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom", ElementName = "name")]
        public string Name { get; set; }
    }

}
