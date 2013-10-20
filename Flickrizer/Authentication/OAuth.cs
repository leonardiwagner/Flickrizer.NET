using System;
using System.Text;

using System.Collections.Generic;
using System.Collections.Specialized;

using System.Net;


using System.Web;



using System.IO;

using System.Globalization; //oAuth timestamp #todo check if can be used without it

namespace Flickrizer.Authentication
{
    public class OAuth
    {
       
        private readonly string URL_REQUEST_TOKEN = "http://www.flickr.com/services/oauth/request_token";
        private readonly string URL_AUTHORIZE_TOKEN = "http://www.flickr.com/services/oauth/authorize";
        private readonly string URL_ACCESS_TOKEN = "http://www.flickr.com/services/oauth/access_token";
        private readonly string URL_REST = "http://api.flickr.com/services/rest/";
        private readonly string URL_UPLOAD = "http://api.flickr.com/services/upload/";

        private String oAuthConsumerKey = "";
        private String oAuthConsumerSecret = "";
        private String oAuthUrlCallBack = "";

        private String oAuthAccessToken = "";
        private String oAuthAccessTokenSecret = "";

        public OAuth(String consumerKey, String consumerSecret)
        {
            this.oAuthConsumerKey = consumerKey;
            this.oAuthConsumerSecret = consumerSecret;
        }

        private String GetOAuthNonce()
        {
            return Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
        }

        private String GetOAuthTimestamp()
        {
            return Convert.ToInt64(((TimeSpan)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds).ToString(CultureInfo.CurrentCulture);
        }

        public Dictionary<String, String> GetOAuthBasicParameters()
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("oauth_nonce", this.GetOAuthNonce());
            parameters.Add("oauth_timestamp", this.GetOAuthTimestamp());
            parameters.Add("oauth_version", "1.0");
            parameters.Add("oauth_signature_method", "HMAC-SHA1");
            parameters.Add("oauth_consumer_key", this.oAuthConsumerKey);

            return parameters;
        }

        public T FlickrRequest<T>(String method, Dictionary<String, String> requestParameter)
        {
            Dictionary<string, string> parameters = this.GetOAuthBasicParameters();
            parameters.Add("method", method);

           foreach (KeyValuePair<string, string> pair in requestParameter)
           {
               parameters.Add(pair.Key,pair.Value);
           }

            parameters.Add("format", "json");
            parameters.Add("nojsoncallback", "1");

            parameters.Add("oauth_token", oAuthAccessToken);

            string sig = OAuthCalculateSignature(URL_REST, parameters);

            parameters.Add("oauth_signature", sig);

            String response = this.getDataResponse(URL_REST, parameters);

            System.Web.Script.Serialization.JavaScriptSerializer jsonDeserializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return jsonDeserializer.Deserialize<T>(response);
           // dynamic photosetJsonId = (dynamic)JsonConvert.DeserializeObject<DynamicDictionary>(response);
        }

        public void FlickrSend(String method, Dictionary<String, String> requestParameter)
        {
            Dictionary<string, string> parameters = this.GetOAuthBasicParameters();
            parameters.Add("method", method);

            foreach (KeyValuePair<string, string> pair in requestParameter)
            {
                parameters.Add(pair.Key, pair.Value);
            }

            parameters.Add("format", "json");
            parameters.Add("nojsoncallback", "1");

            parameters.Add("oauth_token", oAuthAccessToken);

            string sig = OAuthCalculateSignature(URL_REST, parameters);

            parameters.Add("oauth_signature", sig);

            String response = this.getDataResponse(URL_REST, parameters);
            // dynamic photosetJsonId = (dynamic)JsonConvert.DeserializeObject<DynamicDictionary>(response);
        }

        private string OAuthCalculateSignature(String url, Dictionary<string, string> parameters)
        {
            string baseString = "";
            string key = this.oAuthConsumerSecret + "&" + this.oAuthAccessTokenSecret;
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

            SortedList<string, string> sorted = new SortedList<string, string>();
            foreach (KeyValuePair<string, string> pair in parameters) { sorted.Add(pair.Key, pair.Value); }


            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in sorted)
            {
                sb.Append(pair.Key);
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(pair.Value));
                sb.Append("&");
            }

            sb.Remove(sb.Length - 1, 1);

            baseString = "POST" + "&" + Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(sb.ToString());


            System.Security.Cryptography.HMACSHA1 sha1 = new System.Security.Cryptography.HMACSHA1(keyBytes);

