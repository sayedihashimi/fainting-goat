namespace fainting.goat {
    using System.IO;
    using System.Web;
    using System.Web.Mvc;

    public class ConvertFileNotFoundTo404 : FilterAttribute, IExceptionFilter {
        public void OnException(ExceptionContext filterContext) {
            if (filterContext.Exception is FileNotFoundException) {
                throw new HttpException(404, filterContext.Exception.Message);
            }
        }
    }
}