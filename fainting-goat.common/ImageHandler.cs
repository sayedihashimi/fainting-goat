namespace fainting.goat.common {
    using Ninject;
    using System;
    using System.IO;
    using System.Threading;
    using System.Web;

    public class ImageHandler : IHttpAsyncHandler {

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData) {
            AsyncOperation async = new AsyncOperation(cb, context, extraData);
            async.StartAsyncWork();
            return async;
        }

        public void EndProcessRequest(IAsyncResult result) { }

        public bool IsReusable {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context) {
            throw new InvalidOperationException();
        }
    }

    class AsyncOperation : IAsyncResult {
        private bool Completed { get; set; }
        private object State { get; set; }
        private AsyncCallback Callback { get; set; }
        private HttpContext Context { get; set; }

        bool IAsyncResult.IsCompleted { get { return Completed; } }
        WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }
        object IAsyncResult.AsyncState { get { return State; } }
        bool IAsyncResult.CompletedSynchronously { get { return false; } }

        public AsyncOperation(AsyncCallback callback, HttpContext context, object state) {
            Callback = callback;
            Context = context;
            State = state;
            Completed = false;
        }

        public void StartAsyncWork() {
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartAsyncTask), null);
        }

        public void StartAsyncTask(object workItemState) {
            IKernel kernel = KernelManager.GetKernel();
            IConfig config = kernel.Get<IConfig>();
            IPathHelper pathHelper = kernel.Get<IPathHelper>();

            string localPath = Context.Server.MapPath(Context.Request.Url.AbsolutePath);
            string repoFilePath = pathHelper.ConvertMdUriToLocalPath(Context.Request.Url.AbsolutePath,
                (s) => this.Context.Server.MapPath(s));

            string fileToReturn = File.Exists(localPath) ? localPath : repoFilePath;

            // see if the file exists, if so we need to write it to the response      
            if (File.Exists(fileToReturn)) {
                var objImage = System.Drawing.Bitmap.FromFile(fileToReturn);
                MemoryStream objMemoryStream = new MemoryStream();
                objImage.Save(objMemoryStream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageContent = new byte[objMemoryStream.Length];
                objMemoryStream.Position = 0;
                objMemoryStream.Read(imageContent, 0, (int)objMemoryStream.Length);
                Context.Response.ContentType = "image/png";
                Context.Response.BinaryWrite(imageContent);
            }
            else {
                Context.Response.StatusCode = 404;
            }

            Completed = true;
            Callback(this);
        }
    }
}
