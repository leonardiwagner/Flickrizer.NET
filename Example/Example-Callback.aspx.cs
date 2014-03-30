using System;
using System.Web.UI;

namespace WebApplication1
{
    public partial class Example_Callback : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(Request.QueryString["oauth_token"]) && !String.IsNullOrEmpty(Request.QueryString["oauth_verifier"]))
            {
                Flickstein.Authentication.OAuth oAuth = new Flickstein.Authentication.OAuth(Session["consumerKey"].ToString(), Session["secretKey"].ToString());
                String oAuthSecret = oAuth.OAuthGetAuthorizeToken(Request.QueryString["oauth_token"], (String)Session["secretKey"], Request.QueryString["oauth_verifier"]);
            }
        }
    }
}