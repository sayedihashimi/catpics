using System.Collections.Generic;
using System.Threading.Tasks;
using CatPics.Shared.Api;

namespace CatPics.Views {
    public interface ICatImageListView {
        Task FetchAndAddImagesToView(int numToFetch = 10, ImageSize size = ImageSize.Med);
        List<CatImage> GetCatImages();

        Task<string> GetNextImageUrl(string url, bool addMoreImagesIfNearEnd = true);
        string GetPreviousImageUrl(string url, bool addMoreImagesIfNearEnd = true);
    }
}