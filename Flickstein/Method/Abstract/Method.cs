using Flickstein.Authentication;

namespace Flickstein.Method.Abstract
{
    public abstract class Method
    {
        protected OAuth oAuth = null;

        public Method(OAuth oAuth)
        {
            this.oAuth = oAuth;
        }
    }
}