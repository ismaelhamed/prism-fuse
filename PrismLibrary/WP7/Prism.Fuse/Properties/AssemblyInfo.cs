using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Prism.Fuse.Interactivity")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("1dc58df3-a8b8-48e9-a4b0-180b9bbb5bd9")]

//// NOTE: Silverlight for Windows Phone does not currently support XmlnsDefinition. 
//[assembly: XmlnsDefinition("http://www.codeplex.com/prism", "Microsoft.Practices.Prism.Shell")]
//[assembly: XmlnsDefinition("http://www.codeplex.com/prism", "Microsoft.Practices.Prism.Commands")]
//[assembly: XmlnsDefinition("http://www.codeplex.com/prism", "Microsoft.Practices.Prism.Controls")]
//[assembly: XmlnsDefinition("http://www.codeplex.com/prism", "Microsoft.Practices.Prism.Services")]
//[assembly: XmlnsDefinition("http://www.codeplex.com/prism", "Microsoft.Practices.Prism.ViewModel")]
//[assembly: XmlnsDefinition("http://www.codeplex.com/prism", "prism")]

[assembly: XmlnsPrefix("clr-namespace:Microsoft.Practices.Prism;assembly=Prism.Fuse", "prism")]
[assembly: XmlnsDefinition("clr-namespace:Microsoft.Practices.Prism;assembly=Prism.Fuse", "Microsoft.Practices.Prism")]
[assembly: XmlnsPrefix("clr-namespace:Microsoft.Practices.Prism.Converters;assembly=Prism.Fuse", "prismConverters")]
[assembly: XmlnsDefinition("clr-namespace:Microsoft.Practices.Prism.Converters;assembly=Prism.Fuse", "Microsoft.Practices.Prism.Converters")]
[assembly: XmlnsPrefix("clr-namespace:Microsoft.Practices.Prism.Shell;assembly=Prism.Fuse", "prismShell")]
[assembly: XmlnsDefinition("clr-namespace:Microsoft.Practices.Prism.Shell;assembly=Prism.Fuse", "Microsoft.Practices.Prism.Shell")]


