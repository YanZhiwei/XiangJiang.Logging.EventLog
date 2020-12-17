using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XiangJiang.Logging.Abstractions;
using XiangJiang.Logging.Windows.EventLog;


namespace XiangJiang.Logging.Windows.EventLogTests
{
    [TestClass]
    public class EventLogServiceTests
    {
        private readonly ILogService _logService;

        public EventLogServiceTests()
        {
            _logService = new EventLogService();
        }

        [TestMethod]
        public void ErrorTest()
        {
            _logService.Error("hello error");
            try
            {
                throw new ArgumentNullException("hello exception");
            }
            catch (Exception ex)
            {
                _logService.Error("EventLogServiceTests", ex);
            }
        }
    }
}