namespace fainting.goat {
    using fainting.goat.App_Start;
    using fainting.goat.common;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Web;

    public class ImageHandler : IHttpAsyncHandler {
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData) {
            context.Response.Write("<p>Begin IsThreadPoolThread is " + Thread.CurrentThread.IsThreadPoolThread + "</p>\r\n");
            AsyncOperation async = new AsyncOperation(cb, context, extraData);
            async.StartAsyncWork();
            return async;
        }

        public void EndProcessRequest(IAsyncResult result) {

        }

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
            IKernel kernel = new NinjectConfig().CreateKernel();
            IConfig config = kernel.Get<IConfig>();
            PathHelper pathHelper = new PathHelper(config);

            string filePath = pathHelper.ConvertMdUriToLocalPath(Context, Context.Request.Url.AbsolutePath);
            // see if the file exists, if so we need to write it to the response

            Context.Response.Write("<p>Completion IsThreadPoolThread is " + Thread.CurrentThread.IsThreadPoolThread + "</p>\r\n");

            Context.Response.Write("Hello world from Async Handler!");
            Completed = true;
            Callback(this);
        }
    }

}