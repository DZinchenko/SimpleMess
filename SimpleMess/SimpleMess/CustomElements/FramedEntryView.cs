using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SimpleMess
{
    public class FramedEntryView : ContentView
    {
        //public Color BackgroundColor { get; set; }

        public FramedEntryView()
        {
            Content = new Frame
            {
                BorderColor = Color.Black,
                BackgroundColor = Color.LightSteelBlue,
                CornerRadius = 25,
                WidthRequest = 350,
                Padding = 2,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    
                }
            };
        }
    }
}