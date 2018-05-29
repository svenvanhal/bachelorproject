﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using NUnit.Framework;
using Timetabling.Algorithms.FET;
using Timetabling.Exceptions;
using Timetabling.Helper;

namespace Timetabling.Tests.Algorithms.FET
{

    internal class FetProcessInterfaceExposer : FetProcessInterface
    {
        public new readonly Process Process;

        public FetProcessInterfaceExposer(Process fetProcess, CancellationToken t) : base(fetProcess, t)
        {
            Process = fetProcess;
        }
        public new void CheckProcessExitCode() => base.CheckProcessExitCode();
    }

    [TestFixture]
    internal class FetProcessInterfaceTest
    {

        private Process _process;
        private FetProcessInterfaceExposer _fpi;

        [SetUp]
        public void Setup()
        {
            // Can't mock sealed class process, so create a real instance here
            var fpb = new FetProcessBuilder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "fet", "fet-cl"));

            _process = fpb.CreateProcess();
            _fpi = new FetProcessInterfaceExposer(_process, CancellationToken.None);
        }

        [Test]
        public void ConstructorTest()
        {
            Assert.AreEqual(_process, _fpi.Process);
        }

        [Test]
        public void StartProcessTest()
        {
            _fpi.StartProcess();
            Assert.IsFalse(_fpi.Process.HasExited);
            _fpi.KillProcess();
        }

        [Test]
        public void TerminateProcessTest()
        {
            var expected = _fpi.Process.StartInfo;
            _fpi.StartProcess();
            _fpi.KillProcess();

            // HasExited fails after process has been killed (why though??)
            // So when this throws an InvalidOperationException, the process has been terminated
            Assert.Throws<InvalidOperationException>(() => Assert.True(_fpi.Process.HasExited));

            // Verify that the ProcessStartInfo matches the original (one of the few unique bits of information left on the Process object)
            Assert.AreEqual(expected, _fpi.Process.StartInfo);
        }

        [Test]
        public void CheckProcessNotYetExitedCodeTest()
        {
            _fpi.StartProcess();
            var ex = Assert.Throws<InvalidOperationException>(() => _fpi.CheckProcessExitCode());
        }

        [Test]
        public void CheckProcessZeroExitCodeTest()
        {
            // Create process again with different arguments
            var fpb = new FetProcessBuilder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "fet", "fet-cl"));

            // Hopwood runs usually very fast
            fpb.SetInputFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testdata", "fet", "United-Kingdom", "Hopwood", "Hopwood.fet"));
            fpb.SetOutputDir(Util.CreateTempFolder("testIdentifier"));

            _process = fpb.CreateProcess();
            _fpi = new FetProcessInterfaceExposer(_process, CancellationToken.None);

            // Start process
            _fpi.StartProcess();

            var task = _fpi.TaskCompletionSource.Task;
            task.Wait();
        }

        [Test]
        public void CheckProcessNonZeroExitCodeTest()
        {
            // Create process again with different arguments
            var fpb = new FetProcessBuilder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "fet", "fet-cl"));

            // Invalid input file
            fpb.SetInputFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testdata", "fet", "invalid.xml"));
            fpb.SetOutputDir(Util.CreateTempFolder("testIdentifier"));

            _process = fpb.CreateProcess();
            _fpi = new FetProcessInterfaceExposer(_process, CancellationToken.None);

            // Start process
            _fpi.StartProcess();

            var task = _fpi.TaskCompletionSource.Task;

            // Assertions
            var ex = Assert.Throws<AggregateException>(() => task.Wait());
            Assert.IsInstanceOf<InvalidOperationException>(ex.InnerException);
        }

        [Test]
        public void TaskCanceledConstructorTest()
        {
            var tcs = new CancellationTokenSource();
            var token = tcs.Token;

            tcs.Cancel();

            var ex = Assert.Throws<OperationCanceledException>(() => new FetProcessInterface(null, token));
        }

        [Test]
        public void InvalidProcessStartProcessTest()
        {
            var tcs = new CancellationTokenSource();
            var token = tcs.Token;
            var fpb = new FetProcessBuilder();

            _process = fpb.CreateProcess();
            _fpi = new FetProcessInterfaceExposer(_process, token);

            var task = _fpi.StartProcess();

            var ex = Assert.Throws<AggregateException>(() => task.Wait());
            Assert.IsInstanceOf<InvalidOperationException>(ex.InnerException);
        }

    }
}
