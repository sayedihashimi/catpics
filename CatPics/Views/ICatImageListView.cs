using System.Collections.Generic;
using System.Threading.Tasks;
using CatPics.Shared.Api;

namespace CatPics.Views {
    public interface ICatImageListView {
        Task FetchAndAddImagesToView(int numToFetch = 10, ImageSize size = ImageSize.Med);
        List<CatImage> GetCatImages();

        // TODO: Replace with GetPrevious/GetNext - with WrapAround bool parameter
        int GetIndexOfImageWithUrl(string url);
    }
}