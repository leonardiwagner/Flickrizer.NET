using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flickrizer.Model
{

    public class Photo : Flickrizer.Model.Abstract.Response
    {
        public String id { get; set; }
        public String secret { get; set; }
        public String server { get; set; }
        public String farm { get; set; }
        public String isprimary { get; set; }
        public String dateuploaded { get; set; }
        public String isfavorite { get; set; }
        public String license { get; set; }
        public String safety_level { get; set; }
        public String originalsecret { get; set; }
        public String originalformat { get; set; }

        public Content title { get; set; }
        public Content description { get; set; }

        public Url url { get; set; }

        public String media { get; set; }
    }

    public class Url
    {
        public String type { get; set; }
        public Content _content { get; set; }

    }

    

    
}
