using System;
using System.Web.UI;

namespace WebApplication1
{
    public partial class Example : Page
    {
        protected String FlickrPermissionUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //these values are mine, you need to change to yours!
                txtConsumerKey.Text = "1ce7081938204ed1cab66edebf67e76b";
                txtSecretKey.Text = "415488cb491e4d6f";
                txtCallbackUrl.Text = "http://localhost:23019/Example-Callback.aspx";
            }

            this.btnOK.Click += btnOK_Click;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            Session["consumerKey"] = txtConsumerKey.Text;
            Session["secretKey"] = txtSecretKey.Text;
            //Session["callbackUrl"] = txtCallbackUrl.Text;

            this.SetPermissionUrl();
        }

        private void SetPermissionUrl()
        {
            Flickstein.Authentication.OAuth oAuth = new Flickstein.Authentication.OAuth(Session["consumerKey"].ToString(), Session["secretKey"].ToString());
            FlickrPermissionUrl = oAuth.GetAccessUrl(txtCallbackUrl.Text, Flickstein.Authentication.OAuthPermission.WRITE);
        }
    }
}