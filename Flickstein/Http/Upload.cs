namespace Flickstein
{
    public class Upload
    {
        /*
        

        /// <summary>
        /// UploadPicture method that does all the uploading work.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> object containing the pphoto to be uploaded.</param>
        /// <param name="fileName">The filename of the file to upload. Used as the title if title is null.</param>
        /// <param name="title">The title of the photo (optional).</param>
        /// <param name="description">The description of the photograph (optional).</param>
        /// <param name="tags">The tags for the photograph (optional).</param>
        /// <param name="isPublic">false for private, true for public.</param>
        /// <param name="isFamily">true if visible to family.</param>
        /// <param name="isFriend">true if visible to friends only.</param>
        /// <param name="contentType">The content type of the photo, i.e. Photo, Screenshot or Other.</param>
        /// <param name="safetyLevel">The safety level of the photo, i.e. Safe, Moderate or Restricted.</param>
        /// <param name="hiddenFromSearch">Is the photo hidden from public searches.</param>
        /// <returns>The id of the photograph after successful uploading.</returns>
        public string UploadPicture(String filePath, string title, string description)
        {

            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);


            Uri uploadUri = new Uri("http://api.flickr.com/services/upload/");

            Dictionary<string, string> parameters = this.getOAuthBasicParameters();

            if (title != null && title.Length > 0)
            {
                //parameters.Add("title", title);
            }
            if (description != null && description.Length > 0)
            {
                //    parameters.Add("description", description);
            }


            parameters.Add("is_public", "1");
            parameters.Add("is_friend", "0");
            parameters.Add("is_family", "0");


            parameters.Add("oauth_token", this.oAuthAccessToken);

            string sig = this.OAuthCalculateSignature("http://api.flickr.com/services/upload/", parameters);

            parameters.Add("oauth_signature", sig);


            string responseXml = UploadData(stream, filePath, uploadUri, parameters);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            XmlReader reader = XmlReader.Create(new StringReader(responseXml), settings);

            if (!reader.ReadToDescendant("rsp"))
            {
                throw new XmlException("Unable to find response element 'rsp' in Flickr response");
            }
            while (reader.MoveToNextAttribute())
            {
                if (reader.LocalName == "stat" && reader.Value == "fail")
                    //throw ExceptionHandler.CreateResponseException(reader);
                    continue;
            }

            reader.MoveToElement();
            reader.Read();

            UnknownResponse t = new UnknownResponse();
            ((IFlickrParsable)t).Load(reader);
            return t.GetElementValue("photoid");
        }

        private string UploadData(Stream imageStream, string fileName, Uri uploadUri, Dictionary<string, string> parameters)
        {
            string boundary = "FLICKR_MIME_" + DateTime.Now.ToString("yyyyMMddhhmmss", System.Globalization.DateTimeFormatInfo.InvariantInfo);

            byte[] dataBuffer = CreateUploadData(imageStream, fileName, parameters, boundary);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uploadUri);
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;


            req.ContentLength = dataBuffer.Length;

            Stream resStream = req.GetRequestStream();

            int j = 1;
            int uploadBit = Math.Max(dataBuffer.Length / 100, 50 * 1024);
            int uploadSoFar = 0;

            for (int i = 0; i < dataBuffer.Length; i = i + uploadBit)
            {
                int toUpload = Math.Min(uploadBit, dataBuffer.Length - i);
                uploadSoFar += toUpload;

                resStream.Write(dataBuffer, i, toUpload);

                if ((OnUploadProgress != null) && ((j++) % 5 == 0 || uploadSoFar == dataBuffer.Length))
                {
                    OnUploadProgress(this, new UploadProgressEventArgs(i + toUpload, dataBuffer.Length));
                }
            }
            resStream.Close();

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(res.GetResponseStream());
            string s = sr.ReadToEnd();
            sr.Close();
            return s;
        }


        public event EventHandler<UploadProgressEventArgs> OnUploadProgress;
        public class UploadProgressEventArgs : EventArgs
        {
            /// <summary>
            /// Number of bytes transfered so far.
            /// </summary>
            public long BytesSent { get; internal set; }

            /// <summary>
            /// Total bytes to be sent
            /// </summary>
            public long TotalBytesToSend { get; internal set; }

            /// <summary>
            /// True if all bytes have been uploaded.
            /// </summary>
            public bool UploadComplete { get { return ProcessPercentage == 100; } }

            /// <summary>
            /// The percentage of the upload that has been completed.
            /// </summary>
            public int ProcessPercentage
            {
                get { return Convert.ToInt32(BytesSent * 100 / TotalBytesToSend); }
            }

            internal UploadProgressEventArgs()
            {
            }

            internal UploadProgressEventArgs(long bytes, long totalBytes)
            {
                this.BytesSent = bytes;
                this.TotalBytesToSend = totalBytes;
            }
        }

        private byte[] ConvertNonSeekableStreamToByteArray(Stream nonSeekableStream)
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[1024];
            int bytes;
            while ((bytes = nonSeekableStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, bytes);
            }
            byte[] output = ms.ToArray();
            return output;
        }

        private byte[] CreateUploadData(Stream imageStream, string fileName, Dictionary<string, string> parameters, string boundary)
        {


            string[] keys = new string[parameters.Keys.Count];
            parameters.Keys.CopyTo(keys, 0);
            Array.Sort(keys);

            StringBuilder contentStringBuilder = new StringBuilder();

            foreach (string key in keys)
            {
                //if (key.StartsWith("oauth")) continue;

                //hashStringBuilder.Append(key);
                //hashStringBuilder.Append(parameters[key]);
                contentStringBuilder.Append("--" + boundary + "\r\n");
                contentStringBuilder.Append("Content-Disposition: form-data; name=\"" + key + "\"\r\n");
                contentStringBuilder.Append("\r\n");
                contentStringBuilder.Append(parameters[key] + "\r\n");
            }



            // Photo
            contentStringBuilder.Append("--" + boundary + "\r\n");
            contentStringBuilder.Append("Content-Disposition: form-data; name=\"photo\"; filename=\"" + Path.GetFileName(fileName) + "\"\r\n");
            contentStringBuilder.Append("Content-Type: image/jpeg\r\n");
            contentStringBuilder.Append("\r\n");

            UTF8Encoding encoding = new UTF8Encoding();

            byte[] postContents = encoding.GetBytes(contentStringBuilder.ToString());

            byte[] photoContents = ConvertNonSeekableStreamToByteArray(imageStream);

            byte[] postFooter = encoding.GetBytes("\r\n--" + boundary + "--\r\n");

            byte[] dataBuffer = new byte[postContents.Length + photoContents.Length + postFooter.Length];

            Buffer.BlockCopy(postContents, 0, dataBuffer, 0, postContents.Length);
            Buffer.BlockCopy(photoContents, 0, dataBuffer, postContents.Length, photoContents.Length);
            Buffer.BlockCopy(postFooter, 0, dataBuffer, postContents.Length + photoContents.Length, postFooter.Length);

            return dataBuffer;
        }
         */
    }
}