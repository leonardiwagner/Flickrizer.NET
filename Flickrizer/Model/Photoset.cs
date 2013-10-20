using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flickrizer.Model
{
    public class PhotosetsResponse : Flickrizer.Model.Abstract.Response
    {
        public Photosets photosets { get; set; }
    }

    public class Photosets
    {
        public String cancreate { get; set; }
        public String page { get; set; }
        public String pages { get; set; }
        public String perpage { get; set; }
        public String total { get; set; }

        public List<Photoset> photoset { get; set; }
    }

    public class Photoset
    {
        public String id { get; set; }
        public String primary { get; set; }
        public String secret { get; set; }
        public String server { get; set; }
        public String farm { get; set; }
        public String photos { get; set; }
        public String videos { get; set; }

        public Content title { get; set; }
        public Content description { get; set; }

        public String needs_interstitial { get; set; }
        public String visibility_can_see_set { get; set; }
        public String count_views { get; set; }
        public String count_comments { get; set; }
        public String can_comment { get; set; }
        public String date_create { get; set; }
        public String date_update { get; set; }

        public Photo photo { get; set; }

    }

}
