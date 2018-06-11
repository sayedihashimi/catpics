using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CatPics.Shared.Api;

namespace CatPics.Shared {
    
    public class WebApiHelper {

        public async Task<List<Category>> GetCategories(){
            using(HttpClient httpClient = new HttpClient()){
                var url = $"{Common.CatApiBaseUrl}/api/categories/list";
                var catResult = await httpClient.GetStringAsync(url);

                var categories = new ApiBuilder().BuildCategoriesFromXml(catResult);

                return categories;
            }
        }

        public async Task<CatImage> GetCat(string category, ImageSize size = ImageSize.None){
            return (await GetCats(1, category, size)).FirstOrDefault();
        }

        public async Task<List<CatImage>> GetCats(int numResults, string category, ImageSize size = ImageSize.None){
            // string url = $"http://thecatapi.com/api/images/get?format=xml{GetResultsPerPageUrlFragment(numResults)}{GetCategoryUrlFragement(category)}{GetSizeUrlFragment(size)}";

            StringBuilder urlbuilder = new StringBuilder();
            urlbuilder.Append("http://thecatapi.com/api/images/get?format=xml");
            urlbuilder.Append(GetResultsPerPageUrlFragment(numResults));
            urlbuilder.Append(GetCategoryUrlFragement(category));
            urlbuilder.Append(GetSizeUrlFragment(size));

            using(HttpClient httpClient = new HttpClient()){
                var httpResult = await httpClient.GetStringAsync(urlbuilder.ToString());
                return new ApiBuilder().BuildCatImagesFromXml(httpResult);
            }
        }



        public async Task<CatImage> GetCatById(string id, ImageSize size = ImageSize.None) {
            if (string.IsNullOrWhiteSpace(id)) {
                return null;
            }

            string url = $"http://thecatapi.com/api/images/get?format=xml&image_id={id}{GetSizeUrlFragment(size)}";
            using(HttpClient httpClient = new HttpClient()){
                var httpResult = await httpClient.GetStringAsync(url);

                return new ApiBuilder().BuildCatImagesFromXml(httpResult).FirstOrDefault();
            }
        }

        protected string GetCategoryUrlFragement(string category,bool prefixWithAmpersand = true){
            string prefix = string.Empty;
            if(prefixWithAmpersand){
                prefix = "&";
            }

            if(!string.IsNullOrWhiteSpace(category)){
                return $"{prefix}category={category}";
            }

            return string.Empty;
        }
        protected string GetResultsPerPageUrlFragment(int numResults, bool prefixWithAmpersand = true){
            string prefix = string.Empty;
            if(prefixWithAmpersand){
                prefix = "&";
            }

            if(numResults > 0){
                return $"{prefix}results_per_page={numResults}";    
            }

            return string.Empty;
        }

        protected string GetSizeUrlFragment(ImageSize size, bool prefixWithAmpersand = true) {
            string prefix = string.Empty;
            if (prefixWithAmpersand) {
                prefix = "&";
            }

            switch (size) {
                case ImageSize.Small:
                    return $"{prefix}size=small";
                case ImageSize.Med:
                    return $"{prefix}size=med";
                case ImageSize.Full:
                    return $"{prefix}size=full";
                default:
                    return string.Empty;
            }
        }


    }



}
