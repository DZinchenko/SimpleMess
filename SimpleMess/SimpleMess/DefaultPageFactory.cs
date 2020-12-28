using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SimpleMess
{
    public class DefaultPageFactory : IPageFactory
    {
        public PageType CreatePage<PageType>() where PageType : Page
        {
            return DependencyService.Resolve<PageType>();
        }
    }
}
