using System;
using System.Collections.Generic;
using System.Xml;

namespace CatPics.Shared {

    [System.Diagnostics.DebuggerDisplay("Id='{Id}' Name='{Name}'")]  
    public class ApiCategory {
        public string Id { get; set; }
        public string Name { get; set; }

        public static List<ApiCategory> BuildFromXml(string xml) {
            if (xml == null) {
                return new List<ApiCategory>();
            }

            List<ApiCategory> categories = new List<ApiCategory>();
            string catxpath = @"/response/data/categories/category";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var selectResult = xmlDocument.SelectNodes(catxpath);
            foreach (XmlElement node in selectResult) {
                var idString = node.SelectSingleNode("id")?.InnerText;
                var nameString = node.SelectSingleNode("name")?.InnerText;

                if (!string.IsNullOrWhiteSpace(idString) && !string.IsNullOrWhiteSpace(nameString)) {
                    categories.Add(new ApiCategory {
                        Id = idString,
                        Name = nameString
                    });
                }
            }

            return categories;
        }
    }
}
