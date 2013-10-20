using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        private readonly String FLICKR_API_KEY = "1ce7081938204ed1cab66edebf67e76b";
        private readonly String FLICKR_API_KEY_SECRET = "415488cb491e4d6f";

        private readonly String FLICKR_OAUTH_TOKEN = "72157636765234504-9d16714ce861fb87";
        private readonly String FLICKR_OAUTH_TOKEN_SECRET = "8216b37db086dbdb";


        [TestMethod]
        public void TestMethod1()
        {

            Flickrizer.Authentication.OAuth oAuth = new Flickrizer.Authentication.OAuth(FLICKR_API_KEY, FLICKR_API_KEY_SECRET);

            Flickrizer.Method.Photosets flickrizerPhotosets = new Flickrizer.Method.Photosets(oAuth);
            
            Flickrizer.Model.PhotosetsResponse response = flickrizerPhotosets.GetList("48017770@N08", 0, 0);
            
            
            //flickrizerPhotosets.GetPhoto("72157636741946983", 0, 0);
        }
    }
}
