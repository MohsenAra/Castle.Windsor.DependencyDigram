using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.MicroKernel.Registration;
using Component = Castle.MicroKernel.Registration.Component;

namespace Castle.Windsor.DependencyDigram.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string path = @"D:\TIBA\Castle.Windsor.DependencyDigram\TestDiagram\";

            var container = new WindsorContainer();
            container.Register(Component.For<ITestService>().ImplementedBy<TestTestService>());
            container.Register(Component.For<ITestController>().ImplementedBy<TestController>());
            container.Register(Component.For<ITestWcfService>().ImplementedBy<TestWcfService>());

            DependencyDiagram.CreateBigPicture(container, path+"BigPicture.png", pair => pair.Key.Contains("TestWcfService"));
            DependencyDiagram.CreatePictureByRootHead(container, path);
        }
    }

    public class TestTestService : ITestService
    {

    }

    public interface ITestService
    {

    }

    public class TestController : ITestController
    {
        private readonly ITestService testService;
        private readonly ITestWcfService testWcfService;

        public TestController(ITestService testService,ITestWcfService testWcfService)
        {
            this.testService = testService;
            this.testWcfService = testWcfService;
        }
    }
    public class TestWcfService : ITestWcfService 
    {
        private readonly ITestService testService;

        public TestWcfService(ITestService testService)
        {
            this.testService = testService;
        }
    }

    public interface ITestWcfService
    {
    }

    public interface ITestController
    {
    }
}
