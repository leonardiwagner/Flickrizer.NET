using System;
using Flickstein.Authentication;
using Flickstein.Method;
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
            var oAuth = new OAuth(FLICKR_API_KEY, FLICKR_API_KEY_SECRET);

            var FlicksteinPhotosets = new Photosets(oAuth);

            Flickstein.Model.PhotosetsResponse response = FlicksteinPhotosets.GetList("48017770@N08", 0, 0);


            //FlicksteinPhotosets.GetPhoto("72157636741946983", 0, 0);
        }
    }
}