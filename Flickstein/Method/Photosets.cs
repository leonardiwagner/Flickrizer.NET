using System;
using System.Collections.Generic;
using Flickstein.Authentication;

namespace Flickstein.Method
{
    public class Photosets : Abstract.Method
    {
        public Photosets(OAuth oAuth) : base(oAuth)
        {
        }

        /// <summary>
        ///     Add a photo to the end of an existing photoset.
        /// </summary>
        /// <flickrApi>http://www.flickr.com/services/api/flickr.photosets.addPhoto.html</flickrApi>
        public void AddPhoto(String photosetId, String photoId)
        {
            var parameter = new Dictionary<String, String>();
            parameter.Add("photoset_id", photosetId);
            parameter.Add("photo_id", photoId);

            oAuth.FlickrSend("flickr.photosets.addPhoto", parameter);
        }

        /// <summary>
        ///     Create a new photoset for the calling user.
        /// </summary>
        /// <param name="title">Required</param>
        /// <param name="description">Optional</param>
        /// <param name="primaryPhotoId">Required</param>
        /// <flickrApi>http://www.flickr.com/services/api/flickr.photosets.create.html</flickrApi>
        public void Create(String title, String description, String primaryPhotoId)
        {
            var parameter = new Dictionary<String, String>();
            parameter.Add("title", title);
            parameter.Add("description", description);
            parameter.Add("primary_photo_id", primaryPhotoId);

            oAuth.FlickrSend("flickr.photosets.create", parameter);
        }

        /// <summary>
        ///     Delete a photoset.
        /// </summary>
        /// <flickrApi>http://www.flickr.com/services/api/flickr.photosets.delete.html</flickrApi>
        public void Delete(String photosetId)
        {
            var parameter = new Dictionary<String, String>();
            parameter.Add("photoset_id", photosetId);

            oAuth.FlickrSend("flickr.photosets.delete", parameter);
        }

        /// <summary>
        ///     Gets information about a photoset.
        /// </summary>
        /// <flickrApi>http://www.flickr.com/services/api/flickr.photosets.getInfo.html</flickrApi>
        public Model.PhotosetsResponse GetInfo(String photosetId)
        {
            var parameter = new Dictionary<String, String>();
            parameter.Add("photoset_id", photosetId);

            return oAuth.FlickrRequest<Model.PhotosetsResponse>("flickr.photosets.getInfo", parameter);
        }

        /// <summary>
        ///     Returns the photosets belonging to the specified user.
        /// </summary>
        /// <param name="userId">Optional</param>
        /// <param name="page">Optional</param>
        /// <param name="perPage">Optional</param>
        /// <flickrApi>http://www.flickr.com/services/api/flickr.photosets.getList.html</flickrApi>
        public Model.Photosets GetList(String userId, int page, int perPage)
        {
            var parameter = new Dictionary<String, String>();

            if (!String.IsNullOrEmpty(userId))
            {
                parameter.Add("user_id", userId);
            }

            if (page > 0 && perPage > 0)
            {
                parameter.Add("per_page", perPage.ToString());
                parameter.Add("page", page.ToString());
            }

            return oAuth.FlickrRequest<Model.PhotosetsResponse>("flickr.photosets.getList", parameter).photosets;
        }

        /// <summary>
        ///     Get the list of photos in a set.
        /// </summary>
        /// <param name="photosetId">Required</param>
        /// <param name="page">Optional</param>
        /// <param name="perPage">Optional</param>
        /// ///
        /// <flickrApi>http://www.flickr.com/services/api/flickr.photosets.getPhotos.html</flickrApi>
        public Model.PhotosetsResponse GetPhotos(String photosetId, int page, int perPage)
        {
            var parameter = new Dictionary<String, String>();

            if (!String.IsNullOrEmpty(photosetId))
            {
                parameter.Add("photoset_id", photosetId);
            }

            if (page > 0 && perPage > 0)
            {
                parameter.Add("per_page", perPage.ToString());
                parameter.Add("page", page.ToString());
            }

            return oAuth.FlickrRequest<Model.PhotosetsResponse>("flickr.photosets.getPhotos", parameter);
        }

        /// <summary>
        ///     Remove a photo from a photoset.
        /// </summary>
        /// <param name="photosetId">Required</param>
        /// <param name="photoId">Required</param>
        public void RemovePhoto(String photosetId, String photoId)
        {
            var parameter = new Dictionary<String, String>();
            parameter.Add("photoset_id", photosetId);
            parameter.Add("photo_id", photoId);

            oAuth.FlickrSend("flickr.photosets.removePhoto", parameter);
        }
    }
}