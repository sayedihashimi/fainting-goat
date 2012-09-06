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
        private bool _competed;
        private object _state;
        private AsyncCallback _callback;
        private HttpContext _context;

        bool IAsyncResult.IsCompleted { get { return _competed; } }
        WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }
        object IAsyncResult.AsyncState { get { return _state; } }
        bool IAsyncResult.CompletedSynchronously { get { return false; } }

        public AsyncOperation(AsyncCallback callback, HttpContext context, object state) {
            _callback = callback;
            _context = context;
            _state = state;
            _competed = false;
        }

        public void StartAsyncWork() {
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartAsyncTask), null);
        }

        public void StartAsyncTask(object workItemState) {
            IKernel kernel = new NinjectConfig().CreateKernel();
            IConfig config = kernel.Get<IConfig>();
            PathHelper pathHelper = new PathHelper(config);

            string filePath = pathHelper.ConvertMdUriToLocalPath(_context, _context.Request.Url.AbsolutePath);

            _context.Response.Write("<p>Completion IsThreadPoolThread is " + Thread.CurrentThread.IsThreadPoolThread + "</p>\r\n");

            _context.Response.Write("Hello world from Async Handler!");
            _competed = true;
            _callback(this);
        }
    }

}