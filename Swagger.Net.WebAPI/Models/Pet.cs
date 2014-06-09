using System.Collections.Generic;

namespace Swagger.Net.WebAPI.Models {
    public class Pet {
        public int Id;
        public Category Category;
        public string Name;
        public List<string> PhotoUrls;
        public List<Tag> Tags;
        public string Status;
    }

    public class Category {
        public int Id;
        public string Name;
    }
    public class Tag {
        public int Id;
        public string Name;
    }
}