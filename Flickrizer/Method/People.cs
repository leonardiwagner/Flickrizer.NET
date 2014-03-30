using System;
using Flickstein.Authentication;

namespace Flickstein.Method
{
    public class People : Abstract.Method
    {
        public People(OAuth oAuth) : base(oAuth)
        {
        }

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