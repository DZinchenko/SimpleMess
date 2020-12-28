using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SimpleMess
{
    public interface IPageFactory
    {
        PageType CreatePage<PageType>() where PageType : Page;
    }
}
