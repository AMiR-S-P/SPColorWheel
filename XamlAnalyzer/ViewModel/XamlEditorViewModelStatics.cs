using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace XamlAnalyzer.ViewModel
{
    public partial class XamlEditorViewModel
    {
        #region Colors
        public Color MainColor
        {
            get { return (Color)GetValue(MainColorProperty); }
            set { SetValue(MainColorProperty, value); }
        }
        public Color Color2
        {
            get { return (Color)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }
        public Color Color3
        {
            get { return (Color)GetValue(Color3Property); }
            set { SetValue(Color3Property, value); }
        }
        public Color Color4
        {
            get { return (Color)GetValue(Color4Property); }
            set { SetValue(Color4Property, value); }
        }
        public Color Color5
        {
            get { return (Color)GetValue(Color5Property); }
            set { SetValue(Color5Property, value); }
        }
        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Color5Property =
            DependencyProperty.Register("Color5", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));


        // Using a DependencyProperty as the backing store for Color4.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Color4Property =
            DependencyProperty.Register("Color4", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));


        // Using a DependencyProperty as the backing store for Color3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Color3Property =
            DependencyProperty.Register("Color3", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));


        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Color2Property =
            DependencyProperty.Register("Color2", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));


        // Using a DependencyProperty as the backing store for MainColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainColorProperty =
            DependencyProperty.Register("MainColor", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));

        #endregion

        #region Tints
        public Color Tint2
        {
            get { return (Color)GetValue(Tint2Property); }
            set { SetValue(Tint2Property, value); }
        }
        public Color Tint3
        {
            get { return (Color)GetValue(Tint3Property); }
            set { SetValue(Tint3Property, value); }
        }
        public Color Tint4
        {
            get { return (Color)GetValue(Tint4Property); }
            set { SetValue(Tint4Property, value); }
        }
        public Color Tint5
        {
            get { return (Color)GetValue(Tint5Property); }
            set { SetValue(Tint5Property, value); }
        }
        public Color Tint6
        {
            get { return (Color)GetValue(Tint6Property); }
            set { SetValue(Tint6Property, value); }
        }

        // Using a DependencyProperty as the backing store for Tint6.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Tint6Property =
            DependencyProperty.Register("Tint6", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));
        // Using a DependencyProperty as the backing store for Tint5.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Tint5Property =
            DependencyProperty.Register("Tint5", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));
        // Using a DependencyProperty as the backing store for Tint4.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Tint4Property =
            DependencyProperty.Register("Tint4", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));
        // Using a DependencyProperty as the backing store for Tint3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Tint3Property =
            DependencyProperty.Register("Tint3", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));

        // Using a DependencyProperty as the backing store for Tint2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Tint2Property =
            DependencyProperty.Register("Tint2", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));
        #endregion

        #region Tones
        public Color Tone2
        {
            get { return (Color)GetValue(Tone2Property); }
            set { SetValue(Tone2Property, value); }
        }
        public Color Tone3
        {
            get { return (Color)GetValue(Tone3Property); }
            set { SetValue(Tone3Property, value); }
        }
        public Color Tone4
        {
            get { return (Color)GetValue(Tone4Property); }
            set { SetValue(Tone4Property, value); }
        }
        public Color Tone5
        {
            get { return (Color)GetValue(Tone5Property); }
            set { SetValue(Tone5Property, value); }
        }


        public Color Tone6
        {
            get { return (Color)GetValue(Tone6Property); }
            set { SetValue(Tone6Property, value); }
        }

        // Using a DependencyProperty as the backing store for Tone6.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Tone6Property =
            DependencyProperty.Register("Tone6", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent,PropertyChangedCallBack));


        // Using a DependencyProperty as the backing store for Tone5.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Tone5Property =
            DependencyProperty.Register("Tone5", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Tone4Property =
            DependencyProperty.Register("Tone4", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));

        // Using a DependencyProperty as the backing store for Tone3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Tone3Property =
            DependencyProperty.Register("Tone3", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));

        // Using a DependencyProperty as the backing store for Tone2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Tone2Property =
            DependencyProperty.Register("Tone2", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));


        #endregion

        #region Shades


        public Color Shade2
        {
            get { return (Color)GetValue(Shade2Property); }
            set { SetValue(Shade2Property, value); }
        }
        public Color Shade3
        {
            get { return (Color)GetValue(Shade3Property); }
            set { SetValue(Shade3Property, value); }
        }
        public Color Shade4
        {
            get { return (Color)GetValue(Shade4Property); }
            set { SetValue(Shade4Property, value); }
        }
        public Color Shade5
        {
            get { return (Color)GetValue(Shade5Property); }
            set { SetValue(Shade5Property, value); }
        }
        public Color Shade6
        {
            get { return (Color)GetValue(Shade6Property); }
            set { SetValue(Shade6Property, value); }
        }

        // Using a DependencyProperty as the backing store for Shade6.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Shade6Property =
            DependencyProperty.Register("Shade6", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));
        // Using a DependencyProperty as the backing store for Shade5.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Shade5Property =
            DependencyProperty.Register("Shade5", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));
        // Using a DependencyProperty as the backing store for Shade4.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Shade4Property =
            DependencyProperty.Register("Shade4", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));

        // Using a DependencyProperty as the backing store for Shade3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Shade3Property =
            DependencyProperty.Register("Shade3", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));

        // Using a DependencyProperty as the backing store for Shade2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Shade2Property =
            DependencyProperty.Register("Shade2", typeof(Color), typeof(XamlEditorViewModel), new PropertyMetadata(Colors.Transparent, PropertyChangedCallBack));


        #endregion


        #region Common
        private static void PropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as XamlEditorViewModel)?.BrushToXaml?.Convert();
        }
        #endregion
    }
}
