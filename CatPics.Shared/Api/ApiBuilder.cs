using System;
using System.Collections.Generic;
using System.Xml;

namespace CatPics.Shared.Api {
    public class ApiBuilder {
        public List<Category> BuildCategoriesFromXml(string xml) {
            if (xml == null) {
                return new List<Category>();
            }

            List<Category> categories = new List<Category>();
            string catxpath = @"/response/data/categories/category";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var selectResult = xmlDocument.SelectNodes(catxpath);
            foreach (XmlElement node in selectResult) {
                var result = BuildCategoryFromNode(node);
                if (result != null) {
                    categories.Add(result);
                }
            }

            return categories;
        }

        public List<CatImage>BuildCatImagesFromXml(string xml){
            if(string.IsNullOrWhiteSpace(xml)){
                return new List<CatImage>();
            }

            List<CatImage> images = new List<CatImage>();
            string imgxpath = @"/response/data/images/image";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var selectResult = xmlDocument.SelectNodes(imgxpath);
            foreach(XmlElement node in selectResult){
                var image = BuildCatImageFromXmlElement(node);
                if(image != null){
                    images.Add(image);
                }
            }

            return images;
        }


        public Category BuildCategoryFromNode(XmlElement element){
            var idString = element.SelectSingleNode("id")?.InnerText;
            var nameString = element.SelectSingleNode("name")?.InnerText;

            if (!string.IsNullOrWhiteSpace(idString) && !string.IsNullOrWhiteSpace(nameString)) {
                return new  Category {
                    Id = idString,
                    Name = nameString
                };
            }

            return null;
        }

        public CatImage BuildCatImageFromXmlElement(XmlElement element){
            var url = element.SelectSingleNode("url")?.InnerText;
            var id = element.SelectSingleNode("id")?.InnerText;
            var sourceUrl = element.SelectSingleNode("source_url")?.InnerText;

            if(!string.IsNullOrWhiteSpace(url) &&
                !string.IsNullOrWhiteSpace(id) &&
               !string.IsNullOrWhiteSpace(sourceUrl)){

                return new CatImage {
                    Id = id,
                    Url = url,
                    SourceUrl = sourceUrl
                };

            }

            return null;
        }

    }
}
