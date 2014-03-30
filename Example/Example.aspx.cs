using System;
using System.Web.UI;

namespace WebApplication1
{
    public partial class Example : Page
    {
        private Flickstein.Authentication.OAuth Auth;
        protected String FlickrPermissionUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //create your app and get your keys
            //https://www.flickr.com/services/apps/create/apply/

            Session["consumerKey"] = "e19d14e706bd544215c721fa82125889";
            Session["secretKey"] = "d6767924e4d8b062";

            Auth = new Flickstein.Authentication.OAuth(Session["consumerKey"].ToString(), Session["secretKey"].ToString());

            this.LoadPhotosets();
        }

        private void LoadPhotosets()
        {
            Flickstein.Method.Photosets flicksteinPhotosets = new Flickstein.Method.Photosets(this.Auth);
            Flickstein.Model.Photosets photosets = flicksteinPhotosets.GetList("48017770@N08", 0, 0);
        }
    }
}