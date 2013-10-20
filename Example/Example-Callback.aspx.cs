using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Example_Callback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (!String.IsNullOrEmpty(Request.QueryString["oauth_token"]) && !String.IsNullOrEmpty(Request.QueryString["oauth_verifier"]))
            {
                Flickrizer.Authentication.OAuth oAuth = new Flickrizer.Authentication.OAuth("1ce7081938204ed1cab66edebf67e76b", "415488cb491e4d6f", "http://localhost:23019/Example-Callback.aspx");
                String oAuthSecret = oAuth.OAuthGetAuthorizeToken(Request.QueryString["oauth_token"].ToString(), (String)Session["secretToken"], Request.QueryString["oauth_verifier"].ToString());



            }
             */
        }
    }
}