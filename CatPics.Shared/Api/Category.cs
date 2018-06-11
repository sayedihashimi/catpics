using System;
using System.Collections.Generic;
using System.Xml;

namespace CatPics.Shared.Api {

    [System.Diagnostics.DebuggerDisplay("Id='{Id}' Name='{Name}'")]  
    public class Category {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
