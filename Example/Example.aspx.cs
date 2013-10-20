using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Example : System.Web.UI.Page
    {
        protected String FlickrPermissionUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            Flickrizer.Authentication.OAuth oAuth = new Flickrizer.Authentication.OAuth("1ce7081938204ed1cab66edebf67e76b", "415488cb491e4d6f", "http://localhost:23019/Example-Callback.aspx");
            Session["secretToken"] = oAuth.GetSecretToken();
            this.FlickrPermissionUrl = oAuth.GetAccessUrl(Flickrizer.Authentication.OAuthPermission.READ);
            */
            
        }
    }
}