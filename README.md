# Castle.Windsor.DependencyDigram
Castle Windsor Dependency Diagram

Fork From This Sample Source Code In code.google.com 
http://bugsquash.blogspot.com/2009/10/visualizing-windsor-components.html


# How Use this :  
For Big Picture With Filter by Component Name
DependencyDiagram.CreateBigPicture(container, path+"BigPicture.png", pair => pair.Key.Contains("TestWcfService"));

By Header Components 
DependencyDiagram.CreatePictureByRootHead(container, path);
