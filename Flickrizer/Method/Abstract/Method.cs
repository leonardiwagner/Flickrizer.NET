using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flickrizer.Method.Abstract
{
    public abstract class Method
    {
        protected Flickrizer.Authentication.OAuth oAuth = null;

        public Method(Flickrizer.Authentication.OAuth oAuth)
        {
            this.oAuth = oAuth;
        }
    }
}
