using FlaUI.UIA3;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UITests
{
    [TestClass]
    public class ApplicationWindow_Tests
    {
        [TestMethod]
        public void MainWindowOpens()
        {
            var path = Path.Combine("..\\..\\..\\..\\src\\TNCodeApp\\bin\\Debug", "TNCodeApp.exe");
            var app = FlaUI.Core.Application.Launch(path);
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);
                window.Title.Should().Be("TNCode");
                
	        }

            app.Close();
            
        }
    }
}