            byte[] hashBytes = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(baseString));

            string hash = Convert.ToBase64String(hashBytes);

            return hash;
        }

       public String GetAccessUrl(OAuthPermission permission)
       {
           
           
           return URL_AUTHORIZE_TOKEN + "?oauth_token=" + this.oAuthAccessToken + "&perms=write";
       }

       public String GetSecretToken()
       {
           string url = URL_REQUEST_TOKEN;

           Dictionary<string, string> parameters = this.GetOAuthBasicParameters();

           parameters.Add("oauth_callback", this.oAuthUrlCallBack);

           string sig = OAuthCalculateSignature(URL_REQUEST_TOKEN, parameters);

           parameters.Add("oauth_signature", sig);

           string response = this.getDataResponse(url, parameters);

           if (response.Length > 0)
           {
               NameValueCollection query = HttpUtility.ParseQueryString(response);

               if (query["oauth_token"] != null)
                   this.oAuthAccessToken = query["oauth_token"];

               if (query["oauth_token_secret"] != null)
                   this.oAuthAccessTokenSecret = query["oauth_token_secret"];
           }

           return this.oAuthAccessTokenSecret;
       }

       private String getDataResponse(string baseUrl, Dictionary<string, string> parameters)
       {

           // Calculate post data, content header and auth header
           string data = OAuthCalculatePostData(parameters);
           string authHeader = OAuthCalculateAuthHeader(parameters);

           // Download data.
           try
           {

               return DownloadData(baseUrl, data, "application/x-www-form-urlencoded", authHeader);
           }
           catch (WebException ex)
           {
               if (ex.Status != WebExceptionStatus.ProtocolError) throw;

               HttpWebResponse response = ex.Response as HttpWebResponse;
               if (response == null) throw;

               if (response.StatusCode != HttpStatusCode.BadRequest && response.StatusCode != HttpStatusCode.Unauthorized) throw;

               using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
               {
                   string responseData = responseReader.ReadToEnd();
                   responseReader.Close();

                   return responseData;
               }
           }
       }

       private static string DownloadData(string baseUrl, string data, string contentType, string authHeader)
       {
           WebClient client = new WebClient();
           client.Headers.Add("user-agent", "Netbiis / Social Cast Alpha");
           if (!String.IsNullOrEmpty(contentType)) client.Headers.Add("Content-Type", contentType);
           if (!String.IsNullOrEmpty(authHeader)) client.Headers.Add("Authorization", authHeader);

           return client.UploadString(baseUrl, data);
       }

       /// <summary>
       /// Returns the string for the Authorisation header to be used for OAuth authentication.
       /// Parameters other than OAuth ones are ignored.
       /// </summary>
       /// <param name="parameters">OAuth and other parameters.</param>
       /// <returns></returns>
       private string OAuthCalculateAuthHeader(Dictionary<string, string> parameters)
       {
           StringBuilder sb = new StringBuilder("OAuth ");
           foreach (KeyValuePair<string, string> pair in parameters)
           {
               if (pair.Key.StartsWith("oauth"))
               {
                   sb.Append(pair.Key + "=\"" + Uri.EscapeDataString(pair.Value) + "\",");
               }
           }

           return sb.Remove(sb.Length - 1, 1).ToString();
       }

       /// <summary>
       /// Calculates for form encoded POST data to be included in the body of an OAuth call.
       /// </summary>
       /// <remarks>This will include all non-OAuth parameters. The OAuth parameter will be included in the Authentication header.</remarks>
       /// <param name="parameters"></param>
       /// <returns></returns>
       private string OAuthCalculatePostData(Dictionary<string, string> parameters)
       {
           string data = String.Empty;
           foreach (KeyValuePair<string, string> pair in parameters)
           {
               if (!pair.Key.StartsWith("oauth"))
               {
                   data += pair.Key + "=" + Uri.EscapeDataString(pair.Value) + "&";
               }
           }
           return data;
       }

       public String OAuthGetAuthorizeToken(string token, string tokenSecret, string verifier)
       {


           this.oAuthAccessToken = token;
           this.oAuthAccessTokenSecret = tokenSecret;


           Dictionary<string, string> parameters = this.GetOAuthBasicParameters();

           parameters.Add("oauth_verifier", verifier);
           parameters.Add("oauth_token", token);

           string sig = OAuthCalculateSignature(URL_ACCESS_TOKEN, parameters);

           parameters.Add("oauth_signature", sig);

           string response = this.getDataResponse(URL_ACCESS_TOKEN, parameters);

           if (response.Length > 0)
           {
               NameValueCollection query = HttpUtility.ParseQueryString(response);


               return query["oauth_token_secret"];
                 
           }




           return response;
       }

        /*
       //salva o token pra na hora q o flickr retornar saber de quem é o retorno!
       private int saveAccessToken(int accountId)
       {

           Dal.SocialToken dalSocialToken = new Dal.SocialToken();
           Model.SocialToken socialToken = new Model.SocialToken();
           socialToken.token = this.oAuthAccessToken;
           socialToken.tokenSecret = this.oAuthAccessTokenSecret;

           return dalSocialToken.saveSocialToken(accountId, socialToken, (int)Model.enSocial.Flickr);
       }

       public void saveSecretToken()
       {

           Dal.SocialToken dalSocialToken = new Dal.SocialToken();
           Model.ListSocialToken listSocialToken = dalSocialToken.getSocialToken(this.oAuthAccessToken);
           //sempre deve ser um, mas o laço está ai para convenção já que o retorno é uma lista
           foreach (Model.SocialToken socialToken in listSocialToken.socialToken)
           {
               dalSocialToken.saveSecretTokenByToken("", this.oAuthAccessToken, this.oAuthAccessTokenSecret, false);
           }


       }

       public String OAuthGetAuthorizeToken(string token, string verifier)
       {

           Model.SocialToken socialToken = null;

           String oldToken = token;

           Dal.SocialToken dalSocialToken = new Dal.SocialToken();
           Model.ListSocialToken listSocialToken = dalSocialToken.getSocialToken(token);
           //sempre deve ser um, mas o laço está ai para convenção já que o retorno é uma lista
           if (listSocialToken != null && listSocialToken.socialToken.Count > 0)
           {
               socialToken = listSocialToken.socialToken[0];
               this.oAuthAccessTokenSecret = socialToken.tokenSecret;
           }


           this.oAuthAccessToken = token;


           Dictionary<string, string> parameters = this.getOAuthBasicParameters();

           parameters.Add("oauth_verifier", verifier);
           parameters.Add("oauth_token", token);

           string sig = OAuthCalculateSignature(URL_ACCESS_TOKEN, parameters);

           parameters.Add("oauth_signature", sig);

           string response = this.getDataResponse(URL_ACCESS_TOKEN, parameters);

           if (response.Length > 0)
           {
               NameValueCollection query = HttpUtility.ParseQueryString(response);

               if (query["username"] != null)
                   dalSocialToken.saveSecretTokenByToken(token, query["oauth_token"], query["oauth_token_secret"], true);
           }




           return response;
       }

       /// <summary>
       /// Returns the string for the Authorisation header to be used for OAuth authentication.
       /// Parameters other than OAuth ones are ignored.
       /// </summary>
       /// <param name="parameters">OAuth and other parameters.</param>
       /// <returns></returns>
       private string OAuthCalculateAuthHeader(Dictionary<string, string> parameters)
       {
           StringBuilder sb = new StringBuilder("OAuth ");
           foreach (KeyValuePair<string, string> pair in parameters)
           {
               if (pair.Key.StartsWith("oauth"))
               {
                   sb.Append(pair.Key + "=\"" + Uri.EscapeDataString(pair.Value) + "\",");
               }
           }

           return sb.Remove(sb.Length - 1, 1).ToString();
       }

       /// <summary>
       /// Calculates for form encoded POST data to be included in the body of an OAuth call.
       /// </summary>
       /// <remarks>This will include all non-OAuth parameters. The OAuth parameter will be included in the Authentication header.</remarks>
       /// <param name="parameters"></param>
       /// <returns></returns>
       private string OAuthCalculatePostData(Dictionary<string, string> parameters)
       {
           string data = String.Empty;
           foreach (KeyValuePair<string, string> pair in parameters)
           {
               if (!pair.Key.StartsWith("oauth"))
               {
                   data += pair.Key + "=" + Uri.EscapeDataString(pair.Value) + "&";
               }
           }
           return data;
       }

       private string OAuthCalculateSignature(String url, Dictionary<string, string> parameters)
       {
           string baseString = "";
           string key = this.oAuthConsumerSecret + "&" + this.oAuthAccessTokenSecret;
           byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

           SortedList<string, string> sorted = new SortedList<string, string>();
           foreach (KeyValuePair<string, string> pair in parameters) { sorted.Add(pair.Key, pair.Value); }


           StringBuilder sb = new StringBuilder();
           foreach (KeyValuePair<string, string> pair in sorted)
           {
               sb.Append(pair.Key);
               sb.Append("=");
               sb.Append(Uri.EscapeDataString(pair.Value));
               sb.Append("&");
           }

           sb.Remove(sb.Length - 1, 1);

           baseString = "POST" + "&" + Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(sb.ToString());


           System.Security.Cryptography.HMACSHA1 sha1 = new System.Security.Cryptography.HMACSHA1(keyBytes);

           byte[] hashBytes = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(baseString));

           string hash = Convert.ToBase64String(hashBytes);

           return hash;
       }

       private Dictionary<string, string> getOAuthBasicParameters()
       {

           Dictionary<string, string> parameters = new Dictionary<string, string>();
           parameters.Add("oauth_nonce", this.getOAuthNonce());
           parameters.Add("oauth_timestamp", this.getOAuthTimestamp());
           parameters.Add("oauth_version", "1.0");
           parameters.Add("oauth_signature_method", "HMAC-SHA1");
           parameters.Add("oauth_consumer_key", this.oAuthConsumerKey);

           return parameters;
       }

       private String getOAuthNonce()
       {
           return Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
       }

       private String getOAuthTimestamp()
       {
           return Convert.ToInt64(((TimeSpan)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds).ToString(CultureInfo.CurrentCulture);
       }

       */

    }
}
