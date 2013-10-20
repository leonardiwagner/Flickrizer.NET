using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flickrizer.Method
{
    public class People : Flickrizer.Method.Abstract.Method
    {
        public People(Flickrizer.Authentication.OAuth oAuth) : base(oAuth) { }
    
        public void FindByEmail(String findEmail)
        {

        }

        public void FindByUsername(String username)
        {

        }

        public void GetInfo(String userId)
        {

        }

        public void GetPhotos(String userId)
        {

        }

        public void GetPhotosOf(String userId, String ownerId)
        {

        }
    }
}
