using FlaUI.UIA3;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TnCodeApp_Tests
{
    [TestClass]
    public class ApplicationWindow_Tests
    {
        const string APPPATH = "..\\..\\..\\..\\src\\TNCodeApp\\bin\\Debug\\TNCodeApp.exe";

        [TestMethod]
        public void MainWindowOpens()
        {
            var app = FlaUI.Core.Application.Launch(APPPATH);
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);
                window.Title.Should().Be("TNCode");
            }
            app.Close();
        }
    }
}
